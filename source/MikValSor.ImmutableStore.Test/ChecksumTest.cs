using NUnit.Framework;
using System;

namespace MikValSor.Immutable.Test
{
    [TestFixture]
    public class ChecksumTest
    {
        [Test]
        public void CalculateChecksum_null()
        {
            //Arrange

            //Act
            try
            {
                Checksum.CalculateChecksum(null);
            }
            catch (ArgumentNullException)
            {
                return;
            }

            //Assert
            Assert.Fail();
        }

        [Test]
        public void CalculateChecksum_array_length_0()
        {
            //Arrange

            //Act
            try
            {
                Checksum.CalculateChecksum(new byte[0]);
            }
            catch (ArgumentException)
            {
                return;
            }

            //Assert
            Assert.Fail();
        }

        [Test]
        public void CalculateChecksum_array_length_1()
        {
            //Arrange

            //Act

            var result = Checksum.CalculateChecksum(new byte[1]);

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("uCRNAomB1pOve0Vq+O+kytY9KC4Z/xSULCRuUNk1HSJwSoAqccNYC2Nw3kzrKTwySoQjNCVX1OXDhDjw42kQ7g==", result.ToString());
        }

        [Test]
        public void Get_null()
        {
            //Arrange

            //Act
            try
            {
                Checksum.Get((string)null);
            }
            catch (ArgumentNullException)
            {
                return;
            }

            //Assert
            Assert.Fail();
        }

        [Test]
        public void Get_array_length_0()
        {
            //Arrange

            //Act
            try
            {
                Checksum.Get(new byte[0]);
            }
            catch (ArgumentException)
            {
                return;
            }

            //Assert
            Assert.Fail();
        }

        [Test]
        public void Get_array_length_63()
        {
            //Arrange

            //Act
            try
            {
                Checksum.Get(new byte[63]);
            }
            catch (ArgumentException)
            {
                return;
            }

            //Assert
            Assert.Fail();
        }

        [Test]
        public void Get_array_length_64()
        {
            //Arrange

            //Act
            Checksum.Get(new byte[64]);

            //Assert
        }

        [Test]
        public void Get_array_length_65()
        {
            //Arrange

            //Act
            try
            {
                Checksum.Get(new byte[65]);
            }
            catch (ArgumentException)
            {
                return;
            }

            //Assert
            Assert.Fail();
        }
    }
}
