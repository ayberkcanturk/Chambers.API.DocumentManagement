using System;

using Chambers.API.DocumentManagement.AzureStorageBlobs.Options;

using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Blob;
using Microsoft.Extensions.Options;

namespace Chambers.API.DocumentManagement.AzureStorageBlobs
{
    public class CloudBlobContainerProvider
    {
        private readonly CloudBlobContainer _container;
        private readonly IOptions<AzureStorageBlobSettings> _options;

        public CloudBlobContainerProvider(IOptions<AzureStorageBlobSettings> options)
        {
            if (options.Value.ConnectionString == null)
                throw new ArgumentNullException(nameof(options.Value.ConnectionString));

            if (options.Value.ContainerName == null)
                throw new ArgumentNullException(nameof(options.Value.ContainerName));

            CloudStorageAccount cloudStorageAccount = CloudStorageAccount.Parse(options.Value.ConnectionString);
            CloudBlobClient client = cloudStorageAccount.CreateCloudBlobClient();
            _container = client.GetContainerReference(options.Value.ContainerName);
        }

        public CloudBlobContainer Get() => _container;
    }
}