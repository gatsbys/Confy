# Confy
## Intention
The idea of that library is to wrap a config type into a "container" that autorefresh for you the content using parameters.

## Open to new config sources
The code is ready to add new implementations , i.e , get the config from a WebService or Database.
Actual one is for load configuration from json file.

## Versions
### 1.0 
The first one, added the Confy.File and Confy.Tests

## Installation

The Nuget is available at : 

## Example Quick Usage

```csharp
 var container =
                FileContainerBuilder.BuildContainer<ComplexSampleObject>()
                    .LocatedAt(_path + @"\Config_ComplexSectionConfig.json")
                    .UsingSection("NO-SAMPLE")
                    .UsingRefreshMode()
                    .Automatic()
                    .Each(TimeSpan.FromSeconds(2))
                    .Build();
```


                
        
