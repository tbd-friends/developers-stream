using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using core;
using core.Enums;
using MediatR;

namespace application.Commands.Administration.Handlers
{
    public class UpdateStreamerAsRejectedHandler : IRequestHandler<UpdateStreamerAsRejected>
    {
        private readonly IApplicationContext _context;

        public UpdateStreamerAsRejectedHandler(IApplicationContext context)
        {
            _context = context;
        }

        public Task<Unit> Handle(UpdateStreamerAsRejected request, CancellationToken cancellationToken)
        {
            var streamer = _context.Streamers.Single(s => s.Id == request.Id);

            streamer.Status = StreamerStatus.Rejected;

            _context.SaveChanges();

            return Unit.Task;
        }
    }
}