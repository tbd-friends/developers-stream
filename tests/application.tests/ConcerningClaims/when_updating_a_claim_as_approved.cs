using System;
using System.Linq;
using System.Threading;
using application.Commands.Administration;
using application.Commands.Administration.Handlers;
using core;
using core.Models;
using FluentAssertions;
using Moq;
using Xunit;

namespace application.tests.ConcerningClaims
{
    public class when_updating_a_claim_as_approved
    {
        private Mock<IApplicationContext> Context;
        private UpdateClaimRequestAsApprovedHandler Subject;

        private StreamerClaimRequest ClaimRequest;
        private readonly Guid ClaimRequestId = new Guid("58885A43-6CC4-4A41-ABB6-B25253862F40");

        public when_updating_a_claim_as_approved()
        {
            Arrange();

            Act();
        }

        private void Arrange()
        {
            ClaimRequest = new StreamerClaimRequest
            {
                Id = ClaimRequestId,
                IsApproved = false,
                Updated = null
            };

            Context = new Mock<IApplicationContext>();

            Context.Setup(ctx => ctx.StreamerClaimRequests).Returns(new[]
            {
                ClaimRequest
            }.AsQueryable());

            Subject = new UpdateClaimRequestAsApprovedHandler(Context.Object);
        }

        private void Act()
        {
            Subject.Handle(new UpdateClaimRequestAsApproved
            {
                ClaimRequestId = ClaimRequestId
            }, CancellationToken.None).GetAwaiter().GetResult();
        }

        [Fact]
        public void claim_is_marked_as_approved()
        {
            ClaimRequest.IsApproved.Should().BeTrue();
            ClaimRequest.Updated.Should().NotBeNull("Updated should be set when changing the record");
            ClaimRequest.Updated.Should().BeBefore(DateTime.UtcNow);
        }

        [Fact]
        public void save_changes_was_called()
        {
            Context.Verify(ctx => ctx.SaveChanges(), Times.Once);
        }
    }
}