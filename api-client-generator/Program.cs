// See https://aka.ms/new-console-template for more information
using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using NJsonSchema.CodeGeneration;
using NJsonSchema.CodeGeneration.TypeScript;
using NSwag;
using NSwag.CodeGeneration.CSharp;
using NSwag.CodeGeneration.TypeScript;

if (args.Length != 2) throw new ArgumentException("Expecting 2 arguments: URL, generatePath");
var url = args[0];
var generatePath = args[1];

var document = await OpenApiDocument.FromUrlAsync(url);
Console.WriteLine($"Generating {generatePath}...");

await System.IO.File.WriteAllTextAsync(generatePath, GenerateTypeScriptClient(document));

Console.WriteLine($"Finished for {generatePath}...");

static string GenerateTypeScriptClient(OpenApiDocument document)
{
    var settings = new TypeScriptClientGeneratorSettings
    {
        TypeScriptGeneratorSettings =
        {
            TypeStyle = TypeScriptTypeStyle.Interface,
            TypeScriptVersion = 3.5M,
            DateTimeType = TypeScriptDateTimeType.Date,
            MarkOptionalProperties = false,
            ExtensionCode = """
            const config: IConfig = {
                getAuthorization: () => {
                    return "Bearer " + localStorage.getItem("token");
                }
            }

            const apiUrl = import.meta.env.VITE_API_URL || "https://localhost:5000";
            export const api = new Client(config, apiUrl);

            import { AuthorizedApiBase, IConfig } from "./authorized-api-base";

            """,
        },
        WithCredentials = true,
        ConfigurationClass = "IConfig",
        ClientBaseClass = "AuthorizedApiBase",
        UseTransformOptionsMethod = true,
    };
    var generator = new TypeScriptClientGenerator(document, settings);

    return generator.GenerateFile();
}
