// <copyright file="Checksum.cs">
// Copyright (C) 2019 Mikkel Valentin Sorensen
//
// This file is part of MikValSor.ImmutableStore.
//
// MikValSor.ImmutableStore is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 2 of the License, or
// (at your option) any later version.
//
// MikValSor.ImmutableStore is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with MikValSor.ImmutableStore. If not, see http://www.gnu.org/licenses/.
// </copyright>
namespace MikValSor.Immutable
{
    using System;
    using System.Runtime.Caching;
    using System.Runtime.Serialization;
    using System.Security.Cryptography;
    using MikValSor.Encoding;

    /// <summary>Class for checksum of immutable objects.</summary>
    [Serializable]
    public sealed class Checksum : ISerializable
    {
        private static readonly SHA512Managed Sha512 = new SHA512Managed();
        private static readonly MemoryCache Cache = new MemoryCache(nameof(Checksum));

        private readonly Base64 base64;

        private Checksum(Base64 base64)
        {
            this.base64 = base64;
        }

        #pragma warning disable CA1801
        private Checksum(SerializationInfo info, StreamingContext context)
        {
            var bytes = (byte[])info.GetValue("B", typeof(byte[]));
            this.base64 = new Base64(bytes);
        }
        #pragma warning restore CA1801

        /// <summary>Generetes and returns checksum that corosponds to hash value.</summary>
        /// <param name="base64">Base64 string representing checksum of 64 bytes.</param>
        /// <returns>Returns checksum with value of bytes.</returns>
        public static Checksum Get(string base64)
        {
            if (base64 == null)
            {
                throw new ArgumentNullException(nameof(base64));
            }

            var bytes = Base64.Parse(base64).ToByteArray();

            return Get(bytes);
        }

        /// <summary>Generetes and returns checksum that corosponds to hash value.</summary>
        /// <param name="base64">Base64 value representing checksum of 64 bytes.</param>
        /// <returns>Returns checksum with value of bytes.</returns>
        public static Checksum Get(Base64 base64)
        {
            if (base64 == null)
            {
                throw new ArgumentNullException(nameof(base64));
            }

            var bytes = base64.ToByteArray();

            return Get(bytes);
        }

        /// <summary>Generetes and returns checksum that corosponds to hash value.</summary>
        /// <param name="bytes">Array of 64 bytes that reprecent checksum.</param>
        /// <returns>Returns checksum with value of bytes.</returns>
        public static Checksum Get(byte[] bytes)
        {
            if (bytes == null)
            {
                throw new ArgumentNullException(nameof(bytes));
            }

            if (bytes.Length != 64)
            {
                throw new ArgumentException("Length must be 64", nameof(bytes));
            }

            var base64 = new Base64(bytes);

            var checksum = (Checksum)Cache.Get(base64.ToString());

            if (checksum == null)
            {
                checksum = new Checksum(base64);
                var c = (Checksum)Cache.Get(base64.ToString());
                if (c == null)
                {
                    Cache.Add(base64.ToString(), checksum, new CacheItemPolicy());
                }
                else
                {
                    checksum = c;
                }
            }

            return checksum;
        }

        /// <summary>Calculates checksum of a bytearray.</summary>
        /// <param name="bytes">Array of bytes that will have checksum calculated.</param>
        /// <returns>Returns checksum with value of bytes.</returns>
        /// <exception cref="ArgumentNullException">Throws System.ArgumentNullException if bytes was null.</exception>
        /// <exception cref="ArgumentException">Throws System.ArgumentException if bytes was length zero.</exception>
        public static Checksum CalculateChecksum(byte[] bytes)
        {
            if (bytes == null)
            {
                throw new ArgumentNullException(nameof(bytes));
            }

            if (bytes.Length == 0)
            {
                throw new ArgumentException("Length of byte Array was zero.", nameof(bytes));
            }

            return Get(Sha512.ComputeHash(bytes));
        }
        
        /// <inheritdoc/>
        void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("B", this.base64.ToByteArray());
        }

        /// <summary>Returns Base64 encoded of checksum.</summary>
        /// <returns>Base64 representation of checksum.</returns>
        public Base64 ToBase64()
        {
            return this.base64;
        }

        /// <summary>Returns Base32 encoded of checksum.</summary>
        /// <returns>Base63 representation of checksum.</returns>
        public Base32 ToBase32()
        {
            var bytes = this.base64.ToByteArray();
            return new Base32(bytes);
        }

        /// <summary>Returns bytearray of checksum.</summary>
        /// <returns>Bytearray of checksum.</returns>
        public byte[] ToByteArray()
        {
            return this.base64.ToByteArray();
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            return this.base64.ToString();
        }
    }
}
