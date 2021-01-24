using System;
using System.Linq;
using System.Threading;
using application.Commands;
using application.Commands.Handlers;
using core;
using core.Enums;
using core.Models;
using FluentAssertions;
using Moq;
using Xunit;

namespace application.tests.when_a_user_makes_an_ownership_claim
{
    public class when_currently_owned_by_another_user
    {
        private Mock<IApplicationContext> Context;
        private RequestOwnershipHandler Subject;
        private Guid ClaimedStreamerId = Guid.Parse("F889018E-5D64-48E3-AE82-24A9FA3AF5FE");
        private string CurrentEmail = "current-email";
        private string CurrentProfileId = "current-profile-id";
        private string UserEmail = "logged-in-user-email";
        private string ProfileId = "logged-in-profile-id";
        private StreamerOwnershipRequest Output;

        public when_currently_owned_by_another_user()
        {
            Arrange();

            Act();
        }

        private void Arrange()
        {
            Context = new Mock<IApplicationContext>();

            Subject = new RequestOwnershipHandler(Context.Object);

            Context.Setup(ctx => ctx.Streamers)
                .Returns(new[]
                {
                    new Streamer()
                    {
                        Id = ClaimedStreamerId
                    }
                }.AsQueryable());

            Context.Setup(ctx => ctx.RegisteredStreamers)
                .Returns(new[]
                {
                    new RegisteredStreamer
                    {
                        StreamerId = ClaimedStreamerId,
                        Email = CurrentEmail,
                        ProfileId = CurrentProfileId
                    }
                }.AsQueryable());

            Context.Setup(ctx =>
                ctx.Insert(It.IsAny<StreamerOwnershipRequest>())).Callback(
                (StreamerOwnershipRequest request) =>
                {
                    Output = request;
                });
        }

        private void Act()
        {
            Subject.Handle(
                    new RequestOwnership
                    {
                        ClaimedStreamerId = ClaimedStreamerId,
                        Email = UserEmail,
                        ProfileId = ProfileId
                    }, CancellationToken.None)
                .GetAwaiter().GetResult();
        }

        [Fact]
        public void ownership_request_is_made_for_streamer_id()
        {
            Output.ClaimedStreamerId.Should().Be(ClaimedStreamerId);
        }

        [Fact]
        public void ownership_request_is_pending_approval()
        {
            Output.Status.Should().Be(OwnershipRequestStatus.PendingApproval);
        }

        [Fact]
        public void ownership_request_contains_current_registered_email()
        {
            Output.CurrentEmail.Should().Be(CurrentEmail);
        }

        [Fact]
        public void request_contains_current_user_email()
        {
            Output.UpdatedEmail.Should().Be(UserEmail);
        }

        [Fact]
        public void ownership_request_contains_logged_in_user_profile_id()
        {
            Output.ProfileId.Should().Be(ProfileId);
        }

        [Fact]
        public void changes_were_committed()
        {
            Context.Verify(ctx => ctx.SaveChanges(), Times.Once);
        }
    }
}