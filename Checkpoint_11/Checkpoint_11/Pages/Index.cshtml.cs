using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Text;

public class IndexModel : PageModel
{
    private readonly AesEncryptionService _aesService;

    public IndexModel()
    {
        var key = Encoding.UTF8.GetBytes("1234567890123456"); 
        var iv = Encoding.UTF8.GetBytes("1234567890123456"); 

        _aesService = new AesEncryptionService(key, iv);
    }

    [BindProperty]
    public string InputText { get; set; }

    public string Result { get; private set; }

    public void OnPost(string action)
    {
        if (action == "encrypt")
        {
            var encryptedData = _aesService.Encrypt(InputText);
            Result = Convert.ToBase64String(encryptedData);
        }
        else if (action == "decrypt")
        {
            var encryptedData = Convert.FromBase64String(InputText);
            Result = _aesService.Decrypt(encryptedData);
        }
    }
}
