﻿using PetaPoco;

namespace PetaPocoEntities;

[TableName("Sales.OrderLines")]
public class OrderLine
{
    public int OrderLineID { get; set; }

    public int OrderID { get; set; }

    public int StockItemID { get; set; }

    public required string Description { get; set; }

    public int PackageTypeID { get; set; }

    public decimal Quantity { get; set; }

    public decimal? UnitPrice { get; set; }

    public decimal TaxRate { get; set; }

    public int PickedQuantity { get; set; }

    public DateTime? PickingCompletedWhen { get; set; }

    public decimal LastEditedBy { get; set; }

    public DateTime LastEditedWhen { get; set; }
}
