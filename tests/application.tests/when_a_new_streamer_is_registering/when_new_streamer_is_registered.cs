using System.Collections.Generic;
using System.Threading;
using application.Commands;
using application.Commands.Handlers;
using core;
using core.Models;
using Moq;
using Xunit;

namespace application.tests.when_a_new_streamer_is_registering
{
    public class when_new_streamer_is_registered
    {
        private RegisterNewStreamerHandler _subject;
        private Mock<IApplicationContext> _context;

        public when_new_streamer_is_registered()
        {
            Arrange();

            Act();
        }

        private void Arrange()
        {
            _context = new Mock<IApplicationContext>();

            _subject = new RegisterNewStreamerHandler(_context.Object);
        }

        private void Act()
        {
            _subject.Handle(new RegisterNewStreamer
            {
                Name = "StreamerName",
                Platforms = new List<RegisterNewStreamer.Platform>
                    {new RegisterNewStreamer.Platform() {Name = "Platform", Url = "PlatformUrl"}},
                Technologies = null
            }, CancellationToken.None);
        }

        [Fact]
        public void streamer_is_added_in_unverified_state()
        {
            _context.Verify(ctx =>
                ctx.Insert(It.Is<Streamer>(s =>
                    s.Name == "StreamerName" && s.Status == StreamerStatus.PendingVerification)));
        }
    }

    public enum StreamerStatus
    {
        PendingVerification = 0,
        Verified, 
        Rejected
    }
}