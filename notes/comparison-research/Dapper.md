# Odkazy
GitHub: https://github.com/DapperLib/Dapper
NuGet: https://www.nuget.org/packages/Dapper
Dokumentace: https://www.learndapper.com/

Vznik Dapperu: https://samsaffron.com/archive/2011/03/30/How+I+learned+to+stop+worrying+and+write+my+own+ORM

# Úvod
Open-source micro-ORM. 
Nízká latence a velký výkon.
Přidává extension metody na DbConnection ADO.NETu.
Focus na O a M v ORM - object mapping. Relace ne.
Lehkost a jednoduchost - pořád ale lepší než používat ADO.NET napřímo.

Vznikl pro StackOverflow. Autoři používali přeechůdce EF - LINQ TO SQL, potřebovali ale větší výkon a kontrolu nad SQL.

Dobrý pro scénáře, kdy se data převážně jen čtou.
Je možné ho použít i s jiným frameworkem v jedné aplikaci, autoři to dokonce zmiňují. V kombinaci s jiným ORM může být použit, tam kde je potřeba velká rychlost, jinde jiné ORM.
Také dobrý pro načítání výsledků stored procedures.
# Distribuce a vydání
## Licence
Open-source

[Apache 2.0](http://www.apache.org/licenses/LICENSE-2.0)
> A permissive license whose main conditions require preservation of copyright and license notices. Contributors provide an express grant of patent rights. Licensed works, modifications, and larger works may be distributed under different terms and without source code.

[Zdroj](https://choosealicense.com/licenses/apache-2.0/)
## Cena
Zdarma

Dapper Plus - 999$ - 1 vývojář/rok (popis níže)
## Dostupnost
Source code - GitHub
Balíček - NuGet
## Počet stáhnutí
Total - 395 069 437
Poslední rok = 395 069 437 - 274 790 871 = 120 278 566
120M
## Release cyklus
Není fixní. Releasy podle fixování chyb, nových PR. Většinou se jen zvedne patch.
Semantic Versioning
Aktuální verze (3.9.): [2.1.44](https://github.com/DapperLib/Dapper/releases/tag/2.1.44) 12.4.2024
# Komunita a podpora
## Udržovanost
429 issues, 897 closed
Autor aktivně komentuje a řeší issues.
## Dokumentace
Externí stránka: https://www.learndapper.com/
Od sponzora, ale oficiálně na ní odkazují. Motivace - sponzor má svojí extension, tak jí do dokumentace přidal. Ale dobře zdokumentovaný je i základ.

Pro příklady knihovna odkazuje na svoje [testy](https://github.com/DapperLib/Dapper/tree/main/tests/Dapper.Tests), kterých je velké množství.

## Podpora komunity
Částečně GitHub issues, ale odpvědi trvají dlouho.
[StackOverflow](https://stackoverflow.com/questions/tagged/dapper) obsahuje velké množství dotazů, odpovídá na ně i sám autor.

## Podpora verzí .NET
.NET (8, 7 implicit)
.NET Core
.NET Standard 2.0 (implicit)
.NET Framework 4.6 (implicit)
## Enterprise verze
Ne přímo. Dapper plus níže.

## Rozšíření
### Oficiální
**Dapper.EntityFramework** - extension metody pro EF, nepopsané
**Dapper.Rainbow** - Micro-ORM implementováno nad Dapperem, umožňuje CRUD operace bez psaní SQL. Hodně jednoduché.
**Dapper.SqlBuilder** - Builder pattern pro dynamické sestavení SQL.
### Komunitní
[Dapper Plus](https://dapper-plus.net/) - mapování, bulk operace - drahé
[Dapper.Transaction](https://github.com/zzzprojects/Dapper.Transaction) - Podpora transakcí
## Vývojové nástroje
NE

# Databázová podpora
## Podpora systémů a databází
Protože používá ADO.NET, podporuje vše co ADO.NET. ADO.NET je obecně dělaný pro podporu relačních DB.

ADO.NET oficiálně podporuje Microsoft SQL Server, Oracle, SqLite. Přes komunitní balíčky má i podporu [PostgreSQL](https://www.npgsql.org/), [MySQL](https://dev.mysql.com/doc/connector-net/en/connector-net-introduction.html), [Firebird](https://firebirdsql.org/en/net-provider/).

## Vlastnosti specifické pro DB
Jelikož si vývojář píše vlastní SQL, dokáže využít i konkrétní vlastnosti DB.
# Konfigurace a schéma
## Konfigurace
Konfiguruje se jen connection string do databáze. Ten se předá jako parametr při vytvoření SqlConnection z ADO.NET. Je na vývojáři, kam si connection string uloží. 

## Schéma DB
Schéma není v kódu, ale v databázi. Lze využít externí DB projekt.
Při změně DB potřeba přepsat SQL dotazy.
## Migrace a verzování
Ne

# Mapování
## Mapování entit a vztahů
**Entity** - automatické mapování výsledků dotazu na třídu - přes generický parametr.
Mapuje přesně podle názvu sloupce v select dotazu, nelze použít atributy nebo konvenci. Možné s neoficiálními rozšíženími.

```cs
public class Dog
{
    public int? Age { get; set; }
    public Guid Id { get; set; }
    public string Name { get; set; }
    public float? Weight { get; set; }

    public int IgnoredProperty { get { return 1; } }
}

var guid = Guid.NewGuid();
var dog = connection.Query<Dog>("select Age = @Age, Id = @Id", new { Age = (int?)null, Id = guid });
```

**Vztahy** - nelze explicitně definovat. Je potřeba napsat SQL dotaz s JOINem. Pak lze ale výsledek rozdělit do více tříd. A lze specifikovat, jak je spojit. (Tedy knihovna sama provede "groupby" podle definované funkce.)

```csharp
var sql =
@"select * from #Posts p
left join #Users u on u.Id = p.OwnerId
Order by p.Id";

var data = connection.Query<Post, User, Post>(sql, (post, user) => { post.Owner = user; return post;});
var post = data.First();

Assert.Equal("Sams Post1", post.Content);
Assert.Equal(1, post.Id);
Assert.Equal("Sam", post.Owner.Name);
Assert.Equal(99, post.Owner.Id);
```
V podstatě je to ale jen zjednodušení ručního zpracování již výsledných dat.
## Podpora datových formátů a typů
Dapper se nezaměřuje na serializace datových typů.
Umí mapovat běžné datové typy .NETu. Ale složitější jako formáty jako JSON nebo XML si již uživatel musí naparsovat sám.

Některé podporované databáze podporují JSON a XML - například Microsoft SQL server. V rámci SQL dotazu se na ně lze dotazovat - a jelikož si SQL píše uživatel, tak lze teoreticky přes select načítat jejich hodnoty.
## Kolekce
Nepodporuje kolekce.

# Dotazování a databázové operace
## Podpora a pokrytí dotazování
Uživatel si dotazy píše SQL dotazy sám - tedy teoreticky je pokryt jakýkoliv dialekt. Tedy pokud je podporováno připojení k databázi, jež dialekt podporuje.

To také znamená, že lze použít SQL/JSON nebo SQL/XML, pokud ho podporuje databáze.

Dapper nemá podporu pro LINQ, ani jiný další dotazovací jazyk.
## DDL a DML
Přes metodu connection.Execute() lze vykonat jakýkoliv SQL příkaz - který si uživatel ovšem ručně napíše. Data lze doplnit přes parametry.  
## Batch operace
Batch ANO
Bulk bez placeného rozšíření ne.
## Change tracking
NE
## Seed data
NE
## Podpora transakcí
Ano, ale nepřímo přes ADO.NET. Zavoláním BeginTransaction nad connection, následně Commit nebo Rollback.
## Fetch strategie
Nepodporuje mapování vztahů - načítací strategie jsou tedy nerelevantní.
## Caching
Nepodporuje cachování dat.

Pro zajímavost - cachuje si, jak namapovat výsledky dotazu na výsledný objekt a doplňování parametrů. To zvyšuje jeho rychlost.
# Ostatní
## Logování 
NE
## Asynchronní programování
Ano, všechny metody mají asynchronní variantu.
# Závěr
## Shrnutí
Dapper je ORM, které bylo vytvořené se zaměřením na rychlost. Toho dosahuje tím, že nepodporuje všechny vlastnosti, které bychom od ORM čekali.  Což je ale v pořádku, protože od toho jsou tu jiná ORM. 

Podle porovnání v [README](https://github.com/DapperLib/Dapper?tab=readme-ov-file#performance) projektu je dotazování opravdu rychlé. Dapper se vyplatí použít místo ADO.NET hlavně kvůli mapování výsledných dat.

Nevýhodou je ale právě nutnost ručního psaní dotazů. Pro efektivní dotazování je tedy potřeba dobrá znalost SQL, případně i konkrétní použité zvolené databáze.

Dapper je tedy vhodný pro aplikace, které vyžadují opravdu rychlé načítání dat a možnost si detailně upravit SQL dotaz. Lze ho také zkombinovat s jiným ORM a použít ho třeba jen na místech, kde je rychlost čtení dat důležitá.