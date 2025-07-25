﻿using Common;
using linq2dbEntities;
using linq2dbEntities.Models;
using LinqToDB;
using LinqToDB.Data;

namespace linq2dbFeatures
{
    [Collection("linq2db")]
    public class FeatureTests
    {
        private WWIDbConnection GetConnection()
        {
            var options = new DataOptions()
                .UseSqlServer(DatabaseConfig.MSSQLConnectionString);

            return new WWIDbConnection(options);
        }

        public FeatureTests(ITestOutputHelper output)
        {
            DataConnection.TurnTraceSwitchOn();
            DataConnection.WriteTraceLine = (message, category, level) => { output.WriteLine($"[{level}] {category} {message}"); };
        }

        [Fact]
        public void A1_EntityIdenticalToTable()
        {
            using var db = GetConnection();

            var order = db.PurchaseOrders.FirstOrDefault(x => x.PurchaseOrderID == 25);

            Assert.NotNull(order);
            Assert.Multiple(() =>
            {
                Assert.Equal(25, order.PurchaseOrderID);
                Assert.Equal(12, order.SupplierID);
                Assert.Equal(new DateTime(2013, 1, 5), order.OrderDate);
                Assert.Equal(7, order.DeliveryMethodID);
                Assert.Equal(2, order.ContactPersonID);
                Assert.Equal(new DateTime(2013, 1, 25), order.ExpectedDeliveryDate);
                Assert.Equal("237408032", order.SupplierReference);
                Assert.True(order.IsOrderFinalized);
                Assert.Null(order.Comments);
                Assert.Null(order.InternalComments);
                Assert.Equal(14, order.LastEditedBy);
                Assert.Equal(new DateTime(2013, 1, 7, 7, 0, 0), order.LastEditedWhen);
            });
        }

        [Fact]
        public void A2_LimitedEntity()
        {
            using var db = GetConnection();

            var contactInfo = db.Suppliers
            .Where(s => s.SupplierID == 10)
            .Select(s => new SupplierContactInfo
            {
                SupplierID = s.SupplierID,
                SupplierName = s.SupplierName,
                PhoneNumber = s.PhoneNumber,
                FaxNumber = s.FaxNumber,
                WebsiteURL = s.WebsiteURL,
                ValidFrom = s.ValidFrom,
                ValidTo = s.ValidTo
            })
            .Single();

            Assert.Multiple(() =>
            {
                Assert.Equal(10, contactInfo.SupplierID);
                Assert.Equal("Northwind Electric Cars", contactInfo.SupplierName);
                Assert.Equal("(201) 555-0105", contactInfo.PhoneNumber);
                Assert.Equal("(201) 555-0104", contactInfo.FaxNumber);
                Assert.Equal("http://www.northwindelectriccars.com", contactInfo.WebsiteURL);
                Assert.Equal(new DateTime(2013, 1, 1, 0, 5, 0), contactInfo.ValidFrom);
                Assert.Equal(DateTime.MaxValue, contactInfo.ValidTo);
            });
        }

        [Fact]
        public void A3_MultipleEntitiesFromOneResult()
        {
            using var db = GetConnection();

            var result = db.Suppliers
            .Where(s => s.SupplierID == 10)
            .Select(s => new
            {
                ContactInfo = new SupplierContactInfo
                {
                    SupplierID = s.SupplierID,
                    SupplierName = s.SupplierName,
                    PhoneNumber = s.PhoneNumber,
                    FaxNumber = s.FaxNumber,
                    WebsiteURL = s.WebsiteURL,
                    ValidFrom = s.ValidFrom,
                    ValidTo = s.ValidTo
                },
                BankAccount = new SupplierBankAccount
                {
                    SupplierID = s.SupplierID,
                    BankAccountName = s.BankAccountName,
                    BankAccountBranch = s.BankAccountBranch,
                    BankAccountCode = s.BankAccountCode,
                    BankAccountNumber = s.BankAccountNumber,
                    BankInternationalCode = s.BankInternationalCode
                }
            })
            .Select(result => new { result.ContactInfo, result.BankAccount })
            .Single();

            var (contactInfo, bankAccount) = (result.ContactInfo, result.BankAccount);

            Assert.Multiple(() =>
            {
                Assert.Equal(10, contactInfo.SupplierID);
                Assert.Equal("Northwind Electric Cars", contactInfo.SupplierName);
                Assert.Equal("(201) 555-0105", contactInfo.PhoneNumber);
                Assert.Equal("(201) 555-0104", contactInfo.FaxNumber);
                Assert.Equal("http://www.northwindelectriccars.com", contactInfo.WebsiteURL);
                Assert.Equal(new DateTime(2013, 1, 1, 0, 5, 0), contactInfo.ValidFrom);
                Assert.Equal(DateTime.MaxValue, contactInfo.ValidTo);

                Assert.Equal(10, bankAccount.SupplierID);
                Assert.Equal("Northwind Electric Cars", bankAccount.BankAccountName);
                Assert.Equal("Woodgrove Bank Crandon Lakes", bankAccount.BankAccountBranch);
                Assert.Equal("325447", bankAccount.BankAccountCode);
                Assert.Equal("3258786987", bankAccount.BankAccountNumber);
                Assert.Equal("36214", bankAccount.BankInternationalCode);
            });
        }

