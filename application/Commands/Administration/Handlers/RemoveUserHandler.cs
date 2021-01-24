using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using application.Commands;
using application.Commands.Administration;
using core;
using MediatR;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;

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
            var attachedStreamer = (
                from rs in _context.RegisteredStreamers
                join s in _context.Streamers on rs.StreamerId equals s.Id
                where rs.Email == request.Email && s.IsStreamer
                select rs
            ).SingleOrDefault();

            if (attachedStreamer != null)
            {
                await _mediator.Send(new DeleteStreamer { Id = attachedStreamer.StreamerId }, cancellationToken);
            }

            var registeredStreamers = from rs in _context.RegisteredStreamers
                                      where rs.Email == request.Email
                                      select rs;

            foreach (var registeredStreamer in registeredStreamers)
            {
                await _mediator.Send(new RemoveRegisteredStreamer { Id = registeredStreamer.Id }, cancellationToken);
            }

            return Unit.Value;
        }
    }
}