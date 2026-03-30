using Microsoft.AspNetCore.Mvc;
using Checkpoint_8.Models;
using Checkpoint_8.Services;

namespace Checkpoint_8.Controllers
{
    public class HomeController : Controller
    {
        private readonly IEncryptionService _service;

        public HomeController(IEncryptionService service)
        {
            _service = service;
        }

        public IActionResult Index()
        {
            return View(new CryptoViewModel());
        }

        [HttpPost]
        public IActionResult Encrypt(CryptoViewModel model)
        {
            try
            {
                if (string.IsNullOrEmpty(model.InputText))
                {
                    model.ErrorMessage = "Введите текст";
                    return View("Index", model);
                }

                if (model.EncryptionAlgorithm == "AES")
                {
                    if (string.IsNullOrEmpty(model.Key))
                        model.Key = _service.GenerateAESKey();

                    model.EncryptedText = _service.EncryptAES(model.InputText, model.Key);
                }
                else if (model.EncryptionAlgorithm == "RSA")
                {
                    var keys = _service.GenerateRSAKeys();
                    model.PublicKey = keys.publicKey;
                    model.PrivateKey = keys.privateKey;
                    model.EncryptedText = _service.EncryptRSA(model.InputText, model.PublicKey);
                    model.Key = model.PublicKey;
                }
            }
            catch (Exception ex)
            {
                model.ErrorMessage = ex.Message;
            }

            return View("Index", model);
        }

        [HttpPost]
        public IActionResult Decrypt(CryptoViewModel model)
        {
            try
            {
                if (model.EncryptionAlgorithm == "AES")
                {
                    model.DecryptedText = _service.DecryptAES(model.EncryptedText, model.Key);
                }
                else if (model.EncryptionAlgorithm == "RSA")
                {
                    model.DecryptedText = _service.DecryptRSA(model.EncryptedText, model.PrivateKey);
                }
            }
            catch (Exception ex)
            {
                model.ErrorMessage = ex.Message;
            }

            return View("Index", model);
        }

        [HttpPost]
        public IActionResult GenerateAESKey()
        {
            return Json(new { key = _service.GenerateAESKey() });
        }

        [HttpPost]
        public IActionResult GenerateRSAKeys()
        {
            var keys = _service.GenerateRSAKeys();
            return Json(new { publicKey = keys.publicKey, privateKey = keys.privateKey });
        }
    }
}