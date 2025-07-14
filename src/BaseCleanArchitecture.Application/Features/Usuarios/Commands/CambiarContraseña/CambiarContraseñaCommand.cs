using MediatR;
using BaseCleanArchitecture.Application.DTOs;

namespace BaseCleanArchitecture.Application.Features.Usuarios.Commands.CambiarContraseña
{
    public class CambiarContraseñaCommand : IRequest<bool>
    {
        public int Id { get; }
        public string ContraseñaActual { get; }
        public string NuevaContraseña { get; }

        public CambiarContraseñaCommand(int id, string contraseñaActual, string nuevaContraseña)
        {
            Id = id;
            ContraseñaActual = contraseñaActual;
            NuevaContraseña = nuevaContraseña;
        }
    }
} 