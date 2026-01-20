using System.Text.Json;
using System.Threading.Tasks;
using AutoFixture;
using FakeItEasy;
using NUnit.Framework;
using Paramore.Brighter;
using Shouldly;
using Elisoft.Notificator.Core.Enums;
using Elisoft.Notificator.Core.Factories;
using Elisoft.Notificator.Core.Models;
using Elisoft.Notificator.Core.Services;
using Microsoft.Extensions.Logging;

namespace Elisoft.Notificator.Tests.Core
{
    public class NotificationServiceTests
    {
        private Fixture _fixture;
        private IAmACommandProcessor _commandProcessor;
        private IRequestFactory _requestFactory;
        private ILogger<NotificationService> _logger;
        private NotificationService _sut;

        [SetUp]
        public void SetUp()
        {
            _fixture = new Fixture();
            _commandProcessor = A.Fake<IAmACommandProcessor>();
            _requestFactory = A.Fake<IRequestFactory>();
            _logger = A.Fake<ILogger<NotificationService>>();

            _sut = new NotificationService(
                _commandProcessor,
                _requestFactory,
                _logger);
        }

        [Test]
        public async Task DispatchNotificationAsync_ValidNotification_CommandIsSent()
        {
            var notification = CreateNotification();
            var command = new FakeCommand();

            A.CallTo(() => _requestFactory.CreateRequest(notification.Channel, notification.Payload))
                .Returns(command);

            await _sut.DispatchNotificationAsync(notification);

            A.CallTo(() => _commandProcessor.SendAsync<Command>(
                        command,
                        A<RequestContext>._,
                        A<bool>._,
                        A<CancellationToken>._))
                    .MustHaveHappenedOnceExactly();
        }

        [Test]
        public async Task DispatchNotificationAsync_ValidNotification_RequestFactoryIsCalled()
        {
            var notification = CreateNotification();
            var command = new FakeCommand();

            A.CallTo(() => _requestFactory.CreateRequest(A<NotificationEnumChannel>._, A<JsonElement>._))
                .Returns(command);

            await _sut.DispatchNotificationAsync(notification);

            A.CallTo(() => _requestFactory.CreateRequest(notification.Channel, notification.Payload))
                .MustHaveHappenedOnceExactly();
        }

        [Test]
        public async Task DispatchNotificationAsync_ValidNotification_CorrectChannelPassedToFactory()
        {
            var notification = CreateNotification();
            var command = new FakeCommand();

            A.CallTo(() => _requestFactory.CreateRequest(A<NotificationEnumChannel>._, A<JsonElement>._))
                .Returns(command);

            await _sut.DispatchNotificationAsync(notification);

            A.CallTo(() => _requestFactory.CreateRequest(
                    notification.Channel,
                    A<JsonElement>._))
                .MustHaveHappenedOnceExactly();
        }

        [Test]
        public async Task DispatchNotificationAsync_ValidNotification_CorrectPayloadPassedToFactory()
        {
            var notification = CreateNotification();
            var command = new FakeCommand();

            A.CallTo(() => _requestFactory.CreateRequest(A<NotificationEnumChannel>._, A<JsonElement>._))
                .Returns(command);

            await _sut.DispatchNotificationAsync(notification);

            A.CallTo(() => _requestFactory.CreateRequest(
                    A<NotificationEnumChannel>._,
                    notification.Payload))
                .MustHaveHappenedOnceExactly();
        }

        private Notification CreateNotification()
        {
            var payload = JsonDocument
                .Parse("{\"message\":\"test\"}")
                .RootElement;

            return new Notification
            {
                Channel = NotificationEnumChannel.Slack,
                Payload = payload
            };
        }

        [Test]
        public async Task DispatchNotificationAsync_FactoryThrowsException_ExceptionIsPropagated()
        {
            // Arrange
            var notification = CreateNotification();
            var expectedException = new InvalidOperationException("Unknown channel");

            A.CallTo(() => _requestFactory.CreateRequest(A<NotificationEnumChannel>._, A<JsonElement>._))
                .Throws(expectedException);

            // Act & Assert
            var exception = await Should.ThrowAsync<InvalidOperationException>(() =>
                _sut.DispatchNotificationAsync(notification));

            exception.Message.ShouldBe(expectedException.Message);
        }

        [Test]
        public async Task DispatchNotificationAsync_ProcessorThrowsException_ExceptionIsPropagated()
        {
            // Arrange
            var notification = CreateNotification();
            var command = new FakeCommand();
            var expectedException = new Exception("Broker unavailable");

            A.CallTo(() => _requestFactory.CreateRequest(A<NotificationEnumChannel>._, A<JsonElement>._))
                .Returns(command);

            A.CallTo(() => _commandProcessor.SendAsync<Command>(
                    command,
                    A<RequestContext>._,
                    A<bool>._,
                    A<CancellationToken>._))
                .Throws(expectedException);

            // Act & Assert
            var exception = await Should.ThrowAsync<Exception>(() =>
                _sut.DispatchNotificationAsync(notification));

            exception.Message.ShouldBe(expectedException.Message);
        }

        private class FakeCommand : Command
        {
            public FakeCommand() : base(System.Guid.NewGuid()) { }
        }
    }
}