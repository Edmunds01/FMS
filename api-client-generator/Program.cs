// See https://aka.ms/new-console-template for more information
using System;
using System.IO;
using System.Threading.Tasks;
using NJsonSchema.CodeGeneration.TypeScript;
using NSwag;
using NSwag.CodeGeneration.CSharp;
using NSwag.CodeGeneration.TypeScript;

if (args.Length != 2) throw new ArgumentException("Expecting 2 arguments: URL, generatePath, language");

var url = args[0];
var generatePath = args[1];

var document = await OpenApiDocument.FromUrlAsync(url);
Console.WriteLine($"Generating {generatePath}...");

await System.IO.File.WriteAllTextAsync(generatePath, GenerateTypeScriptClient(document));

static string GenerateTypeScriptClient(OpenApiDocument document)
{
    var settings = new TypeScriptClientGeneratorSettings
    {
        TypeScriptGeneratorSettings =
        {
            TypeStyle = TypeScriptTypeStyle.Interface,
            TypeScriptVersion = 3.5M,
            DateTimeType = TypeScriptDateTimeType.Date,
            ExtensionCode = "export const api = new Client(\"https://localhost:5000\");",
        },
        WithCredentials = true
    };
    var generator = new TypeScriptClientGenerator(document, settings);
    
    return generator.GenerateFile();
}
