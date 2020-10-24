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
    public class when_platforms_are_provided
    {
        private const string StreamerPlatformUrl = "https://platform/streamer-name";
        private const string StreamerPlatformName = "platform";
        private RegisterNewStreamerHandler _subject;
        private Mock<IApplicationContext> _context;

        public readonly string StreamerName = "streamer-name";

        public when_platforms_are_provided()
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
                Name = StreamerName,
                Platforms = new List<RegisterNewStreamer.Platform>
                {
                    new RegisterNewStreamer.Platform() {Name = StreamerPlatformName, Url = StreamerPlatformUrl}
                }
            }, CancellationToken.None);
        }

        [Fact]
        public void name_is_captured()
        {
            _context.Verify(ctx =>
                ctx.Insert(It.Is<Streamer>(
                    s => s.Name == StreamerName)), Times.Once);
        }

        [Fact]
        public void platform_is_added()
        {
            _context.Verify(ctx =>
                ctx.Insert(It.Is<StreamerPlatform>(sp =>
                    sp.Url == StreamerPlatformUrl && sp.Name == StreamerPlatformName)), Times.Once);
        }

        [Fact]
        public void information_was_committed()
        {
            _context.Verify(ctx => ctx.SaveChanges(), Times.Once);
        }
    }
}
