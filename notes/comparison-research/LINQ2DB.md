# Odkazy
GitHub: https://github.com/linq2db/linq2db
NuGet: https://www.nuget.org/packages/linq2db
Dokumentace: https://linq2db.github.io/index.html

# Úvod
Prý je nejrychlejší knihovna používající LINQ pro práci s DB.
Jendoduchý, rychlý a hlavně typově bezpečný.
> In other words **LINQ to DB is type-safe SQL**.

LINQ to SQL - ORM předchůdce EF, starý od Microsoftu, podporovalo jen (MS SQL Server). Mělo hodně omezení a tak Microsoft začal vyvíjet EntityFramework

linq2db je ORM které je nezávislé ORM, které vzniklo později.

Není tak komplexní jako EF nebo Nhibernate.
# Distribuce a vydání
## Licence
MIT

> A short and simple permissive license with conditions only requiring preservation of copyright and license notices. Licensed works, modifications, and larger works may be distributed under different terms and without source code.
[zdroj](https://choosealicense.com/licenses/mit/)
## Cena
zdarma
## Dostupnost
Source code - GitHub
Balíček - NuGet
## Počet stáhnutí
Total - 14 038 021
Poslední rok = 14 038 021 - 9 109 950 = 4 928 071
## Release cyklus
Jednou za několik měsíců
Semantic versioning
Aktuální verze (9.10.): [6.0.0-preview.1](https://www.nuget.org/packages/linq2db/6.0.0-preview.1) 17.6.2024
# Komunita a podpora
## Udržovanost
329 issues, 2250 closed
## Dokumentace
Vygenerovaná z README + pár dalších stránek s návody.
+ generovaná api dokumentace - nepřehledná
všude query syntax, který se mi osobně nelíbí

Query:
```
var query = from x in myCollection
            where x.SomeProperty > 10
            select x;
```
Method:
```
var query = myCollection
            .Where(x => x.SomeProperty > 10)
            .Select(x => x);
```
## Podpora komunity
na SO nějaké dotazy, reakce na github issues
## Podpora verzí .NET
.NET (included 8)
.NET Core (compatible)
.NET Standard (included 2.0)
.NET Framework (included 4.6.2, compatible 4.8.1)
## Enterprise verze
NE
## Rozšíření
oficiální:
https://github.com/linq2db/linq2db.EntityFrameworkCore - rozšíření pro EF Core, aby šel použít linq2db

https://linq2db.github.io/articles/CLI.html - CLI tools - lze vygenerovat schéma podle databáze

## Vývojové nástroje
Poskytnuté [interceptory](https://linq2db.github.io/articles/general/interceptors.html), kterými je možné přidat vlastní logiku do různých úrovní pipeliny knihovny.

# Databázová podpora
## Podpora systémů a databází
ClickHouse, DB2, Firebird, Informix, Microsoft Access, Microsoft Sql Azure, **Microsoft Sql Server**, Microsoft SqlCe, **MySql**, **Oracle**, **PostgreSQL**, SQLite, SAP HANA, Sybase ASE, DB2 iSeries
## Vlastnosti specifické pro DB
generuje specifické SQL přes DB providery, ale specifikcé featury asi ne

lze do dotazu doplnit DB specifické hinty (nolock, index hint ...)
# Konfigurace a schéma
## Konfigurace
Připojení přes connection string v konstruktoru.
Případně lze doimplementovat connection string setting provider a specifickovat detailněji.
## Schéma DB
Schéma není v kódu, ale v databázi. Lze využít externí DB projekt.
Při změně DB potřeba přepsat SQL dotazy případně namapované objekty.
Přes rozšíření si lze nechat vygenerovat objetky do kódu reflektující databázi.
## Migrace a verzování
NE
# Mapování
## Mapování entit a vztahů
> Alternatively, you can write them manually and map to database using mapping attributes or `fluent mapping configuration`. Also you can use POCO classes as-is without additional mappings if they use same naming for classes and properties as table and column names in database.

### Entity
POCOs
Automatické mapování podle názvu properties.
- automatické mapování nenamapuje ID a další důležité věci, nedoporučuje se
Lze specifikovat konvence (přidání podtržítek, změna velikosti písma atd).
Případně lze přes atributy specifikovat název tabulky, sloupců, primární klíč atd.
Náhradou atributů je fluent konfigurace.

### Vztahy
Ano, ale nelze pořádně vyčíst z dokumentace !
Association - 1:N a N:1
M:N lze přes join tabulku

```cs
builder.Entity<Product>()
    .HasTableName("Products")
    .HasSchemaName("dbo")
    .HasIdentity(x => x.ProductID)
    .HasPrimaryKey(x => x.ProductID)
    .Ignore(x => x.SomeNonDbProperty)
    .Property(x => x.TimeStamp)
        .HasSkipOnInsert()
        .HasSkipOnUpdate()
    .Association(x => x.Vendor, x => x.VendorID, x => x.VendorID, canBeNull: false);
```
## Podpora datových formátů a typů
.NET základní datové typy.

XML a JSON ne, leda tak přes SQL ve stringu.
## Kolekce
NE
# Dotazování a databázové operace
## Podpora a pokrytí dotazování
LINQ - ORM je pro něj vytvořené
Používá vestavěný LINQ + ho doplňuje vlastními metodami

Podle dokumentaci query syntax, ale dá se psát jako method.

Query syntax vypadá více jak SQL a je lepší pro groupby a joins. Ale jsou plně kompatibilní.

```cs
from p in db.Product orderby p.Name descending select new Product { Name = p.Name };


products.Skip((currentPage - 1) * pageSize).Take(pageSize).ToList();
```

Žádný jiný způsob dotazování nepodporuje, ale to vlastně nevadí.
Protože rozšiřuje LINQ na tolik, že by měli jít vykonat většina SQL dotazů.

Lze se dotazovat přes čisté SQL, ale to je takové ohýbání a jsou proto jiná ORM - Dapper, PetaPoco.

Zajímavost - podpora pro MERGE příkaz - hodně rozepsaná v dokumentaci.
- vypadá to že jediné ORM s nativní podporou merge příkazu.
## DDL a DML
DDL - Našel jsem CreateTable a DropTable, TruncateTable metody ale v dokumentaci o nich není nic psáno. Takže bych řekl že částečný support.
Při prozkoumání githubu CreateTable dokáže podle entity vytvořit tabulku.

DML - db.Insert, Update, Delete
## Batch operace
Batch -
podporuje transakce

Bulk - 
https://linq2db.github.io/articles/sql/Bulk-Copy.html
Pro všechny DB dokáže RowByRow - jednotlivé inserty
Pro některé DB MultipleRows - INSERT FROM SELECT - více řádků najednou
Pro pár "ProviderSpecific" - nejefektivnější - ale není popsáno jak
 - MySql, Oracle, PostgreSQL, Microsoft SQL Server
(+ KeepIdentity  možnost - ponechá dodaná ID)
## Change tracking
NE
## Seed data
NE
## Podpora transakcí
ANO - db.BeginTransaction(), db.RollbackTransaction(), db.CommitTransaction()
## Fetch strategie
Nepodporuje lazy loading.
Eager loading - je potřeba definovat co načíst.
## Caching
NE
# Ostatní
## Logování 
ANO lze upravit db.OnTrace a přidat vlastní způsob logování
případně intYerceptory zmíněné výše
## Asynchronní programování
ANO - async varianty metod
# Závěr
## Shrnutí
ORM říká, že je v podstatě typovaný wrapper nad SQL. Což i splňuje - vypadá to, že přes LINQ má nejrozšířenější podporu dotazování (ověříme porovnáním).
Dobrá je podpora mapování vztahů. Jinak nemá moc funkcionality navíc - není caching a další pokročilejší funkcionality. Opravdu jen na dotazování.
