using System;
using core.Enums;

namespace core.Models
{
    public class Streamer
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsStreamer { get; set; }
        //public string Email { get; set; }
        public StreamerStatus Status { get; set; }
    }
}