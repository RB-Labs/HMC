using System;
using Xunit;
using App.Data;
using App.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;

namespace App.Tests.Models
{
    public class ArticleCategoryTests : IDisposable
    {

        ApplicationDbContext _context;

        public ArticleCategoryTests()
        {
            var serviceProvider = new ServiceCollection()
                .AddEntityFrameworkSqlServer()
                .BuildServiceProvider();

            var builder = new DbContextOptionsBuilder<ApplicationDbContext>();

            builder.UseInMemoryDatabase("ArticleCategoryTestDB");

            _context = new ApplicationDbContext(builder.Options);

        }

        [Fact]
        public async void CreateArticleCategoryTest()
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
        }

        [Fact]
        public async void UpdateArticleCategoryTest()
        {
            var addResult = _context.ArticleCategories.Add(new ArticleCategory
            {
                Name = "Category 2"
            });
            Assert.NotNull(addResult);
            int entitiesCount = _context.SaveChanges();
            Assert.Equal(1, entitiesCount);
            var articleCategory = await _context.ArticleCategories
                .FirstOrDefaultAsync(m => m.Name == "Category 2");
            Assert.NotNull(articleCategory);
            articleCategory.Name = "Category 3";
            var updateResult = _context.ArticleCategories.Update(articleCategory);
            Assert.NotNull(updateResult);
            entitiesCount = _context.SaveChanges();
            Assert.Equal(1, entitiesCount);
            articleCategory = await _context.ArticleCategories
                .FirstOrDefaultAsync(m => m.Name == "Category 3");
            Assert.NotNull(articleCategory);
        }

        [Fact]
        public async void DeleteArticleCategoryTest()
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
            var removeResult = _context.ArticleCategories.Remove(articleCategory);
            Assert.NotNull(removeResult);
            entitiesCount = _context.SaveChanges();
            Assert.Equal(1, entitiesCount);
            articleCategory = await _context.ArticleCategories
                .FirstOrDefaultAsync(m => m.Name == "Category 4");
            Assert.Null(articleCategory);
        }

        public void Dispose()
        {
            _context.Database.EnsureDeleted();
        }

    }
}
