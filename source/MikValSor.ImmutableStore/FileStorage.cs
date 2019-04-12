// <copyright file="FileStorage.cs">
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
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using MikValSor.Encoding;

    /// <summary>Implementation of IStorage for storing immutable objects on disk.</summary>
    public class FileStorage : IStorage
    {
        private const string FileExtension = ".dat";
        private readonly ConcurrentDictionary<string, object> knownExist = new ConcurrentDictionary<string, object>();
        private readonly string folderPath;

        /// <summary>Initializes a new instance of the <see cref="FileStorage"/> class.</summary>
        /// <param name="folderPath">Path for the folder that contain presisted data files.</param>
        public FileStorage(string folderPath)
        {
            this.folderPath = folderPath ?? throw new ArgumentNullException(nameof(folderPath));
        }

        /// <summary>Trys to fetch immutable value from store.</summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        /// <param name="checksum">Checksum of the value that should be fetched.</param>
        public async Task<StorageResult> TryGetAsync(Checksum checksum)
        {
            try
            {
                byte[] result;
                using (FileStream stream = File.Open(this.GetFilename(checksum), FileMode.Open))
                {
                    this.MarkAsExists(checksum);

                    result = new byte[stream.Length];
                    await stream.ReadAsync(result, 0, (int)stream.Length).ConfigureAwait(false);
                }

                return StorageResult.ContainsResult(new MemoryStream(result));
            }
            catch (FileNotFoundException)
            {
                return StorageResult.DoesNotContain;
            }
        }

        /// <summary>Ensures that immutable value is persisted in store.</summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        /// <param name="checksum">Checksum of the byte array that should be presisted.</param>
        /// <param name="bytes">Bytes to be presisted.</param>
        public async Task EnsurePresistAsync(Checksum checksum, IEnumerable<byte> bytes)
        {
            if (checksum == null)
            {
                throw new ArgumentNullException(nameof(checksum));
            }

            if (bytes == null)
            {
                throw new ArgumentNullException(nameof(bytes));
            }

            if (this.Contains(checksum))
            {
                return;
            }

            try
            {
                using (FileStream stream = File.Open(this.GetFilename(checksum), FileMode.CreateNew))
                {
                    var array = bytes as byte[];
                    if (array == null)
                    {
                        bytes.ToArray();
                    }

                    await stream.WriteAsync(array, 0, array.Length).ConfigureAwait(false);
                }

                this.MarkAsExists(checksum);
            }
            catch (IOException)
            {
                this.MarkAsExists(checksum);
            }
        }

        private string GetFilename(Checksum checksum)
        {
            var bytes = Convert.FromBase64String(checksum.ToBase64());
            var base32 = Base32Encoder.Encode(bytes);
            return $"{this.folderPath}/{base32}{FileExtension}";
        }

        private bool Contains(Checksum checksum) => this.knownExist.ContainsKey(checksum.ToBase64());

        private void MarkAsExists(Checksum checksum) => this.knownExist.AddOrUpdate(checksum.ToBase64(), new object(), (k, v) => v);
    }
}
