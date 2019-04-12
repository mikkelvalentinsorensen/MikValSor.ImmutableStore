namespace MikValSor.Immutable.Test
{
    public class DisabledCacheHelperClass : IStoreResultCache
    {
        public StoreResult<T> AddOrGet<T>(StoreResult<T> value) => value;

        public bool TryGet<T>(Checksum checksum, out StoreResult<T> value)
        {
            value = default;
            return false;
        }
    }
}
