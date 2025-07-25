﻿using System.ComponentModel.DataAnnotations.Schema;

namespace RepoDBEntities;

[Table("StockGroups", Schema = "Warehouse")]
public class StockGroup
{
    public int StockGroupID { get; set; }

    public required string StockGroupName { get; set; }

    public List<StockItem> StockItems { get; set; } = [];
}
