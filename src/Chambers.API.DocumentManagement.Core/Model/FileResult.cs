using System;
using System.Collections.Generic;

namespace Chambers.API.DocumentManagement.Core.Model
{
    public class FileResult
    {
        public FileResult()
        {
            FileNames = new List<string>();
            ContentTypes = new List<string>();
        }

        public List<string> FileNames { get; set; }
        public string Description { get; set; }
        public DateTime CreatedTimestamp { get; set; }
        public DateTime UpdatedTimestamp { get; set; }
        public List<string> ContentTypes { get; set; }
    }
}
