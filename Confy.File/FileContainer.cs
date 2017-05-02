using System;
using System.Threading;
using Confy.File.FluentBuilder.Interfaces;
using System.IO;
using Confy.File.Exceptions;
using Confy.File.IO;

namespace Confy.File
{
    [Serializable]
    public class FileContainer<T> : IFileContainer<T>, IFilePath<T>, IParsingOptions<T>, IGetFileConfiguration<T>, IRefreshOptions<T>, IConsistantOptions<T> where T : new()
    {
        private T _configuration = new T();
        public T Configuration
        {
            get
            {
                while (_refreshing)
                {
                }
                if(_throwIfNotConsistant && !_isConsistant) throw  new InconsistantContainerException("After 10 times, unable to lock the file for load the configuration");
                return _configuration;
            }
            set
            {
                _configuration = value;
            }
        }

        public void UpdateManually()
        {
            LoadConfiguration();
        }

        private string _filePath;
        private string _section;
        private bool _refreshMode;
        private bool _refreshing;
        private bool _isConsistant = false;
        private bool _throwIfNotConsistant;
        private delegate void ReloaderDelegate();
        public IParsingOptions<T> LocatedAt(string path)
        {
            _filePath = path;
            return this;
        }

        public IRefreshOptions<T> UsingSection(string section)
        {
            _section = section;
            return this;
        }

        public IRefreshOptions<T> GetAll()
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

        private void LoadConfiguration()
        {
            _refreshing = true;
            if (_section == string.Empty)
            {
                try
                {
                    var intermediateStepConfig = Json.JsonLoader.ConvertFromJson<T>(_filePath);
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
                    var intermediateStepConfig = Json.JsonLoader.ConvertFromJson<T>(_filePath, _section);
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
                var reloadDelegateFBased = new ReloaderDelegate(StartLastUpdateFileBasedReloader);
                reloadDelegateFBased.BeginInvoke(ReloaderCallback, reloadDelegateFBased);
            }
        }
        private void StartLastUpdateFileBasedReloader()
        {
            while (true)
            {
            }
        }
        private void ReloaderCallback(IAsyncResult ar)
        {
            try
            {
                var caller = (ReloaderDelegate)ar.AsyncState;
                caller.EndInvoke(ar);
            }
            catch (Exception)
            { }
        }
        private void SetWatcher()
        {
            FileSystemWatcher watcher = new FileSystemWatcher();
            var path = Path.GetDirectoryName(_filePath);
            var file = Path.GetFileName(_filePath);
            watcher.Path = path;
            /* Watch for changes in LastAccess and LastWrite times, and 
               the renaming of files or directories. */
            watcher.NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite
               | NotifyFilters.FileName | NotifyFilters.DirectoryName;
            watcher.Filter = file;
            // Add event handlers.
            watcher.Changed += new FileSystemEventHandler(OnChanged);
            // Begin watching.
            watcher.EnableRaisingEvents = true;
        }
        private void OnChanged(object source, FileSystemEventArgs e)
        {
            LoadConfiguration();
        }

        public IConsistantOptions<T> WhenFileChange()
        {
            _refreshMode = true;
            return this;
        }

        public IGetFileConfiguration<T> ThrowsIfUnableToRefresh()
        {
            _throwIfNotConsistant = true;
            return this;
        }

        public IGetFileConfiguration<T> NoThrowsIfNotRefresh()
        {
            _throwIfNotConsistant = false;
            return this;
        }
    }
}
