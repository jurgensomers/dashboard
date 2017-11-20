using System;
using System.Collections.Generic;
using System.Runtime.Caching;
using System.Threading.Tasks;
using Dashquoia.Api.Models;

namespace Dashquoia.Api.Managers
{
    public class CacheManager
    {
        private const string CacheKeyServices = "GENERICRESULTS";
        private const string CacheKeyTfs = "TFSRESULTS";
        private const string LockKeyServices = "LOCK_SERVICES";
        private const string LockKeyTfs = "LOCK_TFS";

        public void Store(GenericResults results)
        {
            ObjectCache cache = MemoryCache.Default;
            CacheItemPolicy policy = new CacheItemPolicy { AbsoluteExpiration = new DateTimeOffset(DateTime.Now.AddDays(1)) };

            cache.Set(CacheKeyServices, results, policy);
        }

        public void Store(TfsBuildResults results)
        {
            ObjectCache cache = MemoryCache.Default;
            CacheItemPolicy policy = new CacheItemPolicy { AbsoluteExpiration = new DateTimeOffset(DateTime.Now.AddDays(1)) };

            cache.Set(CacheKeyTfs, results, policy);
        }

        public Task GetLockServices()
        {
            ObjectCache cache = MemoryCache.Default;
            return (Task)cache.Get(LockKeyServices);
        }

        public Task GetLockTfs()
        {
            ObjectCache cache = MemoryCache.Default;
            return (Task)cache.Get(LockKeyTfs);
        }

        public void LockServices(Task task)
        {
            ObjectCache cache = MemoryCache.Default;
            CacheItemPolicy policy = new CacheItemPolicy { AbsoluteExpiration = new DateTimeOffset(DateTime.Now.AddDays(1)) };
            cache.Set(LockKeyServices, task, policy);
        }

        public void LockTfs(Task task)
        {
            ObjectCache cache = MemoryCache.Default;
            CacheItemPolicy policy = new CacheItemPolicy { AbsoluteExpiration = new DateTimeOffset(DateTime.Now.AddDays(1)) };
            cache.Set(LockKeyTfs, task, policy);
        }

        public void ReleaseServices()
        {
            ObjectCache cache = MemoryCache.Default;
            cache.Remove(LockKeyServices);
        }

        public void ReleaseTfs()
        {
            ObjectCache cache = MemoryCache.Default;
            cache.Remove(LockKeyTfs);
        }

        public GenericResults GetServices()
        {
            ObjectCache cache = MemoryCache.Default;
            var results = cache.Get(CacheKeyServices) as GenericResults;
            return results;
        }

        public TfsBuildResults GetTfs()
        {
            ObjectCache cache = MemoryCache.Default;
            var results = cache.Get(CacheKeyTfs) as TfsBuildResults;
            return results;
        }

        public void ClearServices()
        {
            ObjectCache cache = MemoryCache.Default;
            cache.Remove(CacheKeyServices); 
        }

        public void ClearTfs()
        {
            ObjectCache cache = MemoryCache.Default;
            cache.Remove(CacheKeyTfs);
        }
    }
}