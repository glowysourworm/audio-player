using AudioPlayer.Component;
using AudioPlayer.Model.Database;
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

        public LibraryManager()
        {
            this.Status = "Please open existing library or directory";

            this.OpenDirectoryCommand = ReactiveCommand.Create(async () =>
            {
                var dialog = new OpenFolderDialog();

                var result = await dialog.ShowAsync(App.MainWindow);

                if (string.IsNullOrEmpty(result))
                    return;

                this.Status = "Scanning library files...";

                // Scan files and create library
                var preliminaryLibrary = await Task.Run(() =>
                {
                    var directories = new string[] { result };
                   
                    return LibraryLoader.Load(directories);
                });

                // Load Library from library file
                this.Library = new Library(preliminaryLibrary);

                this.Status = "Searching for album artwork...";

                // Resolve artwork
                var completedLibrary = await Task.Run(() =>
                {
                    LibraryArtworkLoader.Load(preliminaryLibrary);

                    return preliminaryLibrary;
                });

                // Load Library from library file
                this.Library = new Library(completedLibrary);

                this.Status = "Library Ready!";
            });
        }
    }
}
