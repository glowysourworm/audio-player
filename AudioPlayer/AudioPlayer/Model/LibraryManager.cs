using AudioPlayer.Component;

using Avalonia.Controls;

using ReactiveUI;

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

        public IReactiveCommand SaveLibraryCommand { get; set; }
        public IReactiveCommand OpenLibraryCommand { get; set; }

        public LibraryManager()
        {
            this.Status = "Please open existing library or directory";

            this.SaveLibraryCommand = ReactiveCommand.Create(async () =>
            {
                var dialog = new OpenFolderDialog();

                var result = await dialog.ShowAsync(App.MainWindow);

                if (string.IsNullOrEmpty(result))
                    return;

                LibraryArchiver.Save(this.Library.Database, result);
            });

            this.OpenLibraryCommand = ReactiveCommand.Create(async () =>
            {
                var dialog = new OpenFolderDialog();

                var result = await dialog.ShowAsync(App.MainWindow);

                if (string.IsNullOrEmpty(result))
                    return;

                var libraryFile = LibraryArchiver.Open(result);

                this.Library = new Library(libraryFile);
            });

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
