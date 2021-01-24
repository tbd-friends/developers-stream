using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using core;
using core.Enums;
using MediatR;

namespace application.Commands.Administration.Handlers
{
    public class RejectOwnershipRequestHandler : IRequestHandler<RejectOwnershipRequest>
    {
        private readonly IApplicationContext _context;

        public RejectOwnershipRequestHandler(IApplicationContext context)
        {
            _context = context;
        }

        public Task<Unit> Handle(RejectOwnershipRequest request, CancellationToken cancellationToken)
        {
            var claimRequest = _context.StreamerClaimRequests.Single(rq => rq.Id == request.ClaimRequestId);

            claimRequest.Status = OwnershipRequestStatus.Rejected;
            claimRequest.Updated = DateTime.UtcNow;

            _context.SaveChanges();

            return Unit.Task;
        }
    }
}