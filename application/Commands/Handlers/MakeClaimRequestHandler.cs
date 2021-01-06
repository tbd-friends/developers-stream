using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using core;
using core.Models;
using MediatR;

namespace application.Commands.Handlers
{
    public class MakeClaimRequestHandler : IRequestHandler<MakeClaimRequest>
    {
        private readonly IApplicationContext _context;

        public MakeClaimRequestHandler(IApplicationContext context)
        {
            _context = context;
        }

        public Task<Unit> Handle(MakeClaimRequest request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(request.Email))
            {
                return Unit.Task;
            }

            var claimedStream = _context.Streamers.Single(s => s.Id == request.ClaimedStreamerId);

            _context.Insert(new StreamerClaimRequest
            {
                CurrentEmail = claimedStream.Email,
                UpdatedEmail = request.Email,
                Created = DateTime.UtcNow
            });

            _context.SaveChanges();

            return Unit.Task;
        }
    }
}