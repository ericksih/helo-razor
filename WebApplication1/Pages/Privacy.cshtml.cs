using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApplication1.Pages
{
    public class PrivacyModel : PageModel
    {
        [BindProperty]
        public string Recipient { get; set; }

        [BindProperty]
        public string Message { get; set; }

        [BindProperty]
        public bool Accepted { get; set; }

        private readonly ILogger<PrivacyModel> _logger;

        public PrivacyModel(ILogger<PrivacyModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
        }
    }

}
