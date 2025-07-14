using AutoMapper;
using BaseCleanArchitecture.Application.DTOs;
using BaseCleanArchitecture.Domain.Entities;

namespace BaseCleanArchitecture.Application.Mappings;

public class TipoCombustibleMappingProfile : Profile
{
    public TipoCombustibleMappingProfile()
    {
        CreateMap<TipoCombustible, TipoCombustibleDto>();
        CreateMap<TipoCombustible, TipoCombustibleSimpleDto>();
    }
} 