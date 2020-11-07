using System.Threading;
using application.Commands;
using application.Commands.Handlers;
using core;
using core.Models;
using Moq;
using Xunit;

namespace application.tests.when_a_new_streamer_is_registering
{
    public class when_all_required_details_are_entered
    {
        private RegisterNewStreamerHandler _subject;
        private Mock<IApplicationContext> _context;

        public const string StreamerName = "streamer-name";
        public const string Description = "description";
        public const string Email = "email-address";

        public when_all_required_details_are_entered()
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
                Email = Email
            }, CancellationToken.None);
        }

        [Fact]
        public void name_is_captured()
        {
            _context.Verify(
                ctx => ctx.Insert(
                    It.Is<Streamer>(s => s.Name == StreamerName)), Times.Once);
        }

        [Fact]
        public void description_is_captured()
        {
            _context.Verify(
                ctx => ctx.Insert(
                    It.Is<Streamer>(s => s.Description == Description)), Times.Once);
        }

        [Fact]
        public void email_is_captured()
        {
            _context.Verify(
                ctx => ctx.Insert(
                    It.Is<Streamer>(s => s.Email == Email)), Times.Once);
        }

        [Fact]
        public void information_was_committed()
        {
            _context.Verify(ctx => ctx.SaveChanges(), Times.Once);
        }
    }
}