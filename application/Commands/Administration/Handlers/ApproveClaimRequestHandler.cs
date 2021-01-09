using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using core;
using MediatR;

namespace application.Commands.Administration.Handlers
{
    public class ApproveClaimRequestHandler : IRequestHandler<ApproveClaimRequest>
    {
        private readonly IApplicationContext _context;
        private readonly IMediator _mediator;

        public ApproveClaimRequestHandler(IApplicationContext context, IMediator mediator)
        {
            _context = context;
            _mediator = mediator;
        }

        public async Task<Unit> Handle(ApproveClaimRequest request, CancellationToken cancellationToken)
        {
            var claimRequest = _context.StreamerClaimRequests.Single(cr => cr.Id == request.ClaimRequestId);

            await _mediator.Send(new UpdateStreamerEmail
            { StreamerId = claimRequest.ClaimedStreamerId, UpdatedEmail = claimRequest.UpdatedEmail },
                cancellationToken);

            await _mediator.Send(new UpdateClaimRequestAsApproved { ClaimRequestId = request.ClaimRequestId },
                cancellationToken);

            return Unit.Value;
        }
    }
}