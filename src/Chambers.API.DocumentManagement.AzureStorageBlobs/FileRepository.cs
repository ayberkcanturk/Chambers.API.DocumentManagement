using System;
using System.Net.Http.Formatting;
using System.Threading;
using System.Threading.Tasks;

using Chambers.API.DocumentManagement.AzureStorageBlobs.Options;
using Chambers.API.DocumentManagement.Core;
using Chambers.API.DocumentManagement.Core.Model;

using Microsoft.AspNetCore.Http;
using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Blob;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Chambers.API.DocumentManagement.AzureStorageBlobs
{
    public class FileRepository : IFileRepository
    {
        private readonly ILogger<FileRepository> _logger;
        private readonly CloudBlobClient _client;
        private readonly CloudBlobContainer _container;

        public FileRepository(ILogger<FileRepository> logger, 
            IOptions<AzureStorageBlobSettings> options)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

            if (options.Value.ConnectionString == null)
                throw new ArgumentNullException(nameof(options.Value.ConnectionString));

            if (options.Value.ContainerName == null)
                throw new ArgumentNullException(nameof(options.Value.ContainerName));

            CloudStorageAccount cloudStorageAccount = CloudStorageAccount.Parse(options.Value.ConnectionString);
            _client = cloudStorageAccount.CreateCloudBlobClient();
            _container = _client.GetContainerReference(options.Value.ContainerName.ToLowerInvariant());
        }

        public async Task<UploadedFileItem> AddFileAsync(string fileName, IFormFile formFile, CancellationToken cancellationToken = default)
        {
            try
            {
                var uploadedFileItem = new UploadedFileItem();

                _container.CreateIfNotExists();

                ICloudBlob blob = _container.GetBlockBlobReference(fileName ?? formFile.FileName);
                
                await blob.UploadFromStreamAsync(formFile.OpenReadStream(), cancellationToken);

                await blob.FetchAttributesAsync(cancellationToken);

                uploadedFileItem.Uri = blob.Uri.ToString();
                uploadedFileItem.Name = blob.Name;
                uploadedFileItem.Length = blob.Properties.Length;

                return uploadedFileItem;
            }
            catch (StorageException e)
            {
                _logger.LogError(e, "An error occured while accessing Azure Storage Blobs.");
                throw;
            }
        }

        /// <summary>
        ///     https://docs.microsoft.com/en-us/azure/storage/blobs/storage-blob-containers-list
        /// </summary>
        /// <param name="prefix"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<UploadedFiles> GetAllFilesAsync(string prefix = "",
            CancellationToken cancellationToken = default)
        {
            var uploadedFiles = new UploadedFiles();

            bool containerExists = await _container.ExistsAsync(cancellationToken);

            if (!containerExists) return uploadedFiles;

            BlobContinuationToken continuationToken = default;

            do
            {
                BlobResultSegment blobs =
                    await _container.ListBlobsSegmentedAsync(prefix, continuationToken, cancellationToken);

                continuationToken = blobs.ContinuationToken;

                foreach (IListBlobItem currentBlob in blobs.Results)
                {
                    var blob = new CloudBlockBlob(currentBlob.Uri);

                    if (!await blob.ExistsAsync(cancellationToken)) continue;

                    await blob.FetchAttributesAsync(cancellationToken);

                    uploadedFiles.Result.Add(new UploadedFileItem
                    {
                        Uri = blob.Uri.ToString(),
                        Length = blob.Properties.Length,
                        Name = blob.Name
                    });
                }
            } while (continuationToken != null);

            return uploadedFiles;
        }

        public async Task<DownloadFile> GetFileAsync(string uri, CancellationToken cancellationToken = default)
        {
            var downloadedFile = new DownloadFile();

            bool containerExists = await _container.ExistsAsync(cancellationToken);

            if (!containerExists) return downloadedFile;

            var blob = new CloudBlockBlob(new Uri(uri));

            if (blob.IsDeleted) return downloadedFile;

            await blob.FetchAttributesAsync(cancellationToken);

            downloadedFile.ContentType = blob.Properties.ContentType;
            downloadedFile.Stream = await blob.OpenReadAsync(cancellationToken);

            return downloadedFile;
        }

        public async Task<RemoveFile> RemoveFileAsync(string uri, CancellationToken cancellationToken = default)
        {
            var removeFile = new RemoveFile(uri);

            bool containerExists = await _container.ExistsAsync(cancellationToken);

            if (!containerExists) return removeFile;

            var blob = new CloudBlockBlob(new Uri(uri));

            if (!blob.IsDeleted) await blob.DeleteIfExistsAsync(cancellationToken);

            removeFile.Result = true;

            return removeFile;
        }
    }
}