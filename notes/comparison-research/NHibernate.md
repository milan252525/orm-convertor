# Odkazy
GitHub: https://github.com/nhibernate/nhibernate-core
NuGet: https://www.nuget.org/packages/NHibernate
Dokumentace: https://nhibernate.info/ https://nhibernate.info/previous-doc/v5.4/ref/index.html

# Úvod
NHibernate původně vznikl jako port Hibernate ORM z Javy. Hibernate už ale dlouhou dobuje nekopíruje, stalo se z něj nezávislé ORM pro .NET. Je to makro-ORM, "fully-featured" - mělo by umět vše,c o od ORM čekáme. Slibuje, že programátora odprostí od 95% "common data persistence related programming tasks"

# Distribuce a vydání
## Licence
Open-source

[GNU Lesser General Public License v2.1 only](https://licenses.nuget.org/LGPL-2.1-only)
>The GNU GPL is the most widely used free software license and has a strong copyleft requirement. When distributing derived works, the source code of the work must be made available under the same license. There are multiple variants of the GNU GPL, each with different requirements.

[Zdroj 2.0](https://choosealicense.com/licenses/gpl-2.0/)
## Cena
Zdarma
## Dostupnost
Source code - GitHub
Balíček - NuGet, [SourceForge](https://sourceforge.net/projects/nhibernate/files/NHibernate/5.5.2/)
## Počet stáhnutí
Total - 47 353 410
Poslední rok = 47 353 410 - 35 555 469 = 11 797 941
## Release cyklus
Nepravidelné release, ale přibližně měsíčně.
Semantic Versioning
Aktuální verze (23.9.): [5.5.2](https://github.com/nhibernate/nhibernate-core/releases/tag/5.5.2) 6.7.2024
# Komunita a podpora
## Udržovanost
604 issues, 852 closed
Otevřené mnoho issues, občas některé mají reakce.
## Dokumentace
Stránka: https://nhibernate.info/previous-doc/v5.4/ref/index.html
Lze stáhnout jako PDF nebo HTML.
Je formátovaná pro PDF a tisk. Černo-bílá, nudná, hodně textu.
Přesto ale vysvětluje koncepty dost podrobně a má spoustu příkladů. Obsahuje některé hodně staré stránky, které třeba popisují nástroje, které již neexistují.  Místy opravdu hodně zastaralá. Založeno v roce 2008. Neobsahuje vyhledávání (jen CTRL+F).

## Podpora komunity
Na problémy se lze ptát na GitHubu, ovšem reakce nevypadají moc rychle.
Na SO je přes 17000 dotazů o NHibernatu. Reakce na dotazy je tam rychlejší.
## Podpora verzí .NET
.NET (included 6, compatible 8)
.NET Core (included 2, compatible 3.1)
.NET Standard 2.0 (included 2.0, 2.1)
.NET Framework 4.6 (included 4.6.1, 4.8, compatible 4.8.1)

v podstatě kompletní pokrytí
## Enterprise verze
NE
## Rozšíření
### Oficiální
[FluentNHibernate](https://github.com/nhibernate/fluent-nhibernate) - Fluent mapování entit
- NHibernate má vestavěnou podporu pro fluentní mapování, nemá ale žádnou dokumentaci. FluentNHibernate ji má a je obecně používám.
[NHibernate.Spatial](https://github.com/nhibernate/NHibernate.Spatial) - podpora prostorových dat (MSSQL, MySQL, PostGis, SpatiaLite)
[NHibernate-Search](https://github.com/nhibernate/NHibernate-Search) - podpora full-text vyhledávání
## Vývojové nástroje
Profiler - [NHibernate Profiler](https://github.com/nhibernate/NHibernate-Search) - bohužel placený nástroj
- logování generovaných SQL

# Databázová podpora
## Podpora systémů a databází
SQL Server, Oracle, DB2, Firebird, Informix, MySQL, PostgreSQL, SQLite, SAP ASE, Ingres

## Vlastnosti specifické pro DB
(podporovaná DB má specifické featury - podporuje je ORM? nebo jen umí společný feature set pro všechny DB)
-
# Konfigurace a schéma
## Konfigurace
Programatická konfigurace (v kódu).
NHibernate.Cfg.Configuration
Je potřeba načíst mapovací soubory z assembly.
XML konfigurace - Web.Config / hibernate.cfg.xml

```xml
<?xml version='1.0' encoding='utf-8'?>
<hibernate-configuration xmlns="urn:nhibernate-configuration-2.2">

  <!-- an ISessionFactory instance -->
  <session-factory>

    <!-- properties -->
    <property name="connection.connection_string">
      Server=localhost;initial catalog=nhibernate;User Id=;Password=
    </property>
    <property name="show_sql">false</property>
    <property name="dialect">NHibernate.Dialect.MsSql2012Dialect</property>

    <!-- mapping files -->
    <mapping resource="NHibernate.Auction.Item.hbm.xml" assembly="NHibernate.Auction" />
    <mapping resource="NHibernate.Auction.Bid.hbm.xml" assembly="NHibernate.Auction" />

  </session-factory>

</hibernate-configuration>
```
```cs
ISessionFactory sf = new Configuration().Configure().BuildSessionFactory();
```

## Schéma DB
code-first - ORM dokáže přes nástroj SchemaUpdate vygenerovat DDL schéma pro DB, případně aktualizovat podle existujícího. Nedokáže pokrýt všechny změny, je potřeba doplnit update skripty. Není doporučený pro [produkci](https://stackoverflow.com/questions/2065845/is-nhibernate-schemaupdate-safe-in-production-code).  Takže asi tak napůl. (SchemaExport)

database-first - nelze vygenerovat schéma podle databáze
## Migrace a verzování
ne - ručně
# Mapování
## Mapování entit a vztahů
### Entity
Lze mapovat přes atritibuty na třídě. Přes XML (.hbm) soubory. Nebo přes mapování kódem (FluentNHibernate). Široká podpora všech nastavení, datových typů, vlastní typy. Lze nastavit konvence pro automatické mapování.
Z mapování kódem lze vygenerovat zpět XML.

```xml
<?xml version="1.0" encoding="utf-8" ?>  
<hibernate-mapping xmlns="urn:nhibernate-mapping-2.2"  
  namespace="QuickStart" assembly="QuickStart">  
  
  <class name="Cat" table="Cat">  
	<id name="Id">  
	  <generator class="identity" />  
	</id>  
  
	<property name="Name">  
	  <column name="Name" length="16" not-null="true" />  
	</property>  
	<property name="Sex" />  
	<many-to-one name="Mate" />  
	<bag name="Kittens">  
	  <key column="mother_id" />  
	  <one-to-many class="Cat" />  
	</bag>  
  </class>  
</hibernate-mapping> 
```
```cs
public class CatMap : ClassMap<Cat>
{
  public CatMap()
  {
	Id(x => x.Id);
	Map(x => x.Name)
	  .Length(16)
	  .Not.Nullable();
	Map(x => x.Sex);
	References(x => x.Mate);
	HasMany(x => x.Kittens);
  }
}
```
### Relace
Plná podpora mapování vztahů. 1:1, 1:N, N:1, M:N, + přes join tabulku.
Mapuje se do kolekcí - ISet, IList, IDictionary, ISortedList(řazené kolekce)

Mapování dědičnosti.
> NHibernate supports the three basic inheritance mapping strategies:
> - table per class hierarchy
> - table per subclass
> - table per concrete class
> In addition, NHibernate supports a fourth, slightly different kind of polymorphism:
> - implicit polymorphism
## Podpora datových formátů a typů
BinaryBlob
XDoc
XmlDoc - ale není podpora nějakého dotazování
Podporuje Postgre, které umí JSON. Ale oficiálně nepodporuje JSON.
## Kolekce
NE
Postgre má array - NHibernate neumí
# Dotazování a databázové operace
## Podpora a pokrytí dotazování
Plně podporuje **HQL** (Hibernate Query Language) - ale ve stringách.
**CriteriaAPI** - Nestandartní dotazovací jazyk. Není type-safe. Spíš historický.
```cs
var cats = sess.CreateCriteria<Cat>()
    .Add( Expression.Like("Name", "Fritz%") )
    .Add( Expression.Between("Weight", minWeight, maxWeight) )
    .List<Cat>();
```
**QueryOver API** - Když přidal .NET v 3.5 extension metody a lambda výrazy, vylepšili CriteriaAPI. Type-safe. Kompilátor může ověřit a refaktorovat.
`.Add(Expression.Eq("Name", "Smith"))` -> `.Where<Person>(p => p.Name == "Smith")`
**LINQ** - provider pro LINQ, použití IQueryable (futures, fetching), z čistého sql lze načítat entity, případně i vztahy
```csharp
IList<Cat> cats =
    session.Query<Cat>()
        .Where(c => c.Color == "white")
        .ToList();
```
**SQL** - lze vykonávat čisté SQL, parametrizovat ho
```cs
sess.CreateSQLQuery("SELECT * FROM CATS")
 .AddScalar("ID", NHibernateUtil.Int64)
 .AddScalar("NAME", NHibernateUtil.String)
 .AddScalar("BIRTHDATE", NHibernateUtil.Date)
```
Named SQL queries - specifikovány v mapování a jsou pojménované
## DDL a DML
DDL - Dokáže vygenerovat DDL ze schématu při běhu. Ale nelze spouštět vlastní příkazy při běhu. (leda tak v čístém SQL)

DML - Přes LINQ lze updatovat a deletovat, vytvářet.
## Batch operace
Batch ANO přes transakce. Ale uložené objekty se načítají zpět do cache.
Je tedy potřeba ji průběžně manuálně vyprazdňovat.
Lze použít StatelessSession, která si nic nepamatuje.

Bulk - lze simulovat nastaveních batch size. ale ne pořádně.

## Change tracking
??? TODO
vypadá to že ne, nikde nic není psáno
## Seed data
NE
## Podpora transakcí
ANO, stejně jako u Dapperu přes ADO.NET 

## Fetch strategie
Pro kolekce a vztahy lze nastavit lazy nebo eager fetching. Lze i jednotlivě podle entit.

Různé strategie:
- join fetching (stejný select - outer join)
- select fetching (druhý select)
- extra-lazy (po jednom až po přístupu na prvek v kolekci)
- batch fecthing (select ale v batchích)
## Caching
query cache - výsledky dotazů - lze zapnout
1st level cache je per session

2nd level cache je sdílená přes sessions 
- vyžaduje práci s daty přes transakce
Providers:
- Hashtable (neprodukční) - memory
- ASP.NET Cache - memory
- Prevalence Cache - memory i disk
Lze nastavit různé strategie podle využití - read only, read/write, nonstrict read/write, never

Má plno dalších komunitních providerů jako memcached, redis
# Ostatní
## Logování 
Lze zapnout logování vygenerovaného SQL.
Používá log4net
## Asynchronní programování
Async varianty metod
# Závěr
## Shrnutí
- zastaralá dokumentace, někdy až chybějící, staré odkazy
- hodně možností mapování + vztahy a kolekce
- cachování
