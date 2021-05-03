using App.Areas.Admin.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Xunit;

namespace App.Tests.Areas.Admin.Controllers
{
    [Collection("Sequential")]
    public class HomeControllerTests
    {
        private readonly HomeController _homeController;
        public HomeControllerTests()
        {
            var controllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal()
                }
            };
            _homeController = new HomeController()
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
