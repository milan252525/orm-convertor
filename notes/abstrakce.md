# Rozdíly pro abstrakci
**Dapper**
- mapování 
	- není
- entity
	- přesně musí odpovídat sloupcům výsledku
		- název, datový typ
	- čisté POCO, bez atributů
- dotazy
	- zdroj - SqlConnection (Microsoft.Data.SqlClient)
	- metoda - Query, QuerySingle, QueryFirst
	- generický parametr - entita
	- parametry
		- string SQL
		- parametry jako anonymní objekt

**PetaPoco**
- mapování
	- pokud entita odpovídá výsledku není potřeba mapovat
	- pokud napíšeme celý SQL dotaz není potřeba mapovat název tabulky
	- jinak volitelně pár atributů - nespecifikují se typy
	- globální konvence
- entity
	- pokud odpovídají výsledku není potřeba mapovat
	- datové typy musí odpovídat, nelze mapovat
- dotazy
	- zdroj IDatabase (PetaPoco)
	- metoda - Single, First, Fetch (List), Query (IEnumerable)
	- generický parametr - entita
	- parametry
		- string SQL (NEBO SQL BUILDER)
		- parametry

**RepoDB**
- mapování
	- potřebuje minimálně schéma a tabulku
	- datové typy a sloupce se odvodí nebo namapují
	- atributy nebo fluent - kompletně vše
- entity
	- POCO odpovídající tabulce, případně transformace přes mapování
- dotazy
	- zdroj - SqlConnection (Microsoft.Data.SqlClient)
	- metoda - Query, QueryAll (IEnumerable), ExecuteQuery (SQL)
	- generický parametr - entita
	- parametry
		- lambda funkce pro filtrování
		- NEBO SQL string a parametry

**linq2db**
- mapování
	- potřebuje minimálně schéma a tabulku
	- datové typy a sloupce se odvodí nebo namapují
	- atributy nebo fluent - kompletně vše
	- je potřeba namapovat vztah
- entity
	- POCO odpovídající tabulce, případně transformace přes mapování
- dotazy
	- zdroj - třída zděděná z DataConnection, obsahuje mapování
	- metoda - connection.GetTable (Query pro SQL)
	- generický parametr - entita
	- parametry žádné
	- následuje LINQ dotaz

**NHibernate**
- mapování
	- potřebuje minimálně schéma, tabulku a *všechny property*
	- datové typy se mohou odvodit, sloupce se namapují
	- XML, ~~atributy~~ nebo fluent - kompletně vše
	- je potřeba namapovat vztah
- entity
	- POCO odpovídající tabulce, případně transformace přes mapování
	- všechny property nutně virtual
- dotazy
	- zdroj - ISession vytvořená přes ISessionFactory
	- metoda - session.Query (CreateSQLQuery pro SQL)
	- generický parametr - entita
	- parametry žádné
	- následuje LINQ dotaz

**EF Core**
- mapování
	- potřebuje minimálně schéma, tabulku
	- datové typy a sloupce se odvodí nebo namapují
	- atributy nebo fluent - kompletně vše
	- je potřeba namapovat vztah nebo označit klíče 
- entity
	- POCO odpovídající tabulce, případně transformace přes mapování
- dotazy
	- zdroj - třída zděděná z DbContext, obsahuje mapování
	- metoda - context.DBSet() (SqlQueryRaw pro SQL)
	- generický parametr - entita
	- parametry žádné
	- následuje LINQ dotaz

EF6 stejně

## Shrnutí rozdílů

|                                   | Dapper                         | PetaPoco                                         | RepoDB                                       | linq2db             | NHibernate      | EF Core       |
| --------------------------------- | ------------------------------ | ------------------------------------------------ | -------------------------------------------- | ------------------- | --------------- | ------------- |
| mapování odpovídá výsledku dotazu | x                              | x                                                |                                              |                     |                 |               |
| mapování podle tabulky            |                                |                                                  | x                                            | x                   | x               | x             |
| nutné schéma a tabulka            |                                |                                                  | x                                            | x                   | x               | x             |
| odhad typů podle property         | x                              | x                                                | x                                            | x                   | x               | x             |
| entita                            | POCO                           | POCO                                             | POCO                                         | POCO                | POCO<br>virtual | POCO          |
| zdroj dotazu                      | SqlConnection                  | IDatabase                                        | SqlConnection                                | DataConnection      | ISession        | DbContext     |
| metoda pro LINQ                   |                                |                                                  | Query, QueryAll (IEnumerable)                | connection.GetTable | session.Query   | context.DBSet |
| generický parametr                | entita                         | entita                                           | entita                                       | entita              | entita          | entita        |
| argumenty                         | SQL string + parametry         | SQL string + parametry                           | lambda funkce<br>NEBO SQL string a parametry |                     |                 |               |
| metoda pro SQL                    | Query, QuerySingle, QueryFirst | Single, First, Fetch (List), Query (IEnumerable) | ExecuteQuery                                 | Query               | CreateSQLQuery  | SqlQueryRaw   |
| dotaz                             | SQL                            | SQL                                              | lambda/SQL                                   | LINQ                | LINQ            | LINQ          |


