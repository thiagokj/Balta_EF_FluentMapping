using Balta_EntityFramework.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Balta_EF_Mapping.Data.Mappings
{
    public class CategoryMap : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            // Mapeia tabela
            builder.ToTable("Category");

            // Mapeia a chave primária
            builder.HasKey(x => x.Id);

            /*
                Gera um Identity como PRIMARY KEY IDENTITY(1, 1)
                Caso seja gerado um migration para gerar as tabelas no banco,
                essa instrução é a recomendada.
            */
            builder.Property(x => x.Id)
                .ValueGeneratedOnAdd()
                .UseIdentityColumn();

            // Propriedades
            // Especifica a criação do schema no banco de dados
            builder.Property(x => x.Name)
                .IsRequired()
                .HasColumnName("Name")
                .HasColumnType("NVARCHAR")
                .HasMaxLength(80);

            builder.Property(x => x.Slug)
                .IsRequired()
                .HasColumnName("Slug")
                .HasColumnType("VARCHAR")
                .HasMaxLength(80);

            // Índices
            // Cria um índice do tipo UNIQUE
            builder.HasIndex(x => x.Slug, "IX_Category_Slug")
                .IsUnique();
        }
    }
}