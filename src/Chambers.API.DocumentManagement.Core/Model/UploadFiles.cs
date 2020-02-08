using System.Collections.Generic;

using Microsoft.AspNetCore.Http;

namespace Chambers.API.DocumentManagement.Core.Model
{
    public class UploadFiles
    {
        public ICollection<IFormFile> File { get; set; }
    }
}