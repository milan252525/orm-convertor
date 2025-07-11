namespace Model.APIModels;

public record RequiredContentDefinition(
    ORMEnum OrmType, 
    List<RequiredContentUnit> Required
);

