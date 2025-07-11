# Odkazy
GitHub: https://github.com/dotnet/ef6
NuGet: https://www.nuget.org/packages/EntityFramework
Dokumentace: https://learn.microsoft.com/en-gb/ef/ef6/

# Úvod
Tried and tested. Dlouhá historie a stabilizace. Snižuje mismatch mezi relačními daty DB a objektovým programováním. Slibuje change tracking, bohaté mapování, code first definici dat.
Již není aktivně vyvíjen - jen security fixy.

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
Total - 295 158 900
Poslední rok = 295 158 900 - 238 270 885 = 56 888 015
## Release cyklus
Velmi nepravidelně - již není aktivní vývoj, pouze security fixes.
Semantic Versioning
Aktuální verze (24.10.): [6.5.1](https://www.nuget.org/packages/EntityFramework/6.5.1) 17.6.2024
# Komunita a podpora
## Udržovanost
1 issues, 1115 closed
Moc issue se již neotvírá, ale všechny jsou řešeny/uzavírány.
## Dokumentace
Vcelku rozsáhlá dokumentace na Microsoft Learn stránkách. Lze vyhledávat, stáhnout PDF. + API reference
## Podpora komunity
Na problémy se lze ptát na GitHubu, reakce na issues.
Na SO tisíce dotazů a odpovědi - hodně starý má plno zdrojů.
## Podpora verzí .NET
.NET (included 6, compatible 8)
.NET Core (compatible 3.1)
.NET Standard 2.0 (included 2.1)
.NET Framework 4.6 (included 40, 4.5, compatible 4.8.1)
## Enterprise verze
NE
## Rozšíření
komunitní:
https://entityframework-extensions.net/ - Bulk operace
## Vývojové nástroje
Placený profiler - https://entityframework-extensions.net/efcore-profiler
# Databázová podpora
## Podpora systémů a databází
Oracle, MySQL, Microsoft SQL Server, PostgreSQL, SQLite, FireBird
## Vlastnosti specifické pro DB
(podporovaná DB má specifické featury - podporuje je ORM? nebo jen umí společný feature set pro všechny DB)

# Konfigurace a schéma
## Konfigurace
Rozsáhlá dokumentace o konfiguraci.

Konfigurace kódem:
```cs
public class MyConfiguration : DbConfiguration
{
    public MyConfiguration()
    {
        SetExecutionStrategy("System.Data.SqlClient", () => new SqlAzureExecutionStrategy());
        SetDefaultConnectionFactory(new LocalDbConnectionFactory("mssqllocaldb"));
    }
}
```
Lze i dynamicky měnit za běhu/při startu.

Konfigurační soubor:
```xml
<?xml version="1.0" encoding="utf-8"?>
<configuration>
  <configSections>
    <!-- For more information on Entity Framework configuration, visit http://go.microsoft.com/fwlink/?LinkID=237468 -->
    <section name="entityFramework" type="System.Data.Entity.Internal.ConfigFile.EntityFrameworkSection, EntityFramework, Version=4.3.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" />
  </configSections>
</configuration>
```
```xml
<connectionStrings>
  <add name="BlogContext"  
	        providerName="System.Data.SqlClient" connectionString="Server=.\SQLEXPRESS;Database=Blogging;Integrated Security=True;"/>
</connectionStrings>
```

Lze nastavit logování atd.
## Schéma DB
code-first: vývojář napíše model přes POCO entity, pak se z nich vygeneruje DB schéma - přes migrace

EF designer: návrh entit přes GUI - malování boxů a spojení relacemi. Výsledek je uložen v xml (EDMX extension) - Tohle v EF Core již není podporováno.
![[Pasted image 20241101091436.png]]

Deisgner se umí napojit na existující databázi a z ní vygenerovat schéma -> poté entity.
## Migrace a verzování
Migrace fungují stejně jako v EF Core. Jen jsou příkazy trochu jinak pojmenované. Generují stejný kód pro migraci na novou ale i starou verzi.
# Mapování
## Mapování entit a vztahů
Mapování přes atributy nebo kódem přes konfiguraci stejně jako u EF Core.
Jne nemá takovou podporu jako Core. Nejdou třeba mapovat nullable typy, mapovat jdou jen properties, ne fieldy. Nejdou Shadow properties - property, které nejsou v entitě ale jsou sloupce v databázi - třeba updateddate.

## Podpora datových formátů a typů
Narozdíl of EF Core nepodpourje JSON. Ani XML a další.
## Kolekce
NE

# Dotazování a databázové operace
## Podpora a pokrytí dotazování
LINQ jako v EF Core, případně raw SQL, 
žádný jiný zúůsob
## DDL a DML
**DDL**
Vygenerované z entit. Nelze ale spouštět vlastní DDQL SQL příkazy za běhu. Jen přes raw SQL.
**DML**
DML - Přes LINQ lze updatovat, mazat, vytvářet.
## Batch operace
(stejně jako EF core)
## Change tracking
(stejně jako EF core)
## Seed data
(stejně jako EF core)
## Podpora transakcí
Nepodporuje savepointy. Základní transakci jako EF Core.
## Fetch strategie
3 strategie jako v EF Core.
ALE lazy loading je defaultně zapnutý - pokud je property virtual.
## Caching
Stejně jako v EF Core. 2nd level cache není v základu.
# Ostatní
## Logování 
stejně jako Core
## Asynchronní programování
Async varinty metod. Ale není pokryté vše jako v Core.
Také nedovoluje paralelní operace.
# Závěr
## Shrnutí
V podstatě umí to stejné nebo méně jako EF Core.
Dnes tedy snad už jen do starých aplikací, do kterých Core přidat nelze (.NET Framework). EF6 má něj

Co má EF6 a Coru chybí je zde:
https://weblogs.asp.net/ricardoperes/current-limitations-of-entity-framework-core