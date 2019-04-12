// <copyright file="StorageResult.cs">
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
    using System.IO;

    /// <summary>
    ///     Class for signaling if storage contains data and return it.
    /// </summary>
    public class StorageResult
    {
        private readonly MemoryStream stream;
        private readonly bool contains;

        private StorageResult(bool contains, MemoryStream stream)
        {
            this.contains = contains;
            this.stream = stream;
        }

        /// <summary>Gets value for signaling does not exist in storage.</summary>
        public static StorageResult DoesNotContain { get; } = new StorageResult(false, null);

        /// <summary>Gets a value indicating whether data was found.</summary>
        public bool Contains => this.contains;

        /// <summary>Methode creates contains result with data.</summary>
        /// <param name="stream">a <see cref="MemoryStream"/> containing found result</param>
        /// <returns>Returns creates value.</returns>
        public static StorageResult ContainsResult(MemoryStream stream)
        {
            if (stream == null)
            {
                throw new ArgumentNullException(nameof(stream));
            }

            return new StorageResult(true, stream);
        }

        /// <summary>Gets stream of data read from the storage.</summary>
        /// <returns>A <see cref="MemoryStream"/> containing data from storage.</returns>
        /// <exception cref="DoesNotContainValueException">Throws <see cref="DoesNotContainValueException"/> if value was not found.</exception>
        public MemoryStream GetStream()
        {
            if (this.Contains)
            {
                return this.stream;
            }

            throw new DoesNotContainValueException();
        }
    }
}
