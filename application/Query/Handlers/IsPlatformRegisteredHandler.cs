using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using core;
using MediatR;

namespace application.Query.Handlers
{
    public class IsPlatformRegisteredHandler : IRequestHandler<IsPlatformRegistered, bool>
    {
        private readonly IApplicationContext _context;

        public IsPlatformRegisteredHandler(IApplicationContext context)
        {
            _context = context;
        }

        public Task<bool> Handle(IsPlatformRegistered request, CancellationToken cancellationToken)
        {
            var alreadyRegistered = (from p in _context.StreamerPlatforms
                                     where p.Url == request.Url
                                     select p).Any();

            return Task.FromResult(alreadyRegistered);
        }
    }
}