namespace SocialMediaAssistant.Services.OpenAI.Configurations;

/// <summary>
/// This represents the settings entity for the OpenAI API.
/// </summary>
public class OpenAIApiSettings
{
    /// <summary>
    /// Gets the name of the configuration.
    /// </summary>
    public const string Name = "OpenAIApi";

    /// <summary>
    /// Gets or sets the model deployment ID.
    /// </summary>
    public virtual string? DeploymentId { get; set; }

    /// <summary>
    /// Gets or sets the API endpoint.
    /// </summary>
    public virtual string? Endpoint { get; set; }

    /// <summary>
    /// Gets or sets the API key.
    /// </summary>
    public virtual string? AuthKey { get; set; }

    /// <summary>
    /// Gets or sets the maximum number of tokens.
    /// </summary>
    public virtual int? MaxTokens { get; set; }

    /// <summary>
    /// Gets or sets the temperature.
    /// </summary>
    public virtual float? Temperature { get; set; }
}
