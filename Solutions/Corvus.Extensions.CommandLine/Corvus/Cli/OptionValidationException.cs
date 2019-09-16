// <copyright file="OptionValidationException.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Corvus.Cli
{
    using System;
    using System.Runtime.Serialization;

    /// <summary>
    /// An exception thrown if option validation fails.
    /// </summary>
    [Serializable]
    internal class OptionValidationException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OptionValidationException"/> class.
        /// </summary>
        public OptionValidationException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OptionValidationException"/> class.
        /// </summary>
        /// <param name="message">The error message.</param>
        public OptionValidationException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OptionValidationException"/> class.
        /// </summary>
        /// <param name="message">The error message.</param>
        /// <param name="innerException">The inner exception that caused this exception to be thrown.</param>
        public OptionValidationException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OptionValidationException"/> class.
        /// </summary>
        /// <param name="info">The serialization information.</param>
        /// <param name="context">The serialization context.</param>
        protected OptionValidationException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}