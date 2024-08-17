using System;
using System.Threading.Tasks;
using Application.Client.Commands.DeleteClient;

namespace Application.UnitTests.Client;

[Collection("Client")]
public class DeleteClientCommandValidatorTest
{
    public DeleteClientCommandValidatorTest()
    {

    }

    [Fact]
    public async Task ShouldBeValidCommandAsync()
    {
        //Arrange
        var command = new DeleteClientCommand(Guid.NewGuid());

        var commandValidator = new DeleteClientCommandValidator();
        //Act
        var validationResult = await commandValidator.ValidateAsync(command);

        //Assert
        validationResult.IsValid.Should().BeTrue();
        validationResult.Errors.Should().BeEmpty();
    }

    [Fact]
    public async Task ShouldBeInvalidIdFieldAsync()
    {
        //Arrange
        var errorMessage = "Id is required.";
        var command = new DeleteClientCommand(Guid.Empty);

        var commandValidator = new DeleteClientCommandValidator();
        //Act
        var validationResult = await commandValidator.ValidateAsync(command);

        //Assert
        validationResult.IsValid.Should().BeFalse();
        validationResult.Errors.Should().NotBeEmpty().And
                               .HaveCount(1).And
                               .Contain(x => x.ErrorMessage == errorMessage);
    }
}