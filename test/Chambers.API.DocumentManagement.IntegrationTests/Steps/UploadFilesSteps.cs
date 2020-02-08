using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

using Chambers.API.DocumentManagement.IntegrationTests.Context;

using TechTalk.SpecFlow;

using Xunit;

namespace Chambers.API.DocumentManagement.IntegrationTests.Steps
{
    [Binding]
    public class UploadFilesSteps
    {
        private readonly UploadFileContext _context;

        public UploadFilesSteps(UploadFileContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        [Given(@"I have a max pdf size of (.*)MB")]
        public void GivenIHaveAMaxPdfSizeOfMB(int p0)
        {
            //Ignore - Max pdf size is coming from Options.
        }

        [Given(@"I have a non-pdf to upload")]
        public void GivenIHaveANon_PdfToUpload()
        {
            var fileStream = new FileStream("./Context/TestFiles/CV.doc", FileMode.Open);

            string generatedName = $"CV-{Guid.NewGuid().ToString()}.doc";

            _context.AddFile(generatedName, fileStream);
        }

        [Given(@"I have a PDF to upload")]
        public void GivenIHaveAPDFToUpload()
        {
            var fileStream = new FileStream("./Context/TestFiles/CV.pdf", FileMode.Open);

            string generatedName = $"CV-{Guid.NewGuid().ToString()}.pdf";

            _context.AddFile(generatedName, fileStream);
        }

        [Then(@"it is uploaded successfully")]
        public void ThenItIsUploadedSuccessfully()
        {
            bool statusCode = _context.GetResponseMessage().IsSuccessStatusCode;

            Assert.True(_context.GetResponseMessage().IsSuccessStatusCode,
                $"httpResponseMessage.IsSuccessStatusCode is not success:{statusCode}");
        }

        [Then(@"the API does not accept the file and returns the appropriate messaging and status")]
        public void ThenTheAPIDoesNotAcceptTheFileAndReturnsTheAppropriateMessagingAndStatus()
        {
            HttpResponseMessage responseMessage = _context.GetResponseMessage();
            Assert.Equal(HttpStatusCode.BadRequest, responseMessage.StatusCode);
            //We can check the appropriate message here as well.
        }

        [When(@"I send the non-pdf to the API (.*)")]
        public void WhenISendTheNon_PdfToTheAPI(string p0)
        {
            var client = new HttpClient();

            var requestMessage = new HttpRequestMessage
            {
                RequestUri = new Uri(p0),
                Method = HttpMethod.Post,
                Content = _context.GetContent()
            };

            HttpResponseMessage httpResponseMessage = client.SendAsync(requestMessage).GetAwaiter().GetResult();
            _context.SetHttpResponseMessage(httpResponseMessage);
        }

        [When(@"I send the pdf to the API (.*)")]
        public void WhenISendThePdfToTheAPI(string p0)
        {
            var client = new HttpClient();

            var requestMessage = new HttpRequestMessage
            {
                RequestUri = new Uri(p0),
                Method = HttpMethod.Post,
                Content = _context.GetContent()
            };

            HttpResponseMessage httpResponseMessage = client.SendAsync(requestMessage).GetAwaiter().GetResult();
            _context.SetHttpResponseMessage(httpResponseMessage);
        }

        [When(@"I send the PDF to the API (.*)")]
        public void WhenISendThePDFToTheAPI(string p0)
        {
            var client = new HttpClient();

            var requestMessage = new HttpRequestMessage
            {
                RequestUri = new Uri(p0),
                Method = HttpMethod.Post,
                Content = _context.GetContent()
            };

            HttpResponseMessage httpResponseMessage = client.SendAsync(requestMessage).GetAwaiter().GetResult();
            _context.SetHttpResponseMessage(httpResponseMessage);
        }
    }
}