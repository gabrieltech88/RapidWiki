using MediatR;

namespace RapidWiki.Application.CreateRole;

public class CreateRoleRequest : IRequest<Guid>
{
    required public string RoleName { get; set; } 
}