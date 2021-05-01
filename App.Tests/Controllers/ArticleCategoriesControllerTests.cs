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
        readonly ApplicationDbContext _context;
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
            ArticleCategoriesController controller = new(_context);
            ViewResult result = await controller.Index() as ViewResult;
            Assert.NotNull(result);
        }

        [Fact]
        public async void IndexArticleCategoryViewDataNotNull()
        {
            ArticleCategoriesController controller = new(_context);
            ArticleCategory newArticleCategory = new()
            { 
                Name = "Category 1"
            };
            var createResult = await controller.Create(newArticleCategory) as RedirectToActionResult;
            Assert.NotNull(createResult);
            Assert.Equal("Index", createResult.ActionName);
            int entitiesCount = _context.SaveChanges();
            var articleCategory = await _context.ArticleCategories
                .FirstOrDefaultAsync(x => x.Name == "Category 1");
            Assert.NotNull(articleCategory);
            ViewResult indexResult = await controller.Index() as ViewResult;
            Assert.NotNull(indexResult);
            var categoryList = indexResult.Model as IEnumerable<ArticleCategory>;
            Assert.NotNull(categoryList);
            var category = categoryList.FirstOrDefault(x => x.Name == "Category 1");
            Assert.NotNull(category);
            Assert.Equal("Category 1", category.Name);
        }

        [Fact]
        public void CreateArticleCategoryViewResultNotNull()
        {
            ArticleCategoriesController controller = new(_context);
            var result = controller.Create() as ViewResult;
            Assert.NotNull(result);
        }

        [Fact]
        public async void CreateArticleCategoryViewDataNotNull()
        {
            ArticleCategoriesController controller = new(_context);
            ArticleCategory newArticleCategory = new() { Name = "Category 2" };
            var result = await controller.Create(newArticleCategory) as RedirectToActionResult;
            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName);
            int entitiesCount = _context.SaveChanges();
            var articleCategory = await _context.ArticleCategories
                .FirstOrDefaultAsync(m => m.Name == "Category 2");
            Assert.NotNull(articleCategory);
        }

        [Fact]
        public async void DetailsArticleCategoryViewResultNotNull()
        {
            ArticleCategoriesController controller = new(_context);
            ArticleCategory newArticleCategory = new() { Name = "Category 3" };
            var createResult = await controller.Create(newArticleCategory) as RedirectToActionResult;
            Assert.NotNull(createResult);
            Assert.Equal("Index", createResult.ActionName);
            int entitiesCount = _context.SaveChanges();
            var articleCategory = await _context.ArticleCategories
                .FirstOrDefaultAsync(m => m.Name == "Category 3");
            Assert.NotNull(articleCategory);
            var detailsResult = await controller.Details(articleCategory.Id) as ViewResult;
            Assert.NotNull(detailsResult);
            var articleCategoryDetails = detailsResult.Model as ArticleCategory;
            Assert.NotNull(articleCategoryDetails);
            Assert.Equal("Category 3", articleCategoryDetails.Name);
        }

        [Fact]
        public async void EditArticleCategoryViewResultNotNull()
        {
            ArticleCategoriesController controller = new(_context);
            ArticleCategory newArticleCategory = new()
            {
                Name = "Category 4"
            };
            var createResult = await controller.Create(newArticleCategory) as RedirectToActionResult;
            Assert.NotNull(createResult);
            Assert.Equal("Index", createResult.ActionName);
            int entitiesCount = _context.SaveChanges();
            var articleCategory = await _context.ArticleCategories
                .FirstOrDefaultAsync(m => m.Name == "Category 4");
            Assert.NotNull(articleCategory);
            var result = await controller.Edit(articleCategory.Id) as ViewResult;
            Assert.NotNull(result);
            var articleCategoryDetails = result.Model as ArticleCategory;
            Assert.NotNull(articleCategoryDetails);
            Assert.Equal("Category 4", articleCategoryDetails.Name);
        }

        [Fact]
        public async void EditArticleCategoryViewDataNotNull()
        {
            ArticleCategoriesController controller = new(_context);
            ArticleCategory newArticleCategory = new()
            {
                Name = "Category 5"
            };
            var createResult = await controller.Create(newArticleCategory) as RedirectToActionResult;
            Assert.NotNull(createResult);
            Assert.Equal("Index", createResult.ActionName);
            int entitiesCount = _context.SaveChanges();
            var articleCategory = await _context.ArticleCategories
                .FirstOrDefaultAsync(m => m.Name == "Category 5");
            Assert.NotNull(articleCategory);
            articleCategory.Name = "Category 6";
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
            ArticleCategoriesController controller = new(_context);
            ArticleCategory newArticleCategory = new() { Name = "Category 7" };
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
