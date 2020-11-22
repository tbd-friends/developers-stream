using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using core;
using core.Enums;
using MediatR;

namespace application.Commands.Administration.Handlers
{
    public class VerifyStreamerHandler : IRequestHandler<VerifyStreamer>
    {
        private readonly IApplicationContext _context;

        public VerifyStreamerHandler(IApplicationContext context)
        {
            _context = context;
        }

        public Task<Unit> Handle(VerifyStreamer request, CancellationToken cancellationToken)
        {
            var streamer = _context.Streamers.Single(s => s.Id == request.Id);

            streamer.Status = StreamerStatus.Verified;

            _context.SaveChanges();

            return Unit.Task;
        }
    }
}