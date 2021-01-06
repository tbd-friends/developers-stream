using System;
using System.Linq;
using System.Threading;
using application.Commands;
using application.Commands.Handlers;
using core;
using core.Models;
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
        public void claim_is_made()
        {
            Context.Verify(ctx =>
                ctx.Insert(
                    It.Is<StreamerClaimRequest>(
                        r =>
                            r.CurrentEmail == CurrentEmail &&
                            r.UpdatedEmail == UserEmail &&
                            r.IsApproved == false)), Times.Once);
        }

        [Fact]
        public void save_changes_was_called()
        {
            Context.Verify(ctx => ctx.SaveChanges(), Times.Once);
        }
    }
}