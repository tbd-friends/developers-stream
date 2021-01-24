using System;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using application.Commands;
using application.Commands.Handlers;
using core;
using core.Models;
using Moq;
using Xunit;

namespace application.tests.when_a_new_streamer_is_registering
{
    public class when_associating_streamer_with_registrar
    {
        public AssociateStreamerWithRegistrarHandler Subject;
        public Mock<IApplicationContext> Context;

        public const string Email = "email";
        public const string ProfileId = "profile-id";
        public readonly Guid StreamerId = new Guid("50BF8A72-0296-4774-B585-08AB14FD1777");

        public when_associating_streamer_with_registrar()
        {
            Arrange();

            Act();
        }

        private void Arrange()
        {
            Context = new Mock<IApplicationContext>();

            Subject = new AssociateStreamerWithRegistrarHandler(Context.Object);
        }

        private void Act()
        {
            Subject.Handle(new AssociateStreamerWithRegistrar
            {
                Email = Email,
                StreamerId = StreamerId,
                ProfileId = ProfileId
            }, CancellationToken.None).GetAwaiter().GetResult();
        }

        [Fact]
        public void streamer_is_associated()
        {
            Context.Verify(
                ctx => ctx.Insert(It.Is<RegisteredStreamer>(s =>
                    s.Email == Email && s.ProfileId == ProfileId && s.StreamerId == StreamerId)), Times.Once);
        }

        [Fact]
        public void changes_are_committed()
        {
            Context.Verify(ctx => ctx.SaveChangesAsync(CancellationToken.None), Times.Once);
        }
    }
}