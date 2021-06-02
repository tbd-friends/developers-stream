using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using core;
using core.Enums;
using core.Models;
using MediatR;

namespace application.Commands.Handlers
{
    public class RequestOwnershipHandler : IRequestHandler<RequestOwnership>
    {
        private readonly IApplicationContext _context;

        public RequestOwnershipHandler(IApplicationContext context)
        {
            _context = context;
        }

        public Task<Unit> Handle(RequestOwnership requestOwnership, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(requestOwnership.Email))
            {
                return Unit.Task;
            }

            var claimedStream = (from s in _context.Streamers
                                 join rs in _context.RegisteredStreamers on s.Id equals rs.StreamerId
                                 where s.Id == requestOwnership.ClaimedStreamerId
                                 select new
                                 {
                                     rs.Email
                                 }).SingleOrDefault();

            _context.Insert(new StreamerOwnershipRequest
            {
                ClaimedStreamerId = requestOwnership.ClaimedStreamerId,
                CurrentEmail = claimedStream?.Email,
                UpdatedEmail = requestOwnership.Email,
                ProfileId = requestOwnership.ProfileId,
                Details = requestOwnership.Details,
                Created = DateTime.UtcNow,
                Status = OwnershipRequestStatus.PendingApproval
            });

            _context.SaveChanges();

            return Unit.Task;
        }
    }
}