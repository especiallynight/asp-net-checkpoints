using System;
using System.Text;
using Sodium;

public class EncryptionService
{
    private readonly byte[] _key;

    public EncryptionService()
    {
        _key = SecretBox.GenerateKey();
    }

    public string Encrypt(string plaintext)
    {
        var nonce = SecretBox.GenerateNonce();
        var ciphertext = SecretBox.Create(Encoding.UTF8.GetBytes(plaintext), nonce, _key);
        return Convert.ToBase64String(nonce) + ":" + Convert.ToBase64String(ciphertext);
    }

    public string Decrypt(string encryptedData)
    {
        var parts = encryptedData.Split(':');
        if (parts.Length != 2)
            throw new ArgumentException("Invalid encrypted data format.");

        var nonce = Convert.FromBase64String(parts[0]);
        var ciphertext = Convert.FromBase64String(parts[1]);
        var decrypted = SecretBox.Open(ciphertext, nonce, _key);
        return Encoding.UTF8.GetString(decrypted);
    }
}
