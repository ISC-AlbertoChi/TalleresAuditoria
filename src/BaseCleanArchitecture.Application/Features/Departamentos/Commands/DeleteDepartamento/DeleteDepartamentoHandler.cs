using BaseCleanArchitecture.Application.Features.Departamentos.Commands.DeleteDepartamento;
using BaseCleanArchitecture.Domain.Interfaces;
using MediatR;

namespace BaseCleanArchitecture.Application.Features.Departamentos.Commands.DeleteDepartamento
{
    public class DeleteDepartamentoHandler : IRequestHandler<DeleteDepartamentoCommand, bool>
    {
        private readonly IDepartamentoRepository _departamentoRepository;

        public DeleteDepartamentoHandler(IDepartamentoRepository departamentoRepository)
        {
            _departamentoRepository = departamentoRepository;
        }

        public async Task<bool> Handle(DeleteDepartamentoCommand request, CancellationToken cancellationToken)
        {
            var departamento = await _departamentoRepository.GetByIdAsync(request.Id);
            if (departamento == null)
                throw new Exception("Departamento no encontrado");

            return await _departamentoRepository.DeleteAsync(request.Id);
        }
    }
} 