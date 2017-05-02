namespace Confy.File
{
    public interface IFileContainer<T>
    {
        T Configuration { get; }
        void UpdateManually();
    }
}
