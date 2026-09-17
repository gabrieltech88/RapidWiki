using AutoMapper;
using RapidWiki.Application.CreateUsuario;
using RapidWiki.Application.SignIn;
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
        
        CreateMap<Usuario, SignInResult>();
    }
}