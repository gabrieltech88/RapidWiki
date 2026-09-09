using AutoMapper;
using RapidWiki.Api.Usuario.CreateUsuario;
using RapidWiki.Domain.Entities;

namespace RapidWiki.Application.Common.Mappings;

public class UsuarioProfile : Profile
{
    public UsuarioProfile()
    {
        CreateMap<CreateUsuarioRequest, Usuario>()
            .ForMember(
                dest => dest.Id,
                opt => opt.Ignore()
            )
            .ForMember(
                dest => dest.Departamentos,
                opt => opt.Ignore()
            );
    }
}