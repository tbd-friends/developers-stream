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
    public class when_updating_an_ownership_request_as_approved
    {
        private Mock<IApplicationContext> Context;
        private UpdateOwnershipRequestAsApprovedHandler Subject;

        private StreamerOwnershipRequest _ownershipRequest;
        private readonly Guid ClaimRequestId = new Guid("58885A43-6CC4-4A41-ABB6-B25253862F40");

        public when_updating_an_ownership_request_as_approved()
        {
            Arrange();

            Act();
        }

        private void Arrange()
        {
            _ownershipRequest = new StreamerOwnershipRequest
            {
                Id = ClaimRequestId,
                Status = OwnershipRequestStatus.PendingApproval,
                Updated = null
            };

            Context = new Mock<IApplicationContext>();

            Context.Setup(ctx => ctx.StreamerClaimRequests).Returns(new[]
            {
                _ownershipRequest
            }.AsQueryable());

            Subject = new UpdateOwnershipRequestAsApprovedHandler(Context.Object);
        }

        private void Act()
        {
            Subject.Handle(new UpdateOwnershipRequestAsApproved
            {
                ClaimRequestId = ClaimRequestId
            }, CancellationToken.None).GetAwaiter().GetResult();
        }

        [Fact]
        public void claim_is_marked_as_approved()
        {
            _ownershipRequest.Status.Should().Be(OwnershipRequestStatus.Approved);
            _ownershipRequest.Updated.Should().NotBeNull("Updated should be set when changing the record");
            _ownershipRequest.Updated.Should().BeBefore(DateTime.UtcNow);
        }

        [Fact]
        public void save_changes_was_called()
        {
            Context.Verify(ctx => ctx.SaveChanges(), Times.Once);
        }
    }
}