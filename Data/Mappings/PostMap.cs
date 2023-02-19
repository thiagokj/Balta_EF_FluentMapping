using Balta_EntityFramework.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Balta_EF_Mapping.Data.Mappings
{
    public class PostMap : IEntityTypeConfiguration<Post>
    {
        public void Configure(EntityTypeBuilder<Post> builder)
        {
            builder.ToTable("Post");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id)
                .ValueGeneratedOnAdd()
                .UseIdentityColumn();

            builder.Property(x => x.LastUpdateDate)
                .IsRequired()
                .HasColumnName("LastUpdateDate")
                .HasColumnType("SMALLDATETIME")
                // Usa função do SQL SERVER
                .HasDefaultValueSql("GETDATE()");
            // Recomendado atribuir via aplicação.
            // .HasDefaultValue(DateTime.Now.ToUniversalTime()); 

            builder.HasIndex(x => x.Slug, "IX_Post_Slug")
                .IsUnique();

            // Relacionamentos
            /*
                1:N | Um Autor tem muitos Posts.
                Obs: A classe User tem uma lista de Posts.
                Essa instrução irá gerar uma Constraint no banco.

                OnDelete | quando excluir um post, tambem apaga um Autor.
                essa instrução deve ser usada com cautela.
            */
            builder.HasOne(x => x.Author)
                .WithMany(x => x.Posts)
                .HasConstraintName("FK_Post_Author")
                .OnDelete(DeleteBehavior.Cascade);

            /*
                N:N | Os Posts podem ter muitas Tags e as Tags podem ter muitos Posts.
                Obs: A classe Post tem lista de Tags e a classe Tag tem uma lista de Posts.
                Essa instrução irá gerar uma Constraint no banco.

                UsingEntity | Cria uma classe virtual para vincular as duas classes.
                Dessa forma, não é necessário criar uma Model PostTag para representar
                uma tabela de associação no banco de dados.
            */
            builder.HasMany(x => x.Tags)
                .WithMany(x => x.Posts)
                .UsingEntity<Dictionary<string, object>>(
                    "PostTag",
                    post => post.HasOne<Tag>()
                        .WithMany()
                        .HasForeignKey("PostId")
                        .HasConstraintName("FK_PostTag_PostId")
                        .OnDelete(DeleteBehavior.Cascade),
                        tag => tag.HasOne<Post>()
                            .WithMany()
                            .HasForeignKey("TagId")
                            .HasConstraintName("FK_PostTag_TagId")
                            .OnDelete(DeleteBehavior.Cascade)
                );

        }
    }
}