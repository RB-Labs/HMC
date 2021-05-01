using App.Controllers;
using App.Data;
using App.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace App.Tests.Controllers
{
    [Collection("Sequential")]
    public class ArticleCategoriesControllerTests : IDisposable
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
            const string categoryName = "Controller Category 1";
            ArticleCategoriesController controller = new(_context);
            ArticleCategory newArticleCategory = new() { Name = categoryName };
            var createResult = controller.Create(newArticleCategory) as RedirectToActionResult;
            Assert.NotNull(createResult);
            Assert.Equal("Index", createResult.ActionName);
            var articleCategory = await _context.ArticleCategories
                .FirstOrDefaultAsync(x => x.Name == categoryName);
            Assert.NotNull(articleCategory);
            ViewResult indexResult = await controller.Index() as ViewResult;
            Assert.NotNull(indexResult);
            var categoryList = indexResult.Model as IEnumerable<ArticleCategory>;
            Assert.NotNull(categoryList);
            var category = categoryList.FirstOrDefault(x => x.Name == categoryName);
            Assert.NotNull(category);
            Assert.Equal(categoryName, category.Name);
        }

        [Fact]
        public void CreateArticleCategoryViewResultNotNull()
        {
            ArticleCategoriesController controller = new(_context);
            var result = controller.Create() as ViewResult;
            Assert.NotNull(result);
        }

        [Fact]
        public void CreateArticleCategoryViewDataNotNull()
        {
            const string categoryName = "Controller Category 2";
            ArticleCategoriesController controller = new(_context);
            ArticleCategory newArticleCategory = new() { Name = categoryName };
            var result = controller.Create(newArticleCategory) as RedirectToActionResult;
            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName);
            var articleCategory = _context.ArticleCategories
                .FirstOrDefault(x => x.Name == categoryName);
            Assert.NotNull(articleCategory);
        }

        [Fact]
        public async void DetailsArticleCategoryViewResultNotNull()
        {
            const string categoryName = "Controller Category 3";
            ArticleCategoriesController controller = new(_context);
            ArticleCategory newArticleCategory = new() { Name = categoryName };
            var createResult = controller.Create(newArticleCategory) as RedirectToActionResult;
            Assert.NotNull(createResult);
            Assert.Equal("Index", createResult.ActionName);
            var articleCategory = _context.ArticleCategories
                .FirstOrDefault(x => x.Name == categoryName);
            Assert.NotNull(articleCategory);
            var detailsResult = await controller.Details(articleCategory.Id) as ViewResult;
            Assert.NotNull(detailsResult);
            var articleCategoryDetails = detailsResult.Model as ArticleCategory;
            Assert.NotNull(articleCategoryDetails);
            Assert.Equal(categoryName, articleCategoryDetails.Name);
        }

        [Fact]
        public async void EditArticleCategoryViewResultNotNull()
        {
            const string categoryName = "Controller Category 4";
            ArticleCategoriesController controller = new(_context);
            ArticleCategory newArticleCategory = new() { Name = categoryName };
            var createResult = controller.Create(newArticleCategory) as RedirectToActionResult;
            Assert.NotNull(createResult);
            Assert.Equal("Index", createResult.ActionName);
            var articleCategory = _context.ArticleCategories
                .FirstOrDefault(x => x.Name == categoryName);
            Assert.NotNull(articleCategory);
            var result = await controller.Edit(articleCategory.Id) as ViewResult;
            Assert.NotNull(result);
            var articleCategoryDetails = result.Model as ArticleCategory;
            Assert.NotNull(articleCategoryDetails);
            Assert.Equal(categoryName, articleCategoryDetails.Name);
        }

        [Fact]
        public void EditArticleCategoryViewDataNotNull()
        {
            const string categoryName = "Controller Category 5";
            const string newCategoryName = "Controller Category 6";
            ArticleCategoriesController controller = new(_context);
            ArticleCategory newArticleCategory = new() { Name = categoryName };
            var createResult = controller.Create(newArticleCategory) as RedirectToActionResult;
            Assert.NotNull(createResult);
            Assert.Equal("Index", createResult.ActionName);
            var articleCategory = _context.ArticleCategories
                .FirstOrDefault(x => x.Name == categoryName);
            Assert.NotNull(articleCategory);
            articleCategory.Name = newCategoryName;
            var result = controller.Edit(articleCategory.Id, articleCategory) as RedirectToActionResult;
            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName);
            articleCategory = _context.ArticleCategories
                .FirstOrDefault(x => x.Name == categoryName);
            Assert.Null(articleCategory);
            articleCategory = _context.ArticleCategories
                .FirstOrDefault(x => x.Name == newCategoryName);
            Assert.NotNull(articleCategory);
        }

        [Fact]
        public async void DeleteArticleCategoryViewResultNotNull()
        {
            const string categoryName = "Controller Category 7";
            ArticleCategoriesController controller = new(_context);
            ArticleCategory newArticleCategory = new() { Name = categoryName };
            var result = controller.Create(newArticleCategory) as RedirectToActionResult;
            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName);
            var articleCategory = _context.ArticleCategories
                .FirstOrDefault(x => x.Name == categoryName);
            Assert.NotNull(articleCategory);
            result = await controller.DeleteConfirmed(articleCategory.Id) as RedirectToActionResult;
            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName);
            articleCategory = _context.ArticleCategories
                .FirstOrDefault(x => x.Name == categoryName);
            Assert.Null(articleCategory);
        }

        public void Dispose()
        {
            _context.Database.EnsureDeleted();
        }
    }
}
