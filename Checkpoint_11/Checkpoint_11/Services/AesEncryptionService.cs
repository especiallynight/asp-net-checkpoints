using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

public class AesEncryptionService
{
    private readonly byte[] _key;
    private readonly byte[] _iv;

    public AesEncryptionService(byte[] key, byte[] iv)
    {
        _key = key;
        _iv = iv;
    }

    public byte[] Encrypt(string plainText)
    {
        using (var aes = new AesCryptoServiceProvider())
        {
            aes.Key = _key;
            aes.IV = _iv;

            using (var encryptor = aes.CreateEncryptor(aes.Key, aes.IV))
            using (var ms = new MemoryStream())
            using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
            {
                using (var writer = new StreamWriter(cs))
                {
                    writer.Write(plainText);
                }
                return ms.ToArray();
            }
        }
    }

    public string Decrypt(byte[] cipherText)
    {
        using (var aes = new AesCryptoServiceProvider())
        {
            aes.Key = _key;
            aes.IV = _iv;

            using (var decryptor = aes.CreateDecryptor(aes.Key, aes.IV))
            using (var ms = new MemoryStream(cipherText))
            using (var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
            using (var reader = new StreamReader(cs))
            {
                return reader.ReadToEnd();
            }
        }
    }
}