**Důležité pro porovnání POCO:**
atributy, vztahy + kardinalita, identifikátory, tabulky + schéma, datové typy

## Porovnání POCO

|                  | Dapper                | PetaPoco                                                                            | RepoDB                                 | linq2db                                                                          | NHibernate                                                                                                                                                                             | EF Core                                                                                                                                                                                                                                             |
| ---------------- | --------------------- | ----------------------------------------------------------------------------------- | -------------------------------------- | -------------------------------------------------------------------------------- | -------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- | --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| primární klíč    | -                     | [PrimaryKey("Id", AutoIncrement = true)])],<br>na třídě,<br>automaticky property Id | [Primary]<br>na property               | [PrimaryKey, Identity]<br>na property<br><br>HasPrimaryKey(x => x.OrderID)       | `<id name="OrderID" column="" type=""><generator class="identity" /></id>`                                                                                                             | [Key]                                                                                                                                                                                                                                               |
| cizí klíč        | -                     | -                                                                                   | -                                      | v [Association]<br>fluent taky                                                   | v many-to-one                                                                                                                                                                          | [ForeignKey(nameof())]                                                                                                                                                                                                                              |
| tabulka a schéma | -                     | [TableName("x.y")]                                                                  | [Table("y", Schema = "x")]             | Table(Schema = "y", Name = "y")<br><br>.HasSchemaName("x")<br>.HasTableName("y") | `<class name="Order" table="x" schema="y">`                                                                                                                                            | [Table("y", Schema = "x")]                                                                                                                                                                                                                          |
| DB typ           | odvezení dle property | dle property                                                                        | [DbType(System.Data.DbType.DateTime2)] | [DataType(LinqToDB.DataType.DateTime2)]<br><br>.HasDbType                        | type="decimal"                                                                                                                                                                         | [Column(TypeName="")]                                                                                                                                                                                                                               |
| precision        | -                     | -                                                                                   | [Precision]                            | v Column<br><br>.HasPrecision                                                    | precision="18" scale="2"                                                                                                                                                               | [Precision(18,2)]                                                                                                                                                                                                                                   |
| nullability      | odvození dle property | dle property                                                                        | [IsNullable(true)]                     | [Nullable]<br><br>.IsNullable()                                                  | not-null="false"                                                                                                                                                                       | dle property                                                                                                                                                                                                                                        |
| one-to-one       | -<br>(join + splitOn) | -<br>(join + Fetch<T1,T2>)                                                          | -<br>(QueryMultiple)                   | [Association]<br>- reference na klíče na obou stranách                           | jako níže jen one-to-one                                                                                                                                                               | nastavením Key a ForeignKey na obou stranách + typ property (jedna vs kolekce)                                                                                                                                                                      |
| one-to-many      | -<br>(join + splitOn) | -<br>(join + Fetch<T1,T2>)                                                          | -<br>(QueryMultiple)<br>               | [Association]<br>- reference na klíče na obou stranách                           | `<bag name="Transactions" ><key column="CustomerID" /><one-to-many class="CustomerTransaction" /</bag>`                                                                                | nastavením Key a ForeignKey na obou stranách + typ property (jedna vs kolekce)                                                                                                                                                                      |
| many-to-many     | -<br>(join + splitOn) | -<br>(join + Fetch<T1,T2>)                                                          | -<br>(raw SQL + in memory)             | [Association]<br>- reference na klíče na obou stranách                           | `<bag name="StockGroups" schema="Warehouse" table="StockItemStockGroups" cascade="none"> <key column="StockItemID" /> <many-to-many class="StockGroup" column="StockGroupID" /></bag>` | Fluent definice join table:<br>UsingEntity(<br> "Warehouse.StockItemStockGroups",<br> l => l.HasOne(typeof(StockGroup)).WithMany().HasForeignKey("StockGroupID"),<br> r => r.HasOne(typeof(StockItem)).WithMany().HasForeignKey("StockItemID")<br>) |
