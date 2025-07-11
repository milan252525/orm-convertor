MIT# Odkazy
GitHub: https://github.com/dotnet/efcore
NuGet: https://www.nuget.org/packages/Microsoft.EntityFrameworkCore
Dokumentace: https://learn.microsoft.com/en-gb/ef/core/

# Úvod
Popsán jako odlehčená, rozšiřitelná a cross-platform verze staré EF6. Jako ORM zlehčuje práci s daty přes .NET objekty a eliminuje kód pro přístup k datům.

EF Core podporuje většinu funkcionlity jako EF 6. Kromě věcí co vývojáři označili jako nepotřebné. Plus přidává věci navíc.

# Distribuce a vydání
## Licence
MIT

> A short and simple permissive license with conditions only requiring preservation of copyright and license notices. Licensed works, modifications, and larger works may be distributed under different terms and without source code.
[zdroj](https://choosealicense.com/licenses/mit/)
## Cena
Zdarma
## Dostupnost
Source code - GitHub
Balíček - NuGet
## Počet stáhnutí
Total -  1 255 579 543
Poslední rok - 1 255 579 543 - 867 076 319 = 388 503 224
## Release cyklus
Časté release, přibližně měsíčně.
Semantic Versioning
Aktuální verze (24.10.): [9.0.0-rc.2.24474.1](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore/9.0.0-rc.2.24474.1) 8.10.2024
# Komunita a podpora
## Udržovanost
2197 issues, 19642 closed
Hodně moc issues, ale na většinu jsou nějaké reakce - komentáře.
## Dokumentace
Vcelku rozsáhlá dokumentace na Microsoft Learn stránkách. Lze vyhledávat, stáhnout PDF. + API reference
## Podpora komunity
Na problémy se lze ptát na GitHubu, reakce na issues.
Na SO tisíce dotazů a odpovědi - hodně používaný má plno zdrojů.
## Podpora verzí .NET
Nejnovější verze vždy odpovídá verzi .NET
EF Core verze 8 - podporuje jen .NET 8 atd.
Žádné jiné verze nejsou podporovány. Pro starší verzi .NET je potřeba si stáhnout odpovídající verzi EF Core.
## Enterprise verze
NE
## Rozšíření
komunitní:
**EF Core Power Tools** - Rozšíření pro Visual Studio 2022 - GUI rozhraní pro vizualizaci mapování, nástroje pro generování mapování z DB
- vyzkoušeno, funguje dobře

**Microsoft.EntityFrameworkCore.AutoHistory** - Knihovna, která automaticky zapisuje auditní data do historizačních tabulek.

**EFCoreSecondLevelCacheInterceptor** Umožní 2nd level caching

Mnoho dalších: https://learn.microsoft.com/en-us/ef/core/extensions/
## Vývojové nástroje
Profilery - ale placené
https://entityframework-extensions.net/efcore-profiler
https://hibernatingrhinos.com/products/efprof

Zdarma: https://miniprofiler.com/

Intergace s debugovacími nástroji:
![[Pasted image 20241025143808.png]]
# Databázová podpora
## Podpora systémů a databází
Oracle, MySQL, Microsoft SQL Server, PostgreSQL, SQLite, Firebird, In-memory (testování), Azure Cosmos DB
## Vlastnosti specifické pro DB
Teoreticky by mělo lépe fungovat s Microsfot SQL Server.
TODO
# Konfigurace a schéma
## Konfigurace
Nastavuje se přes DbContext. "Unit of work" instance.

Konfigurace při depndency injection (asp.net):
```cs
public void ConfigureServices(IServiceCollection services)
{
    services.AddControllers();

    services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer("name=ConnectionStrings:DefaultConnection"));
}
```

Nebo přes konstruktor:
```cs
public class ApplicationDbContext : DbContext
{
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=Test;ConnectRetryCount=0");
    }
}
```

Nastavení přes DbContextOptions což je vidět výše přes builder.
Případně v kódu přes factory.

Konfigurace přes soubor ne. Ten connection string ale samozřejmě necheme mít přímo v kódu.
## Schéma DB
code-first - Vytvoříme si v aplikaci modely. Přes mapování je nakonfigurujeme a propojíme. Poté si přes migrace necháme vytvořit databázi a její schéma. Přes další migrace toto schéma aktualizujeme.

database-first - Máme schéma v databázi a přes Scaffolding (Reverse Engineering) nám EF Core může z databáze vygenerovat entity a jejich mapování. Následně lze přepnout na code-first a použít migrace. Případně lze aktulaizovat eneityručně podle DB. Nebo opakovaně scaffoldovat - ale nástroj přepíše veškeré naše změny, takže to není moc ideální. Přidávají se ale partial třídy, které lze dodefinovat jinde, lze tedy vyřešit

nebo database-first přes DB project

## Migrace a verzování
ANO
Přechod na starou i novou verzi.
Přes CLI nástroj který k ORM patří.
V kódu se upraví entity, jejich vztahy atd. Vývojář použije nástroj na vygenerování migrace. Migrace je soubor, který definuje jak aktualizovat databázi z jedné verze na druhou. Je to C# kód, který lze upravit a uložiz do source control.
Je možné se vracet i zpět ke starým verzím.

# Mapování
## Mapování entit a vztahů
### Entity
Lze mapovat kódem nebo přes atributy na entitě.
```cs
internal class MyContext : DbContext
{
    public DbSet<Blog> Blogs { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Blog>()
            .Property(b => b.Url)
            .IsRequired();
    }
}
```

Případně nemusí být v kontexu a lze extrahovat do separátní třídy a přidat přes atribut.
```
[EntityTypeConfiguration(typeof(BookConfiguration))]
public class Book
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Isbn { get; set; }
}
```

Atributy jako:
\[Table("Blogs")\]  \[Required\] \[MaxLength(500)\] a podobně
### Relace
Pro relace je explicitně vyžadováno namapování primary a foregin key. Oproti NHibernate, kde lze jen specifikovat referenci.

```cs
protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    modelBuilder.Entity<Blog>()
        .HasMany(e => e.Posts)
        .WithOne(e => e.Blog)
        .HasForeignKey(e => e.BlogId)
        .HasPrincipalKey(e => e.Id);
}
```
Navázání 1:N vztahu Blog - Post. Jak je viděť je potřeba specifikovat primární klíč a cizí klíč vztahu. Pokud ale mají názvy jako Entita + Id, EF rozpozná vztah automaticky podle názvu a typu property.

Podporování 1:1, 1:N i M:N přes join tabulku. Join tabulku lze explicitně namapovat, nebo jí dokáže EF schovat.
```cs
public class Post
{
    public int Id { get; set; }
    public List<Tag> Tags { get; } = [];
}

public class Tag
{
    public int Id { get; set; }
    public List<Post> Posts { get; } = [];
}
```

## Podpora datových formátů a typů
Podpora namapování JSON sloupců na entity. Lze definovat entitu pro obsah JSON a pak se do ní dotazovat běžnými dotazy. EF přeloží na příslušné JSON funkce.
```cs
var authorsInChigley = await context.Authors
    .Where(author => author.Contact.Address.City == "Chigley")
    .ToListAsync();
```
Přeloží do 
```sql
SELECT [a].[Id], [a].[Name], JSON_QUERY([a].[Contact],'$')
FROM [Authors] AS [a]
WHERE CAST(JSON_VALUE([a].[Contact],'$.Address.City') AS nvarchar(max)) = N'Chigley'
```
Podpora pro MS Sql Server, Azure SQL, SQLite, PostgreSQL

XML ne
## Kolekce
NE
# Dotazování a databázové operace
## Podpora a pokrytí dotazování
Lze jako v ostatních ORM nechat vykonat čisté SQL. Hlavní podpora je ale pro LINQ.
amozřejmě lze namapovat funkce a views.

**LINQ**
Dotazování přes DbContext. 

```cs
using (var context = new BloggingContext())
{
    var blogs = context.Blogs
        .Where(b => b.Url.Contains("dotnet"))
        .ToList();
}
```


Na dotazy lze přidat tagy metodou TagWith () -> v logu se pak vygenerují komentáře
## DDL a DML
**DDL**
Vygenerované z entit. Nelze ale spouštět vlastní DDQL SQL příkazy za běhu. Jen přes raw SQL.
**DML**
DML - Přes LINQ lze updatovat, mazat, vytvářet.
## Batch operace
**Batch**
podporuje transakce - takže ano

**Bulk**
bez 3rd party rozšíření ne a jelikož obě rozšíření, která to umí jsou placené, tak je nebudeme uvažovat
Případně přes ADO.NET SqlBulkCopy - čisté SQL.
## Change tracking
Každý použitý DbContext selduje změny na entitách, které se přes něj získají z databáze. Změny jsou průběžně uloženy a když se zavolá SaveChanges, tak se změny zpropagují do DB. Případně lze připojit entitu do change trackingu.

Změny jsou sledovány na úrovni properties - a update poté aktualizuje jen ty dané property.

Dotazy defaultně zapnou trackování na vrácených entitách. Dotaz lze rozšířit o AsNoTracking() a trackování se vypne.

Debugování view:
![[Pasted image 20241029165703.png]]
## Seed data
ANO
Zaplnění databáze iniciálními daty.

```cs
protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    => optionsBuilder
        .UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=EFDataSeeding;Trusted_Connection=True;ConnectRetryCount=0")
        .UseSeeding((context, _) =>
        {
            var testBlog = context.Set<Blog>().FirstOrDefault(b => b.Url == "http://test.com");
            if (testBlog == null)
            {
                context.Set<Blog>().Add(new Blog { Url = "http://test.com" });
                context.SaveChanges();
            }
        });
```
případně přímo na entitě
```cs
modelBuilder.Entity<Country>(b =>
{
    b.Property(x => x.Name).IsRequired();
    b.HasData(
        new Country { CountryId = 1, Name = "USA" },
        new Country { CountryId = 2, Name = "Canada" },
        new Country { CountryId = 3, Name = "Mexico" });
});
```
Může se hodit na data potřebná pro start aplikace, číselníky...
## Podpora transakcí
Pro každý context je zavolání SaveChanges obaleno v transakci.

Přes kontext je možné transakci vytvářet manuálně.
`context.Database.BeginTransaction();` a `transaction.Commit();`

Navíc umožňuje vytvářet savepointy. Tedy body (stav dat), do kterých se lze vracet.
`transaction.CreateSavepoint("SavepointName");`
`transaction.RollbackToSavepoint("SavepointName");`

## Fetch strategie
Podporuje:
- eager loading - související data (relace) jsou načteny uhned v prvním dotazu
	- Přes metodu Include, případně nested ThenInclude 
	- `var blogs = context.Blogs .Include(blog => blog.Posts) .ToList();`
- explicit loading - načtení, až když o to vyloženě "požádám" v kódu 
	- `context.Entry(blog).Collection(b => b.Posts).Load();`
- lazy loading - načtení až při přístupu na property

Default je eager, lazy loading může mít nechtěné efekty. Dotazy navíc (N+1).
Je potřeba použít proxy a metody označit virtual.
## Caching
DbContext má uložené již načtené entity. Pokud se dotážeme znovu, nenačte jí z databáze. Což je v podstatě cache. Ale jeb v rámci DBcontextu (unit of work).

2nd- level cache v základu nemá. 
Je potřeba použít neoficiální knihovnu.
Například: https://github.com/VahidN/EFCoreSecondLevelCacheInterceptor

Po nainstalování se cachují výsledky dotazů, opakované dotazy vrátí entity z cache.
Lze použít MemoryCache nebo třeba Redis.

Potřeba nakonfigurovat.
```cs
services.AddEFSecondLevelCache(options => options
.UseMemoryCacheProvider()
.UseCacheKeyPrefix("EF_") .UseDbCallsIfCachingProviderIsDown(TimeSpan.FromMinutes(1)));
```
# Ostatní
## Logování 
Velmi jednoduché nastavit.
Třeba
```cs
optionsBuilder .UseSqlServer("YourConnectionString") .LogTo(Console.WriteLine, LogLevel.Information) .EnableSensitiveDataLogging();
```
EnableSensitiveDataLogging Zapne i logování parametrů.
Samozřejmě lze nastavit jiný logger než konzoli.

Společně s debugovacími a logovacími nástroji lepší než NHibernate.
## Asynchronní programování
Asynchronní varianty metod.
Ale nepodporuje paralelní operace v rámci jendoho kontextu. Tedy vždy await jen na jeden task. Nelze vytvořit více tasků a awaitnout najednou. Případně context per task.
# Závěr
## Shrnutí
- aktualizovaná dokumentace
- podpora Microsoftu - což znamená časté aktualizace, dokumentace a další zdroje
- vyvíjen v podstatě s .NETem, dnes je již standard ho používat
- velmi dobré debugovací nástroje a plno extension knihoven/toolů

mínusy: 
- chybí 2nd level cache - ale existuje knihovna
- (možná) velký důraz na unit of work a change tracking
	- v případě, že nechceme musíme dělat kroky navíc.