        [Fact]
        public void A4_StoredProcedureToEntity()
        {
            using var db = GetConnection();

            var from = new DateTime(2014, 1, 1);
            var to = new DateTime(2015, 1, 1);

            var orders = db.QueryProc<PurchaseOrderUpdate>(
                    "WideWorldImporters.Integration.GetOrderUpdates",
                    new DataParameter("LastCutoff", from),
                    new DataParameter("NewCutoff", to)
                ).ToList();

            Assert.Multiple(() =>
            {
                Assert.Equal(66741, orders.Count);
                Assert.True(orders.All(o => o.Quantity > 0));
                Assert.True(orders.All(o => o.TaxRate > 0));
                Assert.True(orders.All(o => o.UnitPrice > 0));
            });

            var firstOrder = orders.First();
            Assert.Multiple(() =>
            {
                Assert.Equal(1219, firstOrder.OrderID);
                Assert.Equal("\"The Gu\" red shirt XML tag t-shirt (White) XS", firstOrder.Description);
                Assert.Equal(96, firstOrder.Quantity);
                Assert.Equal(18.00m, firstOrder.UnitPrice);
                Assert.Equal(15m, firstOrder.TaxRate);
                Assert.Equal(1987.2m, firstOrder.TotalIncludingTax);
            });
        }

        [Fact]
        public void B1_SelectionOverIndexedColumn()
        {
            using var db = GetConnection();

            int orderId = 26866;

            var orderLines = db.OrderLines
                .Where(ol => ol.OrderID == orderId)
                .ToList();

            Assert.Equal(5, orderLines.Count);
            Assert.True(orderLines.All(ol => ol.OrderID == orderId));

            var expectedFirstOrderLine = new OrderLine
            {
                OrderLineID = 85261,
                OrderID = orderId,
                StockItemID = 141,
                Description = "Furry animal socks (Pink) XL",
                PackageTypeID = 10,
                Quantity = 48,
                UnitPrice = 5m,
                TaxRate = 15m,
                PickedQuantity = 48,
                PickingCompletedWhen = new DateTime(2014, 5, 14, 11, 0, 0),
                LastEditedBy = 19,
                LastEditedWhen = new DateTime(2014, 5, 14, 11, 0, 0)
            };

            var actualOrderLine = orderLines.First(ol => ol.OrderLineID == 85261);

            Assert.Multiple(() =>
            {
                Assert.Equal(expectedFirstOrderLine.OrderLineID, actualOrderLine.OrderLineID);
                Assert.Equal(expectedFirstOrderLine.OrderID, actualOrderLine.OrderID);
                Assert.Equal(expectedFirstOrderLine.StockItemID, actualOrderLine.StockItemID);
                Assert.Equal(expectedFirstOrderLine.Description, actualOrderLine.Description);
                Assert.Equal(expectedFirstOrderLine.PackageTypeID, actualOrderLine.PackageTypeID);
                Assert.Equal(expectedFirstOrderLine.Quantity, actualOrderLine.Quantity);
                Assert.Equal(expectedFirstOrderLine.UnitPrice, actualOrderLine.UnitPrice);
                Assert.Equal(expectedFirstOrderLine.TaxRate, actualOrderLine.TaxRate);
                Assert.Equal(expectedFirstOrderLine.PickedQuantity, actualOrderLine.PickedQuantity);
                Assert.Equal(expectedFirstOrderLine.PickingCompletedWhen, actualOrderLine.PickingCompletedWhen);
                Assert.Equal(expectedFirstOrderLine.LastEditedBy, actualOrderLine.LastEditedBy);
                Assert.Equal(expectedFirstOrderLine.LastEditedWhen, actualOrderLine.LastEditedWhen);
            });
        }

