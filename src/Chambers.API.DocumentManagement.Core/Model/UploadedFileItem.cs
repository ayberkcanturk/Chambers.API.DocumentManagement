using System;
using System.Collections.Generic;
using System.Text;

namespace Chambers.API.DocumentManagement.Core.Model
{
    public class UploadedFileItem
    {
        public string Name { get; set; }
        public string ContentType { get; set; }
        public long Length { get; set; }
        public string Uri { get; set; }
    }
}
