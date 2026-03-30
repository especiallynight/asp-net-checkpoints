using System.Security.Cryptography;
using System.Text;

namespace Checkpoint_8.Services
{
    public interface IEncryptionService
    {
        string EncryptAES(string plainText, string key);
        string DecryptAES(string cipherText, string key);
        string GenerateAESKey();
        (string publicKey, string privateKey) GenerateRSAKeys();
        string EncryptRSA(string plainText, string publicKey);
        string DecryptRSA(string cipherText, string privateKey);
    }

    public class EncryptionService : IEncryptionService
    {
        private static Aes CreateAes(string key)
        {
            var aes = Aes.Create();
            aes.Key = Convert.FromBase64String(key);
            return aes;
        }

        public string GenerateAESKey()
        {
            using var aes = Aes.Create();
            aes.KeySize = 256;
            aes.GenerateKey();
            return Convert.ToBase64String(aes.Key);
        }

        public string EncryptAES(string plainText, string key)
        {
            using var aes = CreateAes(key);
            aes.GenerateIV();

            using var encryptor = aes.CreateEncryptor();
            var plainBytes = Encoding.UTF8.GetBytes(plainText);
            var cipherBytes = encryptor.TransformFinalBlock(plainBytes, 0, plainBytes.Length);

            var result = new byte[aes.IV.Length + cipherBytes.Length];
            Array.Copy(aes.IV, result, aes.IV.Length);
            Array.Copy(cipherBytes, 0, result, aes.IV.Length, cipherBytes.Length);

            return Convert.ToBase64String(result);
        }

        public string DecryptAES(string cipherText, string key)
        {
            var fullCipher = Convert.FromBase64String(cipherText);
            using var aes = CreateAes(key);

            var iv = new byte[aes.BlockSize / 8];
            var cipherBytes = new byte[fullCipher.Length - iv.Length];
            Array.Copy(fullCipher, iv, iv.Length);
            Array.Copy(fullCipher, iv.Length, cipherBytes, 0, cipherBytes.Length);
            aes.IV = iv;

            using var decryptor = aes.CreateDecryptor();
            var plainBytes = decryptor.TransformFinalBlock(cipherBytes, 0, cipherBytes.Length);
            return Encoding.UTF8.GetString(plainBytes);
        }

        public (string publicKey, string privateKey) GenerateRSAKeys()
        {
            using var rsa = RSA.Create(2048);
            return (Convert.ToBase64String(rsa.ExportRSAPublicKey()),
                    Convert.ToBase64String(rsa.ExportRSAPrivateKey()));
        }

        public string EncryptRSA(string plainText, string publicKey)
        {
            using var rsa = RSA.Create();
            rsa.ImportRSAPublicKey(Convert.FromBase64String(publicKey), out _);
            var cipherBytes = rsa.Encrypt(Encoding.UTF8.GetBytes(plainText), RSAEncryptionPadding.Pkcs1);
            return Convert.ToBase64String(cipherBytes);
        }

        public string DecryptRSA(string cipherText, string privateKey)
        {
            using var rsa = RSA.Create();
            rsa.ImportRSAPrivateKey(Convert.FromBase64String(privateKey), out _);
            var plainBytes = rsa.Decrypt(Convert.FromBase64String(cipherText), RSAEncryptionPadding.Pkcs1);
            return Encoding.UTF8.GetString(plainBytes);
        }
    }
}