        [Fact]
        public void B2_SelectionOverNonIndexedColumn()
        {
            using var db = GetConnection();

            decimal unitPrice = 25m;

            var orderLines = db.OrderLines
                .Where(ol => ol.UnitPrice == unitPrice)
                .ToList();

            Assert.Equal(13553, orderLines.Count);
            Assert.True(orderLines.All(ol => ol.UnitPrice == unitPrice));
        }

        [Fact]
        public void B3_RangeQuery()
        {
            using var db = GetConnection();

            var from = new DateTime(2014, 12, 20);
            var to = new DateTime(2014, 12, 31);

            var orderLines = db.OrderLines
                .Where(ol => ol.PickingCompletedWhen >= from && ol.PickingCompletedWhen <= to)
                .ToList();

            Assert.Equal(1883, orderLines.Count);
            Assert.True(orderLines.All(ol => ol.PickingCompletedWhen >= from && ol.PickingCompletedWhen <= to));
        }

        [Fact]
        public void B4_InQuery()
        {
            using var db = GetConnection();

            var orderIds = new[] { 1, 10, 100, 1000, 10000 };

            var orderLines = db.OrderLines
                .Where(ol => orderIds.Contains(ol.OrderID))
                .ToList();

            Assert.Equal(15, orderLines.Count);
            Assert.True(orderLines.All(ol => orderIds.Contains(ol.OrderID)));
        }

        [Fact]
        public void B5_TextSearch()
        {
            using var db = GetConnection();

            string text = "C++";

            var orderLines = db.OrderLines
                .Where(ol => ol.Description.Contains(text))
                .ToList();

            Assert.Equal(2098, orderLines.Count);
            Assert.True(orderLines.All(ol => ol.Description.Contains(text)));
        }

        [Fact]
        public void B6_PagingQuery()
        {
            using var db = GetConnection();

            int skip = 1000;
            int take = 50;

            var orderLines = db.OrderLines
                .OrderBy(ol => ol.OrderLineID)
                .Skip(skip)
                .Take(take)
                .ToList();

            Assert.Equal(50, orderLines.Count);
            Assert.Equal(Enumerable.Range(skip + 1, take), orderLines.Select(ol => ol.OrderLineID));
        }

        [Fact]
        public void C1_AggregationCount()
        {
            using var db = GetConnection();

            var taxRates = db.OrderLines
                .GroupBy(ol => ol.TaxRate)
                .Select(g => new { TaxRate = g.Key, Count = g.Count() })
                .OrderByDescending(x => x.Count)
                .ToDictionary(x => x.TaxRate, x => x.Count);

            Assert.Equal(2, taxRates.Count);
            Assert.Equal(230376, taxRates[15m]);
            Assert.Equal(1036, taxRates[10m]);
        }

        [Fact]
        public void C2_AggregationMax()
        {
            using var db = GetConnection();

            var maxUnitPrice = db.OrderLines.Max(ol => ol.UnitPrice);

            Assert.Equal(1899m, maxUnitPrice);
        }

        [Fact]
        public void C3_AggregationSum()
        {
            using var db = GetConnection();

            var totalSales = db.OrderLines
                .Sum(ol => ol.Quantity * ol.UnitPrice);

            Assert.Equal(177634276.4m, totalSales);
        }

