using Confy.File.FluentBuilder.Interfaces;

namespace Confy.File.FluentBuilder
{
    public class FileContainerBuilder
    {
        public static IFilePath<T> BuildContainer<T>()
        {
            return new FileContainer<T>();
        }
    }
}
