// <copyright file="CommandLineApplicationExtensions.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Corvus.Cli
{
    using McMaster.Extensions.CommandLineUtils;

    /// <summary>
    /// Extension methods for the <see cref="CommandLineApplication"/>.
    /// </summary>
    public static class CommandLineApplicationExtensions
    {
        /// <summary>
        /// Adds a command to the application.
        /// </summary>
        /// <typeparam name="TCommand">The command to add.</typeparam>
        /// <param name="application">The application to which to add the command.</param>
        /// <returns>The command line application created for the command.</returns>
        public static CommandLineApplication AddCommand<TCommand>(this CommandLineApplication application)
            where TCommand : Command<TCommand>, new()
        {
            if (application.OptionHelp is null)
            {
                application.HelpOption();
            }

            var command = new TCommand();
            return command.Add(application);
        }
    }
}