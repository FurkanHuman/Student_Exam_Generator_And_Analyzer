using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.CryptoTool.FileOperations
{
    internal interface IEncrypttFile
    {
        Task<byte[]> DecryptAsync(byte[] data, byte[] key);
        Task<byte[]> EncryptAsync(byte[] data, byte[] key);
    }
}
