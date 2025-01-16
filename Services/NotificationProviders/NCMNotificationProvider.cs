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
    public class NCMNotificationProvider : INotificationProvider, IHostedService
    {
        public string Name { get; set; } = "NCM歌词提醒";
        public string Description { get; set; } = "与NCM同步的歌词提醒。";
        public Guid ProviderGuid { get; set; } = new Guid("44DDDCBD-F43A-4DFB-8792-0B9C5491B826");
        public object? SettingsElement { get; set; }
        public object? IconElement { get; set; }

        /// <summary>
        /// 这个属性用来存储提醒的设置。
        /// </summary>
        private NCMNotificationSettings Settings { get; }

        //订阅课表处理前事件PreMainTimerTicked
        private INotificationHostService NotificationHostService { get; }
        public ILessonsService LessonsService { get; }

        private DateTime _startTime;
        private DateTime _notificationStartTime;
        private TextBlock _overlayTextBlock;
        private bool _isPlaying;

        public NCMNotificationProvider(INotificationHostService notificationHostService,
         ILessonsService lessonsService)
        {
            //提醒设置界面的初始化
            NotificationHostService = notificationHostService;
            LessonsService = lessonsService;
            NotificationHostService.RegisterNotificationProvider(this);
            Settings = NotificationHostService.GetNotificationProviderSettings<NCMNotificationSettings>(ProviderGuid);
            SettingsElement = new NCMNotificationProviderSettingsControl(Settings);

            //提醒设置界面的图标
            IconElement = new PackIcon
            {
                Kind = PackIconKind.QueueMusic,
                Width = 24,
                Height = 24
            };

            LessonsService.PreMainTimerTicked += LessonsServiceOnPreMainTimerTicked;  //注册事件
        }

        private void LessonsServiceOnPreMainTimerTicked(object? sender, EventArgs e)
        {
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
                //OverlayDuration = Settings.OverlayDuration // 设置正文显示时长
            });

        }

        private void UpdateOverlayContent()
        {
            
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
         
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
        
        }
    }
}
