using System.Threading;
using System.Threading.Tasks;

using Chambers.API.DocumentManagement.Core.Model;

using Microsoft.AspNetCore.Http;

namespace Chambers.API.DocumentManagement.Core
{
    public interface IFileRepository
    {
        Task<UploadedFileItem> AddFileAsync(string fileName, IFormFile formFile,
            CancellationToken cancellationToken = default);

        Task<UploadedFiles> GetAllFilesAsync(string prefix = "", CancellationToken cancellationToken = default);

        Task<DownloadFile> GetFileAsync(string uri, CancellationToken cancellationToken = default);

        Task<RemoveFile> RemoveFileAsync(string uri, CancellationToken cancellationToken = default);
    }
}