using MediatR;

namespace application.Commands.Administration
{
    public class RemoveUser : IRequest
    {
        public string Email { get; set; }
    }
}