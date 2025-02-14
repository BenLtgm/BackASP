﻿using Microsoft.AspNetCore.Authorization;

namespace FilmStore.Auth;

public class HasScopeHandler : AuthorizationHandler<HasScopeRequirement>
{
    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context, 
        HasScopeRequirement requirement
    ) {
        if (!context.User.HasClaim(c => c.Type == "permissions" && c.Issuer == requirement.Issuer)) {
            return Task.CompletedTask;
        }
        
        var scopes = context.User.FindAll(c => c.Type == "permissions" && c.Issuer == requirement.Issuer);
        
        if (scopes.Any(s => s.Value == requirement.Scope))
            context.Succeed(requirement);

        return Task.CompletedTask;
    }
}