using Foreman.Server.Utility;
using Microsoft.AspNetCore.Http;

namespace Foreman.Server.Services
{
    public interface IFileService
    {
        public Result StoreFile(IFormFile file);
        public Result DeleteFile(int dbFileId);
        public Result<byte[]> GetFile(string fileHash);
    }
}