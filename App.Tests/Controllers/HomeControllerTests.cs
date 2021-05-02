using App.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Xunit;

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
            _homeController = new HomeController(logger);
        }
        [Fact]
        public void IndexViewResultNotNull()
        {
            var result = _homeController.Index() as ViewResult;
            Assert.NotNull(result);
        }
    }
}
