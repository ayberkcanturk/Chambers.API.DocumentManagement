using System.Collections.Generic;

namespace Chambers.API.DocumentManagement.Core.Model
{
    public class UploadedFiles
    {
        public UploadedFiles()
        {
            Result = new List<UploadedFileItem>();
        }

        public IList<UploadedFileItem> Result { get; set; }
    }
}