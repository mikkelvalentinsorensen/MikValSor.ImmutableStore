// <copyright file="StoreResult.cs">
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

    /// <summary>Class for signaling if store contains data and return it.</summary>
    /// <typeparam name="T">Type of presisted value.</typeparam>
    public class StoreResult<T>
    {
        private readonly Persisted<T> presistedValue;
        private readonly bool contains;

        /// <summary>Initializes a new instance of the <see cref="StoreResult{T}"/> class.</summary>
        /// <param name="contains">True if contains value.</param>
        /// <param name="presistedValue">Valus that was presisted.</param>
        /// <exception cref="ArgumentNullException">Throws <see cref="ArgumentNullException"/> if contains is true and value is null.</exception>
        public StoreResult(bool contains, Persisted<T> presistedValue)
        {
            if (contains && presistedValue == null)
            {
                throw new ArgumentNullException(nameof(presistedValue));
            }

            this.contains = contains;
            this.presistedValue = presistedValue;
        }

        /// <summary>Gets a value indicating whether data was found.</summary>
        public bool Contains => this.contains;

        #pragma warning disable CA2225
        /// <summary>Allows for automatic cast to StoreResult of T.</summary>
        /// <param name="presisted">Presisted value that will be converted.</param>
        public static implicit operator StoreResult<T>(Persisted<T> presisted)
        {
            if (presisted == null)
            {
                return null;
            }

            return new StoreResult<T>(true, presisted);
        }
        #pragma warning restore CA2225

        /// <summary>Gets value of data read from the store.</summary>
        /// <returns>Value that was presisted.</returns>
        /// <exception cref="DoesNotContainValueException">Throws a <see cref="DoesNotContainValueException"/> if value was not found.</exception>
        public T GetValue()
        {
            if (this.contains)
            {
                return this.presistedValue;
            }

            throw new DoesNotContainValueException();
        }

        /// <summary>Gets value of data read from the store.</summary>
        /// <returns>Value that was presisted.</returns>
        /// <exception cref="DoesNotContainValueException">Throws a <see cref="DoesNotContainValueException"/> if value was not found.</exception>
        public Persisted<T> GetPresistedValue()
        {
            if (this.contains)
            {
                return this.presistedValue;
            }

            throw new DoesNotContainValueException();
        }
    }
}
