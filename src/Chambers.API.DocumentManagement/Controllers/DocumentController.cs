using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Chambers.API.DocumentManagement.Caching;
using Chambers.API.DocumentManagement.Core;
using Chambers.API.DocumentManagement.Core.Model;
using Chambers.API.DocumentManagement.Extensions;
using Chambers.API.DocumentManagement.Filters;
using Chambers.API.DocumentManagement.Options;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;

namespace Chambers.API.DocumentManagement.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DocumentController : ControllerBase
    {
        private const string ORDER_CACHE_KEY = "ORDER_CACHE";

        private readonly IFileRepository _fileRepository;
        private readonly ILogger<DocumentController> _logger;
        private readonly IMemoryCache _memoryCache;
        private readonly IOptions<UploadSettings> _uploadSettings;

        public DocumentController(ILogger<DocumentController> logger,
            IOptions<UploadSettings> uploadSettings,
            IFileRepository fileRepository,
            IMemoryCache memoryCache)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _uploadSettings = uploadSettings ?? throw new ArgumentNullException(nameof(uploadSettings));
            _fileRepository = fileRepository ?? throw new ArgumentNullException(nameof(fileRepository));
            _memoryCache = memoryCache ?? throw new ArgumentNullException(nameof(memoryCache));
        }

        [HttpGet("/download/{uri}")]
        public async Task<FileStreamResult> DownloadFile(string uri, CancellationToken cancellationToken = default)
        {
            DownloadFile downloadFile = await _fileRepository.GetFileAsync(uri, cancellationToken);

            return File(downloadFile.Stream, downloadFile.ContentType);
        }

        [HttpGet("/list")]
        public async Task<UploadedFiles> ListFiles()
        {
            if (!_memoryCache.TryGetValue(ORDER_CACHE_KEY, out OrderCacheEntry entry))
                return await _fileRepository.GetAllFilesAsync();

            List<UploadedFileItem> files = (await _fileRepository.GetAllFilesAsync()).Result.AsQueryable()
                .OrderBy(entry.Order, entry.OrderBy)
                .ToList();

            return new UploadedFiles {Result = files};
        }

        [HttpDelete("/id/{uri}")]
        public async Task<RemoveFile> RemoveFile(string uri, CancellationToken cancellationToken = default) =>
            await _fileRepository.RemoveFileAsync(uri, cancellationToken);

        [HttpPut("/reorder/")]
        public async Task<UploadedFiles> Reorder(string orderBy = "Name", string order = "ASC")
        {
            _memoryCache.Set(ORDER_CACHE_KEY, new OrderCacheEntry
            {
                Order = order,
                OrderBy = orderBy
            });

            return await ListFiles();
        }

        [HttpPost("upload")]
        [ServiceFilter(typeof(ValidateMimeMultipartContentFilter))]
        public async Task<IActionResult> UploadFiles(List<IFormFile> files,
            CancellationToken cancellationToken = default)
        {
            if (!ModelState.IsValid) return BadRequest();

            var allUploadedFiles = new UploadedFiles();

            foreach (IFormFile file in files)
            {
                if (file.Length <= 0) return BadRequest("Attached file is broken.");
                if (file.Length >= _uploadSettings.Value.MaximumFileSizeInBytes)
                    return BadRequest($"Attached file exceeds maximum file size. {_uploadSettings.Value.MaximumFileSizeInBytes}");
                if (!_uploadSettings.Value.AcceptedFileTypes.Contains(
                    file.FileName[(file.FileName.LastIndexOf('.')+1)..]))
                    return BadRequest("Attached file type is not allowed.");

                string fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.ToString().Trim('"');

                UploadedFileItem uploadedFile = await _fileRepository.AddFileAsync(fileName, file, cancellationToken);

                allUploadedFiles.Result.Add(uploadedFile);
            }

            return Ok(allUploadedFiles);
        }
    }
}