Stronger Types
==============

Stronger Types is a .NET [Portable Class Library](http://msdn.microsoft.com/en-us/library/gg597391.aspx) that provides general purpose types
to make your code safer and easier to read and maintain. Each type is its own namespace so you can pick and choose to suit your needs.

## NonNullable

Unlike value types (e.g. `int`), reference types can be `null`. In some cases, a function may return null as valid output; in other cases null
represents an error. This ambiguous use of `null` causes two problems:

* __Lack of null handling__

```cs
string myString = GetValue(); // GetValue() returns null
Console.WriteLine(myString.ToUpper()); // Throws a NullReferenceException
```

Forgetting to check for `null` when it an expected value is a common source of easily-avoidable errors.

* __Ubiquitous null checking__

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

Returning a `NonNullable` from a function signafies that a `null` is never an expected result and frees users of your code from spinkling null checks
all over their code. Consider the previous example if both `GetName()` and `GetAddress()` returned `NonNullable<string>`:

```cs
string name;
string address;

name = GetName();
address = GetAddress(name);
Console.WriteLine(address);
```

## Exceptional

Sometimes an `Exception` may be thrown far away from the code that is best suited to handle it. This may especially be true in service-based architectures
where a query or operation may pass through several layers of code, any of which may throw an exception, and where the recovery logic may depend on business
rules in the client.

Often these situations lead to "triagle code" where try/catch blocks are nested and each function call must handle a wide range of error conditions. `Exceptional`
reduces the try/catch burden and allows users of your code to clearly see that an exception may be thrown. To use Exceptional in your code:

```cs
public static Exceptional<string> FunctionThatMightThrow()
{
    try
    {
        return DoSomething();
    }
    catch (Exception e)
    {
        return new Exceptional(e);
    }
}
```

Or more consise:

```cs
public static Exceptional<string> FunctionThatMightThrow()
{
    return new Exceptional(DoSomething); // DoSomething is a Func that may return a string or may throw an exception! 
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

The Exceptional object can be passed around like a value, reducing error handling.