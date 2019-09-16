// <copyright file="Command.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Corvus.Cli
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Corvus.Cli.Internal;
    using McMaster.Extensions.CommandLineUtils;

    /// <summary>
    ///     The command.
    /// </summary>
    /// <typeparam name="T">The type of the command. Using the curiously recursive type pattern.</typeparam>
    /// <remarks>
    /// You will typically create a command which encapsulates the options and parameters, and then add it to the root of your CLI application.
    /// </remarks>
    /// <example>
    ///     <code>
    /// <![CDATA[
    ///     using Corvus.Cli;
    ///     using McMaster.Extensions.CommandLineUtils;
    ///
    ///     class Program
    ///     {
    ///         static void Main(string[] args)
    ///         {
    ///             var application = new CommandLineApplication();
    ///
    ///             application.AddCommand<TestCommand>();
    ///
    ///             application.Execute(args);
    ///         }
    ///     }
    ///
    ///     public class TestCommand : Command<TestCommand>
    ///     {
    ///         [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0044:Add readonly modifier", Justification = "This is set through the command option binding.")]
    ///         private int number;
    ///
    ///         public TestCommand() : base("test")
    ///         {
    ///         }
    ///
    ///         public override void AddOptions(CommandLineApplication application)
    ///         {
    ///             this.AddSingleOption(
    ///                 application,
    ///                 "-n|--number <value>",
    ///                 "The number to count",
    ///                 () => this.number,
    ///                 number =>
    ///                 {
    ///                     return number >= 1 && number <= 10 ? null : "The number must be between 1 and 10";
    ///                 });
    ///         }
    ///
    ///         public override Task<int> ExecuteAsync()
    ///         {
    ///             Console.WriteLine($"Testing {string.Join(",", Enumerable.Range(1, this.number).ToArray())}");
    ///             return Task.FromResult(0);
    ///         }
    ///     }
    /// ]]>
    ///     </code>
    /// </example>
    public abstract class Command<T>
            where T : Command<T>
    {
        private readonly List<IOptionBinding> bindings = new List<IOptionBinding>();

        /// <summary>
        /// Initializes a new instance of the <see cref="Command{T}"/> class.
        /// </summary>
        /// <param name="name">
        /// The name of the command (this is what will be invoked from the command line).
        /// </param>
        protected Command(string name)
        {
            this.Name = name;
        }

        /// <summary>
        /// Gets the name of the command.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Add options and parameters for the command to the given syntax.
        /// </summary>
        /// <param name="command">
        /// The command to which to add options and parameters.
        /// </param>
        public virtual void AddOptions(CommandLineApplication command)
        {
        }

        /// <summary>
        /// Execute the command.
        /// </summary>
        /// <param name="token">The cancellation token.</param>
        /// <returns>A task which, when complete, provides the result.</returns>
        public abstract Task<int> ExecuteAsync(CancellationToken token);

        /// <summary>
        /// Adds the command to the application.
        /// </summary>
        /// <param name="application">The application to which to add the command.</param>
        /// <returns>The application created for the command.</returns>
        internal CommandLineApplication Add(CommandLineApplication application)
        {
            return application.Command(this.Name, command =>
            {
                command.OnExecuteAsync(ct =>
                {
                    this.bindings.ForEach(b => b.ApplyBinding());
                    return this.ExecuteAsync(ct);
                });
                this.AddOptions(command);
            });
        }

        /// <summary>
        /// Stash a binding so it doesn't get collected.
        /// </summary>
        /// <param name="optionBinding">The binding to add.</param>
        internal void AddBinding(IOptionBinding optionBinding)
        {
            if (optionBinding is null)
            {
                throw new System.ArgumentNullException(nameof(optionBinding));
            }

            this.bindings.Add(optionBinding);
        }
    }
}