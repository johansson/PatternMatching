PatternMatching
===============

A simple .NET library with extension methods to allow you to do
pattern matching with fluent syntax instead of doing a switch
or a dictionary.

## Usage

Simply make a reference to the assembly in your project, then
add the line

```CSharp
using PatternMatching;
```

A few examples are:

```CSharp
int a = 5;
a.With(x => x == 5).Do(_ => Console.WriteLine("I am five!"));
```

That example should print "I am five!" to the standard output.

You can even chain `With`/`Do` operations together!

```CSharp
int a = 5;
a.With(x => x == 5).Do(_ => Console.WriteLine("I am five!"))
 .With(x => x != 5).Do(_ => Console.WriteLine("I am not five!"))
 .With(x => x >= 5).Do(x => Console.WriteLine("{0} is greater than or equal to five!", x))
 .With(x => x <  5).Do(x => Console.WriteLine("{0} is less than five!", x));
```

That example should print only the lines "I am five!" and
"5 is greater than or equal to five!" to the standard output.

Finally, there's the `Otherwise` operation, which is the
default fallthrough in case nothing else has been satisfied.

We'll demonstrate this with the classical [FizzBuzz](http://imranontech.com/2007/01/24/using-fizzbuzz-to-find-developers-who-grok-coding/) problem.

```CSharp
foreach (var i in Enumerable.Range(1, 100))
    i.With(x => x % 3 == 0).Do(_ => Console.WriteLine("Fizz"))
     .With(x => x % 5 == 0).Do(_ => Console.WriteLine("Buzz"))
     .Otherwise(x => Console.WriteLine));
```

## License

PatternMatching is licensed under the BSD License.