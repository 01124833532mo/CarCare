using LinkDev.Talabat.Core.Application.Abstraction;
using System.Security.Claims;

namespace LinkDev.Talabat.APIs.Services
{
	public class LoggedInUserService : ILoggedInUserService
	{
		private readonly IHttpContextAccessor? _httpcontextAccessor;

		public LoggedInUserService(/*IHttpContextAccessor? contextAccessor*/)
		{
			//_httpcontextAccessor = contextAccessor;
			UserId = _httpcontextAccessor?.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier)!;
		}

		public string UserId { get; }
	}
}