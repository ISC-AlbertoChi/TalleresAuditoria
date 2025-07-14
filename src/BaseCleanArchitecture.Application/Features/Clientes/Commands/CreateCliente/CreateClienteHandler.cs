using AutoMapper;
using BaseCleanArchitecture.Application.Features.Clientes.Commands.CreateCliente;
using BaseCleanArchitecture.Application.DTOs;
using BaseCleanArchitecture.Domain.Interfaces;
using MediatR;

namespace BaseCleanArchitecture.Application.Features.Clientes.Commands.CreateCliente
{
    public class CreateClienteHandler : IRequestHandler<CreateClienteCommand, ClienteResponseDto>
    {
        private readonly IClienteRepository _clienteRepository;
        private readonly IMapper _mapper;

        public CreateClienteHandler(IClienteRepository clienteRepository, IMapper mapper)
        {
            _clienteRepository = clienteRepository;
            _mapper = mapper;
        }

        public async Task<ClienteResponseDto> Handle(CreateClienteCommand request, CancellationToken cancellationToken)
        {
            if (await _clienteRepository.ExistsByRazonSocialRfcDireccionAsync(
                request.Cliente.RazonSocial ?? string.Empty,
                request.Cliente.Rfc ?? string.Empty,
                request.Cliente.Direccion))
                throw new Exception("Ya existe un cliente con esa combinación de razón social, RFC y dirección");

            var cliente = _mapper.Map<Domain.Entities.Cliente>(request.Cliente);
            cliente.IdUser = request.UserId;
            cliente.Status = true;
            cliente.DateCreate = DateTime.UtcNow;
            cliente.DateUpdate = null;
            cliente.IdUserUpdate = null;

            var createdCliente = await _clienteRepository.CreateAsync(cliente);
            return _mapper.Map<ClienteResponseDto>(createdCliente);
        }
    }
} 