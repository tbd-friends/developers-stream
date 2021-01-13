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
    public class when_email_is_not_configured
    {
        private Mock<IApplicationContext> Context;
        private MakeClaimRequestHandler Subject;
        private Guid ClaimedStreamerId = Guid.Parse("EFF33F3E-85CF-4544-B9F1-88710FAA5F12");

        public when_email_is_not_configured()
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
                        Email = "CurrentAttachedEmail"
                    }
                }.AsQueryable());
        }

        private void Act()
        {
            Subject.Handle(new MakeClaimRequest
            {
                ClaimedStreamerId = ClaimedStreamerId,
                Email = null
            }, CancellationToken.None).GetAwaiter().GetResult();
        }

        [Fact]
        public void claim_is_not_made()
        {
            Context.Verify(ctx => ctx.Insert(It.IsAny<StreamerClaimRequest>()), Times.Never);
        }
    }
}