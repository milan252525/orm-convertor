using Model;

namespace ORMConvertorAPI.Dtos;

internal record ConvertRequest(
    ORMEnum SourceOrm,
    ORMEnum TargetOrm,
    List<ConversionSource> Sources
);
