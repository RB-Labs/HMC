using System;
using Xunit;
using App.Data;
using App.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace App.Tests.Models
{
    [Collection("Sequential")]
    public class UserTests : IDisposable
    {
        private readonly ApplicationDbContext _context;

        public UserTests()
        {
            var services = new ServiceCollection();
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseInMemoryDatabase("UserTestDB")
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
                .AddEntityFrameworkStores<ApplicationDbContext>();
        }

        private User CreateUser(string userName, string userEmail)
        {
            var addUserResult = _context.Users.Add(new User
            {
                UserName = userName,
                Email = userEmail
            });
            Assert.NotNull(addUserResult);
            int entitiesCount = _context.SaveChanges();
            Assert.Equal(1, entitiesCount);
            return _context.Users
                .FirstOrDefault(m => m.UserName == userName);
        }

        [Fact]
        public void CreateUserTest()
        {
            const string userName = "User1";
            const string userEmail = "user1@mail.com";
            var newUser = CreateUser(userName, userEmail);
            Assert.NotNull(newUser);
            Assert.Equal(userName, newUser.UserName);
            Assert.Equal(userEmail, newUser.Email);
        }

        [Fact]
        public void UpdateUserTest()
        {
            const string userName = "User2";
            const string userEmail = "user2@mail.com";
            const string newUserEmail = "new-user2@mail.com";
            var newUser = CreateUser(userName, userEmail);
            Assert.NotNull(newUser);
            newUser.Email = newUserEmail;
            var updateUserResult = _context.Users.Update(newUser);
            Assert.NotNull(updateUserResult);
            var entitiesCount = _context.SaveChanges();
            Assert.Equal(1, entitiesCount);
            var updatedUser = _context.Users
                .FirstOrDefault(m => m.UserName == userName);
            Assert.NotNull(updatedUser);
            Assert.Equal(newUserEmail, updatedUser.Email);
        }

        [Fact]
        public void DeleteUserTest()
        {
            const string userName = "User3";
            const string userEmail = "user3@mail.com";
            var newUser = CreateUser(userName, userEmail);
            Assert.NotNull(newUser);
            var removeUserResult = _context.Users.Remove(newUser);
            Assert.NotNull(removeUserResult);
            var entitiesCount = _context.SaveChanges();
            Assert.Equal(1, entitiesCount);
            var deletedUser = _context.Users
                .FirstOrDefault(m => m.UserName == userName);
            Assert.Null(deletedUser);
        }

#pragma warning disable CA1816
        public void Dispose() => _context.Database.EnsureDeleted();
#pragma warning restore CA1816
    }
}
