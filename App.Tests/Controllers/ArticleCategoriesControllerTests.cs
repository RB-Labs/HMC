using App.Controllers;
using App.Data;
using App.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace App.Tests.Controllers
{
    public class ArticleCategoriesControllerTests
    {
        ApplicationDbContext _context;
        public ArticleCategoriesControllerTests()
        {
            var serviceProvider = new ServiceCollection()
                .AddEntityFrameworkSqlServer()
                .BuildServiceProvider();

            var builder = new DbContextOptionsBuilder<ApplicationDbContext>();

            builder.UseInMemoryDatabase("ArticleCategoryTestDB");

            _context = new ApplicationDbContext(builder.Options);
        }

        [Fact]
        public async void IndexArticleCategoryViewResultNotNull()
        {
            ArticleCategoriesController controller = new ArticleCategoriesController(_context);
            ViewResult result = await controller.Index() as ViewResult;
            Assert.NotNull(result);
        }

        [Fact]
        public async void IndexArticleCategoryViewDataNotNull()
        {
            var addResult = _context.ArticleCategories.Add(new ArticleCategory
            {
                Name = "Category 1"
            });
            Assert.NotNull(addResult);
            int entitiesCount = _context.SaveChanges();
            Assert.Equal(1, entitiesCount);
            var articleCategory = await _context.ArticleCategories
                .FirstOrDefaultAsync(x => x.Name == "Category 1");
            Assert.NotNull(articleCategory);
            ArticleCategoriesController controller = new ArticleCategoriesController(_context);
            ViewResult result = await controller.Index() as ViewResult;
            Assert.NotNull(result);
            var categoryList = result.Model as IEnumerable<ArticleCategory>;
            Assert.NotNull(categoryList);
            var category = categoryList.FirstOrDefault(x => x.Name == "Category 1");
            Assert.NotNull(category);
            Assert.Equal("Category 1", category.Name);
        }

        [Fact]
        public void CreateArticleCategoryViewResultNotNull()
        {
            ArticleCategoriesController controller = new ArticleCategoriesController(_context);
            var result = controller.Create() as ViewResult;
            Assert.NotNull(result);
        }

        [Fact]
        public async void CreateArticleCategoryViewDataNotNull()
        {
            ArticleCategoriesController controller = new ArticleCategoriesController(_context);
            ArticleCategory newArticleCategory = new ArticleCategory { Name = "Category 2" };
            var result = await controller.Create(newArticleCategory) as RedirectToActionResult;
            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName);
            var articleCategory = await _context.ArticleCategories
                .FirstOrDefaultAsync(m => m.Name == "Category 2");
            Assert.NotNull(articleCategory);
        }

        [Fact]
        public async void DetailsArticleCategoryViewResultNotNull()
        {
            var addResult = _context.ArticleCategories.Add(new ArticleCategory
            {
                Name = "Category 3"
            });
            Assert.NotNull(addResult);
            int entitiesCount = _context.SaveChanges();
            Assert.Equal(1, entitiesCount);
            var articleCategory = await _context.ArticleCategories
                .FirstOrDefaultAsync(m => m.Name == "Category 3");
            Assert.NotNull(articleCategory);
            ArticleCategoriesController controller = new ArticleCategoriesController(_context);
            var result = await controller.Details(articleCategory.Id) as ViewResult;
            Assert.NotNull(result);
            var articleCategoryDetails = result.Model as ArticleCategory;
            Assert.NotNull(articleCategoryDetails);
            Assert.Equal("Category 3", articleCategoryDetails.Name);
        }

        [Fact]
        public async void EditArticleCategoryViewResultNotNull()
        {
            var addResult = _context.ArticleCategories.Add(new ArticleCategory
            {
                Name = "Category 4"
            });
            Assert.NotNull(addResult);
            int entitiesCount = _context.SaveChanges();
            Assert.Equal(1, entitiesCount);
            var articleCategory = await _context.ArticleCategories
                .FirstOrDefaultAsync(m => m.Name == "Category 4");
            Assert.NotNull(articleCategory);
            ArticleCategoriesController controller = new ArticleCategoriesController(_context);
            var result = await controller.Edit(articleCategory.Id) as ViewResult;
            Assert.NotNull(result);
            var articleCategoryDetails = result.Model as ArticleCategory;
            Assert.NotNull(articleCategoryDetails);
            Assert.Equal("Category 4", articleCategoryDetails.Name);
        }

        [Fact]
        public async void EditArticleCategoryViewDataNotNull()
        {
            var addResult = _context.ArticleCategories.Add(new ArticleCategory
            {
                Name = "Category 5"
            });
            Assert.NotNull(addResult);
            int entitiesCount = _context.SaveChanges();
            Assert.Equal(1, entitiesCount);
            var articleCategory = await _context.ArticleCategories
                .FirstOrDefaultAsync(m => m.Name == "Category 5");
            Assert.NotNull(articleCategory);
            articleCategory.Name = "Category 6";
            ArticleCategoriesController controller = new ArticleCategoriesController(_context);
            var result = await controller.Edit(articleCategory.Id, articleCategory) as RedirectToActionResult;
            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName);
            articleCategory = await _context.ArticleCategories
                .FirstOrDefaultAsync(m => m.Name == "Category 5");
            Assert.Null(articleCategory);
            articleCategory = await _context.ArticleCategories
                .FirstOrDefaultAsync(m => m.Name == "Category 6");
            Assert.NotNull(articleCategory);
        }

        [Fact]
        public async void DeleteArticleCategoryViewResultNotNull()
        {
            ArticleCategoriesController controller = new ArticleCategoriesController(_context);
            ArticleCategory newArticleCategory = new ArticleCategory { Name = "Category 7" };
            var result = await controller.Create(newArticleCategory) as RedirectToActionResult;
            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName);
            var articleCategory = await _context.ArticleCategories
                .FirstOrDefaultAsync(m => m.Name == "Category 7");
            Assert.NotNull(articleCategory);
            result = await controller.DeleteConfirmed(articleCategory.Id) as RedirectToActionResult;
            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName);
            articleCategory = await _context.ArticleCategories
                .FirstOrDefaultAsync(x => x.Name == "Category 7");
            Assert.Null(articleCategory);
        }
    }
}
