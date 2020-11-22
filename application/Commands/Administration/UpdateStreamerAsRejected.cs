using System;
using MediatR;

namespace application.Commands.Administration
{
    public class UpdateStreamerAsRejected : IRequest
    {
        public Guid Id { get; set; }
    }
}