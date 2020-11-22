using System;
using System.Collections.Generic;

namespace administration.viewmodels
{
    public class StreamerViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsStreamer { get; set; }
        public IEnumerable<PlatformViewModel> Platforms { get; set; }
        public IEnumerable<string> Technologies { get; set; }
        public bool IsPendingVerification { get; set; }
        public bool IsRejected { get; set; }
    }
}