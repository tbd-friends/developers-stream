using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using core;
using MediatR;
using viewmodels;

namespace application.Query.Handlers
{
    public class GetStreamersHandler : IRequestHandler<GetStreamers, PagedResult<StreamViewModel>>
    {
        private readonly IApplicationContext _context;

        public GetStreamersHandler(IApplicationContext context)
        {
            _context = context;
        }

        public Task<PagedResult<StreamViewModel>> Handle(GetStreamers request, CancellationToken cancellationToken)
        {
            var streams = _context.Streamers;

            if (!string.IsNullOrEmpty(request.Term))
            {
                streams = from stream in streams
                          where stream.Name.Contains(request.Term) ||
                                stream.Description.Contains(request.Term)
                          select stream;
            }

            return Task.FromResult(
                new PagedResult<StreamViewModel>
                {
                    TotalItems = streams.Count(),
                    Results = from stream in streams.Skip((request.PageNumber - 1) * request.PageSize)
                            .Take(request.PageSize)
                            .AsEnumerable()
                              select new StreamViewModel
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
                                              }
                              }
                });
        }
    }
}