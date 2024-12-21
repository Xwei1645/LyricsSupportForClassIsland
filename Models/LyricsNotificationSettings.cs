using System.Configuration;
using ClassIsland.Core.Controls;
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
        // 创建一个SettingsCard对象
        SettingsCard settingsCard = new SettingsCard();
        // 设置SettingsCard的标题和描述
        settingsCard.Header = "歌词文件路径";
        settingsCard.Description = "选择本地的LRC歌词文件。";
        /*// 创建一个按钮，用于打开文件对话框选择LRC文件
        settingsCardButton selectFileButton = new Button();
        selectFileButton.Content = "选择歌词文件";
        selectFileButton.Click += (sender, e) =>
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Lyrics Files (*.lrc)|*.lrc|All Files (*.*)|*.*",
                Title = "Select Lyrics File"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                // 将选择的文件路径赋值给SettingsCard的Switcher属性
                settingsCard.Switcher = openFileDialog.FileName;
            }
        };

        // 将按钮添加到SettingsCard中
        settingsCard.Content = selectFileButton;

        // 将SettingsCard添加到设置页面中
        SettingsPage settingsPage = new SettingsPage();
        settingsPage.SettingsCards.Add(settingsCard);*/
    }
}