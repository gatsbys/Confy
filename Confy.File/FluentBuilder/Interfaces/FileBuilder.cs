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
        IConsistantOptions<T> WhenFileChange();
    }
    public interface IConsistantOptions<T>
    {
        IGetFileConfiguration<T> ThrowsIfUnableToRefresh();
        IGetFileConfiguration<T> NoThrowsIfNotRefresh();
    }
}
