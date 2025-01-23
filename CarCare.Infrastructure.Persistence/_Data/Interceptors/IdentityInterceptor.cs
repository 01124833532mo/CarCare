using LinkDev.Talabat.Core.Application.Abstraction;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace CarCare.Infrastructure.Persistence._Data.Interceptors
{
    public class IdentityInterceptor : SaveChangesInterceptor
    {
        private readonly ILoggedInUserService _loggedInUserService;

        public IdentityInterceptor(ILoggedInUserService loggedInUserService)
        {
            _loggedInUserService = loggedInUserService;
        }

        public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
        {
            return base.SavingChangesAsync(eventData, result, cancellationToken);
        }

        public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
        {
            return base.SavingChanges(eventData, result);
        }
    }
}
