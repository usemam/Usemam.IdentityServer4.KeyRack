using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging;

using Newtonsoft.Json;

using Usemam.IdentityServer4.KeyRack.Model;

namespace Usemam.IdentityServer4.KeyRack
{
    /// <summary><see cref="IKeyRepository" /> implementation for persisting keys in file system</summary>
    public class FileSystemKeyRepository : IKeyRepository
    {
        private const string KeyFileFormatPattern = "key-{0}.json";

        private readonly DirectoryInfo _keyDir;

        private readonly ILogger<FileSystemKeyRepository> _logger;

        /// <param name="dirPath">Path to a directory where keys will be stored.</param>
        /// <param name="logger">Logger</param>
        public FileSystemKeyRepository(string dirPath, ILogger<FileSystemKeyRepository> logger)
        {
            _keyDir = new DirectoryInfo(dirPath);
            if (!_keyDir.Exists)
            {
                _keyDir.Create();
            }
        }

        /// <inheritdoc />
        public Task DeleteKeyAsync(string keyId)
        {
            string keyFilePath = Path.Combine(_keyDir.FullName, string.Format(KeyFileFormatPattern, keyId));
            try
            {
                File.Delete(keyFilePath);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, $"Error while deleting file '{keyFilePath}'");
            }

            return Task.CompletedTask;
        }

        /// <inheritdoc />
        public async Task<IEnumerable<SerializedKey>> LoadKeysAsync()
        {
            var result = new List<SerializedKey>();
            var keyFiles = _keyDir.GetFiles(string.Format(KeyFileFormatPattern, "*"));
            foreach (var keyFile in keyFiles)
            {
                try
                {
                    using (var reader = new StreamReader(keyFile.OpenRead()))
                    {
                        result.Add(JsonConvert.DeserializeObject<SerializedKey>(await reader.ReadToEndAsync()));
                    }
                }
                catch (Exception exception)
                {
                    _logger.LogError(exception, $"Error while reading file '{keyFile.Name}'");
                }
            }

            return result;
        }

        /// <inheritdoc />
        public Task StoreKeyAsync(SerializedKey key)
        {
            string keyFileContent = JsonConvert.SerializeObject(key);
            return File.WriteAllTextAsync(
                Path.Combine(_keyDir.FullName, string.Format(KeyFileFormatPattern, key.KeyId)), keyFileContent);
        }
    }
}