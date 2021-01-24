using System;
using System.Linq;
using System.Threading;
using application.Commands.Administration;
using application.Commands.Administration.Handlers;
using core;
using core.Enums;
using core.Models;
using FluentAssertions;
using Moq;
using Xunit;

namespace application.tests.ConcerningClaims
{
    public class when_rejecting_an_ownership_request
    {
        private Mock<IApplicationContext> Context;
        private RejectOwnershipRequestHandler Subject;

        private readonly Guid ClaimRequestId = new Guid("87F38115-775E-41F7-A1FC-3A5BDB1FF754");
        private readonly Guid ClaimedStreamerId = new Guid("E4FDB7C3-DEEF-4621-9E4D-5DB1EF27A4C2");

        private StreamerOwnershipRequest _ownershipRequest;

        public when_rejecting_an_ownership_request()
        {
            Arrange();

            Act();
        }

        private void Arrange()
        {
            _ownershipRequest = new StreamerOwnershipRequest
            {
                Id = ClaimRequestId,
                ClaimedStreamerId = ClaimedStreamerId,
                Status = OwnershipRequestStatus.PendingApproval
            };

            Context = new Mock<IApplicationContext>();

            Context.Setup(ctx => ctx.StreamerClaimRequests).Returns(new[]
            {
                _ownershipRequest
            }.AsQueryable());

            Subject = new RejectOwnershipRequestHandler(Context.Object);
        }

        private void Act()
        {
            Subject.Handle(new RejectOwnershipRequest
            {
                ClaimRequestId = ClaimRequestId
            }, CancellationToken.None).GetAwaiter().GetResult();
        }

        [Fact]
        public void request_claim_status_is_updated_as_rejected()
        {
            _ownershipRequest.Status.Should().Be(OwnershipRequestStatus.Rejected);
        }

        [Fact]
        public void request_claim_updated_date_is_current()
        {
            _ownershipRequest.Updated.Should().BeBefore(DateTime.UtcNow);
        }

        [Fact]
        public void save_changes_was_called()
        {
            Context.Verify(ctx => ctx.SaveChanges(), Times.Once);
        }
    }
}