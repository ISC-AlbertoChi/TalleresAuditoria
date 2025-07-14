using AutoMapper;
using BaseCleanArchitecture.Application.DTOs.Ubicacion;
using BaseCleanArchitecture.Domain.Entities;

namespace BaseCleanArchitecture.Application.Mappings;

public class UbicacionMappingProfile : Profile
{
    public UbicacionMappingProfile()
    {
        CreateMap<Ubicacion, UbicacionDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Zona, opt => opt.MapFrom(src => src.Zona))
            .ForMember(dest => dest.Pasillo, opt => opt.MapFrom(src => src.Pasillo))
            .ForMember(dest => dest.Nivel, opt => opt.MapFrom(src => src.Nivel))
            .ForMember(dest => dest.Subnivel, opt => opt.MapFrom(src => src.Subnivel))
            .ForMember(dest => dest.Clave, opt => opt.MapFrom(src => src.Clave))
            .ForMember(dest => dest.Descripcion, opt => opt.MapFrom(src => src.Descripcion))
            .ForMember(dest => dest.IdSucursal, opt => opt.MapFrom(src => src.IdSucursal))
            .ForMember(dest => dest.IdAlmacen, opt => opt.MapFrom(src => src.IdAlmacen))
            .ForMember(dest => dest.IdEmpresa, opt => opt.MapFrom(src => src.IdEmpresa))
            .ForMember(dest => dest.Activo, opt => opt.MapFrom(src => src.Status))
            .ForMember(dest => dest.DateCreate, opt => opt.MapFrom(src => src.DateCreate))
            .ForMember(dest => dest.DateUpdate, opt => opt.MapFrom(src => src.DateUpdate))
            .ForMember(dest => dest.IdUser, opt => opt.MapFrom(src => src.IdUser))
            .ForMember(dest => dest.IdUserUpdate, opt => opt.MapFrom(src => src.IdUserUpdate));

