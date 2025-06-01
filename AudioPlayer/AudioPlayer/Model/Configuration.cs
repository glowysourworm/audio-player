using System;
using System.IO;
using System.Runtime.Serialization;

using AudioPlayer.Model.Command;

using Avalonia.Controls;
using Avalonia.Platform.Storage;

namespace AudioPlayer.Model
{
    public class Configuration : ModelBase, ISerializable
    {
        public const string LIBRARY_DATABASE_FILE = ".AudioPlayerLibrary";

        string _libraryDatabaseFile;
        LibraryConfiguration _libraryConfiguration;

        ModelCommand _selectLibraryDatabaseFileCommand;

        public LibraryConfiguration LibraryConfiguration
        {
            get { return _libraryConfiguration; }
            set { this.SetProperty(ref _libraryConfiguration, value); }
        }
        public string LibraryDatabaseFile
        {
            get { return _libraryDatabaseFile; }
            set { this.SetProperty(ref _libraryDatabaseFile, value); }
        }
        public ModelCommand SelectLibraryDatabaseFileCommand
        {
            get { return _selectLibraryDatabaseFileCommand; }
            set { this.SetProperty(ref _selectLibraryDatabaseFileCommand, value); }
        }


        public Configuration()
        {
            this.LibraryConfiguration = new LibraryConfiguration();
            this.LibraryDatabaseFile = LIBRARY_DATABASE_FILE;
            this.SelectLibraryDatabaseFileCommand = new ModelCommand(async () =>
            {
                // Get top level from the current control. Alternatively, you can use Window reference instead.
                var topLevel = TopLevel.GetTopLevel(App.MainWindow);

                // Start async operation to open the dialog.
                var file = await topLevel.StorageProvider.SaveFilePickerAsync(new FilePickerSaveOptions
                {
                    Title = "Select Library Database File",
                    SuggestedFileName = LIBRARY_DATABASE_FILE
                });

                if (file != null)
                {
                    this.LibraryDatabaseFile = file.Path.LocalPath;
                }
            });

            this.LibraryConfiguration.PropertyChanged += (sender, args) =>
            {
                if (args.PropertyName == "DirectoryBase")
                {
                    this.LibraryDatabaseFile = Path.Combine(this.LibraryConfiguration.DirectoryBase, LIBRARY_DATABASE_FILE);
                }
            };
        }
        public Configuration(SerializationInfo info, StreamingContext context)
        {
            this.LibraryConfiguration = (LibraryConfiguration)info.GetValue("LibraryConfiguration", typeof(LibraryConfiguration));
            this.LibraryDatabaseFile = (string)info.GetValue("LibraryDatabaseFile", typeof(string));
        }
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("LibraryConfiguration", this.LibraryConfiguration);
            info.AddValue("LibraryDatabaseFile", this.LibraryDatabaseFile);
        }
    }
}
