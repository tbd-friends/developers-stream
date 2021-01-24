using System;
using MediatR;

namespace application.Commands
{
    public class RemoveRegisteredStreamer : IRequest
    {
        public Guid Id { get; set; }
    }
}