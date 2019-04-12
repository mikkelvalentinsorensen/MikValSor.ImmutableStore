using NUnit.Framework;
using System;
using System.IO;

namespace MikValSor.Immutable.Test
{
    [TestFixture]
    public class StoreTest
    {
        [Test]
        public void Construct_storages_null()
        {
            //Arrange

            //Act
            try
            {
                new Store((IStorage) null);
            }
            catch (ArgumentNullException)
            {
                return;
            }

            //Assert
            Assert.Fail();
        }

        [Test]
        public void Construct_storeages_empty()
        {
            //Arrange
            var storages = new IStorage[0];

            //Act
            try
            {
                new Store(storages);
            }
            catch (ArgumentException)
            {
                return;
            }

            //Assert
            Assert.Fail();
        }

        [Test]
        public void Construct_storages_contains_null()
        {
            //Arrange
            var storages = new IStorage[1] { null };

            //Act
            try
            {
                new Store(storages);
            }
            catch (ArgumentException)
            {
                return;
            }

            //Assert
            Assert.Fail();
        }

        [Test]
        public void Construct_storages_contains_filestorage()
        {
            //Arrange
            var path = new DirectoryInfo(TestContext.CurrentContext.TestDirectory).CreateSubdirectory(Guid.NewGuid().ToString("N")).FullName;
            var fileStorage = new FileStorage(path);
            var storages = new IStorage[1] { fileStorage };

            //Act
            new Store(storages);

            //Assert
        }

        [Test]
        public void EnsurePresistAsync_string()
        {
            //Arrange
            var path = new DirectoryInfo(TestContext.CurrentContext.TestDirectory).CreateSubdirectory(Guid.NewGuid().ToString("N")).FullName;

            var fileStorage = new FileStorage(path);
            var storages = new IStorage[1] { fileStorage };
            var target = new Store(storages);
            var value = "StringsAreImmutable";

            //Act
            var task = target.EnsurePresistAsync(value);
            var presistedValue = task.GetAwaiter().GetResult();

            //Assert
            Assert.IsNotNull(presistedValue);
            Assert.IsNotNull(presistedValue.Checksum);
            Assert.AreEqual("YZrsXP5n1OVHAFK8YfUZVXpzXFmt7H9sCeaPhMOfdP32LnUvP+HmNFvHVs1CsYR6IzSz3gwi+l285jvV2aWQng==", presistedValue.Checksum.ToString());
            Assert.AreEqual(target, presistedValue.Store);
            Assert.AreEqual(value, presistedValue.Value);
        }


        [Test]
        public void GetPersistedAsync_string()
        {
            //Arrange
            var path = new DirectoryInfo(TestContext.CurrentContext.TestDirectory).CreateSubdirectory(Guid.NewGuid().ToString("N")).FullName;

            var fileStorage = new FileStorage(path);
            var storages = new IStorage[1] { fileStorage };
            var target = new Store(storages);
            var value = "StringsAreImmutable";

            //Act
            var taskEnsure = target.EnsurePresistAsync(value);
            var ensureValue = taskEnsure.GetAwaiter().GetResult();
            var taskGet = target.TryGetAsync<string>(ensureValue.Checksum);
            var presistedValue = taskGet.GetAwaiter().GetResult().GetPresistedValue();

            //Assert
            Assert.IsNotNull(presistedValue);
            Assert.IsNotNull(presistedValue.Checksum);
            Assert.AreEqual("YZrsXP5n1OVHAFK8YfUZVXpzXFmt7H9sCeaPhMOfdP32LnUvP+HmNFvHVs1CsYR6IzSz3gwi+l285jvV2aWQng==", presistedValue.Checksum.ToString());
            Assert.AreEqual(target, presistedValue.Store);
            Assert.AreEqual(value, presistedValue.Value);
        }

        [Test]
        public void GetPersistedAsync_string_disabled_cache()
        {
            //Arrange
            var path = new DirectoryInfo(TestContext.CurrentContext.TestDirectory).CreateSubdirectory(Guid.NewGuid().ToString("N")).FullName;

            var cache = new DisabledCacheHelperClass();
            var fileStorage = new FileStorage(path);
            var storages = new IStorage[1] { fileStorage };
            var target = new Store(storages, cache: cache);
            var value = "StringsAreImmutable";

            //Act
            var taskEnsure = target.EnsurePresistAsync(value);
            var ensureValue = taskEnsure.GetAwaiter().GetResult();
            var taskGet = target.TryGetAsync<string>(ensureValue.Checksum);
            var presistedValue = taskGet.GetAwaiter().GetResult();

            //Assert
            Assert.IsNotNull(presistedValue);
            Assert.IsTrue(presistedValue.Contains);
            Assert.IsNotNull(presistedValue.GetPresistedValue().Checksum);
            Assert.AreEqual("YZrsXP5n1OVHAFK8YfUZVXpzXFmt7H9sCeaPhMOfdP32LnUvP+HmNFvHVs1CsYR6IzSz3gwi+l285jvV2aWQng==", presistedValue.GetPresistedValue().Checksum.ToString());
            Assert.AreEqual(target, presistedValue.GetPresistedValue().Store);
            Assert.AreEqual(value, presistedValue.GetPresistedValue().Value);
        }

        [Test]
        public void GetPersistedAsync_string_does_not_exist()
        {
            //Arrange
            var path = new DirectoryInfo(TestContext.CurrentContext.TestDirectory).CreateSubdirectory(Guid.NewGuid().ToString("N")).FullName;

            var checksum = Checksum.Get(Convert.FromBase64String("YZrsXP5n1OVHAFK8YfUZVXpzXFmt7H9sCeaPhMOfdP32LnUvP+HmNFvHVs1CsYR6IzSz3gwi+l285jvV2aWQng=="));
            var fileStorage = new FileStorage(path);
            var storages = new IStorage[1] { fileStorage };
            var target = new Store(storages);

            //Act
            var taskGet = target.TryGetAsync<string>(checksum);
            try
            {
                taskGet.GetAwaiter().GetResult();
            }
            catch (DoesNotContainValueException)
            {
                return;
            }

            //Assert
            Assert.Fail();
        }
    }
}
