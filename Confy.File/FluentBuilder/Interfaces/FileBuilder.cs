using System;
using System.Data.SqlClient;

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
        IRefreshOptions<T> UsingSectionInFile(string section);
        IRefreshOptions<T> UsingEntireFile();
    }

    public interface IRefreshOptions<T>
    {
        IGetFileConfiguration<T> NoRefresh();
        IConsistantOptions<T> RefreshingWhenFileChange();
    }
    public interface IConsistantOptions<T>
    {
        IOnChangeEventHndlerOrBuild<T> ThrowsIfUnableToRefresh(bool throwIfNotLoaded);
    }

    public interface IOnChangeEventHndlerOrBuild<T> 
    {
        IFileContainer<T> Build();
        IGetFileConfiguration<T> WithActionOnChanged(ConfigurationReloadHandler handler);
    }
}
