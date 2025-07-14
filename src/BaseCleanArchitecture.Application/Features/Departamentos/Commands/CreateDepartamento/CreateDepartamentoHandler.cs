using AutoMapper;
using BaseCleanArchitecture.Application.Features.Departamentos.Commands.CreateDepartamento;
using BaseCleanArchitecture.Application.DTOs;
using BaseCleanArchitecture.Domain.Entities;
using BaseCleanArchitecture.Domain.Interfaces;
using MediatR;

namespace BaseCleanArchitecture.Application.Features.Departamentos.Commands.CreateDepartamento
{
    public class CreateDepartamentoHandler : IRequestHandler<CreateDepartamentoCommand, DepartamentoResponseDto>
    {
        private readonly IDepartamentoRepository _departamentoRepository;
        private readonly IMapper _mapper;

        public CreateDepartamentoHandler(
            IDepartamentoRepository departamentoRepository, 
            IMapper mapper)
        {
            _departamentoRepository = departamentoRepository;
            _mapper = mapper;
        }

        public async Task<DepartamentoResponseDto> Handle(CreateDepartamentoCommand request, CancellationToken cancellationToken)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(request.Departamento.Nombre))
                    throw new Exception("El nombre del departamento es requerido");

                // Validar que no exista un departamento con el mismo nombre en la misma empresa
                var existingDepartamentos = await _departamentoRepository.GetAllAsync();
                int? idEmpresaToCheck = request.IdEmpresa == 0 ? null : request.IdEmpresa;
                if (existingDepartamentos.Any(d => 
                    d.Nombre.Equals(request.Departamento.Nombre, StringComparison.OrdinalIgnoreCase) &&
                    d.IdEmpresa == idEmpresaToCheck))
                    throw new Exception("Ya existe un departamento con ese nombre en esta empresa");

                var departamento = _mapper.Map<Domain.Entities.Departamento>(request.Departamento);
                departamento.IdUser = request.UserId;
                // Si IdEmpresa es 0, establecerlo como null para evitar problemas de FK
                departamento.IdEmpresa = request.IdEmpresa == 0 ? null : request.IdEmpresa;
                departamento.Status = true;
                departamento.DateCreate = DateTime.UtcNow;
                departamento.DateUpdate = null;
                departamento.IdUserUpdate = null;

                var createdDepartamento = await _departamentoRepository.CreateAsync(departamento);
                return _mapper.Map<DepartamentoResponseDto>(createdDepartamento);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al crear el departamento: {ex.Message}", ex);
            }
        }
    }
} 