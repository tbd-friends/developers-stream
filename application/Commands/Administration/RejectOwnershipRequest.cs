using System;
using MediatR;

namespace application.Commands.Administration
{
    public class RejectOwnershipRequest : IRequest
    {
        public Guid ClaimRequestId { get; set; }
    }
}