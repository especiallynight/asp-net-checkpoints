using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

public class EncryptDecryptModel : PageModel
{
    private readonly EncryptionService _encryptionService;

    public EncryptDecryptModel()
    {
        _encryptionService = new EncryptionService();
    }

    [BindProperty]
    public string Plaintext { get; set; }

    public string EncryptedText { get; private set; }

    public string DecryptedText { get; private set; }

    public void OnPost()
    {
        if (!string.IsNullOrWhiteSpace(Plaintext))
        {
            EncryptedText = _encryptionService.Encrypt(Plaintext);
            DecryptedText = _encryptionService.Decrypt(EncryptedText);
        }
    }
}
