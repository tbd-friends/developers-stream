using System;
using System.Linq;
using System.Threading;
using application.Commands.Administration;
using application.Query.Administration.Handlers;
using core;
using core.Models;
using MediatR;
using Moq;
using Xunit;

namespace application.tests.ConcerningRemovingUser
{
    public class when_removing_a_user
    {
        private Mock<IApplicationContext> Context;
        private Mock<IMediator> Mediator;

        private RemoveUserHandler Subject;

        private const string EmailToRemove = "EmailToRemove";
        private readonly Guid StreamerToRemoveId = Guid.Parse("08360B7A-CE2D-4AB4-B221-AF76B492E3F4");
        private readonly Guid StreamerToKeepId = Guid.Parse("0F927417-6B90-497A-8E97-5823CD3D4A67");

        public when_removing_a_user()
        {
            Arrange();

            Act();
        }

        private void Arrange()
        {
            Mediator = new Mock<IMediator>();
            Context = new Mock<IApplicationContext>();

            Context.Setup(ctx => ctx.Streamers).Returns(new[]
            {
                new Streamer {Id = StreamerToRemoveId, Email = EmailToRemove},
                new Streamer {Id = StreamerToKeepId, Email = "EmailToKeep"}
            }.AsQueryable());

            Subject = new RemoveUserHandler(Context.Object, Mediator.Object);
        }

        private void Act()
        {
            Subject.Handle(new RemoveUser
            {
                Email = EmailToRemove
            }, CancellationToken.None).GetAwaiter().GetResult();
        }

        [Fact]
        public void delete_streamer_is_called()
        {
            Mediator.Verify(m =>
                    m.Send(It.Is<DeleteStreamer>(ds => ds.Id == StreamerToRemoveId),
                        CancellationToken.None),
                Times.Once);
        }

        [Fact]
        public void delete_is_not_called_for_incorrect_streamer()
        {
            Mediator.Verify(m =>
                    m.Send(It.Is<DeleteStreamer>(ds => ds.Id == StreamerToKeepId),
                        CancellationToken.None),
                Times.Never);
        }
    }
}