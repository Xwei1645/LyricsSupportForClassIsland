using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Win32;

namespace LyricsSupportForClassIsland.Models;

public class LyricsNotificationSettings : ObservableRecipient
{
    private string _title = "test";
    private string _lyricsFilePath = "";

    /// <summary>
    /// 要显示的文本
    /// </summary>
    public string Title
    {
        get => _title;
        set
        {
            if (value == _title) return;
            _title = value;
            OnPropertyChanged();
        }
    }

    /// <summary>
    /// Lyrics 文件路径
    /// </summary>
    public string LyricsFilePath
    {
        get => _lyricsFilePath;
        set
        {
            if (value == _lyricsFilePath) return;
            _lyricsFilePath = value;
            OnPropertyChanged();
        }
    }

    /// <summary>
    /// 打开文件对话框，让用户选择 Lyrics 文件
    /// </summary>
    public void SelectLyricsFile()
    {
        OpenFileDialog openFileDialog = new OpenFileDialog
        {
            Filter = "Lyrics Files (*.lrc)|*.lrc|All Files (*.*)|*.*",
            Title = "Select Lyrics File"
        };

        if (openFileDialog.ShowDialog() == true)
        {
            LyricsFilePath = openFileDialog.FileName;
        }
    }
}