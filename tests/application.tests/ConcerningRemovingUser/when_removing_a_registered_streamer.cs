using System;
using System.Linq;
using System.Threading;
using application.Commands;
using application.Commands.Handlers;
using core;
using core.Models;
using Moq;
using Xunit;

namespace application.tests.ConcerningRemovingUser
{
    public class when_removing_a_registered_streamer
    {
        private Mock<IApplicationContext> Context;
        private RemoveRegisteredStreamerHandler Subject;

        private const string EmailToRemove = "email-to-remove";
        private const string ProfileToRemove = "profile-to-remove";

        private readonly Guid RegisteredStreamerId = Guid.Parse("2AD3DB23-43C4-46D6-A68E-DBEA256E7FA3");
        private readonly Guid StreamerId = Guid.Parse("C1711E03-6BB7-4E74-983B-AB94CB813889");

        public when_removing_a_registered_streamer()
        {
            Arrange();

            Act();
        }

        private void Arrange()
        {
            Context = new Mock<IApplicationContext>();

            Context.Setup(ctx => ctx.RegisteredStreamers).Returns(new[]
            {
                new RegisteredStreamer
                {
                    Id = RegisteredStreamerId,
                    Email = EmailToRemove,
                    ProfileId = ProfileToRemove,
                    StreamerId = StreamerId
                }
            }.AsQueryable());

            Subject = new RemoveRegisteredStreamerHandler(Context.Object);
        }

        private void Act()
        {
            Subject.Handle(new RemoveRegisteredStreamer
            {
                Id = RegisteredStreamerId
            }, CancellationToken.None)
                .GetAwaiter()
                .GetResult();
        }

        [Fact]
        public void registered_streamer_is_removed()
        {
            Context.Verify(ctx =>
                ctx.Delete(
                    It.Is<RegisteredStreamer>(
                        rs => rs.Id == RegisteredStreamerId)), Times.Once);
        }

        [Fact]
        public void changes_are_committed()
        {
            Context.Verify(ctx => ctx.SaveChangesAsync(CancellationToken.None), Times.Once);
        }
    }
}