using System;
using System.Threading;
using Confy.File.FluentBuilder.Interfaces;

namespace Confy.File
{
    public class FileContainer<T> : IFileContainer<T>, IFilePath<T>, IParsingOptions<T>, IGetFileConfiguration<T>, IRefreshOptions<T>, IRefreshMode<T>, IRefreshTimingAutomaticOptions<T>, IRefreshTimingLastUpdateOptions<T>
    {
        public T Configuration { get; set; }
        private string _filePath;
        private string _section;
        private DateTime _lastRefresh;
        private TimeSpan _refreshInterval;
        private RefreshType _refreshType;
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
            _refreshType = RefreshType.NoRefresh;
            return this;
        }

        public IRefreshMode<T> Using()
        {
            return this;
        }

        public IRefreshTimingAutomaticOptions<T> Automatic()
        {
            _refreshType = RefreshType.Automatic;
            return this;
        }

        public IRefreshTimingLastUpdateOptions<T> UsingLastUpdate()
        {
            _refreshType = RefreshType.UsingLastUpdateTimeInFile;
            return this;
        }

        public IGetFileConfiguration<T> ReloadingEachMode(TimeSpan interval)
        {
            _refreshInterval = interval;
            return this;
        }

        public IGetFileConfiguration<T> LookingAtFileEachMode(TimeSpan interval)
        {
            _refreshInterval = interval;
            _lastRefresh = DateTime.Now;
            return this;
        }

        public IFileContainer<T> Build()
        {
            IFileContainer<T> container = new FileContainer<T>();
            LoadConfiguration();
            ActivateReloaderDaemon();
            return this;
        }

        private void LoadConfiguration()
        {
            if (_section == string.Empty)
            {
                Configuration = Json.JsonLoader.ConvertFromJson<T>(_filePath);
            }
            else if (_section != string.Empty)
            {
                Configuration = Json.JsonLoader.ConvertFromJson<T>(_filePath, _section);
            }
        }

        private void ActivateReloaderDaemon()
        {
            switch (_refreshType)
            {
                case RefreshType.NotDefined:
                    throw new Exception("Undefined refresh type");
                case RefreshType.Automatic:
                    var reloadDelegate = new ReloaderDelegate(StartAutomaticReloader);
                    reloadDelegate.BeginInvoke(ReloaderCallback, reloadDelegate);
                    break;
                case RefreshType.UsingLastUpdateTimeInFile:
                    var reloadDelegateFBased = new ReloaderDelegate(StartLastUpdateFileBasedReloader);
                    reloadDelegateFBased.BeginInvoke(ReloaderCallback, reloadDelegateFBased);
                    break;
                case RefreshType.NoRefresh:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            
        }

        private void StartAutomaticReloader()
        {
            while (true)
            {
                LoadConfiguration();
                Thread.Sleep((int)_refreshInterval.TotalMilliseconds);
            }
        }

        private void StartLastUpdateFileBasedReloader()
        {
            while (true)
            {
                var currWriteDate = IO.IOHelper.GetLastWriteDateUtc(_filePath);
                if (currWriteDate > _lastRefresh)
                {
                    LoadConfiguration();
                    _lastRefresh = currWriteDate;
                }
                Thread.Sleep((int)_refreshInterval.TotalMilliseconds);
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

    }
}
