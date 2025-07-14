using BaseCleanArchitecture.Application.DTOs.Articulo;
using MediatR;

namespace BaseCleanArchitecture.Application.Features.Articulos.Commands.CreateArticulo;

public class CreateArticuloCommand : IRequest<CreateArticuloResponseDto>
{
    public CreateArticuloDto Articulo { get; }
    public int UserId { get; }

    public CreateArticuloCommand(CreateArticuloDto articulo, int userId)
    {
        Articulo = articulo;
        UserId = userId;
    }
} 