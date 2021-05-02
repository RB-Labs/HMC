using App.Data;
using App.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using Xunit;

namespace App.Tests.Models
{
    [Collection("Sequential")]
    public class ArticleTests : IDisposable
    {
        readonly ApplicationDbContext _context;
        private UserManager<User> _userManager;

        private const string adminEmail = "root@gmail.com";
        private const string adminName = "root";
        private const string adminPassword = "123456";

        public ArticleTests()
        {
            IServiceCollection services = new ServiceCollection();
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseInMemoryDatabase("ArticleTestDB")
            );
            _context = services.BuildServiceProvider()
                .GetService<ApplicationDbContext>();
            _context.Database.EnsureCreated();

            services.AddIdentityCore<User>(options =>
                {
                    options.Password.RequiredLength = 6;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequireLowercase = false;
                    options.Password.RequireUppercase = false;
                    options.Password.RequireDigit = false;
                    options.SignIn.RequireConfirmedAccount = false;
                    options.SignIn.RequireConfirmedEmail = false;
                    options.SignIn.RequireConfirmedPhoneNumber = false;
                })
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddUserManager<UserManager<User>>();

            _userManager = services.BuildServiceProvider()
                .GetService<UserManager<User>>();

            User admin = new User { UserName = adminName, Email = adminEmail };
            var result = _userManager.CreateAsync(admin, adminPassword);
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

        private Article CreateArticle(string articleName, string articleText, ArticleCategory articleCategory)
        {
            var admin = _context.Users.Single(x => x.UserName == adminName);
            Assert.NotNull(admin);
            var addArticleResult = _context.Articles.Add(new Article
            {
                Author = admin,
                Category = articleCategory,
                CreationDate = DateTime.Now,
                Name = articleName,
                Text = articleText
            });
            Assert.NotNull(addArticleResult);
            int entitiesCount = _context.SaveChanges();
            Assert.Equal(1, entitiesCount);
            return _context.Articles
                .FirstOrDefault(m => m.Name == articleName);
        }

        [Fact]
        public void CreateArticleTest()
        {
            const string categoryName = "Category 1";
            const string articleName = "Article 1";
            const string articleText = "Article 1 Text";
            var articleCategory = CreateArcticleCategory(categoryName);
            Assert.NotNull(articleCategory);
            var newArticle = CreateArticle(articleName, articleText, articleCategory);
            Assert.NotNull(newArticle);
            Assert.Equal(articleName, newArticle.Name);
            Assert.Equal(articleText, newArticle.Text);
            Assert.Equal(categoryName, newArticle.Category.Name);
            Assert.Equal(adminName, newArticle.Author.UserName);
        }

        [Fact]
        public void UpdateArticleTest()
        {
            const string categoryName = "Category 2";
            const string articleName = "Article 2";
            const string newArticleName = "New Article 2";
            const string articleText = "Article 2 Text";
            var articleCategory = CreateArcticleCategory(categoryName);
            Assert.NotNull(articleCategory);
            var newArticle = CreateArticle(articleName, articleText, articleCategory);
            Assert.NotNull(newArticle);
            newArticle.Name = newArticleName;
            var updateArticleResult = _context.Articles.Update(newArticle);
            Assert.NotNull(updateArticleResult);
            var entitiesCount = _context.SaveChanges();
            Assert.Equal(1, entitiesCount);
            var updatedArticle = _context.Articles
                .FirstOrDefault(m => m.Name == newArticleName);
            Assert.NotNull(updatedArticle);
            Assert.Equal(articleText, newArticle.Text);
            Assert.Equal(categoryName, newArticle.Category.Name);
            Assert.Equal(adminName, newArticle.Author.UserName);
        }

        [Fact]
        public async void DeleteArticleTest()
        {
            const string categoryName = "Category 3";
            const string articleName = "Article 3";
            const string articleText = "Article 3 Text";
            var articleCategory = CreateArcticleCategory(categoryName);
            Assert.NotNull(articleCategory);
            var newArticle = CreateArticle(articleName, articleText, articleCategory);
            Assert.NotNull(newArticle);
            var removeArticleResult = _context.Articles.Remove(newArticle);
            Assert.NotNull(removeArticleResult);
            var entitiesCount = _context.SaveChanges();
            Assert.Equal(1, entitiesCount);
            var deletedArticle = await _context.Articles
                .FirstOrDefaultAsync(m => m.Name == articleName);
            Assert.Null(deletedArticle);
        }

        public void Dispose()
        {
            _context.Database.EnsureDeleted();
        }
    }
}
