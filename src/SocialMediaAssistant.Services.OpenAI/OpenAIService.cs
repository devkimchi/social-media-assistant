using Azure;
using Azure.AI.OpenAI;

using SocialMediaAssistant.Services.OpenAI.Configurations;

namespace SocialMediaAssistant.Services.OpenAI;

public interface IOpenAIService
{
    Task<string> GetCompletionsAsync(string prompt, SocialMediaType socialMedia = SocialMediaType.Twitter, string locale = "en-us");
}

public class OpenAIService : IOpenAIService
{
    private readonly OpenAIApiSettings _openAISettings;
    private readonly PromptSettings _promptSettings;

    public OpenAIService(OpenAIApiSettings openAISettings, PromptSettings promptSettings)
    {
        this._openAISettings = openAISettings ?? throw new ArgumentNullException(nameof(openAISettings));
        this._promptSettings = promptSettings ?? throw new ArgumentNullException(nameof(promptSettings));
    }

    public async Task<string> GetCompletionsAsync(string prompt, SocialMediaType socialMedia = SocialMediaType.Twitter, string locale = "en-us")
    {
        var endpoint = new Uri(this._openAISettings.Endpoint);
        var credential = new AzureKeyCredential(this._openAISettings.AuthKey);
        var client = new OpenAIClient(endpoint, credential);

        var chatCompletionsOptions = new ChatCompletionsOptions()
        {
            Messages = { new ChatMessage(ChatRole.System, this._promptSettings.System) },
            MaxTokens = this._openAISettings.MaxTokens,
            Temperature = this._openAISettings.Temperature,
        };

        foreach (var user in this._promptSettings.Users)
        {
            var index = this._promptSettings.Users.IndexOf(user);
            var assistant = this._promptSettings.Assistants[index];

            chatCompletionsOptions.Messages.Add(new ChatMessage(ChatRole.User, user));
            chatCompletionsOptions.Messages.Add(new ChatMessage(ChatRole.System, assistant));
        }

        chatCompletionsOptions.Messages.Add(new ChatMessage(ChatRole.User, $"Generate a {socialMedia} post in {locale} using the following information:\n---\n{prompt}"));

        var deploymentId = this._openAISettings.DeploymentId;

        var response = default(string);
        try
        {
            var result = await client.GetChatCompletionsAsync(deploymentId, chatCompletionsOptions);
            response = result.Value.Choices[0].Message.Content;
        }
        catch
        {
            throw;
        }

        return response;
    }
}
