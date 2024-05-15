using Microsoft.AspNetCore.Mvc;

namespace AppMvc.Net.Controllers {
    public class FirstController : Controller {
        private readonly ILogger<FirstController> _logger;
        public FirstController(ILogger<FirstController> logger) {
            _logger = logger;
        }
        public string Index() {
            _logger.LogInformation("Index action");
            return "Toi la Index cua First";
        }

        public IActionResult HelloView(string username) {
            if (string.IsNullOrEmpty(username)) {
                username = "khach";
            }
            return View();
        }
    }
}