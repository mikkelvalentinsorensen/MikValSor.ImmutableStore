// <copyright file="IStorage.cs">
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
    using System.Collections.Generic;
    using System.Threading.Tasks;

    /// <summary>Interface for implementations of storage media for immutable objects.</summary>
    public interface IStorage
    {
        /// <summary>Trys to fetch immutable value from store.</summary>
        /// <param name="checksum">Checksum of the byte array that should be fetched.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task<StorageResult> TryGetAsync(Checksum checksum);

        /// <summary>Ensures that immutable value is persisted in store.</summary>
        /// <param name="checksum">Checksum of the byte array that should be presisted.</param>
        /// <param name="bytes">Bytes that will be presisted.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task EnsurePresistAsync(Checksum checksum, IEnumerable<byte> bytes);
    }
}
