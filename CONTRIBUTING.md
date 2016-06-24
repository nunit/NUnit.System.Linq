# How to contribute

**NUnit.System.Linq** is a partial implementation of System.Linq for use with NUnit's .NET 2.0 framework builds and by the TestEngine. It contains classes in the System.Linq namespace as well as other classes not present in .NET 2.0 but needed in order to support System.Linq. It will cause conflicts if referenced by an assembly that targets a runtime other than .NET 2.0.

This code in this assembly was not written by us nor do we maintain it. It actually comes from the Mono project. Because each assembly we maintain adds another bit to our workload, we would prefer to keep changes of our own out of this class except for boilerplate stuff needed to build it in our environment.

For that reason, don't ask for changes to System.Linq. Do point out any bugs or classes you would like to import from Mono in order to use them in the framework or engine.

## License

NUnit.System.Linq is under the [MIT license](https://github.com/nunit/nunit/blob/master/LICENSE.txt). All the included classes are under that same license.