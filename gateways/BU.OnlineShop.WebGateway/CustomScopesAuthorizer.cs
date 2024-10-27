using Ocelot.Authorization;
using Ocelot.Infrastructure.Claims.Parser;
using Ocelot.Responses;
using System.Security.Claims;

public class CustomScopesAuthorizer : IScopesAuthorizer
{
    private readonly IClaimsParser _claimsParser;
    private readonly string _scope = "scope";
    private readonly string _msScope = "http://schemas.microsoft.com/identity/claims/scope";

    public CustomScopesAuthorizer(IClaimsParser claimsParser)
    {
        _claimsParser = claimsParser;
    }

    public Response<bool> Authorize(ClaimsPrincipal claimsPrincipal, List<string> routeAllowedScopes)
    {
        if (routeAllowedScopes == null || routeAllowedScopes.Count == 0)
        {
            return new OkResponse<bool>(true);
        }

        var values = _claimsParser.GetValuesByClaimType(claimsPrincipal.Claims, _msScope);
        if (!values.IsError && !values.Data.Any())
        {
            values = _claimsParser.GetValuesByClaimType(claimsPrincipal.Claims, _scope);
        }

        if (values.IsError)
        {
            return new ErrorResponse<bool>(values.Errors);
        }

        var userScopes = new List<string>();
        foreach (var item in values.Data)
        {
            userScopes.AddRange(item.Split(' '));
        }

        var matchesScopes = routeAllowedScopes.Intersect(userScopes).ToList();
        if (matchesScopes.Count == 0)
        {
            return new ErrorResponse<bool>(new ScopeNotAuthorizedError($"no one user scope: '{string.Join(",", userScopes)}' match with some allowed scope: '{string.Join(",", routeAllowedScopes)}'"));
        }

        return new OkResponse<bool>(true);
    }
}
