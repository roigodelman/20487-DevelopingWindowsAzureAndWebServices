using System;
using StackExchange.Redis;

namespace ScalableAzureApp
{
    public class GlobalSharedContext
    {
        private readonly IDatabase _cache = RedisProvider.GetCache();
        private const string DISTRIBUTED_KEY_NAMESPACE = nameof(GlobalSharedContext);
        private static readonly string DistributedCounterKey = $"{DISTRIBUTED_KEY_NAMESPACE}.Counter.CacheKey";

        public long GetCounter()
        {
            string counterStr = _cache.StringGet(DistributedCounterKey);
            long counter = counterStr == null ? 0 : Convert.ToInt64(counterStr);
            return counter;
        }

        public long UpdateCounter(long increment)
        {
            long counter = _cache.StringIncrement(DistributedCounterKey, increment);
            return counter;
        }
    }

    public static class RedisProvider
    {
        private static readonly string ConnectionString = System.Configuration.ConfigurationManager.AppSettings["ReidsConnectionString"];
        private static readonly Lazy<ConnectionMultiplexer> LazyConnection = new Lazy<ConnectionMultiplexer>(() => ConnectionMultiplexer.Connect(ConnectionString));


        private static ConnectionMultiplexer Connection => LazyConnection.Value;

        public static IDatabase GetCache()
        {
            IDatabase cache = Connection.GetDatabase();
            return cache;
        }
    }
}