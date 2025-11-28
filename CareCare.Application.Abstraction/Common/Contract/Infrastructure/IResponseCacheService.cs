namespace CareCare.Core.Application.Abstraction.Common.Contract.Infrastructure
{
    public interface IResponseCacheService
    {
        Task CaCheResponseAsync(string key, object response, TimeSpan timetolive);

        Task<string?> GetCachedResponseAsync(string key);

    }
}
