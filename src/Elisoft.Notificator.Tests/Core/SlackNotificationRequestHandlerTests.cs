using AutoFixture;
using Elisoft.Notificator.Core.Handlers;
using Elisoft.Notificator.Core.Requests;
using Elisoft.Slack;
using FakeItEasy;
using Microsoft.Extensions.Logging;
using NUnit.Framework;
using Shouldly;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Elisoft.Notificator.Tests.Core
{
    [TestFixture]
    public class SlackNotificationRequestHandlerTests
    {
        private Fixture _fixture;
        private ISlackNotificator _slackNotificatorFake;
        private ILogger<SlackNotificationRequestHandler> _loggerFake;
        private SlackNotificationRequestHandler _sut;

        [SetUp]
        public void Setup()
        {
            _fixture = new Fixture();
            _slackNotificatorFake = A.Fake<ISlackNotificator>();
            _loggerFake = A.Fake<ILogger<SlackNotificationRequestHandler>>();

            _sut = new SlackNotificationRequestHandler(
                _slackNotificatorFake,
                _loggerFake
            );
        }

        [Test]
        public async Task HandleAsync_ValidCommand_CallSendMessageAsyncWithCorrectArgs()
        {
            // Arrange
            var command = _fixture.Create<SlackNotificationRequest>();


            // Act
            var result = await _sut.HandleAsync(command, CancellationToken.None);


            // Assert
            A.CallTo(() => _slackNotificatorFake.SendMessageAsync(
                    command.WebhookUrl,
                    command.ChannelName,
                    command.Message))
             .MustHaveHappenedOnceExactly();

            result.ShouldBe(command);
        }

        [Test]
        public async Task HandleAsync_NotificatorThrowsException_ThrowException()
        {
            // Arrange
            var command = _fixture.Create<SlackNotificationRequest>();
            var expectedException = new HttpRequestException("Slack API unavailable");

            A.CallTo(() => _slackNotificatorFake.SendMessageAsync(
                    A<string>._,
                    A<string>._,
                    A<string>._))
             .Throws(expectedException);


            // Act & Assert
            var exception = await Should.ThrowAsync<HttpRequestException>(async () =>
                await _sut.HandleAsync(command, CancellationToken.None));

            exception.Message.ShouldBe(expectedException.Message);
        }
    }
}