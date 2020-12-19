using System;

namespace core.Models
{
    public class AvailableTechnology
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Aliases { get; set; }
    }
}