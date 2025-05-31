namespace AudioPlayer.Model
{
    public class Artist : ModelBase
    {
        string _name;

        public string Name
        {
            get { return _name; }
            set { this.SetProperty(ref _name, value); }
        }

        public Artist()
        {
            this.Name = string.Empty;
        }
    }
}
