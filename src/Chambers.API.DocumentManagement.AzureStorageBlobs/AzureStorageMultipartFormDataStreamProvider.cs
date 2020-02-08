using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;

using Chambers.API.DocumentManagement.AzureStorageBlobs.Options;

using Microsoft.Azure.Storage.Blob;
using Microsoft.Extensions.Options;

namespace Chambers.API.DocumentManagement.AzureStorageBlobs
{
    public class AzureStorageMultipartFormDataStreamProvider : MultipartFormDataStreamProvider
    {
        private readonly CloudBlobContainer _blobContainer;
        private readonly IOptions<AzureStorageBlobSettings> _settings;

        public AzureStorageMultipartFormDataStreamProvider(IOptions<AzureStorageBlobSettings> settings,
            CloudBlobContainerProvider cloudBlobContainerProvider) : base("azure")
        {
            _settings = settings ?? throw new ArgumentNullException(nameof(settings));
            
            if (cloudBlobContainerProvider == null)
                throw new ArgumentNullException(nameof(cloudBlobContainerProvider));

            _blobContainer = cloudBlobContainerProvider.Get();
        }

        public override Stream GetStream(HttpContent parent, HttpContentHeaders headers)
        {
            if (parent == null) throw new ArgumentNullException(nameof(parent));
            if (headers == null) throw new ArgumentNullException(nameof(headers));

            if (!_settings.Value.AllowedMimeTypes.Contains(headers.ContentType.ToString().ToLower()))
                throw new NotSupportedException("Mime type not supported.");

            string fileName = Guid.NewGuid().ToString();

            CloudBlockBlob blob = _blobContainer.GetBlockBlobReference(fileName);

            if (headers.ContentType != null)
                blob.Properties.ContentType = headers.ContentType.MediaType;

            FileData.Add(new MultipartFileData(headers, blob.Name));

            return blob.OpenWrite();
        }
    }
}