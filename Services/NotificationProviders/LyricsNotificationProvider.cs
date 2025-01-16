using ClassIsland.Core.Abstractions.Services;
using ClassIsland.Shared.Abstraction.Models;
using ClassIsland.Shared.Interfaces;
using ClassIsland.Shared.Models.Notification;
using LyricsSupportForClassIsland.Controls.NotificationProviders;
using LyricsSupportForClassIsland.Models;
using MaterialDesignThemes.Wpf;
using Microsoft.Extensions.Hosting;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace LyricsSupportForClassIsland.Services.NotificationProviders
{
    public class LyricsNotificationProvider : INotificationProvider, IHostedService
    {
        public string Name { get; set; } = "歌词提醒";
        public string Description { get; set; } = "定时发出歌词提醒。";
        public Guid ProviderGuid { get; set; } = new Guid("95AC7075-EC49-4D4D-BA1A-DDD1740713FC");
        public object? SettingsElement { get; set; }
        public object? IconElement { get; set; }

        /// <summary>
        /// 这个属性用来存储提醒的设置。
        /// </summary>
        private LyricsNotificationSettings Settings { get; }

        //订阅课表处理前事件PreMainTimerTicked
        private INotificationHostService NotificationHostService { get; }
        public ILessonsService LessonsService { get; }

        private DateTime _startTime;
        private DateTime _notificationStartTime;
        private TextBlock _overlayTextBlock;
        private bool _isPlaying;

        public LyricsNotificationProvider(INotificationHostService notificationHostService,
         ILessonsService lessonsService)
        {
            NotificationHostService = notificationHostService;
            LessonsService = lessonsService;  // 将课程服务实例保存到属性中备用
            NotificationHostService.RegisterNotificationProvider(this);

            // 获取这个提醒提供方的设置，并保存到 Settings 属性上备用。
            Settings = NotificationHostService.GetNotificationProviderSettings<LyricsNotificationSettings>(ProviderGuid);

            // 将刚刚获取到的提醒提供方设置传给提醒设置控件，这样提醒设置控件就可以访问到提醒设置了。
            // 然后将 SettingsElement 属性设置为这个控件对象，这样提醒设置界面就会显示我们自定义的提醒设置控件。
            SettingsElement = new LyricsNotificationProviderSettingsControl(Settings);

            // 设置提醒设置界面的图标
            IconElement = new PackIcon
            {
                Kind = PackIconKind.QueueMusic,
                Width = 24,
                Height = 24
            };

            LessonsService.PreMainTimerTicked += LessonsServiceOnPreMainTimerTicked;  // 注册事件

            _startTime = DateTime.MinValue;
            _isPlaying = false;
        }

        private void LessonsServiceOnPreMainTimerTicked(object? sender, EventArgs e)
        {
            // 获取当前时间
            var currentTime = DateTime.Now;

            // 检查是否立即显示提醒
            if (Settings.ShowNotificationNow && !_isPlaying)
            {
                StartNotification();
                return;
            }

            // 检查是否到达设定的提醒时间
            if (currentTime.TimeOfDay >= Settings.NotificationTime && currentTime.TimeOfDay < Settings.NotificationTime.Add(TimeSpan.FromSeconds(1)) && !_isPlaying)
            {
                StartNotification();
                return;
            }

            // 检查是否到达设定的播放时间
            var elapsedTime = currentTime - _startTime;
            if (elapsedTime >= Settings.PlayTime && !_isPlaying)
            {
                StartNotification();
                return;
            }

            // 更新OverlayContent
            if (_isPlaying)
            {
                UpdateOverlayContent();
            }
        }

        private void StartNotification()
        {
            _notificationStartTime = DateTime.Now;
            _startTime = DateTime.Now;
            _isPlaying = true;
            ShowNotification();
        }

        private void ShowNotification()
        {
            _overlayTextBlock = new TextBlock
            {
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Center
            };

            // 创建包含图标和文本的StackPanel
            var stackPanel = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Center
            };

            // 右侧图标
            var rightIcon = new PackIcon
            {
                Kind = PackIconKind.QueueMusic,
                Width = 24,
                Height = 24,
                Margin = new Thickness(4, 0, 0, 0) // 添加左边距
            };

            // 文本
            var textBlock = new TextBlock(new Run("歌词"))
            {
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Center
            };

            // 将图标和文本添加到StackPanel
            stackPanel.Children.Add(textBlock);
            stackPanel.Children.Add(rightIcon);

            // 调用ShowNotification方法显示提醒
            NotificationHostService.ShowNotification(new NotificationRequest()
            {
                MaskContent = stackPanel,
                OverlayContent = _overlayTextBlock,
                OverlayDuration = Settings.OverlayDuration // 设置正文显示时长
            });

            _isPlaying = true; // 设置标志位，表示提醒已显示
        }

        private void UpdateOverlayContent()
        {
            var elapsedTime = DateTime.Now - _notificationStartTime;
            var currentLyrics = Settings.ParsedLyrics
                .Where(line => line.Timestamp <= elapsedTime)
                .OrderByDescending(line => line.Timestamp)
                .FirstOrDefault();

            if (_overlayTextBlock != null)
            {
                _overlayTextBlock.Text = currentLyrics?.Text ?? "";
            }
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            // 启动服务时的逻辑
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            // 停止服务时的逻辑
            _isPlaying = false;
        }
    }
}
