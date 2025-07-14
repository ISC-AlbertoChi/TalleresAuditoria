using AutoMapper;
using BaseCleanArchitecture.Application.DTOs;
using BaseCleanArchitecture.Domain.Entities;

namespace BaseCleanArchitecture.Application.Mappings;

public class AlmacenMappingProfile : Profile
{
    public AlmacenMappingProfile()
    {
        CreateMap<Almacen, AlmacenDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Clave, opt => opt.MapFrom(src => src.Clave))
            .ForMember(dest => dest.Nombre, opt => opt.MapFrom(src => src.Nombre))
            .ForMember(dest => dest.Descripcion, opt => opt.MapFrom(src => src.Descripcion))
            .ForMember(dest => dest.IdSucursal, opt => opt.MapFrom(src => src.IdSucursal))
            .ForMember(dest => dest.IdEmpresa, opt => opt.MapFrom(src => src.IdEmpresa))
            .ForMember(dest => dest.Activo, opt => opt.MapFrom(src => src.Status))
            .ForMember(dest => dest.DateCreate, opt => opt.MapFrom(src => src.DateCreate))
            .ForMember(dest => dest.DateUpdate, opt => opt.MapFrom(src => src.DateUpdate))
            .ForMember(dest => dest.IdUser, opt => opt.MapFrom(src => src.IdUser))
            .ForMember(dest => dest.IdUserUpdate, opt => opt.MapFrom(src => src.IdUserUpdate));

        CreateMap<CreateAlmacenDto, Almacen>()
            .ForMember(dest => dest.Clave, opt => opt.MapFrom(src => src.Clave))
            .ForMember(dest => dest.Nombre, opt => opt.MapFrom(src => src.Nombre))
            .ForMember(dest => dest.Descripcion, opt => opt.MapFrom(src => src.Descripcion))
            .ForMember(dest => dest.IdSucursal, opt => opt.MapFrom(src => src.IdSucursal))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => true));

        CreateMap<UpdateAlmacenDto, Almacen>()
            .ForMember(dest => dest.Clave, opt => opt.MapFrom(src => src.Clave))
            .ForMember(dest => dest.Nombre, opt => opt.MapFrom(src => src.Nombre))
            .ForMember(dest => dest.Descripcion, opt => opt.MapFrom(src => src.Descripcion))
            .ForMember(dest => dest.IdSucursal, opt => opt.MapFrom(src => src.IdSucursal));

        CreateMap<Almacen, AlmacenResponseDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Clave, opt => opt.MapFrom(src => src.Clave))
            .ForMember(dest => dest.Nombre, opt => opt.MapFrom(src => src.Nombre))
            .ForMember(dest => dest.Descripcion, opt => opt.MapFrom(src => src.Descripcion))
            .ForMember(dest => dest.IdSucursal, opt => opt.MapFrom(src => src.IdSucursal))
            .ForMember(dest => dest.IdEmpresa, opt => opt.MapFrom(src => src.IdEmpresa))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
            .ForMember(dest => dest.DateCreate, opt => opt.MapFrom(src => src.DateCreate))
            .ForMember(dest => dest.IdUser, opt => opt.MapFrom(src => src.IdUser))
            .ForMember(dest => dest.DateUpdate, opt => opt.MapFrom(src => src.DateUpdate))
            .ForMember(dest => dest.IdUserUpdate, opt => opt.MapFrom(src => src.IdUserUpdate));

        CreateMap<Almacen, CreateAlmacenResponseDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Clave, opt => opt.MapFrom(src => src.Clave))
            .ForMember(dest => dest.Nombre, opt => opt.MapFrom(src => src.Nombre))
            .ForMember(dest => dest.Descripcion, opt => opt.MapFrom(src => src.Descripcion))
            .ForMember(dest => dest.IdSucursal, opt => opt.MapFrom(src => src.IdSucursal))
            .ForMember(dest => dest.IdEmpresa, opt => opt.MapFrom(src => src.IdEmpresa))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
            .ForMember(dest => dest.DateCreate, opt => opt.MapFrom(src => src.DateCreate))
            .ForMember(dest => dest.IdUser, opt => opt.MapFrom(src => src.IdUser));

        CreateMap<Almacen, UpdateAlmacenResponseDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Clave, opt => opt.MapFrom(src => src.Clave))
            .ForMember(dest => dest.Nombre, opt => opt.MapFrom(src => src.Nombre))
            .ForMember(dest => dest.Descripcion, opt => opt.MapFrom(src => src.Descripcion))
            .ForMember(dest => dest.IdSucursal, opt => opt.MapFrom(src => src.IdSucursal))
            .ForMember(dest => dest.IdEmpresa, opt => opt.MapFrom(src => src.IdEmpresa))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
            .ForMember(dest => dest.DateCreate, opt => opt.MapFrom(src => src.DateCreate))
            .ForMember(dest => dest.IdUser, opt => opt.MapFrom(src => src.IdUser))
            .ForMember(dest => dest.DateUpdate, opt => opt.MapFrom(src => src.DateUpdate))
            .ForMember(dest => dest.IdUserUpdate, opt => opt.MapFrom(src => src.IdUserUpdate));

        CreateMap<Almacen, AlmacenSimpleDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Nombre, opt => opt.MapFrom(src => src.Nombre));
    }
} 