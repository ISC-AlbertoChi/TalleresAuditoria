using AutoMapper;
using BaseCleanArchitecture.Application.Features.TipoUnidades.Commands.UpdateTipoUnidad;
using BaseCleanArchitecture.Application.DTOs;
using BaseCleanArchitecture.Domain.Interfaces;
using MediatR;

namespace BaseCleanArchitecture.Application.Features.TipoUnidades.Commands.UpdateTipoUnidad
{
    public class UpdateTipoUnidadHandler : IRequestHandler<UpdateTipoUnidadCommand, UpdateTipoUnidadResponseDto>
    {
        private readonly ITipoUnidadRepository _tipoUnidadRepository;
        private readonly IMapper _mapper;

        public UpdateTipoUnidadHandler(ITipoUnidadRepository tipoUnidadRepository, IMapper mapper)
        {
            _tipoUnidadRepository = tipoUnidadRepository;
            _mapper = mapper;
        }

        public async Task<UpdateTipoUnidadResponseDto> Handle(UpdateTipoUnidadCommand request, CancellationToken cancellationToken)
        {
            var tipoUnidad = await _tipoUnidadRepository.GetByIdAsync(request.Id);
            if (tipoUnidad == null)
                throw new Exception("Tipo de unidad no encontrado");

            // NO validar por empresa - ignorar completamente
            _mapper.Map(request.TipoUnidad, tipoUnidad);
            tipoUnidad.DateUpdate = DateTime.UtcNow;
            tipoUnidad.IdUserUpdate = request.UserId;

            var updatedTipoUnidad = await _tipoUnidadRepository.UpdateAsync(tipoUnidad);
            return _mapper.Map<UpdateTipoUnidadResponseDto>(updatedTipoUnidad);
        }
    }
} 