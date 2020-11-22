using System;
using MediatR;

namespace application.Commands.Administration
{
    public class DeleteStreamer : IRequest
    {
        public Guid Id { get; set; }
    }
}