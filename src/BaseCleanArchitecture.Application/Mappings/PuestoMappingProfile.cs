using AutoMapper;
using BaseCleanArchitecture.Application.DTOs;
using BaseCleanArchitecture.Domain.Entities;

namespace BaseCleanArchitecture.Application.Mappings;

public class PuestoMappingProfile : Profile
{
    public PuestoMappingProfile()
    {
        CreateMap<Puesto, PuestoDto>();
        CreateMap<Puesto, PuestoSimpleDto>()
            .ForMember(dest => dest.Nombre, opt => opt.MapFrom(src => src.Nombre));
    }
} 