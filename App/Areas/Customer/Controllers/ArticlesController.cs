using App.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace App.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class ArticlesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ArticlesController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var articles = await _context.Articles
                .Include(x => x.Author)
                .Include(x => x.Category)
                .ToListAsync();
            return View(articles);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var article = await _context.Articles
                .Include(x => x.Author)
                .Include(x => x.Category)
                .FirstOrDefaultAsync(x => x.Id == id);
            if (article == null)
            {
                return NotFound();
            }

            return View(article);
        }
    }
}
