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

### 2.0 
Implementation of FileSystemWatcher to reload the configuration.
Removed form the fluent api the old reload descriptor methods. 
## Installation

The Nuget is available at : 

## Example Quick Usage

```csharp
 var container =
                FileContainerBuilder.BuildContainer<ComplexSampleObject>()
                    .LocatedAt(_path + @"\Config_ComplexSectionConfig.json")
                    .UsingSection("NO-SAMPLE")
                    .WhenFileChange()
                    .Build();
```

### 3.0 
Added Camaleonic fields, that fields are fields that allow you to copy at json level the value of other tag.
Added Inconsitant controls, if configuration cannot be loaded correctly when file changes, you can specify if wants to throw an exception the next time you access to the container.
Added New ManualReload method, to give a way for exception control flows.
## Installation

The Nuget is available at : 

## Example Quick Usage

 var container =
                FileContainerBuilder.BuildContainer<ComplexSampleObject>()
                    .LocatedAt(_path + @"\Config_ComplexSectionConfig.json")
                    .UsingSection("NO-SAMPLE")
                    .WhenFileChange()
                    .ThrowsIfUnableToRefresh()
                    .Build();
                
        
