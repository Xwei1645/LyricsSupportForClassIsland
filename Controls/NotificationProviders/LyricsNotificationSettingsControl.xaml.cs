using System.Windows.Controls;
using LyricsSupportForClassIsland.Models;
using System;
using System.Linq;
using System.Windows;
using ClassIsland.Core.Abstractions.Controls;
using ClassIsland.Core.Abstractions.Services;
using ClassIsland.Core.Controls;
using Microsoft.Win32;

namespace LyricsSupportForClassIsland.Controls.NotificationProviders
{
    public partial class LyricsNotificationProviderSettingsControl : UserControl
    {
        /// <summary>
        /// 提醒提供方设置
        /// </summary>
        public LyricsNotificationSettings Settings { get; }

        // 这里通过构造函数参数传入设置对象，这样设置控件就可以访问到提醒提供方的设置了。
        public LyricsNotificationProviderSettingsControl(LyricsNotificationSettings settings)
        {
            // 将设置对象写入到属性中，这样前端就可以访问到这个设置对象，以进行绑定。
            Settings = settings;
            DataContext = Settings; // 设置 DataContext

            InitializeComponent();
        }

        private void SelectLyricsFile_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog
            {
                Filter = "歌词文件 (*.lrc)|*.lrc"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                Settings.LyricsPath = openFileDialog.FileName;
            }
        }

        private void ShowNow_Click(object sender, RoutedEventArgs e)
        {
            // 立即显示提醒
            Settings.ShowNotificationNow = true;
        }
    }
}

