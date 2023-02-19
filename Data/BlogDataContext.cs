using Balta_EF_Mapping.Data.Mappings;
using Balta_EF_Mapping.Models;
using Balta_EntityFramework;
using Balta_EntityFramework.Models;
using Microsoft.EntityFrameworkCore;

namespace Blog.Data
{
    public class BlogDataContext : DbContext
    {
        // Mapeamento de tabelas
        public DbSet<Category> Categories { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Post> Posts { get; set; }

        // Mapeamento de Querys e Views
        public DbSet<PostWithCategoryCount> PostWithCategoryCount { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
         => options.UseSqlServer(AppConfig.CONNECTION_SQLSERVER);

        // Informa ao DataContext os arquivos de mapeamento
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Cria as tabelas no banco conforme mapeamento
            modelBuilder.ApplyConfiguration(new CategoryMap());
            modelBuilder.ApplyConfiguration(new UserMap());
            modelBuilder.ApplyConfiguration(new PostMap());

            // Cria uma entidade para fazer consultas puras no banco
            modelBuilder.Entity<PostWithCategoryCount>(x =>
            {
                x.ToSqlQuery(@"
                    SELECT
                        [Title] AS [Name],
                        COUNT([Id]) AS [Count]
                    FROM 
                        [Post]
                    GROUP BY
                        [Title]").HasNoKey();
            });
        }
    }
}