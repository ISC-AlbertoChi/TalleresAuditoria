using AutoMapper;
using BaseCleanArchitecture.Application.Features.Departamentos.Commands.UpdateDepartamento;
using BaseCleanArchitecture.Application.DTOs;
using BaseCleanArchitecture.Domain.Interfaces;
using MediatR;

namespace BaseCleanArchitecture.Application.Features.Departamentos.Commands.UpdateDepartamento
{
    public class UpdateDepartamentoHandler : IRequestHandler<UpdateDepartamentoCommand, DepartamentoUpdateResponseDto>
    {
        private readonly IDepartamentoRepository _departamentoRepository;
        private readonly IMapper _mapper;

        public UpdateDepartamentoHandler(IDepartamentoRepository departamentoRepository, IMapper mapper)
        {
            _departamentoRepository = departamentoRepository;
            _mapper = mapper;
        }

        public async Task<DepartamentoUpdateResponseDto> Handle(UpdateDepartamentoCommand request, CancellationToken cancellationToken)
        {
            var departamento = await _departamentoRepository.GetByIdAsync(request.Id);
            if (departamento == null)
                throw new Exception("Departamento no encontrado");

            // Verificar si ya existe un departamento con el mismo nombre
            var departamentosExistentes = await _departamentoRepository.GetAllAsync();
            var departamentoExistente = departamentosExistentes.FirstOrDefault(d => 
                d.Nombre == request.Departamento.Nombre && d.Id != request.Id);
            if (departamentoExistente != null)
                throw new Exception("Ya existe un departamento con ese nombre");

            _mapper.Map(request.Departamento, departamento);
            departamento.DateUpdate = DateTime.UtcNow;
            departamento.IdUserUpdate = request.UserId;

            var updatedDepartamento = await _departamentoRepository.UpdateAsync(departamento);
            return _mapper.Map<DepartamentoUpdateResponseDto>(updatedDepartamento);
        }
    }
} 