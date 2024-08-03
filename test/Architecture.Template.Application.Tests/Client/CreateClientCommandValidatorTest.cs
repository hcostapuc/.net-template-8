using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Application.Client.Commands.CreateClient;
using Domain.Entities;
using Domain.Interfaces.Repository;
using FluentAssertions;

namespace Application.UnitTests.Client;
[Collection("Client")]
public class CreateClientCommandValidatorTest
{
    [Fact]
    public async Task ShouldBeValidCommand()
    {
        //Arrange
        var command = new CreateClientCommand("NameTest", "test@test.com", 988887787, "Av test number 3");

        var clientRepository = new Mock<IClientRepository>(MockBehavior.Strict);

        clientRepository.Setup(x => x.ExistAsync(It.IsAny<Expression<Func<ClientEntity, bool>>>(),
                                                 It.IsAny<CancellationToken>()))
                        .ReturnsAsync(false)
                        .Verifiable();

        var validator = new CreateClientCommandValidator(clientRepository.Object);
        //Act
        var validationResult = await validator.ValidateAsync(command);

        //Assert
        validationResult.IsValid.Should().BeTrue();
        validationResult.Errors.Should().BeEmpty();
        clientRepository.VerifyAll();
    }
    [Fact]
    public async Task ShouldBeEmptyName()
    {
        //Arrange
        const string errorMessageEmptyName = "Name is required.";
        var command = new CreateClientCommand(string.Empty, "test@test.com", 988887787, "Av test number 3");

        var clientRepository = new Mock<IClientRepository>(MockBehavior.Strict);

        clientRepository.Setup(x => x.ExistAsync(It.IsAny<Expression<Func<ClientEntity, bool>>>(),
                                                 It.IsAny<CancellationToken>()))
                        .ReturnsAsync(false)
                        .Verifiable();

        var validator = new CreateClientCommandValidator(clientRepository.Object);
        //Act
        var validationResult = await validator.ValidateAsync(command);

        //Assert
        validationResult.IsValid.Should().BeFalse();
        validationResult.Errors.Should().NotBeEmpty().And
                               .HaveCount(1).And
                               .Contain(x => x.ErrorMessage == errorMessageEmptyName);
        clientRepository.VerifyAll();
    }
}
