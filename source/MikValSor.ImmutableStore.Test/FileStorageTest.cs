using NUnit.Framework;
using System;

namespace MikValSor.Immutable.Test
{
    [TestFixture]
    public class FileStorageTest
    {
        [Test]
        public void Construct()
        {
            //Arrange
            var path = $"./{Guid.NewGuid().ToString("N")}";

            //Act
            var target = new FileStorage(path);

            //Assert
        }
    }
}
