# Csharp Entity Framework with Fluent Mapping

Projeto para estudo e revisão de conceitos utilizando o EF e Mapeamento Fluente.

[Link](https://github.com/thiagokj/Balta_EntityFramework) para projeto similar com Data Annotations.

**FluentMapping** | Forma aprimorada fazer o DE/PARA, o Mapeamento Fluente possui uma separação muito boa do EF do que é o Core da aplicação.

## Requisitos

```Csharp
dotnet add package Microsoft.EntityFrameworkCore --version 5.0.9
dotnet add package Microsoft.EntityFrameworkCore.SqlServer --version 5.0.9
```

### Primeiros passos

1. Inicie organizando o codigo criando os Mappings.
1. Cada Map representa o DE/PARA da tabela para classe.

```Csharp
public class CategoryMap : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            // Mapeia tabela
            builder.ToTable("Category");

            // Mapeia a chave primária
            builder.HasKey(x => x.Id);

            // Gera um Identity como PRIMARY KEY IDENTITY(1, 1)
            // Caso seja gerado um migration para gerar as tabelas no banco,
            // essa instrução é a recomendada.
            builder.Property(x => x.Id)
                .ValueGeneratedOnAdd()
                .UseIdentityColumn();
        }
    }
```
