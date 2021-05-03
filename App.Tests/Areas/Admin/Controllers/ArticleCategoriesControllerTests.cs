using App.Areas.Admin.Controllers;
using App.Data;
using App.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Xunit;

namespace App.Tests.Areas.Admin.Controllers
{
    [Collection("Sequential")]
    public class ArticleCategoriesControllerTests : IDisposable
    {
        private readonly ApplicationDbContext _context;
        private readonly ArticleCategoriesController _articleCategoriesController;
        private readonly UserManager<User> _userManager;

        private const string adminEmail = "root@gmail.com";
        private const string adminName = "root";
        private const string adminPassword = "123456";
        public ArticleCategoriesControllerTests()
        {
            var services = new ServiceCollection();
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseInMemoryDatabase("ArticleCategoriesTestDB_Admin")
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

            var admin = new User
            {
                UserName = adminName,
                Email = adminEmail
            };

            var result = _userManager.CreateAsync(admin, adminPassword);

            var user = new ClaimsPrincipal(
                new ClaimsIdentity(
                    new Claim[]
                    {
                        new Claim(ClaimTypes.NameIdentifier, adminName),
                        new Claim(ClaimTypes.Name, adminEmail)
                    }));

            var controllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = user }
            };

            _articleCategoriesController = new ArticleCategoriesController(_context)
            {
                ControllerContext = controllerContext
            };
        }

        private ArticleCategory CreateArticleCategory(string categoryName)
        {
            var newArticleCategory = new ArticleCategory { Name = categoryName };
            var createArticleCategoryResult = _articleCategoriesController.Create(newArticleCategory) as RedirectToActionResult;
            Assert.NotNull(createArticleCategoryResult);
            Assert.Equal("Index", createArticleCategoryResult.ActionName);
            return _context.ArticleCategories
                .FirstOrDefault(x => x.Name == categoryName);
        }

        [Fact]
        public async Task IndexArticleCategoryViewResultNotNull()
        {
            var result = await _articleCategoriesController.Index() as ViewResult;
            Assert.NotNull(result);
        }

        [Fact]
        public async Task IndexArticleCategoryViewDataNotNull()
        {
            const string categoryName = "Category 1";
            var newArticleCategory = CreateArticleCategory(categoryName);
            Assert.NotNull(newArticleCategory);
            var indexResult = await _articleCategoriesController.Index() as ViewResult;
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
            var result = _articleCategoriesController.Create() as ViewResult;
            Assert.NotNull(result);
        }

        [Fact]
        public void CreateArticleCategoryViewDataNotNull()
        {
            const string categoryName = "Category 2";
            var newArticleCategory = CreateArticleCategory(categoryName);
            Assert.NotNull(newArticleCategory);
        }

        [Fact]
        public async Task DetailsArticleCategoryViewResultNotNull()
        {
            const string categoryName = "Category 3";
            var newArticleCategory = CreateArticleCategory(categoryName);
            Assert.NotNull(newArticleCategory);
            var articleCategory = _context.ArticleCategories
               .FirstOrDefault(x => x.Name == categoryName);
            var detailsResult = await _articleCategoriesController.Details(articleCategory.Id) as ViewResult;
            Assert.NotNull(detailsResult);
            var articleCategoryDetails = detailsResult.Model as ArticleCategory;
            Assert.NotNull(articleCategoryDetails);
            Assert.Equal(categoryName, articleCategoryDetails.Name);
        }

        [Fact]
        public async Task EditArticleCategoryViewResultNotNull()
        {
            const string categoryName = "Controller Category 4";
            var newArticleCategory = CreateArticleCategory(categoryName);
            Assert.NotNull(newArticleCategory);
            var articleCategory = _context.ArticleCategories
                .FirstOrDefault(x => x.Name == categoryName);
            Assert.NotNull(articleCategory);
            var editResult = await _articleCategoriesController.Edit(articleCategory.Id) as ViewResult;
            Assert.NotNull(editResult);
            var articleCategoryDetails = editResult.Model as ArticleCategory;
            Assert.NotNull(articleCategoryDetails);
            Assert.Equal(categoryName, articleCategoryDetails.Name);
        }

        [Fact]
        public void EditArticleCategoryViewDataNotNull()
        {
            const string categoryName = "Category 5";
            const string newCategoryName = "Category 6";
            var newArticleCategory = CreateArticleCategory(categoryName);
            Assert.NotNull(newArticleCategory);
            var articleCategory = _context.ArticleCategories
                .FirstOrDefault(x => x.Name == categoryName);
            Assert.NotNull(articleCategory);
            articleCategory.Name = newCategoryName;
            var result = _articleCategoriesController.Edit(articleCategory.Id, articleCategory) as RedirectToActionResult;
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
        public async Task DeleteArticleCategoryViewResultNotNull()
        {
            const string categoryName = "Category 6";
            var newArticleCategory = CreateArticleCategory(categoryName);
            Assert.NotNull(newArticleCategory);
            var articleCategory = _context.ArticleCategories
                .FirstOrDefault(x => x.Name == categoryName);
            var deleteResult = await _articleCategoriesController.Delete(articleCategory.Id) as ViewResult;
            Assert.NotNull(deleteResult);
            var articleCategoryDetails = deleteResult.Model as ArticleCategory;
            Assert.NotNull(articleCategoryDetails);
            Assert.Equal(categoryName, articleCategoryDetails.Name);
        }

        [Fact]
        public async Task DeleteConfirmedArticleCategoryViewResultNotNull()
        {
            const string categoryName = "Category 7";
            var newArticleCategory = CreateArticleCategory(categoryName);
            Assert.NotNull(newArticleCategory);
            var articleCategory = _context.ArticleCategories
                .FirstOrDefault(x => x.Name == categoryName);
            Assert.NotNull(articleCategory);
            var deleteResult = await _articleCategoriesController.DeleteConfirmed(articleCategory.Id) as RedirectToActionResult;
            Assert.NotNull(deleteResult);
            Assert.Equal("Index", deleteResult.ActionName);
            var deletedArticleCategory = _context.ArticleCategories
                .FirstOrDefault(x => x.Name == categoryName);
            Assert.Null(deletedArticleCategory);
        }

#pragma warning disable CA1816
        public void Dispose() => _context.Database.EnsureDeleted();
#pragma warning restore CA1816
    }
}
