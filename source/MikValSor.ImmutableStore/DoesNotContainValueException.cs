// <copyright file="DoesNotContainValueException.cs">
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
    using System.Runtime.Serialization;

    /// <summary>Class for signaling that value did not exist.</summary>
    [Serializable]
    public class DoesNotContainValueException : Exception, ISerializable
    {
        /// <summary>Initializes a new instance of the <see cref="DoesNotContainValueException"/> class.</summary>
        public DoesNotContainValueException()
        {
        }

        /// <summary>Initializes a new instance of the <see cref="DoesNotContainValueException"/> class.</summary>
        /// <param name="checksum">Checksum of the value that did not exist.</param>
        public DoesNotContainValueException(Checksum checksum)
            : base($"Checksum: {checksum.ToBase64()}")
        {
        }

        /// <summary>Initializes a new instance of the <see cref="DoesNotContainValueException"/> class.</summary>
        /// <param name="message">The message that describes the error.</param>
        public DoesNotContainValueException(string message)
            : base(message)
        {
        }

        /// <summary>Initializes a new instance of the <see cref="DoesNotContainValueException"/> class.</summary>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or a null reference (Nothing in Visual Basic) if no inner exception is specified.</param>
        public DoesNotContainValueException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DoesNotContainValueException"/> class.Initializes a new instance of the System.Exception class with serialized data.</summary>
        /// <param name="info">The <see cref="SerializationInfo"/> that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The <see cref="StreamingContext"/> that contains contextual information about the source or destination.</param>
        /// <exception cref="ArgumentNullException">The info parameter is null.</exception>
        /// <exception cref="SerializationException">The class name is null or  <see cref="Exception.HResult"/> is zero (0).</exception>
        protected DoesNotContainValueException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
