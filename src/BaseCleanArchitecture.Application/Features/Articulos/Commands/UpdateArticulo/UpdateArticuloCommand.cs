using BaseCleanArchitecture.Application.DTOs.Articulo;
using MediatR;

namespace BaseCleanArchitecture.Application.Features.Articulos.Commands.UpdateArticulo;

public class UpdateArticuloCommand : IRequest<UpdateArticuloResponseDto>
{
    public int Id { get; }
    public UpdateArticuloDto Articulo { get; }
    public int UserId { get; }

    public UpdateArticuloCommand(int id, UpdateArticuloDto articulo, int userId)
    {
        Id = id;
        Articulo = articulo;
        UserId = userId;
    }
} 