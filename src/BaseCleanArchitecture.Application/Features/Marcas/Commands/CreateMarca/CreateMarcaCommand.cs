using BaseCleanArchitecture.Application.DTOs;
using MediatR;

namespace BaseCleanArchitecture.Application.Features.Marcas.Commands.CreateMarca
{
    public class CreateMarcaCommand : IRequest<CreateMarcaResponseDto>
    {
        public CreateMarcaDto Marca { get; }
        public int UserId { get; }
        public int IdEmpresa { get; }

        public CreateMarcaCommand(CreateMarcaDto marca, int userId, int idEmpresa)
        {
            Marca = marca;
            UserId = userId;
            IdEmpresa = idEmpresa;
        }
    }
} 