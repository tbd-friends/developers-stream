using System;
using application.Infrastructure;

namespace application.Commands
{
    public class MakeClaimRequest : IRequestWithUser
    {
        public Guid ClaimedStreamerId { get; set; }
        public string Email { get; set; }
    }
}