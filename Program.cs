using Balta_EntityFramework.Models;
using Blog.Data;
using Microsoft.EntityFrameworkCore;

Console.Clear();

using (var context = new BlogDataContext())
{

}

void ExemploDeUpdateEmUmSubConjunto(BlogDataContext context)
{
    // Exemplo de Update
    // Lembrando que AsNoTracking() SÓ É USADO PARA CONSULTAS SEM MANIPULAÇÃO DE DADOS POSTERIORES.
    // Os metadados são extremamente importantes para o EF para fazer o processo correto.
    var posts = context
    .Posts
    .Include(x => x.Author)
    .Include(x => x.Category)
    .OrderByDescending(x => x.LastUpdateDate)
    .FirstOrDefault();

    // O retorno da consulta com os metadados habilitam o EF a usar 
    // as propriedades de navegação, sendo possivel fazer as alterações nos valores das propriedades.
    // Tudo é feito de forma dinâmica.
    posts.Author.Name = "Teste";

    context.Update(posts);
    context.SaveChanges();
}

void ExemploDeConsultaComInclude(BlogDataContext context)
{
    // Include | Representa os Joins no banco e associa as tabelas
    // ThenInclude | Faz um SUBSELECT na tabela informada no Include.
    // É util para retornar itens dentro de sublistas. 
    var posts = context
    .Posts
    .AsNoTracking()
    .Include(x => x.Author)
    .Include(x => x.Category)
    //  .ThenInclude(x => x.Tags) // exemplo ilustrativo, não funciona.
    .OrderByDescending(x => x.LastUpdateDate)
    .ToList();

    foreach (var post in posts)
    {
        // Para evitar exceções com nulos, adicione o "?", que ja faz um if
        // e caso não tenha dados a propriedade, retorna vazio.
        Console.WriteLine($@"Titulo do post: {post.Title}.
            Escrito por {post.Author?.Name} na categoria {post.Category?.Name}");
    }
}

void ExemploDePost()
{
    // // Não é necessário informar o Id em nenhum desses casos
    // var user = new User
    // {
    //     Name = "Thiago Carvalho",
    //     Slug = "thiago-carvalho",
    //     Email = "dev@thiagocaja.com.br",
    //     Bio = "Programador C#",
    //     Image = "https://url",
    //     PasswordHash = "14232354"
    // };

    // var category = new Category
    // {
    //     Name = "Backend",
    //     Slug = "backend"
    // };

    // // O EF faz a gerencia automatica e referencia os Ids que serão gerados
    // var post = new Post
    // {
    //     Author = user,
    //     Category = category,
    //     Body = "<p>Novo post no blog</p>",
    //     Slug = "aprendendo-sobre-ef",
    //     Summary = "Nesse artigo vamos falar sobre EF Core",
    //     Title = "Começando com EF Core",
    //     CreateDate = DateTime.Now,
    //     LastUpdateDate = DateTime.Now
    // };

    // context.Posts.Add(post);
    // context.SaveChanges();
}

void ExemploCRUD()
{
    /*
        Exemplo de INSERT no banco.
        um objeto instanciado com new não possui TRACKING do EF
        e não pode ser rastreado.
    */
    // var tag = new Tag { Name = "ASP.NET", Slug = "aspnet" };
    // context.Tags.Add(tag);
    // context.SaveChanges(); // Faz o commit no banco

    /*
        Exemplo de UPDATE no banco.
        É realizada operação de REHYDRATATION ou reidratação.
        Sempre é necessario resgatar do banco o item a ser atualizado.

        O EF tem o retorno dos METADADOS e sabe quais campos devem 
        ser atualizados.
    */
    // var tag = context.Tags.FirstOrDefault(x => x.Id == 1);
    // tag.Name = ".NET";
    // tag.Slug = "dotnet";
    // context.Update(tag);
    // context.SaveChanges();

    /*
        Exemplo de DELETE no banco.
        É realizada operação de REHYDRATATION ou reidratação.
        Sempre é necessario resgatar do banco o item a ser excluído.

        O EF tem o retorno dos METADADOS e sabe quais registros devem 
        ser excluídos.
    */
    // var tag = context.Tags.FirstOrDefault(x => x.Id == 1);
    // context.Remove(tag);
    // context.SaveChanges();

    /*
        Exemplo de SELECT no banco.
        O retorno da consulta no banco só é efetivamente realizado quando
        é feita uma chamada de um método (Ex: ToList()) após a atribuição
        do contexto ou quando é feito um ForEach para percorrer os dados.
    */
    // var tags = context.Tags.ToList();
    // foreach (var tag in tags)
    // {
    //     Console.WriteLine($"Tag: {tag.Name} - Slug: {tag.Slug}");
    // }

    /*
        Exemplo de SELECT aprimorado no banco, filtrando e ordenando.

        Sempre adicione a instrução AsNoTracking() logo após o tipo do contexto.
        Dessa forma o EF não retorna metadados, que são usados apenas nos
        casos de alteração e exclusão de registros, melhorando ainda mais a performance.

        É de EXTREMA importância fazer o filtro ANTES de chamar
        o ToList(). Assim evitamos problemas de performance, pois serão
        recuperados apenas os dados já filtrados. Caso contrário, todos os
        registros são retornados e posteriormente filtrados na aplicação.

    */
    // var tags = context
    //     .Tags
    //     .AsNoTracking()
    //     .Where(x => x.Name.Contains("DOT"))
    //     .ToList();

    // foreach (var tag in tags)
    // {
    //     Console.WriteLine($"Id: {tag.Id} - Tag: {tag.Name} - Slug: {tag.Slug}");
    // }
}

void ExemploDiferencaEntreFirstSingle()
{
    using (var context = new BlogDataContext())
    {
        /*  
            Todos os métodos tipo de lista executam a consulta no banco.

            FirstOrDefault | Método mais utilizado, retorna apenas
            o primeiro registro encontrado.
            Não lança exceção caso haja mais registros conforme o filtro.

            Se não existir o registro, retorna nulo.
            É necessário informar o null safe para exibir
            o item e evitar exceção.
        */
        // var tag = context
        //     .Tags
        //     .AsNoTracking()
        //     .FirstOrDefault(x => x.Id == 5);
        // Console.WriteLine(tag?.Name);

        /*  
            SingleOrDefault | Retorna um único registro encontrado,
            caso haja duplicidade, lança uma exceção.

            Se não existir o registro, retorna nulo.
            É necessário informar o null safe para exibir
            o item e evitar exceção.
        */
        // var tag = context
        //     .Tags
        //     .AsNoTracking()
        //     .SingleOrDefault(x => x.Id == 6);
        // Console.WriteLine(tag?.Name);
    }
}