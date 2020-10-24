using System;

namespace core.Models
{
    public class StreamerPlatform
    {
        public Guid Id { get; set; }
        public Guid StreamerId { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
    }
}