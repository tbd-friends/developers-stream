using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using core;
using MediatR;

namespace application.Commands.Administration.Handlers
{
    public class UpdateStreamerEmailHandler : IRequestHandler<UpdateStreamerEmail>
    {
        private readonly IApplicationContext _context;

        public UpdateStreamerEmailHandler(IApplicationContext context)
        {
            _context = context;
        }

        public Task<Unit> Handle(UpdateStreamerEmail request, CancellationToken cancellationToken)
        {
            var streamer = _context.Streamers.Single(s => s.Id == request.StreamerId);

            streamer.Email = request.UpdatedEmail;

            _context.SaveChanges();

            return Unit.Task;
        }
    }
}