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
    public class when_streamer_is_rejected
    {
        private Mock<IApplicationContext> _context;
        private RejectStreamerHandler _subject;
        private Streamer _streamerFromDb;
        private readonly Guid _streamerId = Guid.NewGuid();

        public when_streamer_is_rejected()
        {
            Arrange();

            Act();
        }

        private void Arrange()
        {
            _context = new Mock<IApplicationContext>();

            _streamerFromDb = new Streamer
            {
                Id = _streamerId,
                Name = "StreamerName",
                Status = StreamerStatus.PendingVerification
            };

            _context.Setup(ctx => ctx.Streamers).Returns(new[] { _streamerFromDb }.AsQueryable());

            _subject = new RejectStreamerHandler(_context.Object);
        }

        private void Act()
        {
            _subject.Handle(new RejectStreamer
            {
                Id = _streamerId
            }, CancellationToken.None).GetAwaiter().GetResult();
        }

        [Fact]
        public void streamer_status_is_updated_as_rejected()
        {
            _streamerFromDb.Status.Should().Be(StreamerStatus.Rejected);
        }

        [Fact]
        public void changes_are_committed()
        {
            _context.Verify(ctx => ctx.SaveChanges());
        }
    }
}