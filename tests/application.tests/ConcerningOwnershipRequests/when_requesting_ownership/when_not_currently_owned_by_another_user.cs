using System;
using System.Linq;
using System.Threading;
using application.Commands;
using application.Commands.Handlers;
using core;
using core.Enums;
using core.Migrations;
using core.Models;
using FluentAssertions;
using Moq;
using Xunit;

namespace application.tests.when_a_user_makes_an_ownership_claim
{
    public class when_not_currently_owned_by_another_user
    {
        private Mock<IApplicationContext> Context;
        private RequestOwnershipHandler Subject;
        private readonly Guid ClaimedStreamerId = Guid.Parse("62206193-1466-424C-B9CF-23682E760469");
        private string RequestingUserEmail = "requesting-user-email";
        private string ProfileId = "profile-id";
        private string Details = "details-of-claim";

        private StreamerOwnershipRequest Output;

        public when_not_currently_owned_by_another_user()
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

            Context.Setup(ctx =>
                ctx.Insert(It.IsAny<StreamerOwnershipRequest>())).Callback(
                (StreamerOwnershipRequest request) =>
                {
                    Output = request;
                });
        }

        private void Act()
        {
            Subject.Handle(new RequestOwnership
            {
                ClaimedStreamerId = ClaimedStreamerId,
                Email = RequestingUserEmail,
                ProfileId = ProfileId,
                Details = Details
            }, CancellationToken.None).GetAwaiter().GetResult();
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
        public void ownership_request_current_registered_email_is_null()
        {
            Output.CurrentEmail.Should().BeNull();
        }

        [Fact]
        public void request_contains_current_user_email()
        {
            Output.UpdatedEmail.Should().Be(RequestingUserEmail);
        }

        [Fact]
        public void request_contains_details_provided()
        {
            Output.Details.Should().Be(Details);
        }

        [Fact]
        public void changes_were_committed()
        {
            Context.Verify(ctx => ctx.SaveChanges(), Times.Once);
        }
    }
}