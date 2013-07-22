Stronger Types
==============

Stronger Types is a .NET [Portable Class Library](http://msdn.microsoft.com/en-us/library/gg597391.aspx) that provides general purpose types.

## NonNullable

Unlike value types (e.g. `int`), reference types can be `null`. In some cases, a function may return null as valid output; in other cases null
represents and error. This ambiguous use of `null` causes two problems:

1. Lack of null handling

    string myString = GetValue(); // GetValue() returns null
    Console.WriteLine(myString.ToUpper()); // Throws a NullReferenceException

Forgetting to check for `null` when it an expected value is a common source of easily-avoidable errors.

2. Ubiquitous null checking

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
    
The opposite of the #1, checking for `null` can clutter code and reduce readability.

Returning a `NonNullable` from a function signafies that a `null` is never an expected results, and frees users of your code from spinkling null checks
all over their code. Consider the previous example if both `GetName()` and `GetAddress()` returned `NonNullable<string>`:

    string name;
    string address;
    
    name = GetName();
    address = GetAddress(name);
    Console.WriteLine(address);
    
    