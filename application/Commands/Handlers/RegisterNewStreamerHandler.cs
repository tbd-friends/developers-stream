using System.Threading;
using System.Threading.Tasks;
using core;
using core.Models;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

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
            InsertStreamerData(request);

            return Unit.Task;
        }

        private void InsertStreamerData(RegisterNewStreamer request)
        {
            var streamer = new Streamer
            {
                Name = request.Name,
                Description = request.Description,
                IsStreamer = request.IsStreamer,
                Email = request.Email
            };

            _context.Insert(streamer);

            foreach (var platform in request.Platforms)
            {
                _context.Insert(new StreamerPlatform
                {
                    StreamerId = streamer.Id,
                    Name = platform.Name,
                    Url = platform.Url
                });
            }

            if (request.Technologies != null)
            {
                foreach (var technology in request.Technologies)
                {
                    _context.Insert(new StreamerTechnology
                    {
                        StreamerId = streamer.Id,
                        TechnologyId = technology
                    });
                }
            }

            _context.SaveChanges();
        }
    }
}