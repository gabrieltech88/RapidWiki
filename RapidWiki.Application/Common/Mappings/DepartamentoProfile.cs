using AutoMapper;
using RapidWiki.Application.CreateDepartamento;
using RapidWiki.Application.GetAllDepartamentos;
using RapidWiki.Domain.Entities;

namespace RapidWiki.Application.Common.Mappings;

public class DepartamentoProfile : Profile
{
    public DepartamentoProfile()
    {
        CreateMap<CreateDepartamentoRequest, Departamento>()
            .ForMember(
                dest => dest.Id,
                opt => opt.Ignore()
            );
        
        CreateMap<Departamento, CreateDepartamentoResult>();
        CreateMap<Departamento, GetAllDepartamentosResult>();
        
    }
}