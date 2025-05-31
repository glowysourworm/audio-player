using System.Collections.ObjectModel;

using AudioPlayer.Component;
using AudioPlayer.Model;

using Avalonia.Media;
using Avalonia.Threading;

namespace AudioPlayer.ViewModel
{
    public class MainViewModel : ModelBase
    {
        // Some View Properties
        public static SolidColorBrush DefaultMusicBrainzBackground = new SolidColorBrush(new Color(255, 220, 220, 220));
        public static SolidColorBrush ValidMusicBrainzBackground = new SolidColorBrush(new Color(255, 220, 255, 220));
        public static SolidColorBrush InvalidMusicBrainzBackground = new SolidColorBrush(new Color(255, 255, 220, 220));

        const int MAX_LOG_COUNT = 10000;

        Library _library;
        Configuration _configuration;

        ObservableCollection<LogMessageViewModel> _outputMessages;

        public Library Library
        {
            get { return _library; }
            set { this.SetProperty(ref _library, value); }
        }
        public Configuration Configuration
        {
            get { return _configuration; }
            set { this.SetProperty(ref _configuration, value); }
        }
        public ObservableCollection<LogMessageViewModel> OutputMessages
        {
            get { return _outputMessages; }
            set { this.SetProperty(ref _outputMessages, value); }
        }


        public MainViewModel()
        {
            this.Library = new Library();
            this.Configuration = new Configuration();
            this.OutputMessages = new ObservableCollection<LogMessageViewModel>();

            OnLog("Welcome to Audio Player!");

            this.Library.LogEvent += (message, type, severity) =>
            {
                OnLog(message, type, severity);
            };
            this.Configuration.LibraryConfiguration.PropertyChanged += OnConfigurationChanged;
        }

        private async void OnConfigurationChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            // LibraryConfiguration -> DirectoryBase (rescan)
            //
            if (e.PropertyName == "DirectoryBase")
            {
                OnLog("Library Base Directory Changed. Scanning for music files...");

                var libraryEntries = await LibraryLoader.Load(this.Configuration.LibraryConfiguration, (message, severity) =>
                {
                    Dispatcher.UIThread.Invoke(() =>
                    {
                        OnLog(message, LogMessageType.General, severity);
                    });                    
                });

                foreach (var entry in  libraryEntries)
                {
                    this.Library.Add(entry);
                }
            }
        }

        private void OnLog(string message, 
                           LogMessageType type = LogMessageType.General, 
                           LogMessageSeverity severity = LogMessageSeverity.Info, 
                           params object[] parameters)
        {
            this.OutputMessages.Insert(0, new LogMessageViewModel()
            {
                Message = string.Format(message, parameters),
                Type = type,
                Severity = severity,
            });

            if (this.OutputMessages.Count > MAX_LOG_COUNT)
            {
                this.OutputMessages.RemoveAt(this.OutputMessages.Count - 1);
            }
        }
    }
}
