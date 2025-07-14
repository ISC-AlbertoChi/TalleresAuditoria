using MediatR;

namespace BaseCleanArchitecture.Application.Features.Clientes.Commands.DeleteCliente
{
    public class DeleteClienteCommand : IRequest<bool>
    {
        public int Id { get; }

        public DeleteClienteCommand(int id)
        {
            Id = id;
        }
    }
} 