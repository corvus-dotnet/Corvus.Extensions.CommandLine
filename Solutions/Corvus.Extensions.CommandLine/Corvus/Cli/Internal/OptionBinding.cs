// <copyright file="OptionBinding.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Corvus.Cli.Internal
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using Corvus.Extensions;
    using Microsoft.Extensions.CommandLineUtils;

    /// <summary>
    /// A binding for an option to a command.
    /// </summary>
    /// <typeparam name="TCommand">The command to which to bind the option.</typeparam>
    /// <typeparam name="T">The type of the value to which the item is bound.</typeparam>
    internal class OptionBinding<TCommand, T> : IOptionBinding
        where TCommand : Command<TCommand>
    {
        private readonly TCommand command;
        private readonly CommandOption option;
        private readonly Func<T, string> validator;
        private readonly object converter;
        private readonly Action<TCommand, T> setter;

        /// <summary>
        /// Initializes a new instance of the <see cref="OptionBinding{TCommand, T}"/> class.
        /// </summary>
        /// <param name="command">The command to which to bind the option.</param>
        /// <param name="option">The command option.</param>
        /// <param name="setter">The property setter in the option binding.</param>
        /// <param name="validator">The validator for the option.</param>
        /// <param name="converter">The converter from the string to target type.</param>
        public OptionBinding(TCommand command, CommandOption option, Expression<Func<T>> setter, Func<T, string> validator = null, object converter = null)
        {
            this.command = command ?? throw new ArgumentNullException(nameof(command));
            this.option = option ?? throw new ArgumentNullException(nameof(option));
            this.setter = this.CreateSetter(setter);
            this.validator = validator;
            this.converter = converter;
        }

        /// <summary>
        /// Apply the binding with validation.
        /// </summary>
        public void ApplyBinding()
        {
            T value = this.ApplyConversion();
            this.Validate(value);
            this.setter(this.command, value);
        }

        private void Validate(T value)
        {
            string validationError = this.validator?.Invoke(value);
            if (!string.IsNullOrEmpty(validationError))
            {
                throw new OptionValidationException(validationError);
            }
        }

        private T ApplyConversion()
        {
            T value;
            if (this.converter is null)
            {
                if (typeof(T) == typeof(string) && this.option.OptionType == CommandOptionType.SingleValue)
                {
                    value = CastTo<T>.From(this.option.Value());
                }
                else if (typeof(T).IsAssignableFrom(typeof(List<T>)) && this.option.OptionType == CommandOptionType.MultipleValue)
                {
                    value = CastTo<T>.From(this.option.Values);
                }
                else if (typeof(T).IsAssignableFrom(typeof(bool)) && this.option.OptionType == CommandOptionType.NoValue)
                {
                    value = CastTo<T>.From(this.option.HasValue());
                }
                else
                {
                    throw new FormatException($"You cannot bind directly from the option {this.option.LongName} with type {this.option.OptionType} option to a property of type {typeof(T)}, without providing a suitable converter.");
                }
            }
            else
            {
                if (this.converter is Func<string, T> stringConverter && this.option.OptionType == CommandOptionType.SingleValue)
                {
                    value = stringConverter(this.option.Value());
                }
                else if (this.converter is Func<List<string>, T> stringListConverter && this.option.OptionType == CommandOptionType.MultipleValue)
                {
                    value = stringListConverter(this.option.Values);
                }
                else if (this.converter is Func<bool, T> boolConverter && this.option.OptionType == CommandOptionType.NoValue)
                {
                    value = boolConverter(this.option.HasValue());
                }
                else
                {
                    throw new FormatException($"You cannot bind from the option {this.option.LongName} with type {this.option.OptionType} option to a property of type {typeof(T)}, using the supplied converter.");
                }
            }

            return value;
        }

        private Action<TCommand, T> CreateSetter(Expression<Func<T>> setter)
        {
            if (setter is null)
            {
                throw new ArgumentNullException(nameof(setter));
            }

            MemberExpression memberExpression = setter.GetMemberExpression();
            ParameterExpression param = Expression.Parameter(typeof(T), "value");
            var set = Expression.Lambda<Action<TCommand, T>>(
                Expression.Assign(memberExpression, param), Expression.Parameter(typeof(TCommand)), param);

            return set.Compile();
        }
    }
}
