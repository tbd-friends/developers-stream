using System;
using System.Collections;
using System.Collections.Generic;
using application.Infrastructure;
using MediatR;

namespace application.Commands
{
    public class RegisterNewStreamer : IRequestWithUser
    {
        public string Name { get; set; }
        public string ProfileId { get; set; }
        public string Email { get; set; }
        public string Description { get; set; }
        public IEnumerable<Platform> Platforms { get; set; }
        public IEnumerable<Guid> Technologies { get; set; }
        public bool IsStreamer { get; set; }


        public class Platform
        {
            public string Name { get; set; }
            public string Url { get; set; }
        }
    }
}