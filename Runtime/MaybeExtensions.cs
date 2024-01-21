using System;

namespace MaybeMonad
{
    /// <summary>
    /// Extension class for MaybeMonad providing additional methods.
    /// </summary>
    public static class MaybeExtensions
    {
        /// <summary>
        /// Performs an action on the contained value if it exists.
        /// </summary>
        /// <typeparam name="T">Type of the value.</typeparam>
        /// <param name="maybe">MaybeMonad instance.</param>
        /// <param name="action">Action to perform on the value.</param>
        /// <returns>The original MaybeMonad instance.</returns>
        public static MaybeMonad<T> Do<T>(this MaybeMonad<T> maybe, Action<T> action)
        {
            if (maybe.HasValue) action(maybe.Value);
            return maybe;
        }

        /// <summary>
        /// Filters the contained value based on a predicate.
        /// </summary>
        /// <typeparam name="T">Type of the value.</typeparam>
        /// <param name="maybe">MaybeMonad instance.</param>
        /// <param name="predicate">Predicate to apply.</param>
        /// <returns>Filtered MaybeMonad instance.</returns>
        public static MaybeMonad<T> Where<T>(this MaybeMonad<T> maybe, Func<T, bool> predicate) =>
            maybe.HasValue && predicate(maybe.Value) ? maybe : MaybeMonad<T>.None;

        /// <summary>
        /// Projects the contained value to a new value type using a selector function.
        /// </summary>
        /// <typeparam name="T">Original value type.</typeparam>
        /// <typeparam name="TResult">Resulting value type.</typeparam>
        /// <param name="maybe">MaybeMonad instance.</param>
        /// <param name="selector">Projection function.</param>
        /// <returns>Resulting MaybeMonad instance.</returns>
        public static MaybeMonad<TResult> Select<T, TResult>(this MaybeMonad<T> maybe, Func<T, TResult> selector) =>
            maybe.HasValue ? MaybeMonad<TResult>.Create(selector(maybe.Value)) : MaybeMonad<TResult>.None;

        /// <summary>
        /// Projects and flattens the contained value using a selection function.
        /// </summary>
        /// <typeparam name="T">Original value type.</typeparam>
        /// <typeparam name="TResult">Resulting value type.</typeparam>
        /// <param name="maybe">MaybeMonad instance.</param>
        /// <param name="selector">Projection and selection function.</param>
        /// <returns>Resulting MaybeMonad instance.</returns>
        public static MaybeMonad<TResult> SelectMany<T, TResult>(this MaybeMonad<T> maybe,
            Func<T, MaybeMonad<TResult>> selector) => maybe.HasValue ? selector(maybe.Value) : MaybeMonad<TResult>.None;

        /// <summary>
        /// Projects, flattens, and selects the contained value using projection and selection functions.
        /// </summary>
        /// <typeparam name="T">Original value type.</typeparam>
        /// <typeparam name="TIntermediate">Intermediate value type.</typeparam>
        /// <typeparam name="TResult">Resulting value type.</typeparam>
        /// <param name="maybe">MaybeMonad instance.</param>
        /// <param name="selector">Projection function.</param>
        /// <param name="resultSelector">Result selection function.</param>
        /// <returns>Resulting MaybeMonad instance.</returns>
        public static MaybeMonad<TResult> SelectMany<T, TIntermediate, TResult>(this MaybeMonad<T> maybe,
            Func<T, MaybeMonad<TIntermediate>> selector, Func<T, TIntermediate, TResult> resultSelector)
        {
            if (!maybe.HasValue) return MaybeMonad<TResult>.None;
            var intermediate = selector(maybe.Value);
            return intermediate.HasValue
                ? MaybeMonad<TResult>.Create(resultSelector(maybe.Value, intermediate.Value))
                : MaybeMonad<TResult>.None;
        }

        /// <summary>
        /// Performs an action if the MaybeMonad instance does not contain a value.
        /// </summary>
        /// <typeparam name="T">Type of the value.</typeparam>
        /// <param name="maybe">MaybeMonad instance.</param>
        /// <param name="action">Action to perform.</param>
        /// <returns>The original MaybeMonad instance.</returns>
        public static MaybeMonad<T> DoOnNone<T>(this MaybeMonad<T> maybe, Action action)
        {
            if (!maybe.HasValue) action();
            return maybe;
        }

        /// <summary>
        /// Returns a default value if the MaybeMonad instance does not contain a value.
        /// </summary>
        /// <typeparam name="T">Type of the value.</typeparam>
        /// <param name="maybe">MaybeMonad instance.</param>
        /// <param name="func">Function to obtain the default value.</param>
        /// <returns>The original MaybeMonad instance or a new instance with the default value.</returns>
        public static MaybeMonad<T> OnNone<T>(this MaybeMonad<T> maybe, Func<T> func) =>
            maybe.HasValue ? maybe : MaybeMonad<T>.Create(func());

