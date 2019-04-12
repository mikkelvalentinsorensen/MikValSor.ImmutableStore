// <copyright file="IStoreResultCache.cs">
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
    using System.Runtime.Caching;

    /// <summary>
    ///     Interface for implementation cache for immutable objects.
    /// </summary>
    public interface IStoreResultCache
    {
        /// <summary>Adds instance to cache, returns value now in cache.</summary>
        /// <typeparam name="T">Type of presisted value.</typeparam>
        /// <param name="value">Presisted value that will be added to cache.</param>
        /// <returns>Value stores in cache.</returns>
        StoreResult<T> AddOrGet<T>(StoreResult<T> value);

        /// <summary>Trys to extract immutable object from cache.</summary>
        /// <typeparam name="T">Type of presisted value.</typeparam>
        /// <param name="checksum">Checksum of the value that should be fetched.</param>
        /// <param name="value">Output of the value that was fetched.</param>
        /// <returns>True if value was found.</returns>
        bool TryGet<T>(Checksum checksum, out StoreResult<T> value);
    }
}
