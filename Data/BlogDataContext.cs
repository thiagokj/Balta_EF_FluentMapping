using Balta_EntityFramework;
using Balta_EntityFramework.Models;
using Microsoft.EntityFrameworkCore;

namespace Blog.Data
{
    public class BlogDataContext : DbContext
    {
        public DbSet<Category> Categories { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
         => options.UseSqlServer(AppConfig.CONNECTION_SQLSERVER);
    }
}