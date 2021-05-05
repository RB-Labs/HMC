using App.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Xunit;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using App.Data;
using Microsoft.EntityFrameworkCore;

namespace App.Tests.Controllers
{
    [Collection("Sequential")]
    public class HomeControllerTests
    {
        private readonly HomeController _homeController;
        public HomeControllerTests()
        {
            var services = new ServiceCollection();
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseInMemoryDatabase("HomeControllerTestDB_Admin")
            );
            var context = services.BuildServiceProvider()
                .GetService<ApplicationDbContext>();
            context.Database.EnsureCreated();

            var controllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal()
                }
            };
            _homeController = new HomeController(context)
            {
                ControllerContext = controllerContext
            };
        }

        [Fact]
        public void IndexViewResultNotNull()
        {
            var result = _homeController.Index() as ViewResult;
            Assert.NotNull(result);
        }
    }
}
