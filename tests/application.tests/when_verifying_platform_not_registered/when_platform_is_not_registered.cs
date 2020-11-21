using System;
using System.Threading;
using application.Commands;
using application.Commands.Handlers;
using application.Query;
using application.Query.Handlers;
using core;
using FluentAssertions;
using Moq;
using Xunit;

namespace application.tests.when_verifying_platform_not_registered
{
    public class when_platform_is_not_registered
    {
        private IsPlatformRegisteredHandler _subject;
        private bool _result;
        private IMock<IApplicationContext> _context;

        public when_platform_is_not_registered()
        {
            Arrange();

            Act();
        }

        private void Arrange()
        {
            _context = new Mock<IApplicationContext>();

            _subject = new IsPlatformRegisteredHandler(_context.Object);
        }

        private void Act()
        {
            _result = _subject.Handle(new IsPlatformRegistered()
            {
                Url = "UrlThatDoesNotExist"
            }, CancellationToken.None).GetAwaiter().GetResult();
        }

        [Fact]
        public void result_is_false()
        {
            _result.Should().BeFalse();
        }
    }
}