
using ClassIsland.Core.Abstractions;
using ClassIsland.Core.Attributes;
using LyricsSupportForClassIsland.Services.NotificationProviders;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace LyricsSupportForClassIsland
{
    [PluginEntrance]
    public class Plugin : PluginBase
    {
        public override void Initialize(HostBuilderContext context, IServiceCollection services)
        {
            services.AddHostedService<LyricsNotificationProvider>();
            services.AddHostedService<NCMNotificationProvider>();
        }
    }

}
