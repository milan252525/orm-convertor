using ORMConvertor.Generators;
using ORMConvertor.Parsers;

namespace ORMConvertor
{
    public class Program
    {
        static void Main(string[] args)
        {
            string property = "public required int? Number { get; set; }";

            Console.WriteLine(property);
            Console.WriteLine();

            IPropertyParser propertyParser = new PropertyParser();

            var abstractRepr = propertyParser.Parse(property);

            Console.WriteLine(abstractRepr);
            Console.WriteLine();

            ICodeGenerationVisitor<string> codeGenerationVisitor = new PropertyGenerator();
            string generatedCode = abstractRepr.Accept(codeGenerationVisitor);

            Console.WriteLine(generatedCode);
            Console.WriteLine();
        }
    }
}
