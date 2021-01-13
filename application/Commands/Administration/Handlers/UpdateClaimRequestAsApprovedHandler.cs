using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using core;
using core.Enums;
using MediatR;

namespace application.Commands.Administration.Handlers
{
    public class UpdateClaimRequestAsApprovedHandler : IRequestHandler<UpdateClaimRequestAsApproved>
    {
        private readonly IApplicationContext _context;

        public UpdateClaimRequestAsApprovedHandler(IApplicationContext context)
        {
            _context = context;
        }

        public Task<Unit> Handle(UpdateClaimRequestAsApproved request, CancellationToken cancellationToken)
        {
            var claimRequest = _context.StreamerClaimRequests.Single(cr => cr.Id == request.ClaimRequestId);

            claimRequest.Status = ClaimRequestStatus.Approved;
            claimRequest.Updated = DateTime.UtcNow;

            _context.SaveChanges();

            return Unit.Task;
        }
    }
}