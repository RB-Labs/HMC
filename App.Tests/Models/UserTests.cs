using System;
using Xunit;
using App.Data;
using App.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;

namespace App.Tests.Models
{
    public class UserTests : IDisposable
    {
        ApplicationDbContext _context;

        public UserTests()
        {
            var serviceProvider = new ServiceCollection()
                .AddEntityFrameworkSqlServer()
                .BuildServiceProvider();

            var builder = new DbContextOptionsBuilder<ApplicationDbContext>();

            builder.UseInMemoryDatabase("UserTestDB");

            _context = new ApplicationDbContext(builder.Options);

        }

        [Fact]
        public async void AddUserTest()
        {
            var addResult = _context.Users.Add(new User
                {
                    UserName = "User1",
                    Email = "user1@mail.com"
                });
            Assert.NotNull(addResult);
            int entitiesCount = _context.SaveChanges();
            Assert.Equal(1, entitiesCount);
            var user = await _context.Users
                .FirstOrDefaultAsync(m => m.UserName == "User1");
            Assert.NotNull(user);
            Assert.Equal("user1@mail.com", user.Email);
        }

        public void Dispose()
        {
            _context.Database.EnsureDeleted();
        }
    }
}
