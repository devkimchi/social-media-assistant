# Social Media Assistant

This provides a total solution of generating social media posts for online marketing purpose, using Power Platform as a front-end app and workflow, and Azure Functions as a back-end API app.

## Architecture

TBD

## Prerequisites

- [Azure Subscription](https://azure.microsoft.com/free?WT.mc_id=dotnet-108200-juyoo)
- [Azure OpenAI Service](https://learn.microsoft.com/azure/ai-services/openai/overview?WT.mc_id=dotnet-108200-juyoo)
- [Azure CLI](https://learn.microsoft.com/cli/azure/what-is-azure-cli?WT.mc_id=dotnet-108200-juyoo)
- [Azure Developer CLI](https://learn.microsoft.com/azure/developer/azure-developer-cli/overview?WT.mc_id=dotnet-108200-juyoo)
- [GitHub CLI](https://cli.github.com)
- [Microsoft 365 Developer Program](https://learn.microsoft.com/office/developer-program/microsoft-365-developer-program?WT.mc_id=dotnet-108200-juyoo)
- [Power Apps Developer Plan](https://learn.microsoft.com/power-platform/developer/plan?WT.mc_id=dotnet-108200-juyoo)

## Getting Started

### Provisioning Azure Resources

1. Fork this repository to your GitHub account, `{{GITHUB_USERNAME}}`.
1. Run the commands below to set up a resource names:

   ```bash
   # PowerShell
   $AZURE_ENV_NAME="social$(Get-Random -Min 1000 -Max 9999)"
   $GITHUB_USERNAME="{{GITHUB_USERNAME}}"

   # Bash
   AZURE_ENV_NAME="social$RANDOM"
   GITHUB_USERNAME="{{GITHUB_USERNAME}}"
   ```

1. Run the commands below to provision Azure resources:

   ```bash
   azd auth login
   azd init -e $AZURE_ENV_NAME
   azd up
   ```

   > You might be asked to input your GitHub username and repository name.

### Deploying Applications to Azure

1. Run the commands below to deploy apps to Azure:

   ```bash
   az login
   gh auth login
   azd pipeline config
   gh workflow run "Azure Dev" --repo $GITHUB_USERNAME/social-media-assistant
   ```

### Deprovisioning Azure Resources

1. To avoid unexpected billing shock, run the commands below to deprovision Azure resources:

   ```bash
   azd down --force --purge --no-prompt
   ```

## Local Development

Use `SocialMediaAssistant.sln` with Visual Studio or Visual Studio Code with the C# Dev Kit extension.

### `local.settings.json` &ndash; `SocialMediaAssistant.ApiApp`

1. Copy `local.settings.sample.json` to `local.settings.json`.
1. Substitute the following values in the `local.settings.json` with the actual values:

   ```json
   "OpenAIApi__Endpoint": "https://aoai-{{AZURE_ENV_NAME}}.openai.azure.com/",
   "OpenAIApi__AuthKey": "{{AOAI_API_KEY}}",
   "OpenAIApi__DeploymentId": "{{DEPLOYMENT_ID}}",
   ```

   - `{{AZURE_ENV_NAME}}`: Azure environment name. It looks like `social****` where `****` is a random number.
   - `{{AOAI_API_KEY}}`: API Key of Azure OpenAI Service.
   - `{{DEPLOYMENT_ID}}`: Azure OpenAI Service deployment ID. It looks like `model-gpt35turbo16k`.
