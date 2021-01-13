using System;
using MediatR;

namespace application.Commands.Administration
{
    public class RejectClaimRequest : IRequest
    {
        public Guid ClaimRequestId { get; set; }
    }
}