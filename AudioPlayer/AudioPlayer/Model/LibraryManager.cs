using AudioPlayer.Component;
using AudioPlayer.Model.Database;

using Avalonia.Controls;

using ReactiveUI;

using System;
using System.Threading.Tasks;

namespace AudioPlayer.Model
{
    public class LibraryManager : ModelBase
    {
        Library _library;
        string _status;

        public Library Library
        {
            get { return _library; }
            set { Update(ref _library, value); }
        }
        public string Status
        {
            get { return _status; }
            set { Update(ref _status, value); }
        }

        public IReactiveCommand OpenDirectoryCommand { get; set; }

        public static LibraryManager Create()
        {
            LibraryManager manager = new LibraryManager();

            // Try and open existing library
            try
            {
                var libraryFile = LibraryArchiver.Open();

                manager.Library = new Library(libraryFile);
                manager.Status = "Library Ready!";

                return manager;
            }
            catch (Exception)
            {
                manager = new LibraryManager();
            }

            // Create an empty library
            manager.Library = new Library(new LibraryFile());
            manager.Status = "Library Ready!";

            return manager;
        }

        public void Save()
        {
            try
            {
                LibraryArchiver.Save(_library.Database);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected LibraryManager()
        {
            this.Status = "Please load library instance";

            this.OpenDirectoryCommand = ReactiveCommand.Create(async () =>
            {
                var dialog = new OpenFolderDialog();

                var result = await dialog.ShowAsync(App.MainWindow);

                if (string.IsNullOrEmpty(result))
                    return;

                this.Status = "Scanning library files...";

                // Scan files and create library
                var libraryFile = await Task.Run(() =>
                {
                    var directories = new string[] { result };

                    return LibraryLoader.Load(directories);
                });

                // Load Library from library file
                this.Library = new Library(libraryFile);

                this.Status = "Searching for album artwork...";

                // Resolve artwork
                var libraryFileWithArtwork = await Task.Run(() =>
                {
                    LibraryArtworkLoader.Load(libraryFile);

                    return libraryFile;
                });

                // Load Library from library file
                this.Library = new Library(libraryFileWithArtwork);

                this.Status = "Library Ready!";
            });
        }
    }
}
