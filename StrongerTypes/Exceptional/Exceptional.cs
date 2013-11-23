using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace StrongerTypes.Exceptional
{
    /// <summary>
    /// Exceptional is a small wrapper around a value or function that may throw an Exception.
    /// Exceptional is preferred to traditional try / catch blocks when the exception is thrown
    /// far away from the handling code, or in distrubuted / thread pool scenarios, where the
    /// handling code may be on a different thread.
    ///
    /// Usage is similar to the Nullable type; if an exception was thrown HasException is true
    /// and Exception is set. If the operation completed without an exception then HasException is false
    /// and Value is set.
    /// </summary>
    /// <remarks>
    /// Original inspiration taken from http://smellegantcode.wordpress.com/2008/12/11/the-maybe-monad-in-c/
    /// and http://stackoverflow.com/questions/10772727/exception-or-either-monad-in-c-sharp
    /// </remarks>
    /// <typeparam name="T">The type of the expected object.</typeparam>
    /// <typeparam name="U">The type of the exception that might be thrown.</typeparam>
    [DataContract]
    public class Exceptional<T, U> where U : Exception
    {
        /// <summary>
        /// True if evaluating the value threw an exception and Exception is set. False if Value is set.
        /// </summary>
        [DataMember]
        public bool HasException { get; private set; }

        /// <summary>
        /// The exception that was thrown. If HasException is false, Exception is null.
        /// </summary>
        [DataMember]
        public U Exception { get; private set; }

        /// <summary>
        /// The value of the operation if no exception was thrown.
        /// </summary>
        [DataMember]
        public T Value { get; private set; }

        /// <summary>
        /// Create a new simple Exceptional. An exception could not be thrown in this case.
        /// </summary>
        /// <param name="value">The value to wrap.</param>
        public Exceptional(T value)
        {
            this.HasException = false;
            this.Exception = null;
            this.Value = value;
        }

        /// <summary>
        /// Create a new Exceptional. The value function is evaluated. If an exception is thrown
        /// it is caught and Exception and HasException is set. If the function completes Value is set.
        /// </summary>
        /// <param name="value">The value to evaluate and wrap.</param>
        public Exceptional(Func<T> value)
        {
            try
            {
                this.HasException = false;
                this.Exception = null;
                this.Value = value();
            }
            catch(U e)
            {
                this.HasException = true;
                this.Exception = e;
                this.Value = default(T);
            }
        }

        /// <summary>
        ///  Create a new Exceptional with the provided Exception. HasException and Exception are set.
        /// </summary>
        /// <param name="e">The Exception to to wrap.</param>
        public Exceptional(U e)
        {
            this.HasException = true;
            this.Exception = e;
            this.Value = default(T);
        }

        /// <summary>
        /// Type conversion operation that allows callers to convert from type T to an Exceptional&lt;T%gt;
        /// without an explict cast.
        /// </summary>
        /// <param name="obj">The object to wrap.</param>
        /// <returns>A new Exceptional that wraps obj.</returns>
        public static implicit operator Exceptional<T, U>(T obj)
        {
            return new Exceptional<T, U>(obj);
        }
    }
}
