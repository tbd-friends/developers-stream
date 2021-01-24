using System;
using System.IO;
using System.Linq;
using System.Threading;
using application.Commands;
using application.Commands.Administration;
using application.Commands.Administration.Handlers;
using core;
using core.Models;
using MediatR;
using Moq;
using Xunit;

namespace application.tests.ConcerningClaims
{
    public class when_approving_an_ownership_request_and_no_ownership_exists
    {
        private Mock<IApplicationContext> Context;
        private Mock<IMediator> Mediator;
        private ApproveOwnershipRequestHandler Subject;

        private const string UpdatedEmail = "updated-email";
        private const string ProfileId = "profile-id";

        private readonly Guid ClaimRequestId = new Guid("D86FF409-6B68-420C-812A-8A2E60B63762");
        private readonly Guid StreamerId = new Guid("864C9763-61D3-4951-A608-757C5E9E0B03");
        public when_approving_an_ownership_request_and_no_ownership_exists()
        {
            Arrange();

            Act();
        }

        private void Arrange()
        {
            Context = new Mock<IApplicationContext>();
            Context.Setup(ctx => ctx.StreamerClaimRequests).Returns(new[]
            {
                new StreamerOwnershipRequest
                {
                    Id = ClaimRequestId,
                    ClaimedStreamerId = StreamerId,
                    UpdatedEmail = UpdatedEmail,
                    ProfileId = ProfileId
                }
            }.AsQueryable());

            Mediator = new Mock<IMediator>();

            Subject = new ApproveOwnershipRequestHandler(Context.Object, Mediator.Object);
        }

        private void Act()
        {
            Subject.Handle(new ApproveOwnershipRequest
            {
                ClaimRequestId = ClaimRequestId
            }, CancellationToken.None).GetAwaiter().GetResult();
        }

        [Fact]
        public void associate_streamer_with_registrar_is_called_for_claimant()
        {
            Mediator.Verify(m => m.Send(It.Is<AssociateStreamerWithRegistrar>(
                c =>
                    c.StreamerId == StreamerId &&
                    c.Email == UpdatedEmail &&
                    c.ProfileId == ProfileId)
                , CancellationToken.None)
            , Times.Once);
        }

        [Fact]
        public void request_to_update_claim_as_approved_is_performed()
        {
            Mediator.Verify(m =>
                    m.Send(
                        It.Is<UpdateOwnershipRequestAsApproved>(acr => acr.ClaimRequestId == ClaimRequestId),
                        It.IsAny<CancellationToken>()),
                Times.Once);
        }
    }
}