using System;

namespace core.Models
{
    public class RegisteredStreamer
    {
        public Guid Id { get; set; }
        public string ProfileId { get; set; }
        public string Email { get; set; }
        public Guid StreamerId { get; set; }
    }
}