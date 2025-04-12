namespace App.Util
{
    using System;
    using System.IO;
    using System.Security.Cryptography;
    using System.Text;

    public class Cryptography
    {
        private readonly byte[] _key;
        private readonly byte[] _iv;
    
        public Cryptography(string encryptionKey)
        {
            using var sha256 = SHA256.Create();
            byte[] keyBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(encryptionKey));

            _key = new byte[32]; // AES-256 requires 32 bytes key
            _iv = new byte[16];  // AES uses a 16-byte IV

            Buffer.BlockCopy(keyBytes, 0, _key, 0, 32);
            Buffer.BlockCopy(keyBytes, 32 - 16, _iv, 0, 16);  // Get last 16 bytes for IV
        }

        public string Encrypt(string clearText)
        {
            using var aes = Aes.Create();
            aes.Key = _key;
            aes.IV = _iv;
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;

            using var encryptor = aes.CreateEncryptor();
            using var memoryStream = new MemoryStream();
            using var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write);
            using var writer = new StreamWriter(cryptoStream, Encoding.UTF8);
        
            writer.Write(clearText);
            writer.Flush();
            cryptoStream.FlushFinalBlock();
        
            return Convert.ToBase64String(memoryStream.ToArray());
        }

        public string Decrypt(string encryptedText)
        {
            byte[] encryptedBytes = Convert.FromBase64String(encryptedText);
        
            using var aes = Aes.Create();
            aes.Key = _key;
            aes.IV = _iv;
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;

            using var decryptor = aes.CreateDecryptor();
            using var memoryStream = new MemoryStream(encryptedBytes);
            using var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);
            using var reader = new StreamReader(cryptoStream, Encoding.UTF8);
        
            return reader.ReadToEnd();
        }
    }

}
