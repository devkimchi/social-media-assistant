namespace SocialMediaAssistant.Services.OpenAI.Configurations;

/// <summary>
/// This represents the settings entity for the prompt.
/// </summary>
public class PromptSettings
{
    /// <summary>
    /// Gets the name of the configuration.
    /// </summary>
    public const string Name = "Prompt";

    /// <summary>
    /// Gets or sets the system prompt.
    /// </summary>
    public virtual string? System { get; set; }

    /// <summary>
    /// Gets or sets the few-shot prompts as users.
    /// </summary>
    public virtual List<string> Users { get; set; } = new();

    /// <summary>
    /// Gets or sets the few-shot prompts as assistants.
    /// </summary>
    public virtual List<string> Assistants { get; set; } = new();
}
