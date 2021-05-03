using App.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Xunit;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace App.Tests.Controllers
{
    [Collection("Sequential")]
    public class HomeControllerTests
    {
        private readonly HomeController _homeController;
        public HomeControllerTests()
        {
            var serviceProvider = new ServiceCollection()
                .AddLogging()
                .BuildServiceProvider();
            var logger = serviceProvider
                .GetService<ILoggerFactory>()
                .CreateLogger<HomeController>();
            var controllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal()
                }
            };
            _homeController = new HomeController(logger)
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
