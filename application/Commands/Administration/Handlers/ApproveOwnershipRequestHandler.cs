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
            var claim =
                (from cr in _context.StreamerClaimRequests
                 join rs in _context.RegisteredStreamers on cr.ClaimedStreamerId equals rs.StreamerId into rsx
                 from owner in rsx.DefaultIfEmpty()
                 where cr.Id == request.ClaimRequestId
                 select new
                 {
                     Request = cr,
                     CurrentOwner = owner
                 }).Single();

            if (claim.CurrentOwner != null)
            {
                await _mediator.Send(new RemoveRegisteredStreamer
                {
                    Id = claim.CurrentOwner.Id
                }, cancellationToken);
            }

            await _mediator.Send(new AssociateStreamerWithRegistrar
            {
                StreamerId = claim.Request.ClaimedStreamerId,
                Email = claim.Request.UpdatedEmail,
                ProfileId = claim.Request.ProfileId
            }, cancellationToken);

            await _mediator.Send(new UpdateOwnershipRequestAsApproved { ClaimRequestId = request.ClaimRequestId },
                cancellationToken);

            return Unit.Value;
        }
    }
}