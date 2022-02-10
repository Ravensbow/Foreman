using Foreman.Server.Data;
using Microsoft.AspNetCore.Http;
using System.Security.Cryptography;
using Microsoft.Extensions.Configuration;
using System.Text;
using System;
using System.IO;
using System.Linq;
using Foreman.Server.Utility;

namespace Foreman.Server.Services
{
    public class FileService : IFileService
    {
        private ApplicationContext _context;
        private IConfiguration _configuration;
        public string StorageDirectory { get; }
        public string StoragePath { get; }
        public FileService(ApplicationContext db, IConfiguration config)
        {
            _context = db;
            _configuration = config;
            StorageDirectory = _configuration.GetSection("ForemanStorage")
                .GetValue<string>("DirectoryName");
            StoragePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, StorageDirectory);
        }

        public Result DeleteFile(int dbFileId)
        {
            try
            {
                var model = _context.Files.Where(f => f.Id == dbFileId).Single();
                _context.Files.Remove(model);

                if(!_context.Files.Where(f => f.ContentHash == model.ContentHash).Any() && File.Exists(Path.Combine(StoragePath,model.ContentHash)))
                {
                    File.Delete(Path.Combine(StoragePath, model.ContentHash));
                }

                return Result.Ok();
            }
            catch (Exception ex)
            {
                return Result.Fail(ex.Message);
            }

        }

        public Result StoreFile(IFormFile file)
        {
            throw new NotImplementedException();
        }

        public Result<byte[]> GetFile(string fileHash)
        {
            throw new NotImplementedException();
        }

        public bool CheckIfStorageDirectoryExists()
        {
            return Directory.Exists(StoragePath);
        }

        public void EnsureStorageDirectoryExists()
        {
            Directory.CreateDirectory(StoragePath);
        }

        private string HashToString(byte[] hashByteArr)
        {
            return BitConverter.ToString(hashByteArr).Replace("-",string.Empty);
        }

        private byte[] HashFunction(string source)
        {
            var hash = SHA1.Create();
            byte[] sourceByteArr = Encoding.UTF8.GetBytes(source);
            return hash.ComputeHash(sourceByteArr);

        }

        private byte[] HasFunction(byte[] source)
        {
            var hash = SHA1.Create();
            return hash.ComputeHash(source);
        }
    }
}
