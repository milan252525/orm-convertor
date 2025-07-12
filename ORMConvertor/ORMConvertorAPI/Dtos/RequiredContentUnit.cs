using Model;

namespace ORMConvertorAPI.Dtos;

public record RequiredContentUnit(
    int Id, 
    ConversionContentType ContentType, 
    string Description
);
