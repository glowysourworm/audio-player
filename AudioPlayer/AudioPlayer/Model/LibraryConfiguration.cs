using System;
using System.Runtime.Serialization;

using AudioPlayer.Model.Command;

using Avalonia.Controls;

using Avalonia.Platform.Storage;

namespace AudioPlayer.Model
{
    public class LibraryConfiguration : ModelBase, ISerializable
    {
        string _directoryBase;
        ModelCommand _openLibraryFileCommand;

        public string DirectoryBase
        {
            get { return _directoryBase; }
            set { this.SetProperty(ref _directoryBase, value); }
        }
        public ModelCommand OpenLibraryFileCommand
        {
            get { return _openLibraryFileCommand; }
            set { this.SetProperty(ref _openLibraryFileCommand, value); }
        }

        public LibraryConfiguration()
        {
            this.DirectoryBase = string.Empty;
            this.OpenLibraryFileCommand = new ModelCommand(async () =>
            {
                // Get top level from the current control. Alternatively, you can use Window reference instead.
                var topLevel = TopLevel.GetTopLevel(App.MainWindow);

                // Start async operation to open the dialog.
                var folders = await topLevel.StorageProvider.OpenFolderPickerAsync(new FolderPickerOpenOptions
                {
                    Title = "Select Library Directory (Base)",
                    AllowMultiple = false
                });

                if (folders.Count >= 1)
                {
                    this.DirectoryBase = folders[0].Path.LocalPath;
                }
            });
        }

        public LibraryConfiguration(SerializationInfo info, StreamingContext context)
        {
            this.DirectoryBase = (string)info.GetValue("DirectoryBase", typeof(string));
        }
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("DirectoryBase", this.DirectoryBase);
        }
    }
}
