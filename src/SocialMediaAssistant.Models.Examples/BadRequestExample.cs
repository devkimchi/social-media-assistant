using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Resolvers;

using Newtonsoft.Json.Serialization;

namespace SocialMediaAssistant.Models.Examples;

/// <summary>
/// This represents the example entity for `Bad Request`.
/// </summary>
public class BadRequestExample : OpenApiExample<string>
{
    /// <inheritdoc />
    public override IOpenApiExample<string> Build(NamingStrategy namingStrategy = null)
    {
        var error = "The prompt is required.";

        this.Examples.Add(OpenApiExampleResolver.Resolve("error", error, namingStrategy));

        return this;
    }
}
