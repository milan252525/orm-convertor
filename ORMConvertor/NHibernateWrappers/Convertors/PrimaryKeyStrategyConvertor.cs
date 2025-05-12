using Model.AbstractRepresentation.Enums;

namespace NHibernateWrappers.Convertors;
public class PrimaryKeyStrategyConvertor
{
    public static PrimaryKeyStrategy FromNHibernate(string? strategy)
    {
        return strategy switch
        {
            "None" => PrimaryKeyStrategy.None,
            "Increment" => PrimaryKeyStrategy.Increment,
            "Identity" => PrimaryKeyStrategy.Identity,
            "Sequence" => PrimaryKeyStrategy.Sequence,
            "HiLo" => PrimaryKeyStrategy.HiLo,
            "Uuid" => PrimaryKeyStrategy.Uuid,
            "Guid" => PrimaryKeyStrategy.Guid,
            _ => throw new NotImplementedException()
        };
    }
}
