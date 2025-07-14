using BaseCleanArchitecture.Application.Features.Almacenes.Commands.DeleteAlmacen;
using BaseCleanArchitecture.Domain.Interfaces;
using MediatR;

namespace BaseCleanArchitecture.Application.Features.Almacenes.Commands.DeleteAlmacen;

public class DeleteAlmacenHandler : IRequestHandler<DeleteAlmacenCommand, bool>
{
    private readonly IAlmacenRepository _almacenRepository;

    public DeleteAlmacenHandler(IAlmacenRepository almacenRepository)
    {
        _almacenRepository = almacenRepository;
    }

    public async Task<bool> Handle(DeleteAlmacenCommand request, CancellationToken cancellationToken)
    {
        var almacen = await _almacenRepository.GetByIdAsync(request.Id);
        if (almacen == null)
            throw new Exception($"No se encontró el almacén con ID: {request.Id}");

        return await _almacenRepository.DeleteAsync(request.Id);
    }
} 