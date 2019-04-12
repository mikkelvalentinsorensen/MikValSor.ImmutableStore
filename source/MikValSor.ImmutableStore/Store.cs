// <copyright file="Store.cs">
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
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Runtime.Serialization;
    using System.Runtime.Serialization.Formatters.Binary;
    using System.Threading.Tasks;
    using MikValSor.Runtime.Serialization;

    /// <summary>Class that handels persistance and read of stored immutable values.</summary>
    public class Store
    {
        private readonly ImmutableValidator immutableValidator;
        private readonly SerializableValidator serializableValidator;
        private readonly IStoreResultCache cache;
        private readonly IFormatter formatter;
        private readonly IStorage[] storages;

        /// <summary>Initializes a new instance of the <see cref="Store"/> class.</summary>
        /// <param name="storage">Storage what values will be presisted to.</param>
        /// <param name="immutableValidator"><see cref="ImmutableValidator"/> used to validate immutablity of values.</param>
        /// <param name="serializableValidator"><see cref="SerializableValidator"/> used to validate serializability of values.</param>
        /// <param name="cache"><see cref="IStoreResultCache"/> used by store.</param>
        /// <param name="formatter"><see cref="IFormatProvider"/> used to serialize and deserialize values.</param>
        /// <exception cref="ArgumentNullException">Throws <see cref="ArgumentNullException"/> is storage is null.</exception>
        public Store(
            IStorage storage,
            ImmutableValidator immutableValidator = null,
            SerializableValidator serializableValidator = null,
            IStoreResultCache cache = null,
            IFormatter formatter = null)
        {
            if (storage == null)
            {
                throw new ArgumentNullException(nameof(storage));
            }

            this.storages = new[] { storage };

            if (immutableValidator == null)
            {
                immutableValidator = new ImmutableValidator();
            }

            this.immutableValidator = immutableValidator;

            if (serializableValidator == null)
            {
                serializableValidator = new SerializableValidator();
            }

            this.serializableValidator = serializableValidator;

            if (cache == null)
            {
                cache = new StoreResultCache();
            }

            this.cache = cache;

            if (formatter == null)
            {
                formatter = new BinaryFormatter();
            }

            this.formatter = formatter;
        }

        /// <summary>Initializes a new instance of the <see cref="Store"/> class.</summary>
        /// <param name="storages">Storages what values will be presisted to.</param>
        /// <param name="immutableValidator"><see cref="ImmutableValidator"/> used to validate immutablity of values.</param>
        /// <param name="serializableValidator"><see cref="SerializableValidator"/> used to validate serializability of values.</param>
        /// <param name="cache"><see cref="IStoreResultCache"/> used by store.</param>
        /// <param name="formatter"><see cref="IFormatProvider"/> used to serialize and deserialize values.</param>
        /// <exception cref="ArgumentNullException">Throws <see cref="ArgumentNullException"/> is storages is null.</exception>
        /// <exception cref="ArgumentException">Throws <see cref="ArgumentException"/> if storage is empty or has null values.</exception>
        public Store(
            IEnumerable<IStorage> storages,
            ImmutableValidator immutableValidator = null,
            SerializableValidator serializableValidator = null,
            IStoreResultCache cache = null,
            IFormatter formatter = null)
        {
            if (storages == null)
            {
                throw new ArgumentNullException(nameof(storages));
            }

            this.storages = storages.ToArray();
            if (this.storages.Length == 0)
            {
                throw new ArgumentException("Did not contain any values.", nameof(storages));
            }

            if (this.storages.Any(s => s == null))
            {
                throw new ArgumentException("Contained a null value.", nameof(storages));
            }

            if (immutableValidator == null)
            {
                immutableValidator = new ImmutableValidator();
            }

            this.immutableValidator = immutableValidator;

            if (serializableValidator == null)
            {
                serializableValidator = new SerializableValidator();
            }

            this.serializableValidator = serializableValidator;

            if (cache == null)
            {
                cache = new StoreResultCache();
            }

            this.cache = cache;

            if (formatter == null)
            {
                formatter = new BinaryFormatter();
            }

            this.formatter = formatter;
        }

        /// <summary>Gets <see cref="ImmutableValidator"/> used bu Store.</summary>
        internal ImmutableValidator ImmutableValidator => this.immutableValidator;

        /// <summary>Gets <see cref="SerializableValidator"/> used bu Store.</summary>
        internal SerializableValidator SerializableValidator => this.serializableValidator;

        /// <summary>Retrive immutable objects.</summary>
        /// <typeparam name="T">Type of presisted value.</typeparam>
        /// <param name="checksum">Checksum of value to get.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public async Task<StoreResult<T>> TryGetAsync<T>(Checksum checksum)
        {
            if (checksum == null)
            {
                throw new ArgumentNullException(nameof(checksum));
            }

            if (this.cache.TryGet<T>(checksum, out var cacheValue))
            {
                return cacheValue;
            }

            foreach (var store in this.storages)
            {
                var result = await store.TryGetAsync(checksum).ConfigureAwait(false);
                if (result.Contains)
                {
                    var value = (T)this.formatter.Deserialize(result.GetStream());
                    StoreResult<T> storeResult = new Persisted<T>(value, this, checksum);
                    return this.cache.AddOrGet(storeResult);
                }
            }

            throw new DoesNotContainValueException(checksum);
        }

        /// <summary>Presists immutable objects, and returns link to object.</summary>
        /// <typeparam name="T">Type of presisted value.</typeparam>
        /// <param name="value">Value to ensure is presisted in store.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public async Task<Persisted<T>> EnsurePresistAsync<T>(T value)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            this.ImmutableValidator.EnsureImmutable(value);
            this.SerializableValidator.EnsureSerializable(value);

            var memoryStream = new MemoryStream();
            this.formatter.Serialize(memoryStream, value);
            var bytes = memoryStream.ToArray();
            var checksum = Checksum.CalculateChecksum(bytes);
            if (this.cache.TryGet<T>(checksum, out var cacheValue))
            {
                return cacheValue.GetPresistedValue();
            }

            var persistTasks = new List<Task>(this.storages.Length);
            foreach (var store in this.storages)
            {
                persistTasks.Add(store.EnsurePresistAsync(checksum, bytes));
            }

            StoreResult<T> persistedValue = new Persisted<T>(value, this, checksum);
            foreach (var task in persistTasks)
            {
                await task.ConfigureAwait(false);
            }

            var cachedValue = this.cache.AddOrGet(persistedValue);
            return cachedValue.GetPresistedValue();
        }
    }
}
