namespace Chambers.API.DocumentManagement.Options
{
    public class UploadSettings
    {
        public string[] AcceptedFileTypes { get; set; }
        public long MaximumFileSizeInBytes { get; set; }
    }
}