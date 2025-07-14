using BaseCleanArchitecture.Application.DTOs;
using MediatR;

namespace BaseCleanArchitecture.Application.Features.Clientes.Commands.CreateCliente
{
    public class CreateClienteCommand : IRequest<ClienteResponseDto>
    {
        public CreateClienteDto Cliente { get; }
        public int UserId { get; }

        public CreateClienteCommand(CreateClienteDto cliente, int userId)
        {
            Cliente = cliente;
            UserId = userId;
        }
    }
} 