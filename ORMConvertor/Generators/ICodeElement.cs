namespace ORMConvertor.Generators;
public interface ICodeElement
{
    T Accept<T>(ICodeGenerationVisitor<T> visitor);
}
