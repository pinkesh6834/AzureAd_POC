using Microsoft.AspNetCore.Authorization;

namespace AzureAd_POC
{
    public class HasScopesRequirement : IAuthorizationRequirement
    {
        public string Scope { get; }
        public HasScopesRequirement(string scope)
        {
            Scope = scope ?? throw new ArgumentNullException(nameof(scope));
        }
    }

    public class HasScopesHandler : AuthorizationHandler<HasScopesRequirement>
    {
        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, HasScopesRequirement requirement)
        {

            await Task.CompletedTask;

            // If user does not have the scope claim, get out of here
            if (!context.User.HasClaim(c => c.Type == "scp"))
                return;

            var scopes = context.User.FindAll(c => c.Type == "scp");
            if (scopes.Any(s => s.Value == requirement.Scope))
            {
                context.Succeed(requirement);
            }

        }
    }
}
