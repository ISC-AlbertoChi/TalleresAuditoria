using AutoMapper;
using BaseCleanArchitecture.Application.DTOs;
using BaseCleanArchitecture.Domain.Entities;

namespace BaseCleanArchitecture.Application.Mappings;

public class ModeloMappingProfile : Profile
{
    public ModeloMappingProfile()
    {
        CreateMap<Modelo, ModeloDto>();
        CreateMap<Modelo, ModeloResponseDto>();
        CreateMap<CreateModeloDto, Modelo>();
        CreateMap<UpdateModeloDto, Modelo>();
    }
} 