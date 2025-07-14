using AutoMapper;
using BaseCleanArchitecture.Application.DTOs;
using BaseCleanArchitecture.Domain.Entities;

namespace BaseCleanArchitecture.Application.Mappings;

public class RolMappingProfile : Profile
{
    public RolMappingProfile()
    {
        CreateMap<Rol, RolDto>();
        CreateMap<Rol, RolSimpleDto>()
            .ForMember(dest => dest.Nombre, opt => opt.MapFrom(src => src.Nombre));
    }
} 