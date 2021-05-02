using App.Controllers;
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
using Xunit;

namespace App.Tests.Controllers
{
    [Collection("Sequential")]
    public class ArticleCategoriesControllerTests : IDisposable
    {
        readonly ApplicationDbContext _context;
        private readonly ArticleCategoriesController _articleCategoriesController;
        private UserManager<User> _userManager;

        private const string adminEmail = "root@gmail.com";
        private const string adminName = "root";
        private const string adminPassword = "123456";
        public ArticleCategoriesControllerTests()
        {
            IServiceCollection services = new ServiceCollection();
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseInMemoryDatabase("ArticleCategoriesTestDB")
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
            var currentUser = _context.Users.Single(x => x.UserName == admin.UserName);

            ControllerContext controllerContext = null;
            if (result.Result.Succeeded)
            {
                var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.NameIdentifier, adminName),
                    new Claim(ClaimTypes.Name, adminEmail)
                }));
                controllerContext = new ControllerContext()
                {
                    HttpContext = new DefaultHttpContext { User = user }
                };
            }

            _articleCategoriesController = new ArticleCategoriesController(_context);
            _articleCategoriesController.ControllerContext = controllerContext;
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
        public async void IndexArticleCategoryViewResultNotNull()
        {
            ViewResult result = await _articleCategoriesController.Index() as ViewResult;
            Assert.NotNull(result);
        }

        [Fact]
        public async void IndexArticleCategoryViewDataNotNull()
        {
            const string categoryName = "Category 1";
            var newArticleCategory = CreateArticleCategory(categoryName);
            Assert.NotNull(newArticleCategory);
            ViewResult indexResult = await _articleCategoriesController.Index() as ViewResult;
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
        public async void DetailsArticleCategoryViewResultNotNull()
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
        public async void EditArticleCategoryViewResultNotNull()
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
        public async void DeleteArticleCategoryViewResultNotNull()
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
        public async void DeleteConfirmedArticleCategoryViewResultNotNull()
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

        public void Dispose()
        {
            _context.Database.EnsureDeleted();
        }
    }
}
