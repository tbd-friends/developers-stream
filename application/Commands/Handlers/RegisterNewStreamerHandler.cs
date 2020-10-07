using System.Threading;
using System.Threading.Tasks;
using core;
using core.Models;
using MediatR;

namespace application.Commands.Handlers
{
    public class RegisterNewStreamerHandler : IRequestHandler<RegisterNewStreamer>
    {
        private readonly IApplicationContext _context;

        public RegisterNewStreamerHandler(IApplicationContext context)
        {
            _context = context;
        }

        public Task<Unit> Handle(RegisterNewStreamer request, CancellationToken cancellationToken)
        {
            _context.Insert(new Streamer
            {
                Name = request.Name,
                Description = request.Description
            });

            _context.SaveChanges();

            return Unit.Task;
        }
    }
}