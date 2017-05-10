namespace Confy.File
{
    public interface IFileContainer<T>
    {
        T Configuration { get; }
        event ConfigurationReloadHandler OnConfigurationReload;
        void UpdateManually();
    }
}
