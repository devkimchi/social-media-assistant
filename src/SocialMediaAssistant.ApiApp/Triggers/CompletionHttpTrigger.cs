using System.Net;

using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;

using SocialMediaAssistant.Models.Examples;
using SocialMediaAssistant.Services.OpenAI;

namespace SocialMediaAssistant.ApiApp.Triggers;

/// <summary>
/// This represents the HTTP trigger entity for ChatGPT completion.
/// </summary>
public class CompletionHttpTrigger
{
    private readonly IOpenAIService _service;
    private readonly ILogger _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="CompletionHttpTrigger"/> class.
    /// </summary>
    /// <param name="service"><see cref="IOpenAIService"/> instance.</param>
    /// <param name="loggerFactory"><see cref="ILoggerFactory"/> instance.</param>
    public CompletionHttpTrigger(IOpenAIService service, ILoggerFactory loggerFactory)
    {
        this._service = service.ThrowIfNullOrDefault();
        this._logger = loggerFactory.ThrowIfNullOrDefault().CreateLogger<CompletionHttpTrigger>();
    }

    /// <summary>
    /// Invokes the HTTP trigger that completes the prompt.
    /// </summary>
    /// <param name="req"><see cref="HttpRequestData"/> instance.</param>
    /// <returns><see cref="HttpResponseData"/> instance.</returns>
    [Function(nameof(CompletionHttpTrigger.GetCompletionsAsync))]
    [OpenApiOperation(operationId: "getCompletions", tags: new[] { "completions" }, Summary = "Gets the completion from the OpenAI API", Description = "This gets the completion from the OpenAI API.", Visibility = OpenApiVisibilityType.Important)]
    [OpenApiSecurity(schemeName: "function_key", schemeType: SecuritySchemeType.ApiKey, Name = "x-functions-key", In = OpenApiSecurityLocationType.Header)]
    [OpenApiParameter(name: "locale", In = ParameterLocation.Query, Required = false, Type = typeof(string), Summary = "The locale to generate the completion.", Description = "This specifies the locale to generate the completion. The default value is `en-us`.")]
    [OpenApiParameter(name: "social", In = ParameterLocation.Query, Required = false, Type = typeof(string), Summary = "The social media to generate the completion.", Description = "This specifies the social media to generate the completion. The default value is `twitter`.")]
    [OpenApiRequestBody(contentType: "text/plain", bodyType: typeof(string), Required = true, Example = typeof(PromptExample), Description = "The prompt to generate the completion.")]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/plain", bodyType: typeof(string), Example = typeof(CompletionExample), Summary = "The completion generated from the OpenAI API.", Description = "This returns the completion generated from the OpenAI API.")]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.BadRequest, contentType: "text/plain", bodyType: typeof(string), Example = typeof(BadRequestExample), Summary = "Invalid request.", Description = "This indicates the request is invalid.")]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.InternalServerError, contentType: "text/plain", bodyType: typeof(string), Example = typeof(InternalServerErrorExample), Summary = "Internal server error.", Description = "This indicates the server is not working as expected.")]
    public async Task<HttpResponseData> GetCompletionsAsync(
        [HttpTrigger(AuthorizationLevel.Function, "POST", Route = "completions")] HttpRequestData req
    )
    {
        this._logger.LogInformation("C# HTTP trigger function processed a request.");

        var locale = req.Query["locale"] ?? "en-us";
        var social = req.Query["social"] ?? "twitter";

        var response = default(HttpResponseData);
        var prompt = req.ReadAsString();
        if (prompt.IsNullOrWhiteSpace())
        {
            this._logger.LogError("No prompt");

            response = req.CreateResponse(HttpStatusCode.BadRequest);
            response.Headers.Add("Content-Type", "text/plain; charset=utf-8");

            response.WriteString("The prompt is required.");

            return response;
        }

        try
        {
            var socialMedia = Enum.TryParse<SocialMediaType>(social, ignoreCase: true, out var result) ? result : SocialMediaType.Twitter;
            var message = await this._service.GetCompletionsAsync(prompt, socialMedia, locale);

            this._logger.LogInformation(message);

            response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Type", "text/plain; charset=utf-8");

            response.WriteString(message);
        }
        catch (Exception ex)
        {
            this._logger.LogError(ex, ex.Message);

            response = req.CreateResponse(HttpStatusCode.InternalServerError);
            response.Headers.Add("Content-Type", "text/plain; charset=utf-8");

            response.WriteString("Internal server error.");
        }

        return response;
    }
}
