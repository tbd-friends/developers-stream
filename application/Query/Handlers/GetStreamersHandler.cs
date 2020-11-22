using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using core;
using core.Enums;
using MediatR;
using viewmodels;

namespace application.Query.Handlers
{
    public class GetStreamersHandler : IRequestHandler<GetStreamers, PagedResult<StreamerViewModel>>
    {
        private readonly IApplicationContext _context;

        public GetStreamersHandler(IApplicationContext context)
        {
            _context = context;
        }

        public Task<PagedResult<StreamerViewModel>> Handle(GetStreamers request, CancellationToken cancellationToken)
        {
            var streams = from stream in _context.Streamers
                          where stream.Status == StreamerStatus.Verified
                          select stream;

            if (!string.IsNullOrEmpty(request.Term))
            {
                streams = from stream in streams
                          where stream.Name.Contains(request.Term) ||
                                stream.Description.Contains(request.Term)
                          select stream;
            }

            return Task.FromResult(
                new PagedResult<StreamerViewModel>
                {
                    TotalItems = streams.Count(),
                    Results = from stream in streams.Skip((request.PageNumber - 1) * request.PageSize)
                            .Take(request.PageSize)
                            .AsEnumerable()
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
                                              }
                              }
                });
        }
    }
}