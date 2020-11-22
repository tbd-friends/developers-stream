using System;
using MediatR;

namespace application.Commands.Administration
{
    public class VerifyStreamer : IRequest
    {
        public Guid Id { get; set; }
    }
}