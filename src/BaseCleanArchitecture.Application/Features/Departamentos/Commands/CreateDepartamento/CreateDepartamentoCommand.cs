using BaseCleanArchitecture.Application.DTOs;
using MediatR;

namespace BaseCleanArchitecture.Application.Features.Departamentos.Commands.CreateDepartamento
{
    public class CreateDepartamentoCommand : IRequest<DepartamentoResponseDto>
    {
        public CreateDepartamentoDto Departamento { get; }
        public int UserId { get; }
        public int IdEmpresa { get; }

        public CreateDepartamentoCommand(CreateDepartamentoDto departamento, int userId, int idEmpresa)
        {
            Departamento = departamento;
            UserId = userId;
            IdEmpresa = idEmpresa;
        }
    }
} 