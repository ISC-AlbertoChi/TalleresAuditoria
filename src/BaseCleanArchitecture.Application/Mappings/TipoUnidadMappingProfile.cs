using AutoMapper;
using BaseCleanArchitecture.Application.DTOs;
using BaseCleanArchitecture.Domain.Entities;

namespace BaseCleanArchitecture.Application.Mappings
{
    public class TipoUnidadMappingProfile : Profile
    {
        public TipoUnidadMappingProfile()
        {
            CreateMap<TipoUnidad, TipoUnidadDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Clave, opt => opt.MapFrom(src => src.Clave))
                .ForMember(dest => dest.Nombre, opt => opt.MapFrom(src => src.Nombre))
                .ForMember(dest => dest.Descripcion, opt => opt.MapFrom(src => (string?)null))
                .ForMember(dest => dest.FechaCreacion, opt => opt.MapFrom(src => src.DateCreate))
                .ForMember(dest => dest.FechaModificacion, opt => opt.MapFrom(src => src.DateUpdate))
                .ForMember(dest => dest.IdUsuarioCreacion, opt => opt.MapFrom(src => src.IdUser))
                .ForMember(dest => dest.IdUsuarioModificacion, opt => opt.MapFrom(src => src.IdUserUpdate))
                .ForMember(dest => dest.Activo, opt => opt.MapFrom(src => src.Status));

            CreateMap<CreateTipoUnidadDto, TipoUnidad>()
                .ForMember(dest => dest.Clave, opt => opt.MapFrom(src => src.Clave))
                .ForMember(dest => dest.Nombre, opt => opt.MapFrom(src => src.Nombre))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => true));

            CreateMap<UpdateTipoUnidadDto, TipoUnidad>()
                .ForMember(dest => dest.Clave, opt => opt.MapFrom(src => src.Clave))
                .ForMember(dest => dest.Nombre, opt => opt.MapFrom(src => src.Nombre));

            CreateMap<TipoUnidad, TipoUnidadResponseDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Clave, opt => opt.MapFrom(src => src.Clave))
                .ForMember(dest => dest.Nombre, opt => opt.MapFrom(src => src.Nombre))
                .ForMember(dest => dest.IdEmpresa, opt => opt.MapFrom(src => src.IdEmpresa))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
                .ForMember(dest => dest.DateCreate, opt => opt.MapFrom(src => src.DateCreate))
                .ForMember(dest => dest.IdUser, opt => opt.MapFrom(src => src.IdUser))
                .ForMember(dest => dest.DateUpdate, opt => opt.MapFrom(src => src.DateUpdate))
                .ForMember(dest => dest.IdUserUpdate, opt => opt.MapFrom(src => src.IdUserUpdate));

            CreateMap<TipoUnidad, CreateTipoUnidadResponseDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Clave, opt => opt.MapFrom(src => src.Clave))
                .ForMember(dest => dest.Nombre, opt => opt.MapFrom(src => src.Nombre))
                .ForMember(dest => dest.IdEmpresa, opt => opt.MapFrom(src => src.IdEmpresa))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
                .ForMember(dest => dest.DateCreate, opt => opt.MapFrom(src => src.DateCreate))
                .ForMember(dest => dest.IdUser, opt => opt.MapFrom(src => src.IdUser));

            CreateMap<TipoUnidad, UpdateTipoUnidadResponseDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Clave, opt => opt.MapFrom(src => src.Clave))
                .ForMember(dest => dest.Nombre, opt => opt.MapFrom(src => src.Nombre))
                .ForMember(dest => dest.IdEmpresa, opt => opt.MapFrom(src => src.IdEmpresa))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
                .ForMember(dest => dest.DateCreate, opt => opt.MapFrom(src => src.DateCreate))
                .ForMember(dest => dest.IdUser, opt => opt.MapFrom(src => src.IdUser))
                .ForMember(dest => dest.DateUpdate, opt => opt.MapFrom(src => src.DateUpdate))
                .ForMember(dest => dest.IdUserUpdate, opt => opt.MapFrom(src => src.IdUserUpdate));

            CreateMap<TipoUnidad, TipoUnidadSimpleDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Nombre, opt => opt.MapFrom(src => src.Nombre));
        }
    }
} 