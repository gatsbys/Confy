# ConfigurationManager
## Intention
The idea of that library is to wrap a config type into a "container" that autorefresh for you the content using parameters.

## Open to new config sources
The code is ready to add new implementations , i.e , get the config from a WebService or Database.

## Versions
### 1.0 
The first one
### 2.0
Removed not needed methods from IConfigurationContainer and added the new Unit Test Project

## Installation

The Nuget is available at : https://www.nuget.org/packages/ConfigurationManager.Implementations/2.0.0

## Example Quick Usage

```csharp
IConfigurationContainerBuilder containerBuilder = new ConfigurationContainerBuilder();
            var container = containerBuilder.GetFileConfigurationContainer<SampleComplexObject>(
                x => x.WithPath("File Path to you config in JSON file").RefreshingEach(TimeSpan.FromSeconds(30)));
```


                
        
