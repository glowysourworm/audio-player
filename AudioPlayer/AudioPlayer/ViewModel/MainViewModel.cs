using System;
using System.Collections.ObjectModel;
using System.IO;

using AudioPlayer.Component;
using AudioPlayer.Model;
using AudioPlayer.Model.Command;
using AudioPlayer.Model.Database;

using Avalonia.Media;
using Avalonia.Threading;

namespace AudioPlayer.ViewModel
{
    public class MainViewModel : ModelBase
    {
        public const string CONFIGURATION_FILE = ".AudioPlayer";

        // Some View Properties
        public static SolidColorBrush DefaultMusicBrainzBackground = new SolidColorBrush(new Color(255, 220, 220, 220));
        public static SolidColorBrush ValidMusicBrainzBackground = new SolidColorBrush(new Color(255, 220, 255, 220));
        public static SolidColorBrush InvalidMusicBrainzBackground = new SolidColorBrush(new Color(255, 255, 220, 220));

        const int MAX_LOG_COUNT = 1000;

        Library _library;
        Configuration _configuration;
        bool _showOutputMessages;

        ObservableCollection<LogMessageViewModel> _outputMessages;

        ModelCommand _saveCommand;
        ModelCommand _openCommand;

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
        public bool ShowOutputMessages
        {
            get { return _showOutputMessages; }
            set { this.SetProperty(ref _showOutputMessages, value); }
        }
        public ModelCommand SaveCommand
        {
            get { return _saveCommand; }
            set { this.SetProperty(ref _saveCommand, value); }
        }
        public ModelCommand OpenCommand
        {
            get { return _openCommand; }
            set { this.SetProperty(ref _openCommand, value); }
        }

        public MainViewModel()
        {
            this.Library = new Library();
            this.Configuration = new Configuration();
            this.ShowOutputMessages = true;
            this.OutputMessages = new ObservableCollection<LogMessageViewModel>();

            OnLog("Welcome to Audio Player!");

            this.Library.LogEvent += (message, type, severity) =>
            {
                OnLog(message, type, severity);
            };
            this.Configuration.LibraryConfiguration.PropertyChanged += OnConfigurationChanged;

            this.SaveCommand = new ModelCommand(() =>
            {
                Save();
            });
            this.OpenCommand = new ModelCommand(() =>
            {
                Open();
            });
        }

        private void Save()
        {
            try
            {
                // Current working directory + configuration file name
                var configPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, CONFIGURATION_FILE);

                // Configuration
                Serializer.Serialize(this.Configuration, configPath);

                OnLog("Configuration saved successfully: {0}", LogMessageType.General, LogMessageSeverity.Info, configPath);

                // Database
                var database = new LibraryDatabase(this.Library);

                database.Save(this.Configuration.LibraryDatabaseFile);

                OnLog("Library database saved successfully: {0}", LogMessageType.General, LogMessageSeverity.Info, this.Configuration.LibraryDatabaseFile);
            }
            catch (Exception ex)
            {
                OnLog("Error saving configuration / data files:  {0}", LogMessageType.General, LogMessageSeverity.Error, ex.Message);
            }
        }
        private void Open()
        {
            try
            {
                // Current working directory + configuration file name
                var configPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, CONFIGURATION_FILE);

                this.Configuration = Serializer.Deserialize<Configuration>(configPath);

                OnLog("Configuration read successfully!");

                var database = LibraryDatabase.Open(this.Configuration.LibraryDatabaseFile);

                OnLog("Library database read successfully! Opening library...");

                this.Library = database.CreateLibrary();
            }
            catch (Exception ex)
            {
                OnLog("Error reading configuration file. Please try saving the working configuration first and then restarting.");
            }
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
