namespace Model.APIModels;

public record RequiredContentUnit(
    int Id, 
    ConversionContentType ContentType, 
    string Description
);
