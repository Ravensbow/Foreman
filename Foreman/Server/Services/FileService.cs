using Foreman.Server.Data;
using Microsoft.AspNetCore.Http;
using System.Security.Cryptography;
using Microsoft.Extensions.Configuration;
using System.Text;
using System;
using System.IO;
using System.Linq;
using Foreman.Server.Utility;
using System.Threading.Tasks;
using Foreman.Shared.Data.Courses;
using Foreman.Shared.Models.Category;
using Foreman.Shared.Services;

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

                if(!_context.Files.Where(f => f.ContentHash == model.ContentHash).Any())
                {
                    this.DeleteFileFromStorage(Path.Combine(StoragePath, model.ContentHash));
                }

                return Result.Ok();
            }
            catch (Exception ex)
            {
                return Result.Fail(ex.Message);
            }

        }

        public Result StoreFile(IForemanFileModel file)
        {
            try
            {
                if (file.FileData != null)
                {
                    var byteArr = file.FileData;
                    var hasBytes = this.HashFunction(byteArr);

                    if (!File.Exists(Path.Combine(StoragePath, HashToString(hasBytes))))
                    {
                        File.WriteAllBytes(Path.Combine(StoragePath, HashToString(hasBytes)), byteArr);
                    }
                    _context.Files.Add(new ForemanFile()
                    {
                        MimeType = file.MimeType,
                        PathNameHash = HashToString(HashFunction($"/{file.ContextId}/{file.Component}/{file.Filename}")),
                        ContentHash = HashToString(byteArr),
                        CreateTime = DateTime.Now,
                        Filename = file.Filename,
                        Component = file.Component,
                        ItemId = file.ItemId,
                        UserId = file.UserId,
                        ContextId = file.ContextId
                    });
                    _context.SaveChanges();
                }

                return Result.Ok();
            }
            catch (Exception ex)
            {
                return Result.Fail(ex.Message);
            }

        }

        public Result<byte[]> GetFile(string fileHash)
        {
            var filePath = Path.Combine(StoragePath, fileHash);
            byte[] file = this.GetFileFromStorage(filePath);

            if (file == null)
               return Result.Fail<byte[]>("File does not exist.");
            if (CheckIfFileIsCorrupted(fileHash, file))
                return Result.Fail<byte[]>("File is corrupted");

            return Result.Ok<byte[]>(file);
        }

        public Result<byte[]> GetFile(int logicalDbFileId)
        {
            try
            {
                var record = _context.Files.Single(x => x.Id == logicalDbFileId);
                return this.GetFile(record.ContentHash);
            }
            catch (Exception ex)
            {
                return Result.Fail<byte[]>(ex.Message);
            }
        }

        public bool CheckIfStorageDirectoryExists()
        {
            return Directory.Exists(StoragePath);
        }

        public void EnsureStorageDirectoryExists()
        {
            Directory.CreateDirectory(StoragePath);
        }

        public string HashToString(byte[] hashByteArr)
        {
            return BitConverter.ToString(hashByteArr).Replace("-",string.Empty);
        }

        private byte[] HashFunction(string source)
        {
            var hash = SHA1.Create();
            byte[] sourceByteArr = Encoding.UTF8.GetBytes(source);
            return hash.ComputeHash(sourceByteArr);

        }

        private byte[] HashFunction(byte[] source)
        {
            var hash = SHA1.Create();
            return hash.ComputeHash(source);
        }

        private byte[] GetFileFromStorage(string filePath)
        {
            if (File.Exists(filePath))
            {
                return File.ReadAllBytes(filePath);
            }
            return null;
        }

        private void DeleteFileFromStorage(string filePath)
        {
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }

        private bool CheckIfFileIsCorrupted(string fileHash, byte[] file)
        {
            return string.Equals(HashToString(HashFunction(file)), fileHash);
        }
    }
}
