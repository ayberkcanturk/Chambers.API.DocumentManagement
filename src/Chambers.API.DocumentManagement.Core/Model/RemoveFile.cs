namespace Chambers.API.DocumentManagement.Core.Model
{
    public class RemoveFile
    {
        public RemoveFile(string uri)
        {
            this.Uri = uri;
        }

        public string Uri { get; protected set; }
        public bool Result { get; set; }
    }
}