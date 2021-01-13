using System;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using application.Commands;
using application.Commands.Handlers;
using core;
using core.Enums;
using core.Models;
using FluentAssertions;
using Moq;
using Xunit;

namespace application.tests.when_a_user_claims_a_streamer
{
    public class when_email_is_configured
    {
        private Mock<IApplicationContext> Context;
        private MakeClaimRequestHandler Subject;
        private Guid ClaimedStreamerId = Guid.Parse("F889018E-5D64-48E3-AE82-24A9FA3AF5FE");
        private string CurrentEmail = "CurrentEmail";
        private string UserEmail = "LoggedInUserEmail";
        private StreamerClaimRequest Output;

        public when_email_is_configured()
        {
            Arrange();

            Act();
        }

        private void Arrange()
        {
            Context = new Mock<IApplicationContext>();

            Subject = new MakeClaimRequestHandler(Context.Object);

            Context.Setup(ctx => ctx.Streamers)
                .Returns(new[]
                {
                    new Streamer()
                    {
                        Id = ClaimedStreamerId,
                        Email = CurrentEmail
                    }
                }.AsQueryable());

            Context.Setup(ctx => ctx.Insert(It.IsAny<StreamerClaimRequest>())).Callback(
                (StreamerClaimRequest request) =>
                {
                    Output = request;
                });
        }

        private void Act()
        {
            Subject.Handle(
                    new MakeClaimRequest
                    {
                        ClaimedStreamerId = ClaimedStreamerId,
                        Email = UserEmail
                    }, CancellationToken.None)
                .GetAwaiter().GetResult();
        }

        [Fact]
        public void claim_is_made_for_claimed_streamer_id()
        {
            Output.ClaimedStreamerId.Should().Be(ClaimedStreamerId);
        }

        [Fact]
        public void claim_is_created_in_pending_approval_status()
        {
            Output.Status.Should().Be(ClaimRequestStatus.PendingApproval);
        }

        [Fact]
        public void claim_current_email_is_current_streamer_registered_email()
        {
            Output.CurrentEmail.Should().Be(CurrentEmail);
        }

        [Fact]
        public void claim_updated_email_is_requesting_users_email()
        {
            Output.UpdatedEmail.Should().Be(UserEmail);
        }

        [Fact]
        public void save_changes_was_called()
        {
            Context.Verify(ctx => ctx.SaveChanges(), Times.Once);
        }
    }
}