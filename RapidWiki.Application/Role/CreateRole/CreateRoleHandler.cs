using MediatR;
using RapidWiki.Application.Interfaces;

namespace RapidWiki.Application.CreateRole;

public class CreateRoleHandler : IRequestHandler<CreateRoleRequest, Guid>
{
    private readonly IRoleService _roleService;

    public CreateRoleHandler(IRoleService roleService)
    {
        _roleService = roleService;
    }

    public async Task<Guid> Handle(CreateRoleRequest request, CancellationToken cancellationToken)
    {
        if (request is null) throw new ArgumentNullException(nameof(request));

        var roleId = await _roleService.CreateAsync(request.RoleName);

        if (roleId == Guid.Empty) throw new InvalidOperationException("Failed to create role.");

        return roleId;
    }
}