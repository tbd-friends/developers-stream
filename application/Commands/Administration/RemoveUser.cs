using application.Infrastructure;
using MediatR;

namespace application.Commands.Administration
{
    public class RemoveUser : IRequestWithUser
    {
        public string Email { get; set; }
        public string ProfileId { get; set; }
    }
}