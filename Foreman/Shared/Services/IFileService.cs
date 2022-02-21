using Foreman.Server.Utility;
using Foreman.Shared.Models.Category;

namespace Foreman.Shared.Services
{
    public interface IFileService
    {
        public string StorageDirectory { get; }
        public string StoragePath { get; }
        public Result DeleteFile(int dbFileId);

        public Result StoreFile(IForemanFileModel file);

        public Result<byte[]> GetFile(string fileHash);

        public Result<byte[]> GetFile(int logicalDbFileId);

        public bool CheckIfStorageDirectoryExists();

        public void EnsureStorageDirectoryExists();

        string HashToString(byte[] hashByteArr);
    }
}