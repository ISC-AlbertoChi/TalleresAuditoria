using BaseCleanArchitecture.Application.DTOs;
using MediatR;

namespace BaseCleanArchitecture.Application.Features.Clientes.Commands.UpdateCliente
{
    public class UpdateClienteCommand : IRequest<ClienteUpdateResponseDto>
    {
        public int Id { get; }
        public UpdateClienteDto Cliente { get; }
        public int UserId { get; }

        public UpdateClienteCommand(int id, UpdateClienteDto cliente, int userId)
        {
            Id = id;
            Cliente = cliente;
            UserId = userId;
        }
    }
} 