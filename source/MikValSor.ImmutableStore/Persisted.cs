// <copyright file="Persisted.cs">
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

    /// <summary>Class that warps value to indicate that it has been presisted already.</summary>
    /// <typeparam name="T">Type of presisted value.</typeparam>
    public class Persisted<T>
    {
        private readonly T value;
        private readonly Checksum checksum;
        private readonly Store store;

        /// <summary>Initializes a new instance of the <see cref="Persisted{T}"/> class.</summary>
        /// <param name="value">Value that was presisted.</param>
        /// <param name="checksum">Checksum of the value that was presisted.</param>
        /// <param name="store">Store that presistd value.</param>
        internal Persisted(T value, Store store, Checksum checksum)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            this.value = value;
            this.store = store ?? throw new ArgumentNullException(nameof(store));
            this.checksum = checksum ?? throw new ArgumentNullException(nameof(checksum));
        }

        /// <summary>Gets the value that was presisted.</summary>
        public T Value => this.value;

        /// <summary>Gets checksum of the presisted value.</summary>
        public Checksum Checksum => this.checksum;

        /// <summary>Gets store that the value was presisted in.</summary>
        public Store Store => this.store;

        #pragma warning disable CA2225
        /// <summary>Allows for automatic cast to T.</summary>
        /// <param name="presisted">Presisted value that will be converted.</param>
        public static implicit operator T(Persisted<T> presisted)
        {
            return presisted.Value;
        }
        #pragma warning restore CA2225

    }
}