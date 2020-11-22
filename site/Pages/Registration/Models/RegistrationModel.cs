using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using site.Models;

namespace site.Pages.Registration.Models
{
    public class RegistrationModel
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Description { get; set; }
        public bool IsStreamer { get; set; }

        public List<Platform> Platforms { get; set; } = new List<Platform>();
        public Guid[] Technologies { get; set; }
    }
}