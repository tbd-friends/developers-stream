using System;
using MediatR;

namespace application.Commands
{
    public class AssociateStreamerWithRegistrar : IRequest
    {
        public string ProfileId { get; set; }
        public string Email { get; set; }
        public Guid StreamerId { get; set; }
    }
}