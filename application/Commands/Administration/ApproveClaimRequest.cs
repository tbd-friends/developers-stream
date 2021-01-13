using System;
using MediatR;

namespace application.Commands.Administration
{
    public class ApproveClaimRequest : IRequest
    {
        public Guid ClaimRequestId { get; set; }
    }
}