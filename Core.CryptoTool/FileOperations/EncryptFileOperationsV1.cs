using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.CryptoTool.FileOperations
{
    internal class EncryptFileOperationsV1 : IEncrypttFile
    {
        public Task<byte[]> DecryptAsync(byte[] data, byte[] key)
        {
            throw new NotImplementedException();
        }

        public Task<byte[]> EncryptAsync(byte[] data, byte[] key)
        {
            throw new NotImplementedException();
        }
    }
}
