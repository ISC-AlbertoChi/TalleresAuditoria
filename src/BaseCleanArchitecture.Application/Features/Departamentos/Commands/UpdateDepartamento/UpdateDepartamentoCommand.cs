using BaseCleanArchitecture.Application.DTOs;
using MediatR;

namespace BaseCleanArchitecture.Application.Features.Departamentos.Commands.UpdateDepartamento
{
    public class UpdateDepartamentoCommand : IRequest<DepartamentoUpdateResponseDto>
    {
        public int Id { get; }
        public UpdateDepartamentoDto Departamento { get; }
        public int UserId { get; }

        public UpdateDepartamentoCommand(int id, UpdateDepartamentoDto departamento, int userId)
        {
            Id = id;
            Departamento = departamento;
            UserId = userId;
        }
    }
} 