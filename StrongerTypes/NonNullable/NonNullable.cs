using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StrongerTypes.NonNullable
{
    /// <summary>
    /// A small wrapper around reference types that specifies that the reference will not be null.
    /// Think of this type as the opposite of Nullable&lt;T&gt; (e.g. int?). The goal is to provide callers
    /// with the additional context that null is never an expected value and should be considered an error.
    ///
    /// It is possible to create a null reference by using the default, parameterless constructor that
    /// comes with every struct, though this is difficult to do in practice as using the parameterless
    /// constructor in most cases requires setting the fields prior to use and the <see cref="Value"/> field
    /// is read only. In these cases the wrapper will throw an exception on access to the value.
    /// </summary>
    /// <remarks>
    /// Original inspiration comes from http://msmvps.com/blogs/jon_skeet/archive/2008/10/06/non-nullable-reference-types.aspx.
    /// </remarks>
    /// <typeparam name="T">The type to wrap.</typeparam>
    public struct NonNullable<T> where T : class
    {
        /// <summary>
        /// The actual object / value we are holding onto.
        /// </summary>
        private readonly T value;

        /// <summary>
        /// Create a new wrapper around an instance of <paramref name="value"/> of type <typeparamref name="T"/>.
        /// </summary>
        /// <param name="value">The not null object to wrap.</param>
        public NonNullable(T value)
        {
            if (value == null)
            {
                throw new ArgumentNullException("value");
            }
            this.value = value;
        }

        /// <summary>
        /// The unwrapped object. Throws an <see cref="InvalidOperationException"/> if the value
        /// is null (e.g. by using the parameterless constructor to create the wrapper).
        /// </summary>
        public T Value
        {
            get
            {
                if (value == null)
                {
                    throw new InvalidOperationException("Value is null");
                }
                return value;
            }
        }

        /// <summary>
        /// Type conversion operation that allows callees to convert a NonNullable
        /// to it's wrapped type (i.e. T) without a cast.
        /// </summary>
        /// <param name="obj">The NonNullable object to convert.</param>
        /// <returns>The wrapped value.</returns>
        public static implicit operator T(NonNullable<T> obj)
        {
            return obj.Value;
        }

        /// <summary>
        /// Type conversion operation that allows callees to convert from a type T
        /// to a NonNullable&lt;T%gt;. This cast is explict because casting a null
        /// to NonNullable is not allows and will throw an exception.
        /// </summary>
        /// <param name="obj">The object of type T to convert to a NonNullable</param>
        /// <returns>a NonNullable that wraps obj.</returns>
        public static explicit operator NonNullable<T>(T obj)
        {
            return new NonNullable<T>(obj);
        }
    }
}
