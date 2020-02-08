using System;
using System.IO;
using System.Net.Http;

namespace Chambers.API.DocumentManagement.IntegrationTests.Context
{
    public class UploadFileContext : IDisposable
    {
        private readonly MultipartFormDataContent _content;

        private HttpResponseMessage _httpResponseMessage;

        public UploadFileContext()
        {
            _content = new MultipartFormDataContent();
        }

        public void Dispose()
        {
            _content?.Dispose();
            _httpResponseMessage?.Dispose();
        }

        public void AddFile(string fileName, FileStream stream)
        {
            _content.Add(new StreamContent(stream), "files", fileName);
        }

        public MultipartFormDataContent GetContent() => _content;

        public HttpResponseMessage GetResponseMessage() => _httpResponseMessage;

        public void SetHttpResponseMessage(HttpResponseMessage httpResponseMessage) =>
            _httpResponseMessage = httpResponseMessage;
    }
}