targetScope = 'subscription'

param name string
param location string

param apiManagementPublisherName string = 'Dev Kimchi'
param apiManagementPublisherEmail string = 'apim@devkimchi.com'

@secure()
param gitHubUsername string
param gitHubRepositoryName string
param gitHubBranchName string = 'main'

param aoaiModelName string = 'gpt-35-turbo-16k'
param aoaiModelVersion string = '0613'
param aoaiModelSkuName string = 'Standard'
param aoaiModelSkuCapacity int = 30

// tags that should be applied to all resources.
var tags = {
  // Tag all resources with the environment name.
  'azd-env-name': name
}
var storageContainerName = 'openapis'

resource rg 'Microsoft.Resources/resourceGroups@2021-04-01' = {
  name: 'rg-${name}'
  location: location
  tags: tags
}

module cogsvc './provision-CognitiveServices.bicep' = {
  name: 'CognitiveServices'
  scope: rg
  params: {
    name: name
    tags: tags
    aoaiModelName: aoaiModelName
    aoaiModelVersion: aoaiModelVersion
    aoaiModelSkuName: aoaiModelSkuName
    aoaiModelSkuCapacity: aoaiModelSkuCapacity
  }
}

var apps = [
  {
    name: 'aoai'
    isFunctionApp: true
    functionAppSuffix: 'aoai'
    appSettings: {
      openApi: loadYamlContent('./appsettings-aoai-openapi.yaml')
      aoaiService: loadYamlContent('./appsettings-aoai-openai.yaml')
      prompt: loadYamlContent('./appsettings-aoai-prompt.yaml')
    }
    apimIntegrated: true
    api: {
      name: 'AOAI'
      path: 'aoai'
      serviceUrl: 'https://fncapp-{{AZURE_ENV_NAME}}-{{SUFFIX}}.azurewebsites.net/api'
      referenceUrl: 'https://raw.githubusercontent.com/${gitHubUsername}/${gitHubRepositoryName}/${gitHubBranchName}/infra/openapi-{{SUFFIX}}.{{EXTENSION}}'
      format: 'openapi+json-link'
      extension: 'json'
      subscription: true
      product: 'default'
      operations: []
    }
  }
]

module apim './provision-ApiManagement.bicep' = {
  name: 'ApiManagement'
  scope: rg
  params: {
    name: name
    location: location
    tags: tags
    apiManagementPublisherName: apiManagementPublisherName
    apiManagementPublisherEmail: apiManagementPublisherEmail
    apiManagementPolicyFormat: 'xml-link'
    apiManagementPolicyValue: 'https://raw.githubusercontent.com/${gitHubUsername}/${gitHubRepositoryName}/${gitHubBranchName}/infra/apim-policy-global.xml'
  }
}

module fncapps './provision-FunctionApp.bicep' = [for (app, index) in apps: if (app.isFunctionApp == true) {
  name: 'FunctionApp_${app.name}'
  scope: rg
  dependsOn: [
    apim
  ]
  params: {
    name: name
    suffix: app.functionAppSuffix
    location: location
    tags: tags
    storageContainerName: storageContainerName
    openApiSettings: app.appSettings.openApi
    aoaiServiceSettings: app.appSettings.aoaiService
    promptSettings: app.appSettings.prompt
  }
}]

module apis './provision-ApiManagementApi.bicep' = [for (app, index) in apps: if (app.apimIntegrated == true) {
  name: 'ApiManagementApi_${app.name}'
  scope: rg
  dependsOn: [
    apim
  ]
  params: {
    name: name
    location: location
    apiManagementApiName: app.api.name
    apiManagementApiDisplayName: app.api.name
    apiManagementApiDescription: app.api.name
    apiManagementApiSubscriptionRequired: app.api.subscription
    apiManagementApiServiceUrl: replace(replace(app.api.serviceUrl, '{{AZURE_ENV_NAME}}', name), '{{SUFFIX}}', app.functionAppSuffix)
    apiManagementApiPath: app.api.path
    apiManagementApiFormat: app.api.format
    apiManagementApiValue: replace(replace(app.api.referenceUrl, '{{SUFFIX}}', app.functionAppSuffix), '{{EXTENSION}}', app.api.extension)
    apiManagementApiPolicyFormat: 'xml-link'
    apiManagementApiPolicyValue: 'https://raw.githubusercontent.com/${gitHubUsername}/${gitHubRepositoryName}/${gitHubBranchName}/infra/apim-policy-api-${replace(toLower(app.api.name), '-', '')}.xml'
    apiManagementApiOperations: app.api.operations
    apiManagementProductName: app.api.product
  }
}]
