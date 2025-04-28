﻿namespace Model.AbstractRepresentation;

public class EntityMap
{
    public required Entity Entity { get; set; }

    public string? Table { get; set; }

    public string? Schema { get; set; }

    public List<PropertyMap> PropertyMaps { get; set; } = [];
}
