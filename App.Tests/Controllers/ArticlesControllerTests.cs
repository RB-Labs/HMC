using App.Controllers;
using App.Data;
using App.Models;
using App.ViewModels;
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
    public class ArticlesControllerTests : IDisposable
    {
        private readonly ApplicationDbContext _context;
        private readonly ArticlesController _articlesController;
        private readonly ArticleCategoriesController _articleCategoriesController;
        private UserManager<User> _userManager;

        private const string adminEmail = "root@gmail.com";
        private const string adminName = "root";
        private const string adminPassword = "123456";
        public ArticlesControllerTests()
        {
            IServiceCollection services = new ServiceCollection();
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseInMemoryDatabase("ArticlesTestDB")
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

            _articlesController = new ArticlesController(_context);
            _articlesController.ControllerContext = controllerContext;

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

        private ArticleViewModel CreateArticleViewModel(string articleName, string articleText, ArticleCategory articleCategory)
        {
            ArticleViewModel articleViewModel = new ArticleViewModel(_context.ArticleCategories.ToList());
            articleViewModel.Name = articleName;
            articleViewModel.Text = articleText;
            articleViewModel.CategoryId = articleCategory.Id.ToString();
            return articleViewModel;
        }

        private Article CreateArticle(string articleName, string articleText, ArticleCategory articleCategory)
        {
            ArticleViewModel articleViewModel = CreateArticleViewModel(articleName, articleText, articleCategory);
            var createArticleResult = _articlesController.Create(articleViewModel) as RedirectToActionResult;
            Assert.NotNull(createArticleResult);
            Assert.Equal("Index", createArticleResult.ActionName);
            return _context.Articles
                .FirstOrDefault(x => x.Name == articleName);
        }

        [Fact]
        public async void IndexArticleViewResultNotNull()
        {
            ViewResult result = await _articlesController.Index() as ViewResult;
            Assert.NotNull(result);
        }

        [Fact]
        public async void IndexArticleViewDataNotNull()
        {
            const string categoryName = "Category 1";
            const string articleName = "Article 1";
            const string articleText = "Article 1 Text";
            var articleCategory = CreateArticleCategory(categoryName);
            Assert.NotNull(articleCategory);
            var newArticle = CreateArticle(articleName, articleText, articleCategory);
            Assert.NotNull(newArticle);
            ViewResult indexResult = await _articlesController.Index() as ViewResult;
            Assert.NotNull(indexResult);
            var articleList = indexResult.Model as IEnumerable<Article>;
            Assert.NotNull(articleList);
            var article = articleList.FirstOrDefault(x => x.Name == articleName);
            Assert.NotNull(article);
            Assert.Equal(articleName, article.Name);
            Assert.Equal(articleText, article.Text);
            Assert.Equal(categoryName, article.Category.Name);
            Assert.Equal(adminName, article.Author.UserName);
        }

        [Fact]
        public void CreateArticleViewResultNotNull()
        {
            var result = _articlesController.Create() as ViewResult;
            Assert.NotNull(result);
        }

        [Fact]
        public void CreateArticleViewDataNotNull()
        {
            const string categoryName = "Category 2";
            const string articleName = "Article 2";
            const string articleText = "Article 2 Text";
            var articleCategory = CreateArticleCategory(categoryName);
            Assert.NotNull(articleCategory);
            var newArticle = CreateArticle(articleName, articleText, articleCategory);
            Assert.NotNull(newArticle);
        }

        [Fact]
        public async void DetailsArticleViewResultNotNull()
        {
            const string categoryName = "Category 3";
            const string articleName = "Article 3";
            const string articleText = "Article 3 Text";
            var articleCategory = CreateArticleCategory(categoryName);
            Assert.NotNull(articleCategory);
            var newArticle = CreateArticle(articleName, articleText, articleCategory);
            Assert.NotNull(newArticle);
            var detailsResult = await _articlesController.Details(articleCategory.Id) as ViewResult;
            Assert.NotNull(detailsResult);
            var articleDetails = detailsResult.Model as Article;
            Assert.NotNull(articleDetails);
            Assert.Equal(articleName, articleDetails.Name);
            Assert.Equal(articleText, articleDetails.Text);
            Assert.Equal(categoryName, articleDetails.Category.Name);
            Assert.Equal(adminName, articleDetails.Author.UserName);
        }

        [Fact]
        public async void EditArticleViewResultNotNull()
        {
            const string categoryName = "Category 4";
            const string articleName = "Article 4";
            const string articleText = "Article 4 Text";
            var articleCategory = CreateArticleCategory(categoryName);
            Assert.NotNull(articleCategory);
            var newArticle = CreateArticle(articleName, articleText, articleCategory);
            Assert.NotNull(newArticle);
            var result = await _articlesController.Edit(articleCategory.Id) as ViewResult;
            Assert.NotNull(result);
            var articleModel = result.Model as ArticleViewModel;
            Assert.NotNull(articleModel);
            var categoryId = Convert.ToInt32(articleModel.CategoryId);
            var category = _context.ArticleCategories.Single(x => x.Id == categoryId);
            Assert.NotNull(category);
            Assert.Equal(articleName, articleModel.Name);
            Assert.Equal(articleText, articleModel.Text);
            Assert.Equal(categoryName, category.Name);
            Assert.Equal(adminName, articleModel.Author);
        }

        [Fact]
        public void EditArticleViewDataNotNull()
        {
            const string categoryName = "Category 5";
            const string articleName = "Article 5";
            const string newArticleName = "Article 6";
            const string articleText = "Article 5 Text";
            var articleCategory = CreateArticleCategory(categoryName);
            Assert.NotNull(articleCategory);
            var newArticle = CreateArticle(articleName, articleText, articleCategory);
            Assert.NotNull(newArticle);
            var newArticleModel = CreateArticleViewModel(newArticleName, articleText, articleCategory);
            newArticleModel.Id = newArticle.Id;
            _context.Entry(newArticle).State = EntityState.Detached;
            var result = _articlesController.Edit(newArticle.Id, newArticleModel) as RedirectToActionResult;
            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName);
            var oldArtile = _context.Articles
                .FirstOrDefault(x => x.Name == articleName);
            Assert.Null(oldArtile);
            var editedArticle = _context.Articles
                .FirstOrDefault(x => x.Name == newArticleName);
            Assert.NotNull(editedArticle);
            Assert.Equal(newArticleName, editedArticle.Name);
            Assert.Equal(articleText, editedArticle.Text);
            Assert.Equal(categoryName, editedArticle.Category.Name);
            Assert.Equal(adminName, editedArticle.Author.UserName);
        }

        [Fact]
        public async void DeleteArticleViewResultNotNull()
        {
            const string categoryName = "Category 6";
            const string articleName = "Article 6";
            const string articleText = "Article 6 Text";
            var articleCategory = CreateArticleCategory(categoryName);
            Assert.NotNull(articleCategory);
            var newArticle = CreateArticle(articleName, articleText, articleCategory);
            Assert.NotNull(newArticle);
            var result = await _articlesController.Delete(newArticle.Id) as ViewResult;
            Assert.NotNull(result);
            var articleDetails = result.Model as Article;
            Assert.NotNull(articleDetails);
            Assert.Equal(articleName, articleDetails.Name);
            Assert.Equal(articleText, articleDetails.Text);
            Assert.Equal(categoryName, articleDetails.Category.Name);
            Assert.Equal(adminName, articleDetails.Author.UserName);
        }

        [Fact]
        public async void DeleteConfirmedArticleViewResultNotNull()
        {
            const string categoryName = "Category 7";
            const string articleName = "Article 7";
            const string articleText = "Article 7 Text";
            var articleCategory = CreateArticleCategory(categoryName);
            Assert.NotNull(articleCategory);
            var newArticle = CreateArticle(articleName, articleText, articleCategory);
            Assert.NotNull(newArticle);
            var result = await _articlesController.DeleteConfirmed(newArticle.Id) as RedirectToActionResult;
            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName);
            var deletedArticle = _context.Articles
                .FirstOrDefault(x => x.Name == articleName);
            Assert.Null(deletedArticle);
        }

        public void Dispose()
        {
            _context.Database.EnsureDeleted();
        }
    }
}
