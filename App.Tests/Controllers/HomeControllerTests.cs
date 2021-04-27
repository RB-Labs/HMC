using System;
using Xunit;
using App.Data;
using App.Models;
using App.Controllers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace App.Tests.Controllers
{
    public class HomeControllerTests
    {
        private ILogger<HomeController> _logger;
        public HomeControllerTests()
        {
            var serviceProvider = new ServiceCollection()
                .AddLogging()
                .BuildServiceProvider();
            var factory = serviceProvider.GetService<ILoggerFactory>();
            _logger = factory.CreateLogger<HomeController>();
        }
        [Fact]
        public void IndexViewResultNotNull()
        {
            HomeController controller = new HomeController(_logger);
            ViewResult result = controller.Index() as ViewResult;
            Assert.NotNull(result);
        }
    }
}
