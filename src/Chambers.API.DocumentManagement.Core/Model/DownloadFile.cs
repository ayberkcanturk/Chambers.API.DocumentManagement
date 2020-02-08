using System.IO;

namespace Chambers.API.DocumentManagement.Core.Model
{
    public class DownloadFile
    {
        public string ContentType { get; set; }
        public Stream Stream { get; set; }
    }
}