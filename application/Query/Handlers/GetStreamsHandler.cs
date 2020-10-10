using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using core;
using MediatR;
using viewmodels;

namespace application.Query.Handlers
{
    public class GetStreamsHandler : IRequestHandler<GetStreams, IEnumerable<StreamViewModel>>
    {
        private readonly IApplicationContext _context;

        public GetStreamsHandler(IApplicationContext context)
        {
            _context = context;
        }

        public Task<IEnumerable<StreamViewModel>> Handle(GetStreams request, CancellationToken cancellationToken)
        {
            var streams = from stream in _context.Streamers
                          select new StreamViewModel
                          {
                              Id = stream.Id,
                              Name = stream.Name,
                              Description = stream.Description
                          };

            return Task.FromResult(streams.AsEnumerable());
        }
    }
}