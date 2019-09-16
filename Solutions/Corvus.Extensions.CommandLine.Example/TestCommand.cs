namespace Corvus.Extensions.CommandLine.Example
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Corvus.Cli;
    using McMaster.Extensions.CommandLineUtils;

#pragma warning disable CS0649, IDE0044

    public class TestCommand : Command<TestCommand>
    {
        private int number;
        private List<int> numbersToOmit;
        private bool greet;

        public TestCommand() : base("test", "Perform a test count.")
        {
        }

        public override void AddOptions(CommandLineApplication application)
        {
            this.AddSingleOption(
                application,
                "-n|--number <value>",
                "The number to count",
                () => this.number,
                number =>
                {
                    return number >= 1 && number <= 10 ? null : "The number must be between 1 and 10";
                });

            this.AddMultipleOption(
                application,
                "-o|--omit <value>",
                "Omit the number from the count (allows multiple)",
                () => this.numbersToOmit);

            this.AddBooleanOption(
                application,
                "-g|--greet",
                "Add a polite greeting",
                () => this.greet);
        }

        public override Task<int> ExecuteAsync(CancellationToken token)
        {
            if (this.greet)
            {
                Console.WriteLine("Hello! Delightful to see you.");
            }

            Console.WriteLine($"Testing {string.Join(",", Enumerable.Range(1, this.number).Where(i => !this.numbersToOmit.Contains(i)).ToArray())}");
            return Task.FromResult(0);
        }
    }
}

#pragma warning restore CS0649, IDE0044
