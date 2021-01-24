using System;
using MediatR;

namespace application.Commands.Administration
{
    public class UpdateOwnershipRequestAsApproved : IRequest
    {
        public Guid ClaimRequestId { get; set; }
    }
}