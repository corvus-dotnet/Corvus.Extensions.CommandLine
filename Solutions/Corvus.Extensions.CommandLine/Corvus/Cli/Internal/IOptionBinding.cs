// <copyright file="IOptionBinding.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Corvus.Cli.Internal
{
    /// <summary>
    /// The option binding interface.
    /// </summary>
    internal interface IOptionBinding
    {
        /// <summary>
        /// Apply the binding.
        /// </summary>
        void ApplyBinding();
    }
}