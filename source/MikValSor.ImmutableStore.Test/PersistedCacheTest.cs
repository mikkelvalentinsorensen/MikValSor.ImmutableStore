using NUnit.Framework;
using System;

namespace MikValSor.Immutable.Test
{
    [TestFixture]
    public class PersistedCacheTest
    {
        [Test]
        public void Construct()
        {
            //Arrange

            //Act
            new StoreResultCache();

            //Assert
        }

        [Test]
        public void TryGet_unknown_checksum()
        {
            //Arrange
            var target = new StoreResultCache();
            var checksum = Checksum.Get(Convert.FromBase64String("YZrsXP5n1OVHAFK8YfUZVXpzXFmt7H9sCeaPhMOfdP32LnUvP+HmNFvHVs1CsYR6IzSz3gwi+l285jvV2aWQng=="));

            //Act
            var resultFound = target.TryGet<string>(checksum, out var resultValue);

            //Assert
            Assert.IsFalse(resultFound);
            Assert.IsNull(resultValue);
        }

        [Test]
        public void TryGet_null()
        {
            //Arrange
            var target = new StoreResultCache();

            //Act
            try
            {
                target.TryGet<string>(null, out var resultValue);
            }
            catch (ArgumentNullException)
            {
                return;
            }

            //Assert
            Assert.Fail();
        }

        [Test]
        public void AddOrGet_string()
        {
            //Arrange
            var store = new Store(new IStorage[] { new DisabledStorageHelperClass() }, cache: new DisabledCacheHelperClass());
            StoreResult<string> presistedValue = store.EnsurePresistAsync("StringsAreImmutable").GetAwaiter().GetResult();
            var target = new StoreResultCache();

            //Act
            var result = target.AddOrGet(presistedValue);

            //Assert
            Assert.AreSame(presistedValue, result);
        }

        [Test]
        public void AddOrGet_null()
        {
            //Arrange
            var target = new StoreResultCache();

            //Act
            try
            {
                target.AddOrGet<string>(null);
            }
            catch (ArgumentNullException)
            {
                return;
            }

            //Assert
            Assert.Fail();
        }

        [Test]
        public void TryGet_string()
        {
            //Arrange
            var store = new Store(new IStorage[] { new DisabledStorageHelperClass() }, cache: new DisabledCacheHelperClass());
            StoreResult<string> presistedValue = store.EnsurePresistAsync("StringsAreImmutable").GetAwaiter().GetResult();
            var target = new StoreResultCache();
            target.AddOrGet(presistedValue);

            //Act
            var result = target.TryGet<string>(presistedValue.GetPresistedValue().Checksum, out var resultValue);

            //Assert
            Assert.IsTrue(result);
            Assert.AreSame(presistedValue, resultValue);
        }
    }
}
