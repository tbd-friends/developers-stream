using System;
using core.Enums;

namespace core.Models
{
    public class StreamerOwnershipRequest
    {
        public Guid Id { get; set; }
        public Guid ClaimedStreamerId { get; set; }
        public string CurrentEmail { get; set; }
        public string UpdatedEmail { get; set; }
        public string ProfileId { get; set; }
        public string Details { get; set; }
        public OwnershipRequestStatus Status { get; set; }
        public DateTime Created { get; set; }
        public DateTime? Updated { get; set; }
    }
}