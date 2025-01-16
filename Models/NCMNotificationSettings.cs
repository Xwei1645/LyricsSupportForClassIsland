using CommunityToolkit.Mvvm.ComponentModel;

namespace LyricsSupportForClassIsland.Models
{
    public class NCMNotificationSettings : ObservableRecipient
    {
        private bool _isEnabled = true;
        private string _lyricsPushUrl = "http://localhost:5000";

        public bool IsEnabled
        {
            get => _isEnabled;
            set
            {
                if (value == _isEnabled) return;
                _isEnabled = value;
                OnPropertyChanged();
            }
        }

        public string LyricsPushUrl
        {
            get => _lyricsPushUrl;
            set
            {
                if (value == _lyricsPushUrl) return;
                _lyricsPushUrl = value;
                OnPropertyChanged();
            }
        }
    }
}
