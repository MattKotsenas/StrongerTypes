using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StrongerTypes.ReadOnly
{
    /// <summary>
    /// A set of extension methods for <see cref="T:IDictionary"/> objects.
    /// </summary>
    public static class DictionaryExtensions
    {
        /// <summary>
        /// Returns a read-only wrapper for the current <see cref="T:Dictionary"/>.
        /// </summary>
        /// <typeparam name="TKey">The key type of the Dictionary.</typeparam>
        /// <typeparam name="TValue">The value type of the Dictionary.</typeparam>
        /// <param name="dictionary">The Dictionary collection to wrap</param>
        /// <returns>A read-only view of the Dictionary</returns>
        /// <remarks>
        /// A <see cref="T:ReadOnlyDictionary"/> does not expose any methods to modify the collection, but if any changes
        /// are made to the underlying data they will be reflected in the read-only version as well.
        /// </remarks>
        public static ReadOnlyDictionary<TKey, TValue> AsReadOnly<TKey, TValue>(this IDictionary<TKey, TValue> dictionary)
        {
            return new ReadOnlyDictionary<TKey, TValue>(dictionary);
        }
    }
}
