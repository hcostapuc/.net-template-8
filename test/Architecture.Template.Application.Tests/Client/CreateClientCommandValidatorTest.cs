using System;
using System.Collections.Generic;
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
    public async Task ShouldBeValidCommandAsync()
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
    public static IEnumerable<object[]> InvalidFieldsCommandCollection =>
        [
            [new CreateClientCommand(string.Empty, "test@test.com", 988887787, "Av test number 3") , "Name is required."],

            [new CreateClientCommand("Name Test", string.Empty, 988887787, "Av test number 3") , "Email is required."],
            [new CreateClientCommand("Name Test", "testinvalidemail", 988887787, "Av test number 3") , "'Email' is not a valid email address."],

            [new CreateClientCommand("Name Test", "test@test.com", 0, "Av test number 3") , "PhoneNumber is required."],
            [new CreateClientCommand("Name Test", "test@test.com", 9887784, "Av test number 3") , "PhoneNumber needs to be 9 numbers."],

        ];
    [Theory]
    [MemberData(nameof(InvalidFieldsCommandCollection))]
    public async Task ShouldBeInvalidFieldsAsync(CreateClientCommand command, string errorMessage)
    {
        //Arrange
        var clientRepository = new Mock<IClientRepository>();

        var validator = new CreateClientCommandValidator(clientRepository.Object);
        //Act
        var validationResult = await validator.ValidateAsync(command);

        //Assert
        validationResult.IsValid.Should().BeFalse();
        validationResult.Errors.Should().NotBeEmpty().And
                               .Contain(x => x.ErrorMessage == errorMessage);
    }

    [Fact]
    public async Task ShouldAlreadyExistClientEmailAsync()
    {
        //Arrange
        var errorMessageEmptyName = "Email already exists.";
        var command = new CreateClientCommand("NameTest", "test@test.com", 988887787, "Av test number 3");

        var clientRepository = new Mock<IClientRepository>(MockBehavior.Strict);

        clientRepository.Setup(x => x.ExistAsync(It.IsAny<Expression<Func<ClientEntity, bool>>>(),
                                                 It.IsAny<CancellationToken>()))
                        .ReturnsAsync(true)
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
