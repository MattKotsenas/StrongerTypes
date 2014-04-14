Stronger Types [![Build Status](http://mattkotsenas-ci.cloudapp.net/job/StrongerTypes/badge/icon)](http://mattkotsenas-ci.cloudapp.net/job/StrongerTypes/)
==============

Stronger Types is a .NET [Portable Class Library](http://msdn.microsoft.com/en-us/library/gg597391(v=vs.110).aspx) (PCL) that provides general purpose types
to make your code safer and easier to read and maintain. Each type is its own namespace so you can pick and choose to suit your needs. Because it's
a Portable Class Library you can reuse this code across:

* .NET 4+
* Windows 8+
* Windows Phone 8.1
* Windows Phone Silverlight 8
* Silverlight 5

## Getting Started

The easiest way to get StrongerTypes is via [NuGet](https://www.nuget.org/packages/StrongerTypes/). Just add the package via Visual Studio (right click >
Manage NuGet Packages) or

```
nuget install StrongerTypes
```

If you want to build the library yourself, clone the repository and run

```
msbuild .\Build\Build.msbuild
```

from the project's root directory.

## NonNullable

Unlike value types (e.g. `int`), reference types can be `null`. In some cases, a function may return null as valid output; in other cases null
represents an error. This ambiguous use of `null` causes two problems:

### Lack of null handling

```cs
string str = GetValue(); // GetValue() returns null
Console.WriteLine(str.ToUpper()); // Throws a NullReferenceException
```

Forgetting to check for `null` when it an expected value is a common source of easily-avoidable errors.

### Ubiquitous null checking

```cs
string name;
string address;

name = GetName();
if (name != null)
{
    address = GetAddress(name);
}
if (address != null)
{
    Console.WriteLine(address);
}
```
    
Oppositely, checking for `null` can clutter code and reduce readability.

Returning a `NonNullable` from a function signifies that `null` is never an expected result and frees others from spinkling null checks
all over their code. Consider the previous example if both `GetName()` and `GetAddress()` returned `NonNullable<string>`:

```cs
string name;
string address;

name = GetName();
address = GetAddress(name);
Console.WriteLine(address);
```

Converting from the NonNullable to the naked type happens implicitly (going the other way requires an explict cast).

## Exceptional

Sometimes an `Exception` may be thrown far away from the code that is best suited to handle it. This may especially be true in service-based architectures
where a query or operation may pass through several layers of code, any of which may throw, and where the recovery logic may depend on business
rules in the client.

Other times these situations lead to "triagle code" where try/catch blocks are nested and each function call must handle a wide range of error conditions. `Exceptional`
reduces the try/catch burden and allows users of your code to clearly see that an exception may be thrown. To use Exceptional in your code:

```cs
public static Exceptional<string, Exception> FunctionThatMightThrow()
{
    try
    {
        return DoSomething();
    }
    catch (Exception e)
    {
        return new Exceptional<string, Exception>(e);
    }
}
```

Or more consise:

```cs
public static Exceptional<string, ArgumentException> FunctionThatMightThrow()
{
    return new Exceptional<string, ArgumentException>(DoSomething); // DoSomething is a Func that may return a string or may throw an ArgumentException!
}
```

In either case callers can then use the Exceptional like this:

```cs
var sample = FunctionThatMightThrow();
if (sample.HasException)
{
    // Error handling code ...
}
else
{
    Console.WriteLine(sample.Value);
}
```

The `Exceptional` object can be passed around like a value, reducing error handling.

## Weak

A [weak reference](http://msdn.microsoft.com/en-us/library/ms404247.aspx) allows you to keep a reference an object, but unlike a normal (strong) reference,
not prevent the object from being garbage collected. Weak references are useful when building caches or transferring ephemeral data like events. In these
cases you want to reference potentially large objects but not prevent the runtime from reclaiming memory when needed.

### Usage

Using a Weak object is simple:

```cs
Weak<MyClass> weakRef = new Weak<MyClass>(myClassObj);

// ... do lots of work here ...

MyClass newClassObj;
bool success = weakRef.TryGetTarget(out newClassObj));
```

### Doesn't .NET have this already?
Well, sort of. There is a generic [`WeakReference<T>` class](http://msdn.microsoft.com/en-us/library/gg712738.aspx), but it's only available in .NET 4.5.
A non-generic [`WeakReference` class](http://msdn.microsoft.com/en-us/library/system.weakreference.aspx) is available more broadly, but it's a bit cumbersome
and promotes incorect use. `Weak<T>` brings the goodness of the .NET 4.5 WeakReference<T> to the PCL.

## ReadOnlyDictionary

A simple read-only wrapper around the generic `Dictionary<TKey, TValue>` type. 'Nuff said.

### Usage

You can create a ReadOnlyDictionary two ways:

```cs
var source = new Dictionary<string, int>() { { "key1", 1 }, { "key2", 2 } };
var readOnly = new ReadOnlyDictionary<string, int>(source);
```

or the simpler:

```cs
var readOnly = (new Dictionary<string, int>() { { "key1", 1 }, { "key2", 2 } }).AsReadOnly();
```

### Doesn't .NET have this already too?
Again, sort of. If you're using .NET 4.5 or newer you can use the built in [`ReadOnlyDictionary<TKey, TValue>` class](http://msdn.microsoft.com/en-us/library/gg712875(v=vs.110).aspx).
However, like with `WeakReference<T>`, it isn't available in the PCL.
