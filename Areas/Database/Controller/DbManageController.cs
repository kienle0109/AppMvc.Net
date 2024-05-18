using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using razorweb.models;

namespace AppMvc.Net.Areas.Database.Controllers
{
    [Area("Database")]
    [Route("/database-manage/[action]")]
    public class DbManageController : Controller
    {
        private readonly AppDbContext _dbContext;

        public DbManageController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        // GET: DbManageController
        public ActionResult Index()
        {
            return View();
        }
        [TempData]
        public string StatusMessage {set; get;}
        [HttpGet]
        public IActionResult DeleteDb() {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> DeleteDbAsync() {

            var success = await _dbContext.Database.EnsureDeletedAsync();

            StatusMessage = success ? "Xoa thanh cong" : "Khong xoa duoc";

            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        public async Task<IActionResult> Migrate() {

            await _dbContext.Database.MigrateAsync();

            StatusMessage = "Tao thanh cong";

            return RedirectToAction(nameof(Index));
        }
    }
}
