using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using core;
using MediatR;
using viewmodels;

namespace application.Query.Handlers
{
    public class GetStreamsHandler : IRequestHandler<GetStreams, PagedResult<StreamViewModel>>
    {
        private readonly IApplicationContext _context;

        public GetStreamsHandler(IApplicationContext context)
        {
            _context = context;
        }

        public Task<PagedResult<StreamViewModel>> Handle(GetStreams request, CancellationToken cancellationToken)
        {
            var streams = from stream in _context.Streamers
                          select new StreamViewModel
                          {
                              Id = stream.Id,
                              Name = stream.Name,
                              Description = stream.Description
                          };

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
                    Results = streams.Skip((request.PageNumber - 1) * request.PageSize).Take(request.PageSize)
                        .AsEnumerable()
                });
        }
    }
}