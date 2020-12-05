using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using core;
using MediatR;
using viewmodels;

namespace application.Query.Handlers
{
    public class GetAllTwitchStreamersHandler : IRequestHandler<GetAllTwitchStreamers, IEnumerable<TwitchStreamerViewModel>>
    {
        private readonly IApplicationContext _context;

        public GetAllTwitchStreamersHandler(IApplicationContext context)
        {
            _context = context;
        }

        public Task<IEnumerable<TwitchStreamerViewModel>> Handle(GetAllTwitchStreamers request, CancellationToken cancellationToken)
        {
            var streamers = from s in _context.Streamers
                            join p in _context.StreamerPlatforms on s.Id equals p.StreamerId
                            where p.Name == "twitch"
                            select new TwitchStreamerViewModel()
                            {
                                Id = s.Id,
                                Name = p.Url.Substring(p.Url.LastIndexOf('/') + 1)
                            };

            return Task.FromResult(streamers.AsEnumerable());
        }
    }
}