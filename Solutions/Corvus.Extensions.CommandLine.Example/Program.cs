﻿namespace Corvus.Extensions.CommandLine.Example
{
    using Corvus.Cli;
    using Microsoft.Extensions.CommandLineUtils;

    class Program
    {
        static void Main(string[] args)
        {
            var application = new CommandLineApplication();

            application.AddCommand<TestCommand>();

            application.Execute(args);
        }
    }
}