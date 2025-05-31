using AudioPlayer.Model;

namespace AudioPlayer.ViewModel
{
    public enum LogMessageSeverity
    {
        Info,
        Warning,
        Error
    }
    public enum LogMessageType
    {
        General,
        MusicBrainz,
        LastFm
    }
    public class LogMessageViewModel : ModelBase
    {
        string _message;
        LogMessageSeverity _severity;
        LogMessageType _type;

        public string Message
        {
            get { return _message; }
            set { this.SetProperty(ref _message, value); }
        }
        public LogMessageSeverity Severity
        {
            get { return _severity; }
            set { this.SetProperty(ref _severity, value); }
        }
        public LogMessageType Type
        {
            get { return _type; }
            set { this.SetProperty(ref _type, value); }
        }

        public LogMessageViewModel()
        {
            this.Message = string.Empty;
        }
    }
}