        /// <summary>
        /// Projects a new value if the MaybeMonad instance does not contain a value.
        /// </summary>
        /// <typeparam name="T">Type of the value.</typeparam>
        /// <param name="maybe">MaybeMonad instance.</param>
        /// <param name="func">Projection function.</param>
        /// <returns>The original MaybeMonad instance or a new instance with the projected value.</returns>
        public static MaybeMonad<T> OnNone<T>(this MaybeMonad<T> maybe, Func<T, T> func) =>
            maybe.HasValue ? maybe : MaybeMonad<T>.Create(func(maybe.Value));

        /// <summary>
        /// Projects a new value and performs an action if the MaybeMonad instance does not contain a value.
        /// </summary>
        /// <typeparam name="T">Type of the value.</typeparam>
        /// <param name="maybe">MaybeMonad instance.</param>
        /// <param name="func">Projection function.</param>
        /// <param name="action">Action to perform.</param>
        /// <returns>The original MaybeMonad instance or a new instance with the projected value.</returns>
        public static MaybeMonad<T> OnNone<T>(this MaybeMonad<T> maybe, Func<T, T> func, Action action)
        {
            if (!maybe.HasValue) action();
            return maybe.HasValue ? maybe : MaybeMonad<T>.Create(func(maybe.Value));
        }

        /// <summary>
        /// Returns a new MaybeMonad instance provided by a function if the current one does not contain a value.
        /// </summary>
        /// <typeparam name="T">Type of the value.</typeparam>
        /// <param name="maybe">MaybeMonad instance.</param>
        /// <param name="func">Function to obtain the new MaybeMonad instance.</param>
        /// <returns>The original MaybeMonad instance or a new instance provided by the function.</returns>
        public static MaybeMonad<T> Else<T>(this MaybeMonad<T> maybe, Func<MaybeMonad<T>> func) =>
            maybe.HasValue ? maybe : func();

        /// <summary>
        /// Returns a new MaybeMonad instance provided by a function if the current one does not contain a value.
        /// </summary>
        /// <typeparam name="T">Type of the value.</typeparam>
        /// <param name="maybe">MaybeMonad instance.</param>
        /// <param name="func">Function to obtain the new MaybeMonad instance.</param>
        /// <returns>The original MaybeMonad instance or a new instance provided by the function.</returns>
        public static MaybeMonad<T> Else<T>(this MaybeMonad<T> maybe, Func<T, MaybeMonad<T>> func) =>
            maybe.HasValue ? maybe : func(maybe.Value);

        /// <summary>
        /// Returns a new MaybeMonad instance with a default value if the current one does not contain a value.
        /// </summary>
        /// <typeparam name="T">Type of the value.</typeparam>
        /// <param name="maybe">MaybeMonad instance.</param>
        /// <param name="value">Default value.</param>
        /// <returns>The original MaybeMonad instance or a new instance with the default value.</returns>
        public static MaybeMonad<T> Else<T>(this MaybeMonad<T> maybe, T value) =>
            maybe.HasValue ? maybe : MaybeMonad<T>.Create(value);

        /// <summary>
        /// Returns a new MaybeMonad instance provided if the current one does not contain a value.
        /// </summary>
        /// <typeparam name="T">Type of the value.</typeparam>
        /// <param name="maybe">MaybeMonad instance.</param>
        /// <param name="other">Another MaybeMonad instance.</param>
        /// <returns>The original MaybeMonad instance or the provided other instance.</returns>
        public static MaybeMonad<T> Else<T>(this MaybeMonad<T> maybe, MaybeMonad<T> other) =>
            maybe.HasValue ? maybe : other;

        /// <summary>
        /// Converts the MaybeMonad instance to a nullable value.
        /// </summary>
        /// <typeparam name="T">Type of the value.</typeparam>
        /// <param name="maybe">MaybeMonad instance.</param>
        /// <returns>Nullable value.</returns>
        public static T? ToNullable<T>(this MaybeMonad<T> maybe) where T : struct => maybe.HasValue ? maybe.Value : null;

        /// <summary>
        /// Converts the MaybeMonad instance to a default value if it does not contain a value.
        /// </summary>
        /// <typeparam name="T">Type of the value.</typeparam>
        /// <param name="maybe">MaybeMonad instance.</param>
        /// <returns>Default value.</returns>
        public static T ToDefault<T>(this MaybeMonad<T> maybe) where T : class => maybe.HasValue ? maybe.Value : null;

        /// <summary>
        /// Converts a nullable value to a MaybeMonad instance.
        /// </summary>
        /// <typeparam name="T">Type of the value.</typeparam>
        /// <param name="nullable">Nullable value.</param>
        /// <returns>MaybeMonad instance.</returns>
        public static MaybeMonad<T> AsMaybe<T>(this T? nullable) where T : struct =>
            nullable.HasValue ? MaybeMonad<T>.Create(nullable.Value) : MaybeMonad<T>.None;

        /// <summary>
        /// Converts a value to a MaybeMonad instance.
        /// </summary>
        /// <typeparam name="T">Type of the value.</typeparam>
        /// <param name="default">Default value.</param>
        /// <returns>MaybeMonad instance.</returns>
        public static MaybeMonad<T> AsMaybe<T>(this T @default) where T : class =>
            @default is not null ? MaybeMonad<T>.Create(@default) : MaybeMonad<T>.None;
    }
}