        CreateMap<CreateUbicacionDto, Ubicacion>()
            .ForMember(dest => dest.Zona, opt => opt.MapFrom(src => src.Zona))
            .ForMember(dest => dest.Pasillo, opt => opt.MapFrom(src => src.Pasillo))
            .ForMember(dest => dest.Nivel, opt => opt.MapFrom(src => src.Nivel))
            .ForMember(dest => dest.Subnivel, opt => opt.MapFrom(src => src.Subnivel))
            .ForMember(dest => dest.Clave, opt => opt.MapFrom(src => src.Clave))
            .ForMember(dest => dest.Descripcion, opt => opt.MapFrom(src => src.Descripcion))
            .ForMember(dest => dest.IdSucursal, opt => opt.MapFrom(src => src.IdSucursal))
            .ForMember(dest => dest.IdAlmacen, opt => opt.MapFrom(src => src.IdAlmacen))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => true));

        CreateMap<UpdateUbicacionDto, Ubicacion>()
            .ForMember(dest => dest.Zona, opt => opt.MapFrom(src => src.Zona))
            .ForMember(dest => dest.Pasillo, opt => opt.MapFrom(src => src.Pasillo))
            .ForMember(dest => dest.Nivel, opt => opt.MapFrom(src => src.Nivel))
            .ForMember(dest => dest.Subnivel, opt => opt.MapFrom(src => src.Subnivel))
            .ForMember(dest => dest.Clave, opt => opt.MapFrom(src => src.Clave))
            .ForMember(dest => dest.Descripcion, opt => opt.MapFrom(src => src.Descripcion))
            .ForMember(dest => dest.IdSucursal, opt => opt.MapFrom(src => src.IdSucursal))
            .ForMember(dest => dest.IdAlmacen, opt => opt.MapFrom(src => src.IdAlmacen));

        CreateMap<Ubicacion, UbicacionResponseDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Zona, opt => opt.MapFrom(src => src.Zona))
            .ForMember(dest => dest.Pasillo, opt => opt.MapFrom(src => src.Pasillo))
            .ForMember(dest => dest.Nivel, opt => opt.MapFrom(src => src.Nivel))
            .ForMember(dest => dest.Subnivel, opt => opt.MapFrom(src => src.Subnivel))
            .ForMember(dest => dest.Clave, opt => opt.MapFrom(src => src.Clave))
            .ForMember(dest => dest.Descripcion, opt => opt.MapFrom(src => src.Descripcion))
            .ForMember(dest => dest.IdSucursal, opt => opt.MapFrom(src => src.IdSucursal))
            .ForMember(dest => dest.IdAlmacen, opt => opt.MapFrom(src => src.IdAlmacen))
            .ForMember(dest => dest.IdEmpresa, opt => opt.MapFrom(src => src.IdEmpresa))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
            .ForMember(dest => dest.DateCreate, opt => opt.MapFrom(src => src.DateCreate))
            .ForMember(dest => dest.IdUser, opt => opt.MapFrom(src => src.IdUser))
            .ForMember(dest => dest.DateUpdate, opt => opt.MapFrom(src => src.DateUpdate))
            .ForMember(dest => dest.IdUserUpdate, opt => opt.MapFrom(src => src.IdUserUpdate));

        CreateMap<Ubicacion, CreateUbicacionResponseDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Zona, opt => opt.MapFrom(src => src.Zona))
            .ForMember(dest => dest.Pasillo, opt => opt.MapFrom(src => src.Pasillo))
            .ForMember(dest => dest.Nivel, opt => opt.MapFrom(src => src.Nivel))
            .ForMember(dest => dest.Subnivel, opt => opt.MapFrom(src => src.Subnivel))
            .ForMember(dest => dest.Clave, opt => opt.MapFrom(src => src.Clave))
            .ForMember(dest => dest.Descripcion, opt => opt.MapFrom(src => src.Descripcion))
            .ForMember(dest => dest.IdSucursal, opt => opt.MapFrom(src => src.IdSucursal))
            .ForMember(dest => dest.IdAlmacen, opt => opt.MapFrom(src => src.IdAlmacen))
            .ForMember(dest => dest.IdEmpresa, opt => opt.MapFrom(src => src.IdEmpresa))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
            .ForMember(dest => dest.DateCreate, opt => opt.MapFrom(src => src.DateCreate))
            .ForMember(dest => dest.IdUser, opt => opt.MapFrom(src => src.IdUser));

        CreateMap<Ubicacion, UpdateUbicacionResponseDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Zona, opt => opt.MapFrom(src => src.Zona))
            .ForMember(dest => dest.Pasillo, opt => opt.MapFrom(src => src.Pasillo))
            .ForMember(dest => dest.Nivel, opt => opt.MapFrom(src => src.Nivel))
            .ForMember(dest => dest.Subnivel, opt => opt.MapFrom(src => src.Subnivel))
            .ForMember(dest => dest.Clave, opt => opt.MapFrom(src => src.Clave))
            .ForMember(dest => dest.Descripcion, opt => opt.MapFrom(src => src.Descripcion))
            .ForMember(dest => dest.IdSucursal, opt => opt.MapFrom(src => src.IdSucursal))
            .ForMember(dest => dest.IdAlmacen, opt => opt.MapFrom(src => src.IdAlmacen))
            .ForMember(dest => dest.IdEmpresa, opt => opt.MapFrom(src => src.IdEmpresa))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
            .ForMember(dest => dest.DateCreate, opt => opt.MapFrom(src => src.DateCreate))
            .ForMember(dest => dest.IdUser, opt => opt.MapFrom(src => src.IdUser))
            .ForMember(dest => dest.DateUpdate, opt => opt.MapFrom(src => src.DateUpdate))
            .ForMember(dest => dest.IdUserUpdate, opt => opt.MapFrom(src => src.IdUserUpdate));

        CreateMap<Ubicacion, UbicacionSimpleDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Clave, opt => opt.MapFrom(src => src.Clave));
    }
} 