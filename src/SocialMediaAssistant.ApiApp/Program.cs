using Microsoft.Azure.Functions.Worker.Extensions.OpenApi.Extensions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Configurations.AppSettings.Extensions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Configurations;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

using SocialMediaAssistant.Services.OpenAI;
using SocialMediaAssistant.Services.OpenAI.Configurations;

var host = new HostBuilder()
                .ConfigureFunctionsWorkerDefaults(worker => worker.UseNewtonsoftJson())
                //.ConfigureOpenApi()
                .ConfigureHostConfiguration(config => config.AddEnvironmentVariables())
                .ConfigureServices(services =>
                {
                    var openAIApiSettings = services.BuildServiceProvider()
                            .GetService<IConfiguration>()
                            .Get<OpenAIApiSettings>(OpenAIApiSettings.Name);
                    services.AddSingleton(openAIApiSettings);

                    var promptSettings = services.BuildServiceProvider()
                            .GetService<IConfiguration>()
                            .Get<PromptSettings>(PromptSettings.Name);
                    services.AddSingleton(promptSettings);

                    services.AddSingleton<IOpenApiConfigurationOptions>(_ =>
                    {
                        var options = new OpenApiConfigurationOptions()
                        {
                            OpenApiVersion = OpenApiVersionType.V3,
                            Info = new OpenApiInfo()
                            {
                                Version = DefaultOpenApiConfigurationOptions.GetOpenApiDocVersion(),
                                Title = DefaultOpenApiConfigurationOptions.GetOpenApiDocTitle(),
                                Description = DefaultOpenApiConfigurationOptions.GetOpenApiDocDescription(),
                                License = new OpenApiLicense()
                                {
                                    Name = "MIT",
                                    Url = new Uri("http://opensource.org/licenses/MIT"),
                                }
                            },
                        };

                        return options;
                    });

                    services.AddScoped<IOpenAIService, OpenAIService>();
                })
                .Build();

host.Run();
