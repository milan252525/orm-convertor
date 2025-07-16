using OrmConvertor;
using ORMConvertorAPI.Data;
using ORMConvertorAPI.Dtos;

namespace ORMConvertorAPI;

public static class Endpoints
{
    public static void Map(WebApplication app)
    {
        app.MapGet("/required-content", () => RequiredContent.GetRequiredContent)
            .WithName("RequiredContent")
            .Produces<List<RequiredContentDefinition>>(StatusCodes.Status200OK)
            .WithOpenApi();

        app.MapGet("/required-content-advisor", () => RequiredContent.GetRequiredContentAdvisor)
            .WithName("RequiredContentAdvisor")
            .Produces<List<RequiredContentDefinition>>(StatusCodes.Status200OK)
            .WithOpenApi();

        app.MapPost("/convert", ConvertHandler)
           .WithName("Convert")
           .Produces<ConvertResponse>(StatusCodes.Status200OK)
           .ProducesProblem(StatusCodes.Status400BadRequest)
           .WithOpenApi();

        app.MapGet("/samples", () => Samples.GetSamples)
            .Produces<Dictionary<int, string>>(StatusCodes.Status200OK)
            .WithOpenApi();
    }

    private static IResult ConvertHandler(ConvertRequest req)
    {
        try
        {
            var converted = ConversionHandler.Convert(req.SourceOrm, req.TargetOrm, req.Sources);
            return Results.Ok(new ConvertResponse(converted));
        }
        catch (Exception e)
        {
            return Results.BadRequest(e.Message);
        }
    }
}
