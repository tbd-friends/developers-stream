using System;

namespace viewmodels
{
    public class StreamerClaimViewModel
    {
        public Guid Id { get; set; }
        public string CurrentEmail { get; set; }
        public string UpdatedEmail { get; set; }
        public DateTime Created { get; set; }
    }
}