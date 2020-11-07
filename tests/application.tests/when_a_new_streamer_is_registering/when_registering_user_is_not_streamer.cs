using System.Threading;
using application.Commands;
using application.Commands.Handlers;
using core;
using core.Models;
using Moq;
using Xunit;

namespace application.tests.when_a_new_streamer_is_registering
{
    public class when_registering_user_is_not_streamer
    {
        private RegisterNewStreamerHandler _subject;
        private Mock<IApplicationContext> _context;

        public const string StreamerName = "streamer-name";
        public const string Description = "description";
        public const string Email = "email-address";
        public const bool IsStreamer = false;

        public when_registering_user_is_not_streamer()
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
                Description = Description,
                Email = Email,
                IsStreamer = IsStreamer
            }, CancellationToken.None);
        }
        [Fact]
        public void is_streamer_is_false()
        {
            _context.Verify(
                ctx => ctx.Insert(
                    It.Is<Streamer>(s => s.Name == StreamerName && s.IsStreamer == false)), Times.Once);
        }
    }
}