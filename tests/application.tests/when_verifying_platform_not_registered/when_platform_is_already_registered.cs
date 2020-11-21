using System;
using System.Linq;
using System.Threading;
using application.Query;
using application.Query.Handlers;
using core;
using core.Models;
using FluentAssertions;
using Moq;
using Xunit;

namespace application.tests.when_verifying_platform_not_registered
{
    public class when_platform_is_already_registered
    {
        private IsPlatformRegisteredHandler _subject;
        private bool _result;
        private Mock<IApplicationContext> _context;

        public when_platform_is_already_registered()
        {
            Arrange();

            Act();
        }

        private void Arrange()
        {
            _context = new Mock<IApplicationContext>();
            _context.Setup(ctx => ctx.StreamerPlatforms).Returns(new[]
            {
                new StreamerPlatform {Id = Guid.NewGuid(), Name = "platform", Url = "UrlAlreadyExists"}
            }.AsQueryable());

            _subject = new IsPlatformRegisteredHandler(_context.Object);
        }

        private void Act()
        {
            _result = _subject.Handle(new IsPlatformRegistered
            {
                Url = "UrlAlreadyExists"
            }, CancellationToken.None).GetAwaiter().GetResult();
        }

        [Fact]
        public void result_is_true()
        {
            _result.Should().BeTrue();
        }
    }
}