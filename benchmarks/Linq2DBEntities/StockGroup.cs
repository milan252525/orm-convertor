﻿namespace linq2dbEntities;

public class StockGroup
{
    public int StockGroupID { get; set; }

    public required string StockGroupName { get; set; }

    public List<StockItemStockGroup> StockItems { get; set; } = [];
}
