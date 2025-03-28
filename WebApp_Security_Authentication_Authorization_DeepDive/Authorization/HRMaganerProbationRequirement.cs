using Microsoft.AspNetCore.Authorization;

namespace AuthenticationAuthorizationDeepDive.Authorization
{
    public class HRMaganerProbationRequirement : IAuthorizationRequirement
    {
        public HRMaganerProbationRequirement(int probationMonths)
        {
            ProbationMonths = probationMonths;
        }

        public int ProbationMonths { get; }
    }

    public class HRMaganerProbationRequirementHandler : AuthorizationHandler<HRMaganerProbationRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, HRMaganerProbationRequirement requirement)
        {
            if(!context.User.HasClaim(x => x.Type == "EmploymentDate"))
            {
                return Task.CompletedTask;
            }
            if(DateTime.TryParse(context.User.FindFirst(x => x.Type == "EmploymentDate")?.Value, out DateTime employmentDate))
            {
                var period = DateTime.Now - employmentDate;
                if (period.Days > 30 * requirement.ProbationMonths)
                {
                    context.Succeed(requirement);
                }
            }
            return Task.CompletedTask;
        }
    }
}
