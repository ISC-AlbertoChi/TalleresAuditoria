using MediatR;

namespace BaseCleanArchitecture.Application.Features.Departamentos.Commands.DeleteDepartamento
{
    public class DeleteDepartamentoCommand : IRequest<bool>
    {
        public int Id { get; }

        public DeleteDepartamentoCommand(int id)
        {
            Id = id;
        }
    }
} 