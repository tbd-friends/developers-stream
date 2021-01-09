using System;
using core.Enums;

namespace core.Models
{
    public class StreamerClaimRequest
    {
        public Guid Id { get; set; }
        public Guid ClaimedStreamerId { get; set; }
        public string CurrentEmail { get; set; }
        public string UpdatedEmail { get; set; }
        public bool IsApproved { get; set; }
        public ClaimRequestStatus Status { get; set; }
        public DateTime Created { get; set; }
        public DateTime? Updated { get; set; }
    }
}