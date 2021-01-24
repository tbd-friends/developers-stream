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
    public class when_not_currently_owned_by_another_user
    {
        private Mock<IApplicationContext> Context;
        private RequestOwnershipHandler Subject;
        private readonly Guid ClaimedStreamerId = Guid.Parse("EFF33F3E-85CF-4544-B9F1-88710FAA5F12");
        private string RequestingUserEmail = "requesting-user-email";

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
                Email = RequestingUserEmail
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
        public void ownership_request_contains_current_registered_email()
        {
            Output.CurrentEmail.Should().BeNull();
        }

        [Fact]
        public void request_contains_current_user_email()
        {
            Output.UpdatedEmail.Should().Be(RequestingUserEmail);
        }

        [Fact]
        public void changes_were_committed()
        {
            Context.Verify(ctx => ctx.SaveChanges(), Times.Once);
        }
    }
}