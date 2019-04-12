using System.Collections.Generic;
using System.Threading.Tasks;

namespace MikValSor.Immutable.Test
{
    public class DisabledStorageHelperClass : IStorage
    {
        public Task EnsurePresistAsync(Checksum checksum, IEnumerable<byte> bytes) => Task.CompletedTask;

        public Task<StorageResult> TryGetAsync(Checksum checksum) => Task.FromResult(StorageResult.DoesNotContain);
    }
}
