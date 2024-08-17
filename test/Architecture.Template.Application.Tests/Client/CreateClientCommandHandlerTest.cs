using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Client.Commands.CreateClient;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces.Repository;

namespace Application.UnitTests.Client;

[Collection("Client")]
public class CreateClientCommandHandlerTest
{
    public CreateClientCommandHandlerTest()
    {

    }
    [Fact]
    public async Task ShouldCreateSuccesfullyClientAsync()
    {
        //Arrange
        var command = new CreateClientCommand("NameTest", "test@test.com", 878773475, "Av test number 4");
        var entity = new ClientEntity()
        {
            Name = "NameTest",
            Email = "test@test.com",
            PhoneNumber = 878773475,
            Address = "Av test number 4"
        };

        var clientRepository = new Mock<IClientRepository>(MockBehavior.Strict);
        var mapper = new Mock<IMapper>(MockBehavior.Strict);
        var cancellationToken = new CancellationToken();

        mapper.Setup(x => x.Map<CreateClientCommand, ClientEntity>(It.Is<CreateClientCommand>(c => c.Name == command.Name &&
                                                                           c.Email == command.Email &&
                                                                           c.PhoneNumber == command.PhoneNumber &&
                                                                           c.Address == command.Address)))
              .Returns(entity)
              .Verifiable(Times.Once());

        clientRepository.Setup(x => x.InsertAsync(It.Is<ClientEntity>(c => c.Name == command.Name &&
                                                                           c.Id == Guid.Empty &&
                                                                           c.Email == command.Email &&
                                                                           c.PhoneNumber == command.PhoneNumber &&
                                                                           c.Address == command.Address),
                                                                           It.IsAny<CancellationToken>()))
                        .ReturnsAsync(entity)
                        .Verifiable(Times.Once());

        var commandHandler = new CreateClientCommandHandler(clientRepository.Object,
                                                            mapper.Object);
        //Act
        var entityResponse = await commandHandler.Handle(command, cancellationToken);

        //Assert
        clientRepository.VerifyAll();
        mapper.VerifyAll();
    }
}
