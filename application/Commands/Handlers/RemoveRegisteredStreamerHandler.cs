using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using core;
using MediatR;

namespace application.Commands.Handlers
{
    public class RemoveRegisteredStreamerHandler : IRequestHandler<RemoveRegisteredStreamer>
    {
        private readonly IApplicationContext _context;

        public RemoveRegisteredStreamerHandler(IApplicationContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(RemoveRegisteredStreamer request, CancellationToken cancellationToken)
        {
            var registeredStreamer = _context.RegisteredStreamers.Single(rs => rs.Id == request.Id);

            _context.Delete(registeredStreamer);

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}