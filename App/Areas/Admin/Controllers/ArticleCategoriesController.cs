using App.Data;
using App.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace App.Areas.Admin.Controllers
{

    [Area("Admin")]
    [Authorize(Roles = UserRole.Internal)]
    public class ArticleCategoriesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ArticleCategoriesController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.ArticleCategories.ToListAsync());
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Id,Name")] ArticleCategory articleCategory)
        {
            if (ModelState.IsValid)
            {
                _context.Add(articleCategory);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(articleCategory);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var articleCategory = await _context.ArticleCategories.FindAsync(id);
            if (articleCategory == null)
            {
                return NotFound();
            }
            return View(articleCategory);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("Id,Name")] ArticleCategory articleCategory)
        {
            if (id != articleCategory.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(articleCategory);
                    _context.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ArticleCategoryExists(articleCategory.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(articleCategory);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var articleCategory = await _context.ArticleCategories
                .FirstOrDefaultAsync(m => m.Id == id);
            if (articleCategory == null)
            {
                return NotFound();
            }

            return View(articleCategory);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var articleCategory = await _context.ArticleCategories.FindAsync(id);
            _context.ArticleCategories.Remove(articleCategory);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        private bool ArticleCategoryExists(int id)
        {
            return _context.ArticleCategories.Any(e => e.Id == id);
        }
    }
}
