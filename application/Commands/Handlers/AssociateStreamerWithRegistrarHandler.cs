using System;
using System.Threading;
using System.Threading.Tasks;
using core;
using core.Models;
using MediatR;

namespace application.Commands.Handlers
{
    public class AssociateStreamerWithRegistrarHandler : IRequestHandler<AssociateStreamerWithRegistrar>
    {
        private readonly IApplicationContext _context;

        public AssociateStreamerWithRegistrarHandler(IApplicationContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(AssociateStreamerWithRegistrar request, CancellationToken cancellationToken)
        {
            _context.Insert(new RegisteredStreamer
            {
                Email = request.Email,
                ProfileId = request.ProfileId,
                StreamerId = request.StreamerId
            });

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}