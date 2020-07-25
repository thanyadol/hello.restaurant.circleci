using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using hello.restaurant.api.Models;
using hello.restaurant.api.Providers;
using hello.restaurant.api.Services;

namespace hello.restaurant.api.Services
{
    public class CachedRestaurantService : IRestaurantService
    {

        private readonly RestaurantService _usersService;
        private readonly ICacheProvider _cacheProvider;

        private static readonly SemaphoreSlim GetUsersSemaphore = new SemaphoreSlim(1, 1);

        public CachedRestaurantService(RestaurantService usersService, ICacheProvider cacheProvider)
        {
            _usersService = usersService;
            _cacheProvider = cacheProvider;
        }

        public async Task<IEnumerable<Restaurant>> ListAsync(string keyword)
        {
            return await GetCachedResponse(CacheKeys.Restaurants, () => _usersService.ListAsync(keyword));
        }

        private async Task<IEnumerable<Restaurant>> GetCachedResponse(string cacheKey, Func<Task<IEnumerable<Restaurant>>> func)
        {
            var users = _cacheProvider.GetFromCache<IEnumerable<Restaurant>>(cacheKey);
            if (users != null) return users;
            users = await func();
            _cacheProvider.SetCache(cacheKey, users, DateTimeOffset.Now.AddDays(1));

            return users;
        }

        private async Task<IEnumerable<Restaurant>> GetCachedResponse(string cacheKey, SemaphoreSlim semaphore, Func<Task<IEnumerable<Restaurant>>> func)
        {
            var users = _cacheProvider.GetFromCache<IEnumerable<Restaurant>>(cacheKey);

            if (users != null) return users;
            try
            {
                await semaphore.WaitAsync();
                users = _cacheProvider.GetFromCache<IEnumerable<Restaurant>>(cacheKey); // Recheck to make sure it didn't populate before entering semaphore
                if (users != null)
                {
                    return users;
                }
                users = await func();
                _cacheProvider.SetCache(cacheKey, users, DateTimeOffset.Now.AddDays(1));
            }
            finally
            {
                semaphore.Release();
            }

            return users;
        }
    }
}