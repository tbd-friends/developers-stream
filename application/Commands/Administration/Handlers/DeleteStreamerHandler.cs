using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using core;
using MediatR;

namespace application.Commands.Administration.Handlers
{
    public class DeleteStreamerHandler : IRequestHandler<DeleteStreamer>
    {
        private readonly IApplicationContext _context;

        public DeleteStreamerHandler(IApplicationContext context)
        {
            _context = context;
        }

        public Task<Unit> Handle(DeleteStreamer request, CancellationToken cancellationToken)
        {
            var streamer = _context.Streamers.Single(s => s.Id == request.Id);

            RemoveAssociatedPlatforms(request);

            RemoveAssociatedTechnologies(request);

            _context.Delete(streamer);

            _context.SaveChanges();

            return Unit.Task;
        }

        private void RemoveAssociatedPlatforms(DeleteStreamer request)
        {
            var platforms = _context.StreamerPlatforms.Where(p => p.StreamerId == request.Id);

            foreach (var platform in platforms)
            {
                _context.Delete(platform);
            }
        }

        private void RemoveAssociatedTechnologies(DeleteStreamer request)
        {
            var technologies = _context.StreamerTechnologies.Where(p => p.StreamerId == request.Id);

            foreach (var technology in technologies)
            {
                _context.Delete(technology);
            }
        }
    }
}