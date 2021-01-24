using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using core;
using MediatR;

namespace application.Commands.Administration.Handlers
{
    public class ApproveOwnershipRequestHandler : IRequestHandler<ApproveOwnershipRequest>
    {
        private readonly IApplicationContext _context;
        private readonly IMediator _mediator;

        public ApproveOwnershipRequestHandler(IApplicationContext context, IMediator mediator)
        {
            _context = context;
            _mediator = mediator;
        }

        public async Task<Unit> Handle(ApproveOwnershipRequest request, CancellationToken cancellationToken)
        {
            var claimRequest = _context.StreamerClaimRequests.Single(cr => cr.Id == request.ClaimRequestId);

            await _mediator.Send(new AssociateStreamerWithRegistrar
            {
                StreamerId = claimRequest.ClaimedStreamerId,
                Email = claimRequest.UpdatedEmail,
                ProfileId = claimRequest.ProfileId
            }, cancellationToken);

            await _mediator.Send(new UpdateOwnershipRequestAsApproved { ClaimRequestId = request.ClaimRequestId },
                cancellationToken);

            return Unit.Value;
        }
    }
}