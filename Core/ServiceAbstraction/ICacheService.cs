using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceAbstraction
{
    public interface ICacheService
    {
        Task<String?> GetAsync(string cachekey);

        Task SetAsync(string cachekey, Object cachevalue, TimeSpan timeToLive);
    }
}
