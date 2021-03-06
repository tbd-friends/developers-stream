﻿using System;
using System.Linq;
using System.Threading;
using application.Commands;
using application.Commands.Administration;
using application.Query.Administration.Handlers;
using core;
using core.Models;
using MediatR;
using Moq;
using Xunit;

namespace application.tests.ConcerningRemovingUser
{
    public class when_removing_a_user_where_they_are_a_streamer
    {
        private Mock<IApplicationContext> Context;
        private Mock<IMediator> Mediator;

        private RemoveUserHandler Subject;

        private const string EmailToRemove = "EmailToRemove";
        private readonly Guid RegisteredStreamerToRemoveId = Guid.Parse("8777C2D9-D3E5-46C6-A4AD-3616EBED658C");
        private readonly Guid StreamerToRemoveId = Guid.Parse("08360B7A-CE2D-4AB4-B221-AF76B492E3F4");

        public when_removing_a_user_where_they_are_a_streamer()
        {
            Arrange();

            Act();
        }

        private void Arrange()
        {
            Mediator = new Mock<IMediator>();
            Context = new Mock<IApplicationContext>();

            Context.Setup(ctx => ctx.RegisteredStreamers).Returns(new[]
            {
                new RegisteredStreamer
                {
                    Id = RegisteredStreamerToRemoveId, Email = EmailToRemove, StreamerId = StreamerToRemoveId, ProfileId = "Profile"
                }
            }.AsQueryable());

            Context.Setup(ctx => ctx.Streamers).Returns(new[]
            {
                new Streamer {Id = StreamerToRemoveId, IsStreamer = true}
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
        public void remove_registered_streamer_is_called()
        {
            Mediator.Verify(m =>
                    m.Send(It.Is<RemoveRegisteredStreamer>(ds => ds.Id == RegisteredStreamerToRemoveId),
                        CancellationToken.None),
                Times.Once);
        }

        [Fact]
        public void delete_streamer_is_called()
        {
            Mediator.Verify(m =>
                    m.Send(It.Is<DeleteStreamer>(ds => ds.Id == StreamerToRemoveId),
                        CancellationToken.None),
                Times.Once);
        }
    }
}