using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using core;
using core.Enums;
using MediatR;
using administration.viewmodels;

namespace application.Query.Administration.Handlers
{
    public class GetPendingStreamersHandler : IRequestHandler<GetPendingStreamers, IEnumerable<StreamerViewModel>>
    {
        private readonly IApplicationContext _context;

        public GetPendingStreamersHandler(IApplicationContext context)
        {
            _context = context;
        }

        public Task<IEnumerable<StreamerViewModel>> Handle(GetPendingStreamers request, CancellationToken cancellationToken)
        {
            var pendingStreamers = from streamer in _context.Streamers
                                   where streamer.Status == StreamerStatus.Rejected ||
                                         streamer.Status == StreamerStatus.PendingVerification
                                   select streamer;

            return Task.FromResult((from s in pendingStreamers
                                    select new StreamerViewModel
                                    {
                                        Id = s.Id,
                                        Name = s.Name,
                                        Description = s.Description,
                                        IsStreamer = s.IsStreamer,
                                        Platforms = from p in _context.StreamerPlatforms
                                                    where p.StreamerId == s.Id
                                                    select new PlatformViewModel { Name = p.Name, Url = p.Url },
                                        Technologies = from st in _context.StreamerTechnologies
                                                       join t in _context.AvailableTechnologies on st.TechnologyId equals t.Id
                                                       where st.StreamerId == s.Id
                                                       select t.Name,
                                        IsPendingVerification = s.Status == StreamerStatus.PendingVerification,
                                        IsRejected = s.Status == StreamerStatus.Rejected
                                    }).AsEnumerable());
        }
    }
}