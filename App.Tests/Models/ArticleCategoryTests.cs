using App.Data;
using App.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using Xunit;

namespace App.Tests.Models
{
    [Collection("Sequential")]
    public class ArticleCategoryTests : IDisposable
    {
        readonly ApplicationDbContext _context;

        public ArticleCategoryTests()
        {
            IServiceCollection services = new ServiceCollection();
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseInMemoryDatabase("ArticleTestDB")
            );
            _context = services.BuildServiceProvider()
                .GetService<ApplicationDbContext>();
            _context.Database.EnsureCreated();
        }

        private ArticleCategory CreateArcticleCategory(string categoryName)
        {
            var addCategoryResult = _context.ArticleCategories.Add(new ArticleCategory
            {
                Name = categoryName
            });
            Assert.NotNull(addCategoryResult);
            int entitiesCount = _context.SaveChanges();
            Assert.Equal(1, entitiesCount);
            return _context.ArticleCategories
                .FirstOrDefault(m => m.Name == categoryName);
        }

        [Fact]
        public void CreateArticleCategoryTest()
        {
            const string categoryName = "Category 1";
            var articleCategory = CreateArcticleCategory(categoryName);
            Assert.NotNull(articleCategory);
        }

        [Fact]
        public void UpdateArticleCategoryTest()
        {
            const string categoryName = "Category 2";
            const string newCategoryName = "New Category 2";
            var newArticleCategory = CreateArcticleCategory(categoryName);
            Assert.NotNull(newArticleCategory);
            newArticleCategory.Name = newCategoryName;
            var updateArticleCategoryResult = _context.ArticleCategories.Update(newArticleCategory);
            Assert.NotNull(updateArticleCategoryResult);
            var entitiesCount = _context.SaveChanges();
            Assert.Equal(1, entitiesCount);
            var updatedArticleCategory = _context.ArticleCategories
                .FirstOrDefault(m => m.Name == newCategoryName);
            Assert.NotNull(updatedArticleCategory);
        }

        [Fact]
        public async void DeleteArticleCategoryTest()
        {
            const string categoryName = "Category 3";
            var newArticleCategory = CreateArcticleCategory(categoryName);
            Assert.NotNull(newArticleCategory);
            var removeArticleCategoryResult = _context.ArticleCategories.Remove(newArticleCategory);
            Assert.NotNull(removeArticleCategoryResult);
            var entitiesCount = _context.SaveChanges();
            Assert.Equal(1, entitiesCount);
            var deletedArticleCategory = await _context.ArticleCategories
                .FirstOrDefaultAsync(m => m.Name == categoryName);
            Assert.Null(deletedArticleCategory);
        }

        public void Dispose()
        {
            _context.Database.EnsureDeleted();
        }

    }
}
