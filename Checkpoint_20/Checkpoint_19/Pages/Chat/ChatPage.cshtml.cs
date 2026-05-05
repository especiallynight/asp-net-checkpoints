using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Checkpoint_19.Chat
{
    [Authorize]
    public class ChatPageModel : PageModel
    {
        public string Token { get; set; }
        public string Username { get; set; }

        public void OnGet()
        {
            Token = TempData["JwtToken"] as string ?? Request.Cookies["jwt_token"];

            Username = TempData["Username"] as string;

            if (string.IsNullOrEmpty(Token))
            {
                Response.Redirect("/Account/Login");
                return;
            }

            if (string.IsNullOrEmpty(Username))
            {
                try
                {
                    var handler = new JwtSecurityTokenHandler();
                    var jsonToken = handler.ReadToken(Token) as JwtSecurityToken;
                    Username = jsonToken?.Claims
                        .FirstOrDefault(c => c.Type == ClaimTypes.Name ||
                                            c.Type == "unique_name")?.Value
                        ?? "Пользователь";
                }
                catch
                {
                    Username = "Пользователь";
                }
            }
        }
    }
}
