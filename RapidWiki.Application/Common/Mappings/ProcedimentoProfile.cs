using AutoMapper;
using RapidWiki.Application.CreateDepartamento;
using RapidWiki.Application.CreateProcedimento;
using RapidWiki.Application.GetAllDepartamentos;
using RapidWiki.Domain.Entities;

namespace RapidWiki.Application.Common.Mappings;

public class ProcedimentoProfile : Profile
{
    public ProcedimentoProfile()
    {
        CreateMap<CreateProcedimentoRequest, Procedimento>()
            .ForMember(
                dest => dest.Id,
                opt => opt.Ignore()
            );
    }
}