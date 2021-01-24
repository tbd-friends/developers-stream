using System.Threading;
using System.Threading.Tasks;
using core;
using core.Enums;
using core.Models;
using MediatR;

namespace application.Commands.Handlers
{
    public class RegisterNewStreamerHandler : IRequestHandler<RegisterNewStreamer>
    {
        private readonly IMediator _mediator;
        private readonly IApplicationContext _context;

        public RegisterNewStreamerHandler(IMediator mediator, IApplicationContext context)
        {
            _mediator = mediator;
            _context = context;
        }

        public async Task<Unit> Handle(RegisterNewStreamer request, CancellationToken cancellationToken)
        {
            await InsertStreamerData(request, cancellationToken);

            return Unit.Value;
        }

        private async Task InsertStreamerData(RegisterNewStreamer request, CancellationToken cancellationToken)
        {
            var streamer = new Streamer
            {
                Name = request.Name,
                Description = request.Description,
                IsStreamer = request.IsStreamer,
                Status = StreamerStatus.PendingVerification
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

            await _mediator.Send(new AssociateStreamerWithRegistrar
            {
                StreamerId = streamer.Id,
                Email = request.Email,
                ProfileId = request.ProfileId
            }, cancellationToken);

            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}