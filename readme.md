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

### Migrations

As migrações são um recurso do EF que gerenciam a criação e alteração de esquemas no banco de dados. Verifique se as ferramentas estão instaladas com os comandos abaixo:

```Csharp
dotnet tool install --global dotnet-ef
dotnet add package Microsoft.EntityFrameworkCore.Design --version 5.0.9
```

Execute **dotnet ef** para exibir a tela com informações sobre o Entity Framework.

Processo:

1. Execute um **dotnet clean** e **dotnet build** para compilar a aplicação e verificar se não há erros.
1. **dotnet ef migrations add** InitialCreation | Cria a pasta Migrations no projeto, com arquivos.
1. Caso necessario excluir os arquivos gerados, execute **dotnet ef migrations remove**
1. Para aplicar as migrações geradas, execute **dotnet ef database update**

O fluxo de trabalho será cíclico, seguindo esses passos:

1. Verifique se há pendências de migrations com o **dotnet ef database update**.
1. Ao alterar uma Model, atualize o Mapping e crie a migração **dotnet ef migrations add NewPropertyUserSite**.
1. Aplique a migração com **dotnet ef database update**.

Exportar esquema do banco:

1. Execute **dotnet ef database update** para checar se há alterações na base de dados.
1. Agora execute **dotnet ef migrations script -o ./BancoDedadosBlog.sql** para gerar o script em um arquivo no projeto.

Removendo todas as migrações:

Caso não queira mais trabalhar com Migrations, execute o **dotnet ef migrations remove**, exclua a pasta do projeto _Migrations_ e remova o pacote **Microsoft.EntityFrameworkCore**. Para finalizar, acesse o banco de dados e remova a tabela **\_\_EfMigrationsHistory**
