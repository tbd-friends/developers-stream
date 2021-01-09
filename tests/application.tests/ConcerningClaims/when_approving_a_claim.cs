using System;
using System.Linq;
using System.Threading;
using application.Commands.Administration;
using application.Commands.Administration.Handlers;
using core;
using core.Models;
using MediatR;
using Moq;
using Xunit;

namespace application.tests.ConcerningClaims
{
    public class when_approving_a_claim
    {
        private Mock<IApplicationContext> Context;
        private Mock<IMediator> Mediator;
        private ApproveClaimRequestHandler Subject;
        private string UpdatedEmail = "UpdatedEmail";

        private readonly Guid ClaimRequestId = new Guid("D86FF409-6B68-420C-812A-8A2E60B63762");
        private readonly Guid StreamerId = new Guid("864C9763-61D3-4951-A608-757C5E9E0B03");
        public when_approving_a_claim()
        {
            Arrange();

            Act();
        }

        private void Arrange()
        {
            Context = new Mock<IApplicationContext>();
            Context.Setup(ctx => ctx.StreamerClaimRequests).Returns(new[]
            {
                new StreamerClaimRequest
                {
                    Id = ClaimRequestId,
                    ClaimedStreamerId = StreamerId,
                    UpdatedEmail = UpdatedEmail
                }
            }.AsQueryable());

            Mediator = new Mock<IMediator>();

            Subject = new ApproveClaimRequestHandler(Context.Object, Mediator.Object);
        }

        private void Act()
        {
            Subject.Handle(new ApproveClaimRequest
            {
                ClaimRequestId = ClaimRequestId
            }, CancellationToken.None).GetAwaiter().GetResult();
        }

        [Fact]
        public void request_to_update_email_is_performed()
        {
            Mediator.Verify(m =>
                    m.Send(
                        It.Is<UpdateStreamerEmail>(upd =>
                            upd.StreamerId == StreamerId && upd.UpdatedEmail == UpdatedEmail),
                        It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public void request_to_update_claim_as_approved_is_performed()
        {
            Mediator.Verify(m =>
                    m.Send(
                        It.Is<UpdateClaimRequestAsApproved>(acr => acr.ClaimRequestId == ClaimRequestId),
                        It.IsAny<CancellationToken>()),
                Times.Once);
        }
    }
}