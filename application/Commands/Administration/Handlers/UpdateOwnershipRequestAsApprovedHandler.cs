using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using core;
using core.Enums;
using MediatR;

namespace application.Commands.Administration.Handlers
{
    public class UpdateOwnershipRequestAsApprovedHandler : IRequestHandler<UpdateOwnershipRequestAsApproved>
    {
        private readonly IApplicationContext _context;

        public UpdateOwnershipRequestAsApprovedHandler(IApplicationContext context)
        {
            _context = context;
        }

        public Task<Unit> Handle(UpdateOwnershipRequestAsApproved request, CancellationToken cancellationToken)
        {
            var claimRequest = _context.StreamerClaimRequests.Single(cr => cr.Id == request.ClaimRequestId);

            claimRequest.Status = OwnershipRequestStatus.Approved;
            claimRequest.Updated = DateTime.UtcNow;

            _context.SaveChanges();

            return Unit.Task;
        }
    }
}