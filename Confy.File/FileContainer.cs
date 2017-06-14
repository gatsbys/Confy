using System;
using System.Data.SqlClient;
using System.Threading;
using Confy.File.FluentBuilder.Interfaces;
using System.IO;
using System.Runtime.Serialization;
using Confy.File.Exceptions;
using Confy.File.IO;
using Confy.Json;

namespace Confy.File
{
    public delegate void ConfigurationReloadHandler();
    [Serializable]
    public class FileContainer<T> : IFileContainer<T>, IFilePath<T>, IParsingOptions<T>, IGetFileConfiguration<T>, IRefreshOptions<T>, IConsistantOptions<T>, IOnChangeEventHndlerOrBuild<T>, IDisposable where T : new()
    {

        #region Public Members

        public event ConfigurationReloadHandler OnConfigurationReload;
        public T Configuration
        {
            get
            {
                return GetConfig();
            }
            set
            {
                _configuration = value;
            }
        }

        private T GetConfig()
        {
            while (_refreshing)
            {
            }
            if (_throwIfNotConsistant && !_isConsistant) throw new InconsistantContainerException("After 10 times, unable to lock the file for load the configuration");
            return _configuration;
        }

        #endregion

        #region Private Members

        private T _configuration = new T();
        private string _filePath;
        private string _section;
        private bool _refreshMode;
        private bool _refreshing;
        private bool _isConsistant;
        private bool _throwIfNotConsistant;
        private delegate void ReloaderDelegate();
        [NonSerialized]
        private FileSystemWatcher _watcher;


        #endregion

        #region Fluent Api Builder Methods

        public IParsingOptions<T> LocatedAt(string path)
        {
            _filePath = path;
            return this;
        }

        public IRefreshOptions<T> UsingSectionInFile(string section)
        {
            _section = section;
            return this;
        }

        public IRefreshOptions<T> UsingEntireFile()
        {
            _section = string.Empty;
            return this;
        }

        public IGetFileConfiguration<T> NoRefresh()
        {
            return this;
        }

        public IFileContainer<T> Build()
        {
            LoadConfiguration();
            ActivateReloaderDaemon();
            return this;
        }

        public IConsistantOptions<T> RefreshingWhenFileChange()
        {
            _watcher = new FileSystemWatcher();
            _refreshMode = true;
            return this;
        }

        public IGetFileConfiguration<T> WithActionOnChanged(ConfigurationReloadHandler handler)
        {
            OnConfigurationReload += handler;
            return this;
        }

        public IOnChangeEventHndlerOrBuild<T> ThrowsIfUnableToRefresh(bool throwIfNotLoaded)
        {
            _throwIfNotConsistant = throwIfNotLoaded;
            return this;
        }

        #endregion

        #region Exposed Methods

        public void UpdateManually()
        {
            LoadConfiguration();
        }

        #endregion

        #region Private Methods

        private void OnChanged(object source, FileSystemEventArgs e)
        {
            try
            {
                _watcher.EnableRaisingEvents = false;
                LoadConfiguration();
                OnConfigurationReload?.Invoke();
            }
            finally
            {
                _watcher.EnableRaisingEvents = true;
            }

        }

        private void LoadConfiguration()
        {
            _refreshing = true;
            if (_section == string.Empty)
            {
                try
                {
                    var intermediateStepConfig = JsonLoader.ConvertFromJson<T>(_filePath);
                    _configuration = intermediateStepConfig;
                    _isConsistant = true;
                }
                catch (Exception)
                {
                    _isConsistant = false;
                }
            }
            else if (_section != string.Empty)
            {
                try
                {
                    var intermediateStepConfig = JsonLoader.ConvertFromJson<T>(_filePath, _section);
                    _configuration = intermediateStepConfig;
                    _isConsistant = true;
                }
                catch (Exception)
                {
                    _isConsistant = false;
                }
            }
            _refreshing = false;
        }

        private void ActivateReloaderDaemon()
        {
            if (_refreshMode)
            {
                SetWatcher();
            }
        }

        private void SetWatcher()
        {
            var path = Path.GetDirectoryName(_filePath);
            var file = Path.GetFileName(_filePath);
            _watcher.Path = path;
            /* Watch for changes in LastAccess and LastWrite times, and 
               the renaming of files or directories. */
            _watcher.NotifyFilter = NotifyFilters.LastWrite;
            _watcher.Filter = file;
            // Add event handlers.
            _watcher.Changed += new FileSystemEventHandler(OnChanged);
            // Begin watching.
            _watcher.EnableRaisingEvents = true;
        }

        #endregion

        #region Disposed Resources
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                // free managed resources  
                if (_watcher != null)
                {
                    _watcher.Dispose();
                    _watcher = null;
                }
            }
        }
        #endregion
    }
}
