using Balta_EntityFramework.Models;
using Blog.Data;
using Microsoft.EntityFrameworkCore;

Console.Clear();

using (var context = new BlogDataContext())
{
    ExemploDeQueryPuraNoBanco(context);
}

void ExemploCriaUmNovoUsuarioRecuperaOUsuarioEGeraUmPost(BlogDataContext context)
{
    // var user = new User
    // {
    //     Name = "João Felix",
    //     Slug = "jao-felix",
    //     Email = "jao@jaonet.com",
    //     Bio = "Redator tecnico",
    //     Image = "https://meu-endereco",
    //     PasswordHash = "8989645"
    // };

    // context.Users.Add(user);
    // context.SaveChanges();

    var user = context.Users.FirstOrDefault();

    var category = new Category
    {
        Name = "QA",
        Slug = "qa"
    };

    var post = new Post
    {
        Author = user,
        Category = category,
        Body = "<p>Novas regras de abastecimento</p>",
        Slug = "novas-regras-abastecimento",
        Summary = "Veja o impacto das novas regras",
        Title = "Começando com EF Core",
        CreateDate = DateTime.Now,
        // LastUpdateDate = DateTime.Now
    };

    context.Posts.Add(post);
    context.SaveChanges();
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
    // ThenInclude | Faz um SUBSELECT na tabela informada no Include e deve
    // ser usado com moderação, pois a query pode ficar mais pesada.
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

// Dicas iniciais sobre performance
async Task ExemploDeProcessamentoParaleloComAsyncAwaitAsync(BlogDataContext context)
{
    // Task | Uma tarefa pode ser vista como um método que será executado em paralelo.
    // Podemos criar várias tarefas paralelas para lidar com processamentos que demoram
    // mais tempo, permitindo que a aplicação não pare enquanto aguarda o retorno.
    var post = await context.Posts.FirstOrDefaultAsync();
    var users = await context.Users.ToListAsync();
    var posts = await GetPosts(context);

    // Nos exemplos acima, a aplicação vai criar tarefas separadas e processar as
    // solicitações. O Console.WriteLine será executado, ficando independente desses
    // processamentos que podem levar mais tempo para ocorrer.

    Console.Write("Teste");
}

async Task<IEnumerable<Post>> GetPosts(BlogDataContext context)
{
    return await context.Posts.ToListAsync();
}

void ExemploDeCarregamentoComLazyEager(BlogDataContext context)
{
    // Lazy Loading | Carregamento preguiçoso.
    // Esse tipo de instrução acaba gerando um processamento adicional se o retorno 
    // de um objeto não for bem especificado.

    // Aqui seria feito um SELECT * no banco, caso a propriedade estivesse definida
    // como virtual, permitindo sobreesrita. Ex public virtual List<Tag> Tags...
    var lazyPosts = context.Posts.ToList();
    foreach (var post in lazyPosts)
    {
        // E aqui seria feito SELECT adicional no banco para fazer o JOIN.
        foreach (var tag in post.Tags) { }
    }

    // Eager Loading | Carregamento antecipado.
    // Esse é o tipo de instrução padrão do EF gerando um retorno antecipado
    // para o objeto.

    // Executada consulta apenas uma vez com os parametros definidos, mais otimizada,
    // retornando JOIN e filtro em um unico processamento no banco.
    var posts = context.Posts.Include(x => x.Tags).Select(x => x.Id).ToList();
    foreach (var post in lazyPosts)
    {
        foreach (var tag in post.Tags) { }
    }
}

List<Post> ExemploComPaginacao(BlogDataContext context, int skip = 0, int take = 25)
{
    // Caso houvesse 1Mi de registros, tudo seria retornado no objeto, podendo
    // travar a aplicação. É muita informação sendo carregada na memória.
    // var UmMilhaoDePostagens = context
    //     .Posts
    //     .ToList();
    // return UmMilhaoDePostagens;

    // Para evitar isso, podemos utilizar a paginação de dados com os optional
    // parameters Skip e Take. Ex: retorna os primeiros 25 registros.
    var posts = context
        .Posts
        .AsNoTracking()
        .Skip(skip)
        .Take(take)
        .ToList();

    return posts;
}

void ExemploDeRetornoComPaginacao(BlogDataContext context)
{
    // retorna os primeiros 25 registros
    var pagina1 = ExemploComPaginacao(context, 0, 25);

    // pula 25 registros e retorna os próximos 25 registros
    var pagina2 = ExemploComPaginacao(context, 25, 25);

    // pula 50 registros e retorna os próximos 25 registros
    var pagina3 = ExemploComPaginacao(context, 50, 25);

    // pula 75 registros e retorna os próximos 25 registros
    var pagina4 = ExemploComPaginacao(context, 75, 25);
}

void ExemploDeQueryPuraNoBanco(BlogDataContext context)
{
    // Crie uma Model para representar os campos da consulta
    // Crie um DbSet com o tipo da Model
    // Crie uma entidade no modelBuilder para fazer a consulta no banco
    // Trecho do Mapping BlogDataContext
    //...
    // modelBuilder.Entity<PostWithCategoryCount>(x =>
    // {
    //     x.ToSqlQuery(@"
    //         SELECT
    //             [Title] AS [Name],
    //             COUNT([Id]) AS [Count]
    //         FROM 
    //             [Post]
    //         GROUP BY
    //             [Title]").HasNoKey();
    // });
    //...

    var qtyPosts = context.PostWithCategoryCount.AsNoTracking().ToList();

    foreach (var item in qtyPosts)
    {
        Console.WriteLine($"Qtd itens: {item.Count}");
    }
}