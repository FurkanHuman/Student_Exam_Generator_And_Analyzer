using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.CryptoTool.FileOperations
{
    internal class SESFile
    {
        public Guid Id { get; set; }
        public string FileName { get; set; }
        public byte[] EncryptedFileBody { get; set; }
        public string BodyHash { get; set; }
        public string[] Claims { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
    }
}