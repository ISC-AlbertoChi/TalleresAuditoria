using AutoMapper;
using BaseCleanArchitecture.Application.Features.Clientes.Commands.UpdateCliente;
using BaseCleanArchitecture.Application.DTOs;
using BaseCleanArchitecture.Domain.Interfaces;
using MediatR;

namespace BaseCleanArchitecture.Application.Features.Clientes.Commands.UpdateCliente
{
    public class UpdateClienteHandler : IRequestHandler<UpdateClienteCommand, ClienteUpdateResponseDto>
    {
        private readonly IClienteRepository _clienteRepository;
        private readonly IMapper _mapper;

        public UpdateClienteHandler(IClienteRepository clienteRepository, IMapper mapper)
        {
            _clienteRepository = clienteRepository;
            _mapper = mapper;
        }

        public async Task<ClienteUpdateResponseDto> Handle(UpdateClienteCommand request, CancellationToken cancellationToken)
        {
            var cliente = await _clienteRepository.GetByIdAsync(request.Id);
            if (cliente == null)
                throw new Exception("Cliente no encontrado");

            // Si se están actualizando los campos que forman parte de la clave única
            if (!string.IsNullOrEmpty(request.Cliente.RazonSocial) || 
                !string.IsNullOrEmpty(request.Cliente.RFC) || 
                request.Cliente.Direccion != null)
            {
                var razonSocial = request.Cliente.RazonSocial ?? cliente.RazonSocial;
                var rfc = request.Cliente.RFC ?? cliente.RFC;
                var direccion = request.Cliente.Direccion ?? cliente.Direccion;

                if (await _clienteRepository.ExistsByRazonSocialRfcDireccionAsync(razonSocial, rfc, direccion) &&
                    (razonSocial != cliente.RazonSocial || rfc != cliente.RFC || direccion != cliente.Direccion))
                    throw new Exception("Ya existe un cliente con esa combinación de razón social, RFC y dirección");
            }

            _mapper.Map(request.Cliente, cliente);
            cliente.DateUpdate = DateTime.UtcNow;
            cliente.IdUserUpdate = request.UserId;

            var updatedCliente = await _clienteRepository.UpdateAsync(cliente);
            return _mapper.Map<ClienteUpdateResponseDto>(updatedCliente);
        }
    }
} 