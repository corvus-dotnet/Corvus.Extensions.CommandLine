// <copyright file="CommandExtensions.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Corvus.Cli
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using Corvus.Cli.Internal;
    using Corvus.Extensions;
    using Microsoft.Extensions.CommandLineUtils;

    /// <summary>
    /// Extension methods for the <see cref="Command{T}"/> implementations.
    /// </summary>
    public static class CommandExtensions
    {
        /// <summary>
        /// Binds a boolean option to a property in a command.
        /// </summary>
        /// <typeparam name="TCommand">The type of the command.</typeparam>
        /// <param name="command">The command to which to bind.</param>
        /// <param name="application">The command line application to which to bind the option.</param>
        /// <param name="template">The template for the option.</param>
        /// <param name="description">The description of the option.</param>
        /// <param name="setter">The setter for the option.</param>
        public static void AddBooleanOption<TCommand>(this TCommand command, CommandLineApplication application, string template, string description, Expression<Func<bool>> setter)
            where TCommand : Command<TCommand>
        {
            CommandOption option = application.Option(template, description, CommandOptionType.NoValue);
            command.AddBinding(new OptionBinding<TCommand, bool>(command, option, setter));
        }

        /// <summary>
        /// Binds a single value option to a property in a command.
        /// </summary>
        /// <typeparam name="TCommand">The type of the command.</typeparam>
        /// <typeparam name="T">The type of the property.</typeparam>
        /// <param name="command">The command to which to bind.</param>
        /// <param name="application">The command line application to which to bind the option.</param>
        /// <param name="template">The template for the option.</param>
        /// <param name="description">The description of the option.</param>
        /// <param name="setter">The setter for the option.</param>
        /// <param name="validator">The validator for the option.</param>
        public static void AddSingleOption<TCommand, T>(this TCommand command, CommandLineApplication application, string template, string description, Expression<Func<T>> setter, Func<T, string> validator = null)
            where TCommand : Command<TCommand>
        {
            Func<string, T> converter = GetSingleConverter<T>();
            CommandOption option = application.Option(template, description, CommandOptionType.SingleValue);
            command.AddBinding(new OptionBinding<TCommand, T>(command, option, setter, validator, converter));
        }

        /// <summary>
        /// Binds a single value option to a property in a command.
        /// </summary>
        /// <typeparam name="TCommand">The type of the command.</typeparam>
        /// <typeparam name="T">The type of the property.</typeparam>
        /// <param name="command">The command to which to bind.</param>
        /// <param name="application">The command line application to which to bind the option.</param>
        /// <param name="template">The template for the option.</param>
        /// <param name="description">The description of the option.</param>
        /// <param name="setter">The setter for the option.</param>
        /// <param name="validator">The validator for the option.</param>
        public static void AddMultipleOption<TCommand, T>(this TCommand command, CommandLineApplication application, string template, string description, Expression<Func<T>> setter, Func<T, string> validator = null)
            where TCommand : Command<TCommand>
        {
            Func<List<string>, T> converter = GetMultipleConverter<T>();
            CommandOption option = application.Option(template, description, CommandOptionType.MultipleValue);
            command.AddBinding(new OptionBinding<TCommand, T>(command, option, setter, validator, converter));
        }

        private static Func<List<string>, T> GetMultipleConverter<T>()
        {
            Type typeT = typeof(T);

            if (typeT == typeof(List<string>))
            {
                return null;
            }

            if (typeT == typeof(List<bool>))
            {
                return f => CastTo<T>.From(f.Select(s => bool.Parse(s)).ToList());
            }

            if (typeT == typeof(List<int>))
            {
                return f => CastTo<T>.From(f.Select(s => int.Parse(s)).ToList());
            }

            if (typeT == typeof(List<double>))
            {
                return f => CastTo<T>.From(f.Select(s => double.Parse(s)).ToList());
            }

            if (typeT == typeof(List<float>))
            {
                return f => CastTo<T>.From(f.Select(s => float.Parse(s)).ToList());
            }

            if (typeT == typeof(List<Guid>))
            {
                return f => CastTo<T>.From(f.Select(s => Guid.Parse(s)).ToList());
            }

            if (typeT == typeof(List<DateTime>))
            {
                return f => CastTo<T>.From(f.Select(s => DateTime.Parse(s)).ToList());
            }

            if (typeT == typeof(List<DateTimeOffset>))
            {
                return f => CastTo<T>.From(f.Select(s => DateTimeOffset.Parse(s)).ToList());
            }

            if (typeT == typeof(List<TimeSpan>))
            {
                return f => CastTo<T>.From(f.Select(s => TimeSpan.Parse(s)).ToList());
            }

            if (typeT == typeof(List<bool?>))
            {
                return f => CastTo<T>.From(f.Select(s => string.IsNullOrEmpty(s) ? default : bool.Parse(s)).ToList());
            }

            if (typeT == typeof(List<int?>))
            {
                return f => CastTo<T>.From(f.Select(s => string.IsNullOrEmpty(s) ? default : int.Parse(s)).ToList());
            }

            if (typeT == typeof(List<double?>))
            {
                return f => CastTo<T>.From(f.Select(s => string.IsNullOrEmpty(s) ? default : double.Parse(s)).ToList());
            }

            if (typeT == typeof(List<float?>))
            {
                return f => CastTo<T>.From(f.Select(s => string.IsNullOrEmpty(s) ? default : float.Parse(s)).ToList());
            }

            if (typeT == typeof(List<Guid?>))
            {
                return f => CastTo<T>.From(f.Select(s => string.IsNullOrEmpty(s) ? default : Guid.Parse(s)).ToList());
            }

            if (typeT == typeof(List<DateTime?>))
            {
                return f => CastTo<T>.From(f.Select(s => string.IsNullOrEmpty(s) ? default : DateTime.Parse(s)).ToList());
            }

            if (typeT == typeof(List<DateTimeOffset?>))
            {
                return f => CastTo<T>.From(f.Select(s => string.IsNullOrEmpty(s) ? default : DateTimeOffset.Parse(s)).ToList());
            }

            if (typeT == typeof(List<TimeSpan?>))
            {
                return f => CastTo<T>.From(f.Select(s => string.IsNullOrEmpty(s) ? default : TimeSpan.Parse(s)).ToList());
            }

            throw new FormatException($"Unable to create converter for type {typeT}");
        }

        private static Func<string, T> GetSingleConverter<T>()
        {
            Type typeT = typeof(T);

            if (typeT == typeof(string))
            {
                return null;
            }

            if (typeT == typeof(bool))
            {
                return f => CastTo<T>.From(bool.Parse(f));
            }

            if (typeT == typeof(int))
            {
                return f => CastTo<T>.From(int.Parse(f));
            }

            if (typeT == typeof(double))
            {
                return f => CastTo<T>.From(double.Parse(f));
            }

            if (typeT == typeof(float))
            {
                return f => CastTo<T>.From(float.Parse(f));
            }

            if (typeT == typeof(Guid))
            {
                return f => CastTo<T>.From(Guid.Parse(f));
            }

            if (typeT == typeof(DateTime))
            {
                return f => CastTo<T>.From(DateTime.Parse(f));
            }

            if (typeT == typeof(DateTimeOffset))
            {
                return f => CastTo<T>.From(DateTimeOffset.Parse(f));
            }

            if (typeT == typeof(TimeSpan))
            {
                return f => CastTo<T>.From(TimeSpan.Parse(f));
            }

            if (typeT == typeof(bool?))
            {
                return f => string.IsNullOrEmpty(f) ? default : CastTo<T>.From(bool.Parse(f));
            }

            if (typeT == typeof(int?))
            {
                return f => string.IsNullOrEmpty(f) ? default : CastTo<T>.From(int.Parse(f));
            }

            if (typeT == typeof(double?))
            {
                return f => string.IsNullOrEmpty(f) ? default : CastTo<T>.From(double.Parse(f));
            }

            if (typeT == typeof(float?))
            {
                return f => string.IsNullOrEmpty(f) ? default : CastTo<T>.From(float.Parse(f));
            }

            if (typeT == typeof(Guid?))
            {
                return f => string.IsNullOrEmpty(f) ? default : CastTo<T>.From(Guid.Parse(f));
            }

            if (typeT == typeof(DateTime?))
            {
                return f => string.IsNullOrEmpty(f) ? default : CastTo<T>.From(DateTime.Parse(f));
            }

            if (typeT == typeof(DateTimeOffset?))
            {
                return f => string.IsNullOrEmpty(f) ? default : CastTo<T>.From(DateTimeOffset.Parse(f));
            }

            if (typeT == typeof(TimeSpan?))
            {
                return f => string.IsNullOrEmpty(f) ? default : CastTo<T>.From(TimeSpan.Parse(f));
            }

            throw new FormatException($"Unable to create converter for type {typeT}");
        }
    }
}