        [Fact]
        public void D1_OneToManyRelationship()
        {
            using var db = GetConnection();

            var order = db.Orders
                .LoadWith(o => o.OrderLines)
                .Single(o => o.OrderID == 530);

            Assert.NotNull(order);

            var firstOrderLine = order.OrderLines.First();
            Assert.Multiple(() =>
            {
                Assert.Equal(1478, firstOrderLine.OrderLineID);
                Assert.Equal(530, firstOrderLine.OrderID);
                Assert.Equal(129, firstOrderLine.StockItemID);
                Assert.Equal("Plush shark slippers (Gray) XL", firstOrderLine.Description);
                Assert.Equal(7, firstOrderLine.PackageTypeID);
                Assert.Equal(7, firstOrderLine.Quantity);
                Assert.Equal(32.00m, firstOrderLine.UnitPrice);
                Assert.Equal(15.000m, firstOrderLine.TaxRate);
                Assert.Equal(7, firstOrderLine.PickedQuantity);
                Assert.Equal(new DateTime(2013, 1, 10, 11, 0, 0), firstOrderLine.PickingCompletedWhen);
                Assert.Equal(13, firstOrderLine.LastEditedBy);
                Assert.Equal(new DateTime(2013, 1, 10, 11, 0, 0), firstOrderLine.LastEditedWhen);

                Assert.Equal(530, order.OrderID);
                Assert.Equal(115, order.CustomerID);
                Assert.Equal(3, order.SalespersonPersonID);
                Assert.Equal(13, order.PickedByPersonID);
                Assert.Equal(1229, order.ContactPersonID);
                Assert.Null(order.BackorderOrderID);
                Assert.Equal(new DateTime(2013, 1, 10), order.OrderDate);
                Assert.Equal(new DateTime(2013, 1, 11), order.ExpectedDeliveryDate);
                Assert.Equal("19446", order.CustomerPurchaseOrderNumber);
                Assert.True(order.IsUndersupplyBackordered);
                Assert.Null(order.Comments);
                Assert.Null(order.DeliveryInstructions);
                Assert.Null(order.InternalComments);
                Assert.Equal(new DateTime(2013, 1, 10, 11, 0, 0), order.PickingCompletedWhen);
                Assert.Equal(13, order.LastEditedBy);
                Assert.Equal(new DateTime(2013, 1, 10, 11, 0, 0), order.LastEditedWhen);
            });
        }

        [Fact]
        public void D2_ManyToManyRelationship()
        {
            using var db = GetConnection();

            var stockItems = db.StockItems
                .LoadWith(si => si.StockGroups)
                .ThenLoad(sisg => sisg.StockGroup)
                .OrderBy(si => si.StockItemID)
                .ToList();

            var stockGroups = db.StockGroups 
                .LoadWith(sg => sg.StockItems)
                .ThenLoad(sisg => sisg.StockItem)
                .OrderBy(si => si.StockGroupID)
                .ToList();

            Assert.Multiple(() =>
            {
                Assert.Equal(227, stockItems.Count);
                Assert.Equal(10, stockGroups.Count);
            });
            
            Assert.Multiple(() =>
            {
                Assert.Equal(1, stockItems[0].StockItemID);
                Assert.Equal("USB missile launcher (Green)", stockItems[0].StockItemName);
                Assert.Equal(12, stockItems[0].SupplierID);

                Assert.Equal(3, stockItems[0].StockGroups.Count);
                var groupNames = stockItems[0].StockGroups.Select(sisg => sisg.StockGroup.StockGroupName).Order().ToList();
                Assert.Equal(["Computing Novelties", "Novelty Items", "USB Novelties"], groupNames);
            });

            Assert.Multiple(() =>
            {
                Assert.Equal(1, stockGroups[0].StockGroupID);
                Assert.Equal("Novelty Items", stockGroups[0].StockGroupName);
                Assert.Equal(91, stockGroups[0].StockItems.Count);
            });

            var group1ItemIds = stockItems
                .Where(si => si.StockGroups.Any(sisg => sisg.StockGroup.StockGroupID == 1))
                .Select(si => si.StockItemID)
                .ToList();

            Assert.Equal(group1ItemIds, stockGroups[0].StockItems.Select(sisg => sisg.StockItemID).Order().ToList());
        }

        [Fact]
        public void D3_OptionalRelationship()
        {
            using var db = GetConnection();

            var result = db.Customers
            .LoadWith(c => c.Transactions)
            .OrderBy(c => c.CustomerID)
            .ToList();

            Assert.Equal(663, result.Count);
            Assert.Equal(400, result.Where(c => c.Transactions.Count == 0).Count());
        }

