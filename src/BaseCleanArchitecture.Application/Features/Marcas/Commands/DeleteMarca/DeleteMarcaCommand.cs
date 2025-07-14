using MediatR;

namespace BaseCleanArchitecture.Application.Features.Marcas.Commands.DeleteMarca
{
    public class DeleteMarcaCommand : IRequest<bool>
    {
        public int Id { get; }

        public DeleteMarcaCommand(int id)
        {
            Id = id;
        }
    }
} 