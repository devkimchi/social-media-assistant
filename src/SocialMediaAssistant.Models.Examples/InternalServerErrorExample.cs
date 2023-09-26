using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Resolvers;

using Newtonsoft.Json.Serialization;

namespace SocialMediaAssistant.Models.Examples;

/// <summary>
/// This represents the example entity for `Internal Server Error`.
/// </summary>
public class InternalServerErrorExample : OpenApiExample<string>
{
    /// <inheritdoc />
    public override IOpenApiExample<string> Build(NamingStrategy namingStrategy = null)
    {
        var error = "Internal server error.";

        this.Examples.Add(OpenApiExampleResolver.Resolve("error", error, namingStrategy));

        return this;
    }
}
