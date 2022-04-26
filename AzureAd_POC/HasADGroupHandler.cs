using Microsoft.AspNetCore.Authorization;

namespace AzureAd_POC
{
    public class HasADGroupRequirement : IAuthorizationRequirement
    {
        public string[] Group { get; }
        public HasADGroupRequirement(string[] group)
        {
            Group = group ?? throw new ArgumentNullException(nameof(group));
        }
    }

    public class HasADGroupHandler : AuthorizationHandler<HasADGroupRequirement>
    {
        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, HasADGroupRequirement requirement)
        {
            await Task.CompletedTask;

            // If user does not have the group claim, get out of here
            if (!context.User.HasClaim(c => c.Type == "groups"))
            {
                //check for scope if group claim not there don't fail user may have scope
                return;

            }
            var groups = context.User.FindAll(c => c.Type == "groups");

            string[] groupclaims = requirement.Group;


            if (groupclaims.Any())
            {

                foreach (var group in groups)
                {
                    if (groupclaims.Any(s => s.Contains(group.Value)))
                    {
                        context.Succeed(requirement);
                        return;
                    }
                }

            }

        }
    }
}
