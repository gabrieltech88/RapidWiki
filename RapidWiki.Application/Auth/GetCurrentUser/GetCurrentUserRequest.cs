using MediatR;

namespace RapidWiki.Application.GetCurrentUser;

public record GetCurrentUserRequest
    : IRequest<GetCurrentUserResult>;