using System;
using System.Linq;
using System.Threading;
using application.Commands.Administration;
using application.Commands.Administration.Handlers;
using core;
using core.Enums;
using core.Models;
using FluentAssertions;
using Moq;
using Xunit;

namespace application.tests.when_administering_streamers
{
    public class when_streamer_is_verified
    {
        private readonly Guid _streamerId = Guid.NewGuid();
        private Streamer _streamerFromDb;

        private Mock<IApplicationContext> _context;
        private VerifyStreamerHandler _subject;

        public when_streamer_is_verified()
        {
            Arrange();

            Act();
        }

        private void Arrange()
        {
            _streamerFromDb = new Streamer
            {
                Id = _streamerId,
                Name = "streamer-name",
                Status = StreamerStatus.PendingVerification
            };

            _context = new Mock<IApplicationContext>();
            _context.Setup(ctx => ctx.Streamers).Returns(
                new[] { _streamerFromDb }.AsQueryable());

            _subject = new VerifyStreamerHandler(_context.Object);
        }

        private void Act()
        {
            _subject.Handle(new VerifyStreamer { Id = _streamerId }, CancellationToken.None).GetAwaiter().GetResult();
        }

        [Fact]
        public void streamer_status_is_updated_as_verified()
        {
            _streamerFromDb.Status.Should().Be(StreamerStatus.Verified);
        }

        [Fact]
        public void changes_are_committed()
        {
            _context.Verify(ctx => ctx.SaveChanges(), Times.Once);
        }
    }
}