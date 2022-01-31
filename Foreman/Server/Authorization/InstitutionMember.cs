using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;

namespace Foreman.Server.Authorization
{
    public class InstitutionMemberRequirement : IAuthorizationRequirement
    {
        public InstitutionMemberRequirement(int institutionId)
        {
            InstitutionId = institutionId;
        }
        public int InstitutionId { get;}
    }

    public class InstitutionMemberRequirementHandler : AuthorizationHandler<InstitutionMemberRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, InstitutionMemberRequirement requirement)
        {
            if(context.User.HasClaim("Institution", requirement.InstitutionId.ToString()))
            {
                context.Succeed(requirement);
            }
            return Task.CompletedTask;
        }
    }
}
