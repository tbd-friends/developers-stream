using System;
using MediatR;

namespace application.Commands.Administration
{
    public class ApproveOwnershipRequest : IRequest
    {
        public Guid ClaimRequestId { get; set; }
    }
}