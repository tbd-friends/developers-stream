using System;
using System.Linq;
using System.Threading;
using application.Commands.Administration;
using application.Commands.Administration.Handlers;
using core;
using core.Models;
using Moq;
using Xunit;

namespace application.tests.when_administering_streamers
{
    public class when_streamer_is_deleted
    {
        private Mock<IApplicationContext> _context;
        private DeleteStreamerHandler _subject;

        private readonly Guid _streamerId = Guid.NewGuid();

        public when_streamer_is_deleted()
        {
            Arrange();

            Act();
        }

        private void Arrange()
        {
            _context = new Mock<IApplicationContext>();
            _context.Setup(ctx => ctx.Streamers).Returns(
                new[] { new Streamer() { Id = _streamerId, Name = "streamer-name" } }.AsQueryable());

            _context.Setup(ctx => ctx.StreamerPlatforms).Returns(
                new[]
                    {
                        new StreamerPlatform {Id = Guid.NewGuid(), StreamerId = _streamerId, Name = "Platform1"},
                        new StreamerPlatform {Id = Guid.NewGuid(), StreamerId = _streamerId, Name = "Platform2"}
                    }
                    .AsQueryable());

            _context.Setup(ctx => ctx.StreamerTechnologies).Returns(
                new[]
                    {
                        new StreamerTechnology()
                            {Id = Guid.NewGuid(), StreamerId = _streamerId, TechnologyId = Guid.NewGuid()},
                        new StreamerTechnology
                            {Id = Guid.NewGuid(), StreamerId = _streamerId, TechnologyId = Guid.NewGuid()}
                    }
                    .AsQueryable());

            _subject = new DeleteStreamerHandler(_context.Object);
        }

        private void Act()
        {
            _subject.Handle(new DeleteStreamer() { Id = _streamerId }, CancellationToken.None).GetAwaiter().GetResult();
        }

        [Fact]
        public void streamer_is_removed()
        {
            _context.Verify(ctx => ctx.Delete(It.Is<Streamer>(s => s.Id == _streamerId)), Times.Once);
        }

        [Fact]
        public void streamer_platforms_are_removed()
        {
            _context.Verify(ctx => ctx.Delete(It.Is<StreamerPlatform>(sp => sp.StreamerId == _streamerId)),
                Times.Exactly(2));
        }

        [Fact]
        public void streamer_technologies_are_removed()
        {
            _context.Verify(ctx => ctx.Delete(It.Is<StreamerTechnology>(sp => sp.StreamerId == _streamerId)),
                Times.Exactly(2));
        }

        [Fact]
        public void changes_are_committed()
        {
            _context.Verify(ctx => ctx.SaveChanges(), Times.Once());
        }
    }
}