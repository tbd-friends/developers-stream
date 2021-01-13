using System;
using System.Linq;
using System.Threading;
using application.Commands.Administration;
using application.Commands.Administration.Handlers;
using core;
using core.Models;
using FluentAssertions;
using Moq;
using Xunit;

namespace application.tests.ConcerningClaims
{
    public class when_updating_a_streamer_email
    {
        private UpdateStreamerEmailHandler Subject;
        private Mock<IApplicationContext> Context;

        private readonly Guid StreamerId = Guid.Parse("2502787F-897B-4C15-BD45-FA4D8291B003");
        private const string OriginalEmail = "OriginalEmail";
        private const string UpdatedEmail = "UpdatedEmail";

        private Streamer CurrentRecord;

        public when_updating_a_streamer_email()
        {
            Arrange();

            Act();
        }

        private void Arrange()
        {
            CurrentRecord = new Streamer
            {
                Id = StreamerId,
                Email = OriginalEmail
            };

            Context = new Mock<IApplicationContext>();

            Context.Setup(ctx => ctx.Streamers).Returns(new[]
            {
                CurrentRecord
            }.AsQueryable());

            Subject = new UpdateStreamerEmailHandler(Context.Object);
        }

        private void Act()
        {
            Subject.Handle(new UpdateStreamerEmail
            {
                StreamerId = StreamerId,
                UpdatedEmail = UpdatedEmail
            }, CancellationToken.None).GetAwaiter().GetResult();
        }

        [Fact]
        public void email_is_updated()
        {
            CurrentRecord.Email.Should().Be(UpdatedEmail);
        }

        [Fact]
        public void save_changes_is_called()
        {
            Context.Verify(ctx => ctx.SaveChanges(), Times.Once);
        }
    }
}