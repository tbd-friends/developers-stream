using System;
using System.Collections.Generic;

namespace viewmodels
{
    public class StreamerViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public IEnumerable<PlatformViewModel> Platforms { get; set; }
        public IEnumerable<string> Technologies { get; set; }
    }
}