using AutoMapper;
using BaseCleanArchitecture.Application.DTOs;
using BaseCleanArchitecture.Application.DTOs.Vehiculo;
using BaseCleanArchitecture.Domain.Entities;

namespace BaseCleanArchitecture.Application.Mappings;

public class VehiculoMappingProfile : Profile
{
    public VehiculoMappingProfile()
    {
        CreateMap<Vehiculo, VehiculoDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.NumeroEconomico, opt => opt.MapFrom(src => src.NumeroEconomico))
            .ForMember(dest => dest.Placa, opt => opt.MapFrom(src => src.Placa))
            .ForMember(dest => dest.Serie, opt => opt.MapFrom(src => src.Serie))
            .ForMember(dest => dest.Observaciones, opt => opt.MapFrom(src => src.Observaciones))
            .ForMember(dest => dest.IdMarca, opt => opt.MapFrom(src => src.IdMarca))
            .ForMember(dest => dest.IdModelo, opt => opt.MapFrom(src => src.IdModelo))
            .ForMember(dest => dest.IdPropietario, opt => opt.MapFrom(src => src.IdPropietario))
            .ForMember(dest => dest.IdTipoCombustible, opt => opt.MapFrom(src => src.IdTipoCombustible))
            .ForMember(dest => dest.IdTipoUnidad, opt => opt.MapFrom(src => src.IdTipoUnidad))
            .ForMember(dest => dest.Activo, opt => opt.MapFrom(src => src.Status))
            .ForMember(dest => dest.DateCreate, opt => opt.MapFrom(src => src.DateCreate))
            .ForMember(dest => dest.DateUpdate, opt => opt.MapFrom(src => src.DateUpdate))
            .ForMember(dest => dest.IdUser, opt => opt.MapFrom(src => src.IdUser))
            .ForMember(dest => dest.IdUserUpdate, opt => opt.MapFrom(src => src.IdUserUpdate));

