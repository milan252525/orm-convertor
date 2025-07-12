using Model;

namespace ORMConvertorAPI.Dtos;

public record RequiredContentDefinition(
    ORMEnum OrmType, 
    List<RequiredContentUnit> Required
);

