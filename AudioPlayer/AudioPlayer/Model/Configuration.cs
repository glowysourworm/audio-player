namespace AudioPlayer.Model
{
    public class Configuration : ModelBase
    {
        LibraryConfiguration _libraryConfiguration;

        public LibraryConfiguration LibraryConfiguration
        {
            get { return _libraryConfiguration; }
            set { this.SetProperty(ref _libraryConfiguration, value); }
        }

        public Configuration()
        {
            this.LibraryConfiguration = new LibraryConfiguration();
        }
    }
}
