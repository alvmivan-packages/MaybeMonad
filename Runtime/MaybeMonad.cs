using System;

namespace MaybeMonad
{
    /// <summary>
    /// Represents a "Maybe" monadic type that can contain a value or none.
    /// </summary>
    /// <typeparam name="T">Type of the contained value.</typeparam>
    public readonly struct MaybeMonad<T>
    {
        /// <summary>
        /// Private constructor to create a MaybeMonad instance with a value.
        /// </summary>
        MaybeMonad(T val)
        {
            HasValue = val is not null;
            Value = val;
        }

        /// <summary>
        /// Creates a new MaybeMonad instance with a specified value.
        /// </summary>
        /// <param name="value">Value to contain.</param>
        /// <returns>New MaybeMonad instance.</returns>
        public static MaybeMonad<T> Create(T value) => new(value);

        /// <summary>
        /// Gets a MaybeMonad instance with no value.
        /// </summary>
        public static MaybeMonad<T> None => new();

        /// <summary>
        /// Gets a value indicating whether the MaybeMonad instance contains a value.
        /// </summary>
        public bool HasValue { get; }

        /// <summary>
        /// Gets the value contained in the MaybeMonad instance.
        /// </summary>
        public T Value { get; }

        /// <summary>
        /// Applies a function to a contained value, returning a new MaybeMonad resulting instance.
        /// </summary>
        /// <param name="func">Function to apply.</param>
        /// <returns>New resulting MaybeMonad instance.</returns>
        public MaybeMonad<T> Bind(Func<T, MaybeMonad<T>> func) => HasValue ? func(Value) : None;
    }
}