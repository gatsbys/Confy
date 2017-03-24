using System;

namespace Confy.File.FluentBuilder.Interfaces
{
    public interface IGetFileConfiguration<T>
    {
        IFileContainer<T> Build();
    }
    
    public interface IFilePath<T>
    {
        IParsingOptions<T> LocatedAt(string path);
    }

    public interface IParsingOptions<T>
    {
        IRefreshOptions<T> UsingSection(string section);
        IRefreshOptions<T> GetAll();
    }

    public interface IRefreshOptions<T>
    {
        IGetFileConfiguration<T> NoRefresh();
        IRefreshMode<T> Using();
    }

    public interface IRefreshTimingAutomaticOptions<T>
    {
        IGetFileConfiguration<T> ReloadingEachMode(TimeSpan interval);
    }

    public interface IRefreshTimingLastUpdateOptions<T>
    {
        IGetFileConfiguration<T> LookingAtFileEachMode(TimeSpan interval);
    }

    public interface IRefreshMode<T>
    {
        IRefreshTimingAutomaticOptions<T> Automatic();
        IRefreshTimingLastUpdateOptions<T> UsingLastUpdate();

    }

    public enum RefreshType
    {
        NotDefined=0,
        Automatic = 1,
        UsingLastUpdateTimeInFile = 2,
        NoRefresh = 3
    }
}