        [Fact]
        public void E1_ColumnSorting()
        {
            using var db = GetConnection();

            var orders = db.PurchaseOrders
                .OrderBy(po => po.ExpectedDeliveryDate)
                .Take(1000)
                .ToList();

            Assert.Equal(1000, orders.Count);
            Assert.Equal(new DateTime(2013, 1, 15), orders.First().ExpectedDeliveryDate);
            Assert.Equal(new DateTime(2014, 9, 17), orders.Last().ExpectedDeliveryDate);
            Assert.True(orders.SequenceEqual(orders.OrderBy(o => o.ExpectedDeliveryDate)));
        }

        [Fact]
        public void E2_Distinct()
        {
            using var db = GetConnection();

            var supplierReferences = db.PurchaseOrders
                .Select(po => po.SupplierReference)
                .Distinct()
                .ToList();

            Assert.Equal(7, supplierReferences.Count);
            string[] expected = ["AA20384", "BC0280982", "ML0300202", "293092", "08803922", "237408032", "B2084020"];
            Assert.Equal(expected, supplierReferences);
        }

        [Fact]
        public void F1_JSONObjectQuery()
        {
            using var db = GetConnection();

            var sql = """
                SELECT PersonID, FullName, PreferredName, EmailAddress, CustomFields, OtherLanguages
                FROM WideWorldImporters.Application.People
                WHERE JSON_VALUE(CustomFields, '$.Title') = @Title
                ORDER BY PersonID
            """;

            var people = db.Query<Person>(
                sql,
                new DataParameter("Title", "Team Member")
            ).ToList();

            Assert.Equal(13, people.Count);
            Assert.All(people, person => Assert.Equal("Team Member", person.CustomFields?.Title));

            var first = people.First();
            Assert.Equal("Kayla Woodcock", first.FullName);
            Assert.Equal("Kayla", first.PreferredName);
            Assert.Equal("kaylaw@wideworldimporters.com", first.EmailAddress);
            Assert.Equal(new DateTime(2008, 4, 19), first.CustomFields?.HireDate);
        }

        [Fact]
        public void F2_JSONArrayQuery()
        {
            using var db = GetConnection();

            var sql = """
                SELECT PersonID, FullName, PreferredName, EmailAddress, CustomFields, OtherLanguages
                FROM WideWorldImporters.Application.People
                WHERE EXISTS (
                    SELECT 1
                    FROM OPENJSON(OtherLanguages)
                    WHERE value = @Language
                )
            """;

            var people = db.Query<Person>(
                sql,
                new DataParameter("Language", "Slovak")
            ).ToList();

            Assert.Equal(2, people.Count);
            Assert.All(people, person => Assert.Contains("Slovak", person.OtherLanguages!));

            var first = people.First();
            Assert.Equal("Amy Trefl", first.FullName);
            Assert.Equal(["Slovak", "Spanish", "Polish"], first.OtherLanguages);
        }

        [Fact]
        public void G1_Union()
        {
            using var db = GetConnection();

            var first = db.Suppliers
                .Where(s => s.SupplierID < 5)
                .Select(s => s.SupplierID)
                .ToList();

            var last = db.Suppliers
                .Where(s => s.SupplierID >= 5 && s.SupplierID <= 10)
                .Select(s => s.SupplierID)
                .ToList();

            var suppliers = first
                .Union(last)
                .OrderBy(s => s)
                .ToList();

            Assert.Equal(10, suppliers.Count);
            Assert.Equal(Enumerable.Range(1, 10), suppliers);
        }

        [Fact]
        public void G2_Intersection()
        {
            using var db = GetConnection();

            var first = db.Suppliers
                .Where(s => s.SupplierID < 10)
                .Select(s => s.SupplierID)
                .ToList();

            var last = db.Suppliers
                .Where(s => s.SupplierID >= 5 && s.SupplierID <= 15)
                .Select(s => s.SupplierID)
                .ToList();

            var suppliers = first
                .Intersect(last)
                .OrderBy(s => s)
                .ToList();

            Assert.Equal([5, 6, 7, 8, 9], suppliers);
        }

        [Fact]
        public void H1_Metadata()
        {
            using var db = GetConnection();

            var datatype = db.Query<string>(
                """
                    SELECT DATA_TYPE FROM INFORMATION_SCHEMA.COLUMNS 
                        WHERE TABLE_SCHEMA = 'Purchasing'
                        AND TABLE_NAME = 'Suppliers'
                        AND COLUMN_NAME = 'SupplierReference'
                """
            ).Single();

            Assert.Equal("nvarchar", datatype);
        }
    }
}
