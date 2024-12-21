using ClassIsland.Core.Abstractions.Services;
using ClassIsland.Shared.Interfaces;
using ClassIsland.Shared.Models.Notification;
using Microsoft.Extensions.Hosting;
using System;
using System.Timers;
using System.Windows.Controls;

namespace LyricsSupportForClassIsland.Services.NotificationProviders
{
    public class LyricsNotificationProvider : INotificationProvider, IHostedService
    {
        public string Name { get; set; } = "歌词提醒";
        public string Description { get; set; } = "定时发出歌词提醒。";
        public Guid ProviderGuid { get; set; } = new Guid("95AC7075-EC49-4D4D-BA1A-DDD1740713FC");
        public object? SettingsElement { get; set; }
        public object? IconElement { get; set; }

        private INotificationHostService NotificationHostService { get; }
        private System.Timers.Timer _pollingTimer; // 使用 System.Timers.Timer 进行轮询

        public LyricsNotificationProvider(INotificationHostService notificationHostService)
        {
            NotificationHostService = notificationHostService;
            NotificationHostService.RegisterNotificationProvider(this);

            // 初始化轮询定时器
            _pollingTimer = new System.Timers.Timer(1000); // 设置定时器间隔为1秒
            _pollingTimer.Elapsed += PollingTimerElapsed; // 订阅定时器事件
            _pollingTimer.Start(); // 启动定时器
        }

        private void PollingTimerElapsed(object? sender, ElapsedEventArgs e)
        {
            // 检查当前时间是否为16:00:00
            var currentTime = DateTime.Now;
            if (currentTime.Hour == 16 && currentTime.Minute == 0 && currentTime.Second == 0)
            {
                // 显示提醒
                ShowNotification();
                _pollingTimer.Stop(); // 停止定时器，避免重复提醒
            }
        }

        private void ShowNotification()
        {
            // 调用ShowNotification方法显示提醒
            NotificationHostService.ShowNotification(new NotificationRequest()
            {
                // 这里可以添加其他通知请求的属性
            });

            /*MaskContent = new TextBlock(new Run(Settings.Message))
            {
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Center
            };*/
        }



        public async Task StartAsync(CancellationToken cancellationToken)
        {
            // 启动服务时的逻辑
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            // 停止服务时的逻辑
            _pollingTimer?.Stop(); // 确保在服务停止时也停止定时器
        }
    }
}
