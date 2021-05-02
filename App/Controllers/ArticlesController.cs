using App.Data;
using App.Models;
using App.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace App.Controllers
{
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
                .Include(x => x.Category)
                .Include(x => x.Author)
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
                .Include(x => x.Category)
                .Include(x => x.Author)
                .FirstOrDefaultAsync(x => x.Id == id);
            if (article == null)
            {
                return NotFound();
            }

            return View(article);
        }

        [Authorize]
        public IActionResult Create()
        {
            var model = new ArticleViewModel(_context.ArticleCategories.ToList());
            return View(model);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Name,Text,CategoryId")] ArticleViewModel model)
        {
            if (ModelState.IsValid)
            {
                _context.Add(ToArticle(model));
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var article = await _context.Articles
                .Include(x => x.Category)
                .Include(x => x.Author)
                .FirstOrDefaultAsync(x => x.Id == id);
            if (article == null)
            {
                return NotFound();
            }
            return View(ToArticleViewModel(article));
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("Id,Name,Text,CategoryId")] ArticleViewModel model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var article = ToArticle(model);
                    article.Id = id;
                    _context.Update(article);
                    _context.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ArticleExists(model.Id))
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
            return View(model);
        }

        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var article = await _context.Articles
                .FirstOrDefaultAsync(x => x.Id == id);
            if (article == null)
            {
                return NotFound();
            }

            return View(article);
        }

        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var article = await _context.Articles.FindAsync(id);
            _context.Articles.Remove(article);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        private bool ArticleExists(int id)
        {
            return _context.Articles.Any(e => e.Id == id);
        }

        private Article ToArticle(ArticleViewModel model)
        {
            var currentUser = _context.Users.Single(x => x.Email == User.Identity.Name);
            var categoryId = Convert.ToInt32(model.CategoryId);
            var category = _context.ArticleCategories.Single(x => x.Id == categoryId);
            return new Article
            {
                Author = currentUser,
                Category = category,
                CreationDate = DateTime.Now,
                Name = model.Name,
                Text = model.Text
            };
        }

        private ArticleViewModel ToArticleViewModel(Article article)
        {
            var model = new ArticleViewModel(_context.ArticleCategories.ToList());
            model.Id = article.Id;
            model.Name = article.Name;
            model.Text = article.Text;
            model.CategoryId = article.Category.Id.ToString();
            model.Author = article.Author.UserName;
            return model;
        }
    }
}
