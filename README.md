# Tiger.Clock

## What It Is

Tiger.Clock is a library of sources of system time that are injectable and mockable.

## Why You Want It

When writing a method that requires the use of the current time, it's very easy and not *exactly* wrong to reach for properties on the `DateTimeOffset` structure – usually these:

- `DateTimeOffset.Now`
- `DateTimeOffset.UtcNow`

(The structure `DateTime` has been largely superseded by `DateTimeOffset`, and has limited remaining use cases.)

These properties will give the correct answers and work as promised in application code. Where their use becomes troublesome is in *tests* of that application code. The property `DateTimeOffset.Now` is incapable of returning anything but the current system time in the local time offset. If a test requires that it be run on 2010-07-28 in order to achieve a divergence value of 1.048596%, then the test can only be run on that actual day. Tests that are hard or impossible to write will not get written, tests that are not written test no functionality, and functionality that is not tested is fragile.

In a language such as C#, this is typically overcome by use of inversion of control (IoC), and there is nothing preventing date and time calculation from joining in.

## How You Use It

Services that require use of system time should take a dependency (by whatever means) on a value of the type `IClock` (which we'll call `_clock`). Currently, the only type in the library that implements this interface is `StandardClock`, so ensure that an instance of that type is bound to the `IClock` value. Then, whenever `DateTimeOffset.Now` or `DateTimeOffset.UtcNow` would be used, substitute `_clock.Now` and `_clock.UtcNow`, respectively.

When testing that service, it can take a dependency on a mock `IClock` or a specialized fake `IClock`.

### A Note on Testing

From implementation experience, a curious pattern in testing has emerged. If you are using Autofixture or some other test-data–generating library, it's easier to let the data be generated with whatever datetime values are randomly chosen by the library and redefine *now* to be within the test's tolerance of that generated value.

## How You Develop It

This project is using the standard [`dotnet`](https://dot.net) build tool.

This repository is attempting to use the [GitFlow](http://jeffkreeftmeijer.com/2010/why-arent-you-using-git-flow/) branching methodology. Results may be mixed, please be aware.

## Thank You

Seriously, though. Thank you for using this software. The author hopes it performs admirably for you.
