using System;
using System.Threading;
using application.Commands;
using application.Commands.Handlers;
using core;
using core.Models;
using Moq;
using Xunit;

namespace application.tests.when_a_new_streamer_is_registering
{
    public class when_technologies_are_provided
    {
        private readonly Guid TechnologyToBeAdded = new Guid("15F18A34-D08F-4FA9-AEE4-BCF89C3B9426");

        private RegisterNewStreamerHandler _subject;
        private Mock<IApplicationContext> _context;

        public readonly string StreamerName = "streamer-name";

        public when_technologies_are_provided()
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
                Technologies = new[]
                {
                    TechnologyToBeAdded,
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
        public void streamer_technology_is_registered()
        {
            _context.Verify(ctx =>
                    ctx.Insert(It.Is<StreamerTechnology>(st => st.TechnologyId == TechnologyToBeAdded)),
                Times.Once);
        }

        [Fact]
        public void information_was_committed()
        {
            _context.Verify(ctx => ctx.SaveChanges(), Times.Once);
        }
    }
}