using AutoFixture;
using FluentAssertions;
using MailContainerTest.Configuration;
using MailContainerTest.Data;
using MailContainerTest.Services;
using MailContainerTest.Types;
using Moq;
using NUnit.Framework;

namespace MailContainerTest.Tests.UnitTests
{
    public class MailTransferServiceTests
    {
        private Mock<IMailContainerDataStoreStrategyService> _containerDataStoreStrategy;
        private Mock<IMailConfiguration> _mailConfiguration;
        private Mock<IMailContainerValidationService> _containerValidationService;
        private Mock<IMailContainerDataStore> _mailDataStore;
        private MailContainer _mailContainer;

        private Fixture _fixture;

        [SetUp]
        public void SetUp()
        {
            _fixture = new Fixture();

            _mailContainer = new MailContainer()
            {
                AllowedMailType = AllowedMailType.LargeLetter,
                Capacity = 1000,
                MailContainerNumber = "5",
                Status = MailContainerStatus.Operational
            };

            _containerDataStoreStrategy = new Mock<IMailContainerDataStoreStrategyService>();
            _mailConfiguration = new Mock<IMailConfiguration>();
            _containerValidationService = new Mock<IMailContainerValidationService>();
            _mailDataStore = new Mock<IMailContainerDataStore>();

            _mailConfiguration
                .Setup(x => x.DataStoreType)
                .Returns("Type");
        }

        [TearDown]
        public void TearDown()
        {
            _mailConfiguration.Verify();
            _mailDataStore.Verify();
            _containerValidationService.Verify();
            _containerDataStoreStrategy.Verify();
        }

        [Test]
        public void ShouldMakeMailTransfer()
        {
            //Arrange

            var request = _fixture.Create<MakeMailTransferRequest>();
            var endCapacity = _mailContainer.Capacity - request.NumberOfMailItems;

            _mailDataStore
                .Setup(x => x.GetMailContainer(It.IsAny<string>()))
                .Returns(_mailContainer)
                .Verifiable();

            _containerDataStoreStrategy
                .Setup(x => x.GetDataStore(It.IsAny<string>()))
                .Returns(_mailDataStore.Object)
                .Verifiable();

            _containerValidationService
                .Setup(x => x.IsValid(It.IsAny<MakeMailTransferRequest>(), It.IsAny<MailContainer>()))
                .Returns(true)
                .Verifiable();

            var sut = new MailTransferService(
                _containerDataStoreStrategy.Object,
                _mailConfiguration.Object,
                _containerValidationService.Object);

            //Act
            var result = sut.MakeMailTransfer(request);

            //Assert

            result.Success.Should().BeTrue();
            _mailDataStore
                .Verify(x => x.UpdateMailContainer(It.Is<MailContainer>(container => container.Capacity == endCapacity)));
        }

        [Test]
        public void ShouldFail_WhenMailContainerIsNull()
        {
            //Arrange

            _mailDataStore
                .Setup(x => x.GetMailContainer(It.IsAny<string>()))
                .Returns((MailContainer)null)
                .Verifiable();

            _containerDataStoreStrategy
                .Setup(x => x.GetDataStore(It.IsAny<string>()))
                .Returns(_mailDataStore.Object)
                .Verifiable();

            var sut = new MailTransferService(
                _containerDataStoreStrategy.Object,
                _mailConfiguration.Object,
                _containerValidationService.Object);

            //Act

            var result = sut.MakeMailTransfer(_fixture.Create<MakeMailTransferRequest>());

            //Assert

            result.Success.Should().BeFalse();
        }

        [Test]
        public void ShouldFail_WhenContainerHasInInvalidState()
        {
            //Arrange

            _mailDataStore
                .Setup(x => x.GetMailContainer(It.IsAny<string>()))
                .Returns(_mailContainer)
                .Verifiable();

            _containerDataStoreStrategy
                .Setup(x => x.GetDataStore(It.IsAny<string>()))
                .Returns(_mailDataStore.Object)
                .Verifiable();

            _containerValidationService
                .Setup(x => x.IsValid(It.IsAny<MakeMailTransferRequest>(), It.IsAny<MailContainer>()))
                .Returns(false)
                .Verifiable();

            var sut = new MailTransferService(
                _containerDataStoreStrategy.Object,
                _mailConfiguration.Object,
                _containerValidationService.Object);

            //Act

            var result = sut.MakeMailTransfer(_fixture.Create<MakeMailTransferRequest>());

            //Assert

            result.Success.Should().BeFalse();
        }
    }
}
