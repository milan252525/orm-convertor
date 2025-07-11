# Odkazy
GitHub: https://github.com/mikependon/RepoDB
NuGet: https://www.nuget.org/packages/repodb
Dokumentace: https://repodb.net/

# Úvod
Přemosťuje mezeru mezi micro a macro ORM. 
Macro má moc velkou abstrakci a nelze se zaměřit na výkon databáze.
Micro jsou naopak moc náročné a nepraktické - vyžadují často čisté SQL.
Chtějí zlepšit developer experience.
"Nejjednodušší na použití". Stačí jen connection string. 
# Distribuce a vydání
## Licence
Open-source

[Apache 2.0](http://www.apache.org/licenses/LICENSE-2.0)
> A permissive license whose main conditions require preservation of copyright and license notices. Contributors provide an express grant of patent rights. Licensed works, modifications, and larger works may be distributed under different terms and without source code.

[Zdroj](https://choosealicense.com/licenses/apache-2.0/)
## Cena
zdarma
## Dostupnost
Source code - GitHub
Balíček - NuGet
## Počet stáhnutí
Total - 1 619 958
Poslední rok = 1 619 958 - 1 098 045 = 521 913
## Release cyklus
Není fixní, většinou podle nových fetur, oprav.
+ alpha verze
Semantic Versioning
Aktuální verze (3.9.): [1.13.1](https://github.com/mikependon/RepoDB/releases/tag/v1.13.1) 16.3.2023
# Komunita a podpora
## Udržovanost
134 issues, 633 closed
Autor a ostatní uživatelé velmi aktivně komentuje a řeší issues.
## Dokumentace
Stránka: https://repodb.net/
Rozsáhlá dokumentace, popisuje veškerou funkcionalitu. Popisuje jednotlivé featury/třídy/funkce. Má u nich podrobnější vysvětlení a příklady. Chybí mi ale nějaké souvislé delší příklady - je potřeba hodně hledat. 
Lze nahlédnout do docela rozsáhlých unit a integračních testů v GitHub repozitáři.
## Podpora komunity
Na problémy se lze ptát na GitHubu s dobrým časem odezvy.
Na SO jsou nějaké problémy řešené, ale ne třeba tak jako u Dapperu.
## Podpora verzí .NET
.NET (included 6 a 7, compatible 8)
.NET Core (compatible)
.NET Standard 2.0 (included)
.NET Framework 4.6 (compatible)
## Enterprise verze
NE

## Rozšíření
Core knihovna je RepoDb.Core - podporuje čisté SQL
### Oficiální
Oficiální rozšíření pro Microsoft SQL Server, PostgreSQL, SQLite, MySQL

Bulk operace pro [Microsoft SQL Server](https://github.com/mikependon/RepoDB/tree/master/RepoDb.Extensions/RepoDb.SqlServer.BulkOperations) a [PostgreSQL](https://github.com/mikependon/RepoDB/tree/master/RepoDb.Extensions/RepoDb.PostgreSql.BulkOperations)
## Vývojové nástroje
Tracing - https://repodb.net/feature/tracing
Lze si přidat vlastní metody před a po execukci a debugovat průběh.
Lze získat generované SQL, parametry, čas.

# Databázová podpora
## Podpora systémů a databází
Oficiálně má podporu pro Microsoft SQL Server, PostgreSQL, SQLite, MySQL.
Core knihovna podporuje jakýkoliv SQL dialekt, ale je nutné psát SQL ručně.

## Vlastnosti specifické pro DB
Pro ty databáze zmíněné výše má ORM specifické balíčky.
TODO ?

# Konfigurace a schéma
## Konfigurace
Stačí prý jen connection string a zovální jedné metody.
`RepoDb.SqlServerBootstrap.Initialize();`
Nikde v dokumentaci není uvedeno, kam se connection string vkládá.
TODO: ověřit, vyzkoušet
## Schéma DB
Schéma je v databázi, nelze definovat v kódu.
Nelze vygenerovat schéma podle dataabáze.
## Migrace a verzování
Ne

# Mapování
## Mapování entit a vztahů
**Entity**
Základní automatické mapování přes generický parametr. Lze přepsat.
Mapování podle atributů na třídu.
Fluent mapování kódem přes FluentMapper.Entity\<T\>()

```csharp
FluentMapper
    .Entity<Customer>() // Define which Class or Model
    .Table("[sales].[Customer]") // Map the Class/Table
    .Primary(e => e.Id) // Define the Primary
    .Identity(e => e.Id) // Define the Identity
    .Column(e => e.FirstName, "[FName]") // Map the Property/Column
    .Column(e => e.LastName, "[LName]") // Map the Property/Column
    .Column(e => e.DateOfBirth, "[DOB]") // Map the Property/Column
    .DbType(e => e.DateOfBirth, DbType.DateTime2) // Defines the DatabaseType of the Property
    .ClassHandler<CustomerClassHandler>() // Defines the ClassHandler of the Model
    .PropertyHandler<CustomerAddressPropertyHandler>(e => e.Address)
    .PropertyValueAttributes<CustomerAddressPropertyHandler>(e => e.FirstName,
        new PropertyValueAttribute[]
        {
            new NameAttribute("[FName]),
            new SizeAttribute(256),
            new DbTypeAttribute(DbType.NVarChar)
        });
```

[Capabilities](https://repodb.net/mapper/fluentmapper#capabilities)

**Vztahy** - Nepodporuje mapování vztahů. Podporuje QueryMultiple, kde lze napsat více dotazů. Podobné jako u Dapperu. M:N je potřeba si seskupit ručně.
```csharp
var result = connection.QueryMultiple<Customer, Order>(p => p.Id == 10045, o => o.PersonId == 10045);
var customer = result.Item1.FirstOrDefault();
var orders = result.Item2.AsList();
```
## Podpora datových formátů a typů
.NET základní datové typy.

XML a JSON ne, leda tak přes SQL ve stringu.
## Kolekce
NE
# Dotazování a databázové operace
## Podpora a pokrytí dotazování
(SQL, SQL/JSON, SQL/XML, LINQ, případně jiný jazyk)
Čistě SQL přes `ExecuteQuery<Customer>("")`

Přes anonymní typy dynamicky:
(jen equals)
`var result = connection.Query<Customer>(new { Id = 10045 });`

Typovaně přes "expression":
`var result = connection.Query<Customer>(e => e.Id == 10045);`
Přes LINQ .Contains .StartsWith >= <=
Pro namapované třídy.

Netypovaně přes QueryField/QueryGroup:
`var result = connection.Query<Customer>(new QueryField("Id", Operation.Equal, 10045));`
Operation.GreaterThan, Operation.Like...
Pro namapované třídy, ale i nenamapované, tedy silnější.

Vyloženě nepodporuje LINQ Query().Where.First() ...

Podporuje spoustu metod pro dotazování, kromě jen Query.
Například Max, Exists, Average, Sum.
Jen jak zmíněno výše, nelze použít relace nebo joinovat.
## DDL a DML
DDL - Nelze, jen pokud napíšeme čisté SQL
DML - ANO. lze vytvářet objekty v aplikaci a pak je předat metodám jako Insert, Update, Delete. Případně lze hromadně přes InsertAll, DeleteAll
## Batch operace
Batch operace - Shromáždění operací dohromady v transakci (ACID). Operace/data přenášeny po jednom. Nezrychlý výkon. Do objektů doplní zpětně vygnerovaná ID.
- Pro všechny DB

Bulk operace - Všechna data jsou nejdříve přenesena do DB serveru a až tam se vykonají. Přeskočeny kontroly jako typy, constraint atd z aplikace.  Zrychlý výkon. ID nejsou zpětně doplněna.
- Jen pro PostgreSQL a SQL Server

## Change tracking
NE
## Seed data
NE
## Podpora transakcí
ANO, stejně jako u Dapperu přes ADO.NET 
+ ACID batch operace
## Fetch strategie
Nepodporuje mapování vztahů - načítací strategie jsou tedy nerelevantní.
## Caching
Ano, podpora pro cachování.
2nd-layer cache
![[2ndlayercahce.png]]
Výchozí použije MemoryCache, které ukládá do paměti. Ukládá na 180 minut, konfigurovatelně.
Při operaci je potřeba přidat cacheKey, není automatické.
`var products = connection.QueryAll<Product>(cacheKey: "products", cache: cache);` 
Při vytvoření factory není potřeba předávat i cache.
Takže klíč si sestavuje vývojář podle dotazu a objektu. Lze také manuálně vymazat objekt z cache.
Lze doimplementovat vlastní cache s jiným ukládáním. [Dokumentace](https://repodb.net/reference/jsoncache) ukazuje cachování do JSON souborů.

---
Podobně jako Dapper si cachuje schéma databáze a tak by opakované dotazy měly být efektivní.
# Ostatní
## Logování 
Ne napřímo, ale lze přes [Tracing](https://repodb.net/feature/tracing) lze doimplemntovat pro každý dotaz.
## Asynchronní programování
Ano, metody mají asynchronní variantu. Nejsou v dokumentaci.
# Závěr
## Shrnutí
(reakce na to co ORM slibuje, co umí opravdu dobře, co ne)
(vhodné use cases)

RepoDB slibuje lehkost použít a rychlost. Popisuje se jako mezikrok mezi mikro a makro ORM. Narozdíl od Dapperu má větší podporu dotazování a mapování, rozšířenější funkce jako cachování. Očekával jsem větší podporu mapování vztahů, která zůstává podobná jako u Dapperu. 

RepoDB slibuje větší rychlost a díky cachování se přibližuje Dapperu (uvidíme v porování). Není potřeba psát přímo SQL, což může být bonus pro běžné programátory. Kvůli rychlosti ale nezahazuje veškerou očekávanou ORM funkcionalitu.

Co se mi líbilo, je velká podpora rozšíření - lze si doimplemetovat spoustu věcí jako jak se data mapují, cachují. Knihovna podporuje nárhové vzory jako repository nebo Unit of Work.