        CreateMap<CreateVehiculoDto, Vehiculo>()
            .ForMember(dest => dest.NumeroEconomico, opt => opt.MapFrom(src => src.NumeroEconomico))
            .ForMember(dest => dest.Placa, opt => opt.MapFrom(src => src.Placa))
            .ForMember(dest => dest.Serie, opt => opt.MapFrom(src => src.Serie))
            .ForMember(dest => dest.Observaciones, opt => opt.MapFrom(src => src.Observaciones))
            .ForMember(dest => dest.IdMarca, opt => opt.MapFrom(src => src.IdMarca))
            .ForMember(dest => dest.IdModelo, opt => opt.MapFrom(src => src.IdModelo))
            .ForMember(dest => dest.IdPropietario, opt => opt.MapFrom(src => src.IdPropietario))
            .ForMember(dest => dest.IdTipoCombustible, opt => opt.MapFrom(src => src.IdTipoCombustible))
            .ForMember(dest => dest.IdTipoUnidad, opt => opt.MapFrom(src => src.IdTipoUnidad))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => true));

        CreateMap<UpdateVehiculoDto, Vehiculo>()
            .ForMember(dest => dest.NumeroEconomico, opt => opt.MapFrom(src => src.NumeroEconomico))
            .ForMember(dest => dest.Placa, opt => opt.MapFrom(src => src.Placa))
            .ForMember(dest => dest.Serie, opt => opt.MapFrom(src => src.Serie))
            .ForMember(dest => dest.Observaciones, opt => opt.MapFrom(src => src.Observaciones))
            .ForMember(dest => dest.IdMarca, opt => opt.MapFrom(src => src.IdMarca))
            .ForMember(dest => dest.IdModelo, opt => opt.MapFrom(src => src.IdModelo))
            .ForMember(dest => dest.IdPropietario, opt => opt.MapFrom(src => src.IdPropietario))
            .ForMember(dest => dest.IdTipoCombustible, opt => opt.MapFrom(src => src.IdTipoCombustible))
            .ForMember(dest => dest.IdTipoUnidad, opt => opt.MapFrom(src => src.IdTipoUnidad));

        CreateMap<Vehiculo, VehiculoResponseDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.NumeroEconomico, opt => opt.MapFrom(src => src.NumeroEconomico))
            .ForMember(dest => dest.Placa, opt => opt.MapFrom(src => src.Placa))
            .ForMember(dest => dest.Serie, opt => opt.MapFrom(src => src.Serie))
            .ForMember(dest => dest.Observaciones, opt => opt.MapFrom(src => src.Observaciones))
            .ForMember(dest => dest.IdMarca, opt => opt.MapFrom(src => src.IdMarca))
            .ForMember(dest => dest.IdModelo, opt => opt.MapFrom(src => src.IdModelo))
            .ForMember(dest => dest.IdPropietario, opt => opt.MapFrom(src => src.IdPropietario))
            .ForMember(dest => dest.IdTipoCombustible, opt => opt.MapFrom(src => src.IdTipoCombustible))
            .ForMember(dest => dest.IdTipoUnidad, opt => opt.MapFrom(src => src.IdTipoUnidad))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
            .ForMember(dest => dest.DateCreate, opt => opt.MapFrom(src => src.DateCreate))
            .ForMember(dest => dest.IdUser, opt => opt.MapFrom(src => src.IdUser))
            .ForMember(dest => dest.DateUpdate, opt => opt.MapFrom(src => src.DateUpdate))
            .ForMember(dest => dest.IdUserUpdate, opt => opt.MapFrom(src => src.IdUserUpdate));

        CreateMap<Vehiculo, CreateVehiculoResponseDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.NumeroEconomico, opt => opt.MapFrom(src => src.NumeroEconomico))
            .ForMember(dest => dest.Placa, opt => opt.MapFrom(src => src.Placa))
            .ForMember(dest => dest.Serie, opt => opt.MapFrom(src => src.Serie))
            .ForMember(dest => dest.Observaciones, opt => opt.MapFrom(src => src.Observaciones))
            .ForMember(dest => dest.IdMarca, opt => opt.MapFrom(src => src.IdMarca))
            .ForMember(dest => dest.IdModelo, opt => opt.MapFrom(src => src.IdModelo))
            .ForMember(dest => dest.IdPropietario, opt => opt.MapFrom(src => src.IdPropietario))
            .ForMember(dest => dest.IdTipoCombustible, opt => opt.MapFrom(src => src.IdTipoCombustible))
            .ForMember(dest => dest.IdTipoUnidad, opt => opt.MapFrom(src => src.IdTipoUnidad))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
            .ForMember(dest => dest.DateCreate, opt => opt.MapFrom(src => src.DateCreate))
            .ForMember(dest => dest.IdUser, opt => opt.MapFrom(src => src.IdUser));

        CreateMap<Vehiculo, UpdateVehiculoResponseDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.NumeroEconomico, opt => opt.MapFrom(src => src.NumeroEconomico))
            .ForMember(dest => dest.Placa, opt => opt.MapFrom(src => src.Placa))
            .ForMember(dest => dest.Serie, opt => opt.MapFrom(src => src.Serie))
            .ForMember(dest => dest.Observaciones, opt => opt.MapFrom(src => src.Observaciones))
            .ForMember(dest => dest.IdMarca, opt => opt.MapFrom(src => src.IdMarca))
            .ForMember(dest => dest.IdModelo, opt => opt.MapFrom(src => src.IdModelo))
            .ForMember(dest => dest.IdPropietario, opt => opt.MapFrom(src => src.IdPropietario))
            .ForMember(dest => dest.IdTipoCombustible, opt => opt.MapFrom(src => src.IdTipoCombustible))
            .ForMember(dest => dest.IdTipoUnidad, opt => opt.MapFrom(src => src.IdTipoUnidad))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
            .ForMember(dest => dest.DateCreate, opt => opt.MapFrom(src => src.DateCreate))
            .ForMember(dest => dest.IdUser, opt => opt.MapFrom(src => src.IdUser))
            .ForMember(dest => dest.DateUpdate, opt => opt.MapFrom(src => src.DateUpdate))
            .ForMember(dest => dest.IdUserUpdate, opt => opt.MapFrom(src => src.IdUserUpdate));

        CreateMap<Vehiculo, VehiculoSimpleDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.NumeroEconomico, opt => opt.MapFrom(src => src.NumeroEconomico));

        // Mapeos para comboboxes usando los DTOs existentes
        CreateMap<Marca, MarcaSimpleDto>()
            .ForMember(dest => dest.Nombre, opt => opt.MapFrom(src => src.Nombre));

        CreateMap<TipoUnidad, TipoUnidadSimpleDto>()
            .ForMember(dest => dest.Nombre, opt => opt.MapFrom(src => src.Nombre));

        CreateMap<Cliente, ClienteSimpleDto>()
            .ForMember(dest => dest.NombreComercial, opt => opt.MapFrom(src => src.NombreComercial));
    }
} 