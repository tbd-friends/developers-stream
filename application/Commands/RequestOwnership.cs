using System;
using application.Infrastructure;

namespace application.Commands
{
    public class RequestOwnership : IRequestWithUser
    {
        public Guid ClaimedStreamerId { get; set; }
        public string Email { get; set; }
        public string ProfileId { get; set; }
        public string Details { get; set; }
    }
}