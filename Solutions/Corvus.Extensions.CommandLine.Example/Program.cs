namespace Corvus.Extensions.CommandLine.Example
{
    using Corvus.Cli;
    using McMaster.Extensions.CommandLineUtils;

    class Program
    {
        /// <summary>
        /// Example program with a command.
        /// </summary>
        /// <remarks>
        /// Try calling with e.g. <c>test -n 10 -o 3 -o 5 -o 7 --greet</c>
        /// </remarks>
        static void Main(string[] args)
        {
            var application = new CommandLineApplication();

            application.AddCommand<TestCommand>();

            application.Execute(args);
        }
    }
}