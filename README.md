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

 * `HashPrimeNumbers`

#### System.Collections.Generic

 * `CollectionDebuggerView<T, U>`
 * `CollectionDebuggerView<T>`
 * `HashSet<T>`
 * `HashSet<T>.Enumerator`
 * `HashSetEqualityComparer<T>`

#### System.Runtime.CompilerServices

 * `ExtensionAttribute`

## Releasing

NUnit.System.Linq is released as a nuget package on both our AppVeyor project feed and the NUnit MyGet feed. All branch and PR builds appear on the AppVeyor feed. Only actual builds of master are deployed by AppVeyor to the MyGet feed. Note that builds of master may be initiated either by merging a PR or by pushing directly to master.

Builds, including builds of master, may be either tagged or untagged. Tagged builds use the tag itself as the version. Note that you must use the `--tags` option on the `git push` command in order for a local tag to be pushed to GitHub.

### Example Workflows

#### Normal CI Build

This is the normal workflow for making changes. All the artifacts created use the -CI pre-release suffix.

1. Commit your work to the local branch and push to GitHub. Your branch builds and, if successful, produces artifacts on the AppVeyor feed.

2. Create a PR. The PR builds and creates artifacts on the AppVeyor feed.

3. Merge the PR to master. Master builds and creates artifacts on both the AppVeyor and MyGet feeds.

#### Tagged CI build

As above except before pushing tag the commit and use `--tags` option on the push. All artifacts will use the tag you supply as the version number. For example

```
git tag 0.5.0
git push --tags
```

This workflow may not be as useful as first appears. It assumes you know in advance that you want a certain tag on the commit you are making once it is merged. This is normally only the case if you are making simple doc changes or just changing the release itself.

#### Push directly to master

This assumes that master is ready for publication using a particular version number. It's the way to go if you already built the last changes needed for the release and merely want to publish it.

1. Pull master to ensure you have the latest changes

2. Apply the tag and push as above.

3. Master will build and artifacts will be deployed to both AppVeyor and MyGet using the tag you specified as the version.