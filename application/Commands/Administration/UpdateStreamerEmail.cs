using System;
using MediatR;

namespace application.Commands.Administration
{
    public class UpdateStreamerEmail : IRequest
    {
        public Guid StreamerId { get; set; }
        public string UpdatedEmail { get; set; }
    }
}