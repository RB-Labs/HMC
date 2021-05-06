using App.Data;
using App.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Linq;

namespace App.Controllers
{
    public class HomeController : Controller
    {
        const int _lastNewsCount = 5;
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            if (User.IsInRole(UserRole.Admin) || User.IsInRole(UserRole.Manager))
            {
                return RedirectToAction("Index", new { Area = "Admin" });
            }
            else if (User.IsInRole(UserRole.Customer))
            {
                return RedirectToAction("Index", new { Area = "Customer" });
            }
            var articles = _context.Articles
                .Include(x => x.Category)
                .OrderByDescending(x => x.CreationDate)
                .Take(_lastNewsCount)
                .ToList();
            return View(articles);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
