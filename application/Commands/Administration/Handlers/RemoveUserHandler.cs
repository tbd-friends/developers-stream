using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using application.Commands.Administration;
using core;
using MediatR;

namespace application.Query.Administration.Handlers
{
    public class RemoveUserHandler : IRequestHandler<RemoveUser>
    {
        private readonly IApplicationContext _context;
        private readonly IMediator _mediator;

        public RemoveUserHandler(IApplicationContext context, IMediator mediator)
        {
            _context = context;
            _mediator = mediator;
        }

        public async Task<Unit> Handle(RemoveUser request, CancellationToken cancellationToken)
        {
            var streamers = from s in _context.Streamers
                            where s.Email == request.Email
                            select s;

            foreach (var streamer in streamers)
            {
                await _mediator.Send(new DeleteStreamer { Id = streamer.Id }, cancellationToken);
            }

            return Unit.Value;
        }
    }
}