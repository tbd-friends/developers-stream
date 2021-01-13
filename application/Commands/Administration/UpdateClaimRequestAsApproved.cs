using System;
using MediatR;

namespace application.Commands.Administration
{
    public class UpdateClaimRequestAsApproved : IRequest
    {
        public Guid ClaimRequestId { get; set; }
    }
}