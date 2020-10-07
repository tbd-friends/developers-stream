using MediatR;

namespace application.Commands
{
    public class RegisterNewStreamer : IRequest
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }
}