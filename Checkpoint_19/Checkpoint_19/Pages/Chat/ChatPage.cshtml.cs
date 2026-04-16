using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Checkpoint_19.Chat
{
    [Authorize]
    public class ChatPageModel : PageModel
    {
        public void OnGet()
        {
        }
    }
}
