using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Contracts
{
    public interface ICacheRepository
    {
        //get
        Task<String?>GetAsync(string Cachekey);

        //set
        Task SetAsync(string Cachekey, string Cachevalue, TimeSpan TimeToLive);
    }
}
