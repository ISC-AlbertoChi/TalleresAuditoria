using AutoMapper;
using BaseCleanArchitecture.Application.DTOs;
using BaseCleanArchitecture.Domain.Entities;

namespace BaseCleanArchitecture.Application.Mappings
{
    public class ClienteMappingProfile : Profile
    {
        public ClienteMappingProfile()
        {
            CreateMap<Cliente, ClienteDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.NombreComercial, opt => opt.MapFrom(src => src.NombreComercial))
                .ForMember(dest => dest.RazonSocial, opt => opt.MapFrom(src => src.RazonSocial))
                .ForMember(dest => dest.RFC, opt => opt.MapFrom(src => src.RFC))
                .ForMember(dest => dest.Direccion, opt => opt.MapFrom(src => src.Direccion))
                .ForMember(dest => dest.Telefono, opt => opt.MapFrom(src => src.Telefono))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Correo))
                .ForMember(dest => dest.Contacto, opt => opt.MapFrom(src => src.NombreContacto))
                .ForMember(dest => dest.TelefonoContacto, opt => opt.MapFrom(src => src.TelefonoContacto))
                .ForMember(dest => dest.EmailContacto, opt => opt.MapFrom(src => src.CorreoContacto))
                .ForMember(dest => dest.Observaciones, opt => opt.MapFrom(src => (string?)null))
                .ForMember(dest => dest.Activo, opt => opt.MapFrom(src => src.Status))
                .ForMember(dest => dest.DateCreate, opt => opt.MapFrom(src => src.DateCreate))
                .ForMember(dest => dest.DateUpdate, opt => opt.MapFrom(src => src.DateUpdate))
                .ForMember(dest => dest.IdUser, opt => opt.MapFrom(src => src.IdUser))
                .ForMember(dest => dest.IdUserUpdate, opt => opt.MapFrom(src => src.IdUserUpdate));

            CreateMap<CreateClienteDto, Cliente>()
                .ForMember(dest => dest.NombreComercial, opt => opt.MapFrom(src => src.NombreComercial))
                .ForMember(dest => dest.RazonSocial, opt => opt.MapFrom(src => src.RazonSocial))
                .ForMember(dest => dest.RFC, opt => opt.MapFrom(src => src.Rfc))
                .ForMember(dest => dest.Direccion, opt => opt.MapFrom(src => src.Direccion))
                .ForMember(dest => dest.Telefono, opt => opt.MapFrom(src => src.Telefono))
                .ForMember(dest => dest.Correo, opt => opt.MapFrom(src => src.Correo))
                .ForMember(dest => dest.NombreContacto, opt => opt.MapFrom(src => src.NombreContacto))
                .ForMember(dest => dest.TelefonoContacto, opt => opt.MapFrom(src => src.TelefonoContacto))
                .ForMember(dest => dest.CorreoContacto, opt => opt.MapFrom(src => src.CorreoContacto))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => true));

            CreateMap<UpdateClienteDto, Cliente>()
                .ForMember(dest => dest.NombreComercial, opt => opt.MapFrom(src => src.NombreComercial))
                .ForMember(dest => dest.RazonSocial, opt => opt.MapFrom(src => src.RazonSocial))
                .ForMember(dest => dest.RFC, opt => opt.MapFrom(src => src.RFC))
                .ForMember(dest => dest.Direccion, opt => opt.MapFrom(src => src.Direccion))
                .ForMember(dest => dest.Telefono, opt => opt.MapFrom(src => src.Telefono))
                .ForMember(dest => dest.Correo, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.NombreContacto, opt => opt.MapFrom(src => src.Contacto))
                .ForMember(dest => dest.TelefonoContacto, opt => opt.MapFrom(src => src.TelefonoContacto))
                .ForMember(dest => dest.CorreoContacto, opt => opt.MapFrom(src => src.EmailContacto));

            CreateMap<Cliente, ClienteResponseDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.NombreComercial, opt => opt.MapFrom(src => src.NombreComercial))
                .ForMember(dest => dest.RazonSocial, opt => opt.MapFrom(src => src.RazonSocial))
                .ForMember(dest => dest.RFC, opt => opt.MapFrom(src => src.RFC))
                .ForMember(dest => dest.Direccion, opt => opt.MapFrom(src => src.Direccion))
                .ForMember(dest => dest.Telefono, opt => opt.MapFrom(src => src.Telefono))
                .ForMember(dest => dest.Correo, opt => opt.MapFrom(src => src.Correo))
                .ForMember(dest => dest.NombreContacto, opt => opt.MapFrom(src => src.NombreContacto))
                .ForMember(dest => dest.TelefonoContacto, opt => opt.MapFrom(src => src.TelefonoContacto))
                .ForMember(dest => dest.CorreoContacto, opt => opt.MapFrom(src => src.CorreoContacto))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
                .ForMember(dest => dest.DateCreate, opt => opt.MapFrom(src => src.DateCreate))
                .ForMember(dest => dest.IdUser, opt => opt.MapFrom(src => src.IdUser));

            CreateMap<Cliente, ClienteUpdateResponseDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.NombreComercial, opt => opt.MapFrom(src => src.NombreComercial))
                .ForMember(dest => dest.RazonSocial, opt => opt.MapFrom(src => src.RazonSocial))
                .ForMember(dest => dest.RFC, opt => opt.MapFrom(src => src.RFC))
                .ForMember(dest => dest.Direccion, opt => opt.MapFrom(src => src.Direccion))
                .ForMember(dest => dest.Telefono, opt => opt.MapFrom(src => src.Telefono))
                .ForMember(dest => dest.Correo, opt => opt.MapFrom(src => src.Correo))
                .ForMember(dest => dest.NombreContacto, opt => opt.MapFrom(src => src.NombreContacto))
                .ForMember(dest => dest.TelefonoContacto, opt => opt.MapFrom(src => src.TelefonoContacto))
                .ForMember(dest => dest.CorreoContacto, opt => opt.MapFrom(src => src.CorreoContacto))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
                .ForMember(dest => dest.DateCreate, opt => opt.MapFrom(src => src.DateCreate))
                .ForMember(dest => dest.IdUser, opt => opt.MapFrom(src => src.IdUser))
                .ForMember(dest => dest.DateUpdate, opt => opt.MapFrom(src => src.DateUpdate))
                .ForMember(dest => dest.IdUserUpdate, opt => opt.MapFrom(src => src.IdUserUpdate));

            CreateMap<Cliente, ClienteSimpleDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.NombreComercial, opt => opt.MapFrom(src => src.NombreComercial));
        }
    }
} 