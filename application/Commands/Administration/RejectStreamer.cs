using System;
using MediatR;

namespace application.Commands.Administration
{
    public class RejectStreamer : IRequest
    {
        public Guid Id { get; set; }
    }
}