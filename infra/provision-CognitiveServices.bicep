param name string
param location string = 'eastus'

param tags object = {}

param aoaiModelName string = 'gpt-35-turbo-16k'
param aoaiModelVersion string = '0613'
param aoaiModelSkuName string = 'Standard'
param aoaiModelSkuCapacity int = 30

param aoaiModels array = [
  {
    name: aoaiModelName
    deploymentName: 'model-${replace(aoaiModelName, '-', '')}'
    version: aoaiModelVersion
    skuName: aoaiModelSkuName
    skuCapacity: aoaiModelSkuCapacity
  }
]

module aoai './openAI.bicep' = {
  name: 'CognitiveServices_AOAI'
  params: {
    name: name
    location: location
    tags: tags
    aoaiModels: aoaiModels
  }
}

output name string = aoai.outputs.name
output endpoint string = aoai.outputs.endpoint
output apiKey string = aoai.outputs.apiKey
output deploymentId string = aoaiModels[0].deploymentName
