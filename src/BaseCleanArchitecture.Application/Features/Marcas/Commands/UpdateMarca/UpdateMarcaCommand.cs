using BaseCleanArchitecture.Application.DTOs;
using MediatR;

namespace BaseCleanArchitecture.Application.Features.Marcas.Commands.UpdateMarca
{
    public class UpdateMarcaCommand : IRequest<UpdateMarcaResponseDto>
    {
        public int Id { get; }
        public UpdateMarcaDto Marca { get; }
        public int UserId { get; }

        public UpdateMarcaCommand(int id, UpdateMarcaDto marca, int userId)
        {
            Id = id;
            Marca = marca;
            UserId = userId;
        }
    }
} 