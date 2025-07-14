using AutoMapper;
using BaseCleanArchitecture.Application.Features.Marcas.Commands.UpdateMarca;
using BaseCleanArchitecture.Application.DTOs;
using BaseCleanArchitecture.Domain.Interfaces;
using MediatR;

namespace BaseCleanArchitecture.Application.Features.Marcas.Commands.UpdateMarca
{
    public class UpdateMarcaHandler : IRequestHandler<UpdateMarcaCommand, UpdateMarcaResponseDto>
    {
        private readonly IMarcaRepository _marcaRepository;
        private readonly IMapper _mapper;

        public UpdateMarcaHandler(IMarcaRepository marcaRepository, IMapper mapper)
        {
            _marcaRepository = marcaRepository;
            _mapper = mapper;
        }

        public async Task<UpdateMarcaResponseDto> Handle(UpdateMarcaCommand request, CancellationToken cancellationToken)
        {
            var existingMarca = await _marcaRepository.GetByIdAsync(request.Id);
            if (existingMarca == null)
                throw new Exception("Marca no encontrada");

            // NO validar por empresa - ignorar completamente
            var marca = _mapper.Map<Domain.Entities.Marca>(request.Marca);
            marca.Id = request.Id;
            marca.IdUserUpdate = request.UserId;
            marca.DateUpdate = DateTime.UtcNow;
            marca.Status = existingMarca.Status;
            marca.DateCreate = existingMarca.DateCreate;
            marca.IdUser = existingMarca.IdUser;

            var updatedMarca = await _marcaRepository.UpdateAsync(marca);
            return _mapper.Map<UpdateMarcaResponseDto>(updatedMarca);
        }
    }
} 