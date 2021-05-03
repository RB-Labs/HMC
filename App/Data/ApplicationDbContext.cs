using App.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace App.Data
{
    public class ApplicationDbContext : IdentityDbContext<User, UserRole, string>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<ArticleCategory> ArticleCategories { get; set; }

        public DbSet<Article> Articles { get; set; }

        public DbSet<Transaction> Transactions { get; set; }

        public DbSet<Ticket> Tickets { get; set; }
    }
}
