using Microsoft.Extensions.Options;

namespace Chambers.API.DocumentManagement.AzureStorageBlobs.Options
{
    public class AzureStorageBlobSettings
    {
        public string ConnectionString { get; set; }
        public string ContainerName { get; set; }
        public string[] AllowedMimeTypes { get; set; }
        public long MaxFileSize { get; set; }
    }
}