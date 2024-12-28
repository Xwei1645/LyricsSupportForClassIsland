using System;
using System.Collections.Generic;
using System.IO;
using CommunityToolkit.Mvvm.ComponentModel;
using LyricsSupportForClassIsland.Services.LyricsParser;

namespace LyricsSupportForClassIsland.Models
{
    public class LyricsNotificationSettings : ObservableRecipient
    {
        private string _lyricsPath = "";
        private string _lyricsContent = "";
        private TimeSpan _playTime;
        private TimeSpan _overlayDuration;
        private TimeSpan _reminderTime;
        private List<LyricsLine> _parsedLyrics = new();
        private bool _showNotificationNow;

        /// <summary>
        /// Lyrics 文件路径
        /// </summary>
        public string LyricsPath
        {
            get => _lyricsPath;
            set
            {
                if (value == _lyricsPath) return;
                _lyricsPath = value;
                OnPropertyChanged();
                LoadLyricsContent(); // 加载歌词内容
            }
        }

        /// <summary>
        /// 解析并显示歌词文件内容
        /// </summary>
        public string LyricsContent
        {
            get => _lyricsContent;
            set
            {
                if (value == _lyricsContent) return;
                _lyricsContent = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// 播放时间
        /// </summary>
        public TimeSpan PlayTime
        {
            get => _playTime;
            set
            {
                if (value == _playTime) return;
                _playTime = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// 播放时间字符串
        /// </summary>
        public string PlayTimeString
        {
            get => _playTime.ToString(@"hh\:mm\:ss");
            set
            {
                if (TimeSpan.TryParse(value, out var time))
                {
                    PlayTime = time;
                }
            }
        }

        /// <summary>
        /// 提醒持续时间
        /// </summary>
        public TimeSpan OverlayDuration
        {
            get => _overlayDuration;
            set
            {
                if (value == _overlayDuration) return;
                _overlayDuration = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// 提醒持续时间字符串
        /// </summary>
        public string OverlayDurationString
        {
            get => _overlayDuration.ToString(@"hh\:mm\:ss");
            set
            {
                if (TimeSpan.TryParse(value, out var time))
                {
                    OverlayDuration = time;
                }
            }
        }

        /// <summary>
        /// 提醒时间
        /// </summary>
        public TimeSpan ReminderTime
        {
            get => _reminderTime;
            set
            {
                if (value == _reminderTime) return;
                _reminderTime = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// 提醒时间字符串
        /// </summary>
        public string ReminderTimeString
        {
            get => _reminderTime.ToString(@"hh\:mm\:ss");
            set
            {
                if (TimeSpan.TryParse(value, out var time))
                {
                    ReminderTime = time;
                }
            }
        }

        /// <summary>
        /// 解析后的歌词
        /// </summary>
        public List<LyricsLine> ParsedLyrics
        {
            get => _parsedLyrics;
            set
            {
                if (value == _parsedLyrics) return;
                _parsedLyrics = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// 是否立即显示提醒
        /// </summary>
        public bool ShowNotificationNow
        {
            get => _showNotificationNow;
            set
            {
                if (value == _showNotificationNow) return;
                _showNotificationNow = value;
                OnPropertyChanged();
            }
        }

        private void LoadLyricsContent()
        {
            if (File.Exists(_lyricsPath))
            {
                LyricsContent = File.ReadAllText(_lyricsPath);
                ParsedLyrics = new LyricsParser().Parse(LyricsContent);
            }
            else
            {
                LyricsContent = "无法加载歌词文件内容。";
                ParsedLyrics.Clear();
            }
        }
    }
}


