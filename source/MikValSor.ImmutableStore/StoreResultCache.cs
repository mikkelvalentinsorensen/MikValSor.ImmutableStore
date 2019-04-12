// <copyright file="StoreResultCache.cs">
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

    /// <summary>MemoryCache implementation of <see cref="IStoreResultCache"/>.</summary>
    public sealed class StoreResultCache : IStoreResultCache, IDisposable
    {
        private readonly MemoryCache memoryCache = new MemoryCache(nameof(StoreResultCache));

        /// <summary>Adds instance to cache.</summary>
        /// <typeparam name="T">Type of presisted value.</typeparam>
        /// <param name="value">Values that should be added to cache.</param>
        /// <returns>Returns value currently stored in cache.</returns>
        /// <exception cref="ArgumentNullException">Throws <see cref="ArgumentNullException"/> if value is null.</exception>
        /// <exception cref="ArgumentException">Throws <see cref="ArgumentException"/> if value did not have Contains set to true.</exception>
        public StoreResult<T> AddOrGet<T>(StoreResult<T> value)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            if (!value.Contains)
            {
                throw new ArgumentException("Contains was false.", nameof(value));
            }

            var key = value.GetPresistedValue().Checksum.ToString();
            var cacheValue = (StoreResult<T>)this.memoryCache.AddOrGetExisting(key, value, new CacheItemPolicy());
            if (cacheValue == null)
            {
                return value;
            }

            return cacheValue;
        }

        /// <summary>Trys to extract immutable object from cache.</summary>
        /// <typeparam name="T">Type of presisted value.</typeparam>
        /// <param name="checksum">Checksum of the value that should be fetched.</param>
        /// <param name="value">The value that is returned from cache.</param>
        /// <returns>Returns true if found value in cache.</returns>
        /// <exception cref="ArgumentNullException">Throws System.ArgumentNullException if checksum is null.</exception>
        public bool TryGet<T>(Checksum checksum, out StoreResult<T> value)
        {
            if (checksum == null)
            {
                throw new ArgumentNullException(nameof(checksum));
            }

            var key = checksum.ToString();
            value = (StoreResult<T>)this.memoryCache.Get(key);
            return value != null;
        }

        /// <inheritdoc/>
        public void Dispose() => this.memoryCache.Dispose();
    }
}
