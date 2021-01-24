using System;
using System.Collections.Generic;
using System.Threading;
using application.Commands;
using application.Commands.Handlers;
using core;
using core.Enums;
using core.Models;
using MediatR;
using Moq;
using Xunit;

namespace application.tests.when_a_new_streamer_is_registering
{
    public class when_registrar_is_the_streamer
    {
        private RegisterNewStreamerHandler _subject;
        private Mock<IApplicationContext> Context;
        private Mock<IMediator> Mediator;

        public const string StreamerName = "streamer-name";
        public const string Description = "description";
        public const string Email = "email-address";
        public const string ProfileId = "profile-id";
        public const bool IsStreamer = true;

        public readonly Guid StreamerId = Guid.Parse("DCD2355D-3323-4967-8031-30AEBD17A9F5");

        public when_registrar_is_the_streamer()
        {
            Arrange();

            Act();
        }

        private void Arrange()
        {
            Mediator = new Mock<IMediator>();
            Context = new Mock<IApplicationContext>(); 

            Context.Setup(ctx =>
                ctx.Insert(It.IsAny<Streamer>())).Callback((Streamer streamer) =>
            {
                streamer.Id = StreamerId;
            }); 

            _subject = new RegisterNewStreamerHandler(Mediator.Object, Context.Object);
        }

        private void Act()
        {
            _subject.Handle(new RegisterNewStreamer
            {
                Name = StreamerName,
                Description = Description,
                Email = Email,
                IsStreamer = IsStreamer,
                ProfileId = ProfileId,
                Platforms = new List<RegisterNewStreamer.Platform>()
            }, CancellationToken.None).GetAwaiter().GetResult();
        }

        [Fact]
        public void associate_streamer_with_registrar_is_called()
        {
            Mediator.Verify(m =>
                    m.Send(It.Is<AssociateStreamerWithRegistrar>(c =>
                            c.StreamerId == StreamerId &&
                            c.Email == Email &&
                            c.ProfileId == ProfileId),
                        CancellationToken.None),
                Times.Once);
        }

        [Fact]
        public void is_streamer_is_true()
        {
            Context.Verify(
                ctx => ctx.Insert(
                    It.Is<Streamer>(s => s.Name == StreamerName && s.IsStreamer)), Times.Once);
        }

        [Fact]
        public void streamer_is_added_in_unverified_state()
        {
            Context.Verify(ctx =>
                ctx.Insert(It.Is<Streamer>(s =>
                    s.Name == StreamerName && s.Status == StreamerStatus.PendingVerification)));
        }

        [Fact]
        public void changes_are_committed()
        {
            Context.Verify(ctx => ctx.SaveChangesAsync(CancellationToken.None));
        }
    }
}