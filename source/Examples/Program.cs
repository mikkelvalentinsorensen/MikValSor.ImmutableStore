namespace Examples
{
class Program
{
static void Main(string[] args)
{
var program = new Program();
program.ImmutableStoreExample();
}

async void ImmutableStoreExample()
{
var fileStore = new MikValSor.Immutable.FileStorage(".");
var store = new MikValSor.Immutable.Store(fileStore);

MikValSor.Immutable.Persisted<string> presisted = await store.EnsurePresistAsync("StringsAreImmutable");

System.Console.WriteLine($"presisted.Checksum.ToBase64(): {presisted.Checksum.ToBase64()}");

/**
    Output:
    presisted.Checksum.ToBase64(): YZrsXP5n1OVHAFK8YfUZVXpzXFmt7H9sCeaPhMOfdP32LnUvP+HmNFvHVs1CsYR6IzSz3gwi+l285jvV2aWQng==
**/

var checksum = MikValSor.Immutable.Checksum.Get("YZrsXP5n1OVHAFK8YfUZVXpzXFmt7H9sCeaPhMOfdP32LnUvP+HmNFvHVs1CsYR6IzSz3gwi+l285jvV2aWQng==");
MikValSor.Immutable.StoreResult<string> storeResult = await store.TryGetAsync<string>(checksum);

System.Console.WriteLine($"storeResult.GetValue(): {storeResult.GetValue()}");

/**
    Output:
    storeResult.GetValue(): StringsAreImmutable
**/
}
}
}
