using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using administration.viewmodels;
using core;
using MediatR;

namespace application.Query.Streamer.Handlers
{
    public class GetAssociatedStreamersHandler : IRequestHandler<GetAssociatedStreamers, IEnumerable<StreamerViewModel>>
    {
        private readonly IApplicationContext _context;

        public GetAssociatedStreamersHandler(IApplicationContext context)
        {
            _context = context;
        }
        public Task<IEnumerable<StreamerViewModel>> Handle(GetAssociatedStreamers request, CancellationToken cancellationToken)
        {
            var streams = from rs in _context.RegisteredStreamers
                          join s in _context.Streamers on rs.StreamerId equals s.Id
                          where rs.Email == request.Email
                          select s;

            var results = from stream in streams
                          select new StreamerViewModel
                          {
                              Id = stream.Id,
                              Name = stream.Name,
                              Description = stream.Description,
                              Platforms = from p in _context.StreamerPlatforms
                                          where p.StreamerId == stream.Id
                                          select new PlatformViewModel
                                          {
                                              Name = p.Name,
                                              Url = p.Url
                                          },
                              Technologies = from st in _context.StreamerTechnologies
                                             join a in _context.AvailableTechnologies on st.TechnologyId equals a.Id
                                             where st.StreamerId == stream.Id
                                             select a.Name
                          };

            return Task.FromResult(results.AsEnumerable());
        }
    }
}