using System.Threading;
using application.Commands;
using application.Commands.Handlers;
using core;
using core.Models;
using Moq;
using Xunit;

namespace application.tests
{
    public class when_a_new_streamer_is_registering
    {
        private RegisterNewStreamerHandler _subject;
        private Mock<IApplicationContext> _context;

        public readonly string StreamerName = "streamer-name";

        public when_a_new_streamer_is_registering()
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
                Name = StreamerName
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
        public void information_was_committed()
        {
            _context.Verify(ctx => ctx.SaveChanges(), Times.Once);
        }
    }
}