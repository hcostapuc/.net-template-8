using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Application.Client.Commands.DeleteClient;
using Ardalis.GuardClauses;
using Domain.Entities;
using Domain.Interfaces.Repository;

namespace Application.UnitTests.Client;

[Collection("Client")]
public class DeleteClientCommandHandlerTest
{
    public DeleteClientCommandHandlerTest()
    {

    }

    [Fact]
    public async Task ShouldDeleteSuccesfullyClientAsync()
    {
        //Arrange
        var command = new DeleteClientCommand(Guid.NewGuid());
        var entity = new ClientEntity()
        {
            Id = command.Id,
            Name = "NameTest",
            Email = "test@test.com",
            PhoneNumber = 878773475,
            Address = "Av test number 4"
        };

        var clientRepository = new Mock<IClientRepository>(MockBehavior.Strict);
        var cancellationToken = new CancellationToken();

        clientRepository.Setup(x => x.SelectAsync(It.IsAny<Expression<Func<ClientEntity, bool>>>(),
                                                                           It.IsAny<CancellationToken>()))
                        .ReturnsAsync(entity)
                        .Verifiable(Times.Once());

        clientRepository.Setup(x => x.DeleteAsync(It.Is<ClientEntity>(c => c.Name == entity.Name &&
                                                                           c.Id == entity.Id &&
                                                                           c.Email == entity.Email &&
                                                                           c.PhoneNumber == entity.PhoneNumber &&
                                                                           c.Address == entity.Address),
                                                                           It.IsAny<CancellationToken>()))
                        .ReturnsAsync(entity)
                        .Verifiable(Times.Once());

        var commandHandler = new DeleteClientCommandHandler(clientRepository.Object);
        //Act
        await commandHandler.Handle(command, cancellationToken);

        //Assert
        clientRepository.VerifyAll();
    }

    [Fact]
    public async Task ShouldNotFoundClientAsync()
    {
        //Arrange
        var command = new DeleteClientCommand(Guid.NewGuid());

        var clientRepository = new Mock<IClientRepository>(MockBehavior.Strict);
        var cancellationToken = new CancellationToken();

        clientRepository.Setup(x => x.SelectAsync(It.IsAny<Expression<Func<ClientEntity, bool>>>(),
                                                                           It.IsAny<CancellationToken>()))
                        .ReturnsAsync((ClientEntity)null)
                        .Verifiable(Times.Once());

        clientRepository.Setup(x => x.DeleteAsync(It.IsAny<ClientEntity>(), It.IsAny<CancellationToken>()))
                        .Verifiable(Times.Never());

        var commandHandler = new DeleteClientCommandHandler(clientRepository.Object);

        //Act
        var act = async () => await commandHandler.Handle(command, cancellationToken);
        
        //Assert
        act.Should().ThrowAsync<NotFoundException>("Client was not found");
        clientRepository.VerifyAll();
    }
}