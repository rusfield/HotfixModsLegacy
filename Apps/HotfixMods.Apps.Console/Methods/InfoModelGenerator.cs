using System.Reflection;

namespace HotfixMods.Apps.Console.Methods;

public sealed class InfoModelGenerator
{
    public void Generate(Type dtoType)
    {
        var properties = dtoType.GetProperties();

        foreach (var property in properties)
        {
            var type = property.PropertyType;
            if (type.IsGenericType)
            {
                type = type.GetGenericArguments()[0];
            }

            if (type.IsValueType)
                continue;

            System.Console.WriteLine("namespace HotfixMods.Infrastructure.InfoModels");
            System.Console.WriteLine("{");
            System.Console.WriteLine($"public class {type.Name}Info : IInfoModel");
            System.Console.WriteLine("{");

            var db2Properties = type.GetProperties();
            foreach (var db2Property in db2Properties)
            {
                if (db2Property.Name == "VerifiedBuild" || db2Property.Name == "Id")
                    continue;

                System.Console.WriteLine($"public string {db2Property.Name} {{ get; set; }} = \"TODO\";");
            }

            System.Console.WriteLine();
            System.Console.WriteLine("public string ModelInfo { get; set; } = \"TODO\";");
            System.Console.WriteLine("public bool IsRequired { get; set; } = false;");
            System.Console.WriteLine("}");
            System.Console.WriteLine("}");
            System.Console.WriteLine();
        }
    }
}
