## NUnit.System.Linq
Partial implementation of System.Linq for use with NUnit's .NET 2.0 framework builds and by the TestEngine.

This assembly contains classes in the System.Linq namespace as well as other classes not present in .NET 2.0 but needed in order to support System.Linq. It will cause conflicts if referenced by an assembly that targets a runtime other than .NET 2.0.

## Types supported

### System.Linq

 * `Check`
 * `Enumerable`
 * `Grouping<K, T>`
 * `IGrouping<TKey, TElement>`
 * `ILookup<TKey, TElement>`
 * `IOrderedEnumerable<TElement>`
 * `Lookup<TKey, TElement>`
 * `OrderedEnumerable<TElement>`
 * `OrderedSequence<TElement, TKey>`
 * `QuickSort<TElement>`
 * `SortContext<TElement>`
 * `SortDirection`
 * `SortSequenceContext<TElement, TKey>`

### Other Types Included

#### System

 * `Action`
 * `Action<T1, T2>`
 * `Action<T1, T2, T3>`
 * `Action<T1, T2, T3, T4>`
 * `Func<TResult>`
 * `Func<T, TResult>`
 * `Func<T1, T2, TResult>`
 * `Func<T1, T2, T3, TResult>`
 * `Func<T1, T2, T3, T4, TResult>`

#### System.Collections

## Releasing

NUnit.System.Linq is released on the NUnit myget feed only: https://www.myget.org/F/nunit/api/v2

A new release is automatically published for all AppVeyor builds of the master branch. This normally takes place after the merging of a PR on GitHub. Such builds are normally pre-releases, using the -CI- suffix and publication takes place automatically.


 * `HashPrimeNumbers`

#### System.Collections.Generic

 * `CollectionDebuggerView<T, U>`
 * `CollectionDebuggerView<T>`
 * `HashSet<T>`
 * `HashSet<T>.Enumerator`
 * `HashSetEqualityComparer<T>`

#### System.Runtime.CompilerServices

 * `ExtensionAttribute`
