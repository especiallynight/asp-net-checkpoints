namespace Checkpoint_8.Models
{
    public class CryptoViewModel
    {
        public string InputText { get; set; }
        public string EncryptionAlgorithm { get; set; } = "AES";
        public string Key { get; set; }
        public string EncryptedText { get; set; }
        public string DecryptedText { get; set; }
        public string PublicKey { get; set; }
        public string PrivateKey { get; set; }
        public string ErrorMessage { get; set; }
    }
}