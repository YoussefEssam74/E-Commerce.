using DomainLayer.Contracts;
using ServiceAbstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Service
{
    class CacheService(ICacheRepository cacheRepository) : ICacheService
    {
        public async Task<string?> GetAsync(string cachekey)=> await cacheRepository.GetAsync(cachekey);


        public async Task SetAsync(string cachekey, object cachevalue, TimeSpan timeToLive)
        {
            var value = JsonSerializer.Serialize(cachevalue);
            await cacheRepository.SetAsync(cachekey, value, timeToLive);
        }
    }
}
