using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StrongerTypes.Weak
{
    /// <summary>
    /// A weak reference wrapper for an object. This brings a generic
    /// weak reference type to .NET 4 and the PCL. The API is similar
    /// to the generic WeakReference&lt;T&gt; class introduced in .NET 4.5.
    /// (See <a href="http://msdn.microsoft.com/en-us/library/gg712738.aspx">here</a>
    /// for more info). The only differences is the removal of SetTarget, as that
    /// breaks the immutability of the object.
    /// 
    /// This class does not allow for resurrection after finalization. See 
    /// <a href="http://msdn.microsoft.com/en-us/library/ms404247.aspx">here</a> for
    /// more info on weak references in .NET.
    /// </summary>
    /// <typeparam name="T">The type of the element to hold a weak reference to.</typeparam>
    public class Weak<T> where T : class
    {
        private readonly WeakReference target;

        /// <summary>
        /// Create a new weak reference to the specified object.
        /// </summary>
        /// <param name="target">The object to reference.</param>
        public Weak(T target)
        {
            this.target = new WeakReference(target);
        }

        /// <summary>
        /// Try to get the referenced object. If the object is still alive it is assigned
        /// to the out parameter. Use the return value to know if retrieval succeeded.
        /// </summary>
        /// <param name="target">
        /// The referenced object if it is still alive, null if it is dead.
        /// </param>
        /// <returns>
        /// True if the object is still alive and <paramref name="target"/> has been set.
        /// False otherwise.
        /// </returns>
        public bool TryGetTarget(out T target)
        {
            target = (T) this.target.Target;
            return (target != null);
        }
    }

}
