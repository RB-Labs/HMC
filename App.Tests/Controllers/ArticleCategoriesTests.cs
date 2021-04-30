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
    public class ArticleCategoriesTests
    {
        ApplicationDbContext _context;
        public ArticleCategoriesTests()
        {
            var serviceProvider = new ServiceCollection()
                .AddEntityFrameworkSqlServer()
                .BuildServiceProvider();

            var builder = new DbContextOptionsBuilder<ApplicationDbContext>();

            builder.UseInMemoryDatabase("ArticleCategoryTestDB");

            _context = new ApplicationDbContext(builder.Options);
        }

        [Fact]
        public async void IndexViewResultNotNull()
        {
            ArticleCategoriesController controller = new ArticleCategoriesController(_context);
            ViewResult result = await controller.Index() as ViewResult;
            Assert.NotNull(result);
        }

        [Fact]
        public async void IndexViewDataNotNull()
        {
            var addResult = _context.ArticleCategories.Add(new ArticleCategory
            {
                Name = "Category 1"
            });
            Assert.NotNull(addResult);
            int entitiesCount = _context.SaveChanges();
            Assert.Equal(1, entitiesCount);
            var articleCategory = await _context.ArticleCategories
                .FirstOrDefaultAsync(m => m.Name == "Category 1");
            Assert.NotNull(articleCategory);
            ArticleCategoriesController controller = new ArticleCategoriesController(_context);
            ViewResult result = await controller.Index() as ViewResult;
            Assert.NotNull(result);
            var categoryList = result.Model as IEnumerable<ArticleCategory>;
            Assert.NotNull(categoryList);
            var category = categoryList.First();
            Assert.NotNull(category);
            Assert.Equal("Category 1", category.Name);
        }
    }
}
