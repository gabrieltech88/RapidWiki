using AutoMapper;
using RapidWiki.Application.CreateUsuario;
using RapidWiki.Application.SignIn;
using RapidWiki.Domain.Entities;

namespace RapidWiki.Application.Common.Mappings;

public class UsuarioProfile : Profile
{
    public UsuarioProfile()
    {        
        CreateMap<Usuario, SignInResult>();
    }
}