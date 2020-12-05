using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using application.Query;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using twitchstreambot.api;

namespace site.Jobs
{
    public class MonitorTwitchStreamsJob : IHostedService
    {
        private readonly TwitchHelix _helix;
        private readonly IServiceProvider _provider;

        public MonitorTwitchStreamsJob(
            TwitchHelix helix,
            IServiceProvider provider)
        {
            _helix = helix;
            _provider = provider;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            using (var scope = _provider.CreateScope())
            {
                var mediator = scope.ServiceProvider.GetService<IMediator>();

                do
                {
                    //var streamers = await mediator.Send(new GetAllTwitchStreamers(), cancellationToken);

                    //var liveStreams = await _helix.GetStreams(streamers.Select(s => s.Name).ToArray());

                    // update 

                    await Task.Delay(TimeSpan.FromMinutes(1), cancellationToken);
                } while (!cancellationToken.IsCancellationRequested);
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}