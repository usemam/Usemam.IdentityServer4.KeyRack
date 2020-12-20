using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

using Newtonsoft.Json;

using Usemam.IdentityServer4.KeyRack.Model;

namespace Usemam.IdentityServer4.KeyRack
{
    public class FileSystemKeyRepository : IKeyRepository
    {
        private const string KeyFileFormatPattern = "key-{0}.json";

        private readonly DirectoryInfo _keyDir;

        public FileSystemKeyRepository(string dirPath)
        {
            _keyDir = new DirectoryInfo(dirPath);
            if (!_keyDir.Exists)
            {
                _keyDir.Create();
            }
        }

        public Task DeleteKeyAsync(string keyId)
        {
            string keyFilePath = Path.Combine(_keyDir.FullName, string.Format(KeyFileFormatPattern, keyId));
            try
            {
                File.Delete(keyFilePath);
            }
            catch
            {
                // todo: log error
            }

            return Task.CompletedTask;
        }

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
                catch
                {
                    // todo: log error
                }
            }

            return result;
        }

        public Task StoreKeyAsync(SerializedKey key)
        {
            string keyFileContent = JsonConvert.SerializeObject(key);
            return File.WriteAllTextAsync(
                Path.Combine(_keyDir.FullName, string.Format(KeyFileFormatPattern, key.KeyId)), keyFileContent);
        }
    }
}