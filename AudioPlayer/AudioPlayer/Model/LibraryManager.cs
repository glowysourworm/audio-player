using AudioPlayer.Component;

using Avalonia.Controls;

using ReactiveUI;

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

                if (!string.IsNullOrEmpty(result))
                {
                    var directories = new string[] { result };

                    this.Library = LibraryLoader.Load(directories, (status) => this.Status = status);
                }
            });
        }
    }
}
