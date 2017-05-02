using Confy.File.FluentBuilder.Interfaces;

namespace Confy.File.FluentBuilder
{
    public class FileContainerBuilder
    {
        public static IFilePath<T> BuildContainer<T>() where T : new()
        {
            return new FileContainer<T>();
        }
    }
}
