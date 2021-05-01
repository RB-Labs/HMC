using System;
using Xunit;
using App.Data;
using App.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;

namespace App.Tests.Models
{
    [Collection("Sequential")]
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
        public async void CreateUserTest()
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

        [Fact]
        public async void UpdateUserTest()
        {
            var addResult = _context.Users.Add(new User
            {
                UserName = "User2",
                Email = "user2@mail.com"
            });
            Assert.NotNull(addResult);
            int entitiesCount = _context.SaveChanges();
            Assert.Equal(1, entitiesCount);
            var user = await _context.Users
                .FirstOrDefaultAsync(m => m.UserName == "User2");
            Assert.NotNull(user);
            Assert.Equal("user2@mail.com", user.Email);
            user.Email = "user2-new@mail.com";
            var updateResult = _context.Users.Update(user);
            Assert.NotNull(updateResult);
            entitiesCount = _context.SaveChanges();
            Assert.Equal(1, entitiesCount);
            user = await _context.Users
                .FirstOrDefaultAsync(m => m.UserName == "User2");
            Assert.NotNull(user);
            Assert.Equal("user2-new@mail.com", user.Email);
        }

        [Fact]
        public async void DeleteUserTest()
        {
            var addResult = _context.Users.Add(new User
            {
                UserName = "User3",
                Email = "user3@mail.com"
            });
            Assert.NotNull(addResult);
            int entitiesCount = _context.SaveChanges();
            Assert.Equal(1, entitiesCount);
            var user = await _context.Users
                .FirstOrDefaultAsync(m => m.UserName == "User3");
            Assert.NotNull(user);
            Assert.Equal("user3@mail.com", user.Email);
            var removeResult = _context.Users.Remove(user);
            Assert.NotNull(removeResult);
            entitiesCount = _context.SaveChanges();
            Assert.Equal(1, entitiesCount);
            user = await _context.Users
                .FirstOrDefaultAsync(m => m.UserName == "User3");
            Assert.Null(user);
        }

        public void Dispose()
        {
            _context.Database.EnsureDeleted();
        }
    }
}
