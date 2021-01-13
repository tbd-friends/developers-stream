using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using core;
using core.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using viewmodels;

namespace application.Query.Administration.Handlers
{
    public class GetPendingClaimsHandler : IRequestHandler<GetPendingClaims, IEnumerable<StreamerClaimViewModel>>
    {
        private readonly IApplicationContext _context;

        public GetPendingClaimsHandler(IApplicationContext context)
        {
            _context = context;
        }

        public Task<IEnumerable<StreamerClaimViewModel>> Handle(GetPendingClaims request, CancellationToken cancellationToken)
        {
            var pending = from sc in _context.StreamerClaimRequests
                          where sc.Status == ClaimRequestStatus.PendingApproval
                          select new StreamerClaimViewModel
                          {
                              Id = sc.Id,
                              CurrentEmail = sc.CurrentEmail,
                              UpdatedEmail = sc.UpdatedEmail,
                              Created = sc.Created
                          };

            return Task.FromResult(pending.AsEnumerable());
        }
    }
}