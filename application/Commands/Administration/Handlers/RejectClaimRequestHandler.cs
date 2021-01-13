using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using core;
using core.Enums;
using MediatR;

namespace application.Commands.Administration.Handlers
{
    public class RejectClaimRequestHandler : IRequestHandler<RejectClaimRequest>
    {
        private readonly IApplicationContext _context;

        public RejectClaimRequestHandler(IApplicationContext context)
        {
            _context = context;
        }

        public Task<Unit> Handle(RejectClaimRequest request, CancellationToken cancellationToken)
        {
            var claimRequest = _context.StreamerClaimRequests.Single(rq => rq.Id == request.ClaimRequestId);

            claimRequest.Status = ClaimRequestStatus.Rejected;
            claimRequest.Updated = DateTime.UtcNow;

            _context.SaveChanges();

            return Unit.Task;
        }
    }
}