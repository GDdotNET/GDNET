![GDNET Logo](https://github.com/GDdotNET/GDNET/blob/master/assets/GDNET-Logo-1.png?raw=trueraw=tru)
# GDNET
##### An ambitious attempt at a general interfacing library for the Geometry Dash servers, and it's client.

----

**GDNET** is a C# project that runs to make working with the [Geometry Dash](http://robtopgames.com) servers, it's data and values easier, along with adding client-sided features that interface with it's general data.

## Objectives
This wrapper aims to take control of data from Geometry Dash and make manipulating it easier; allowing for other projects, repositories and dependants to interface with the game through readable and predictable methods, at performant speeds to make the workload easier on developers wishing to make these connections, and grab data.
* Build full possible coverage over the Geometry Dash "API", and the data provided for/fromt it.
* Easier and programmatical ways of interfacing with Geometry Dash save data, with strings provided properly decoded.
* Keep readability at a high, and prevent messiness to make it easier on the contributors, and the dependants.

## Building
GDNET can be built for multiple platforms, primarily depending on IDE, and compilation method. Most IDE's will provide an option to Build the instance you have of GDNET, and it's usage is preferred for testing. Compiling and running tests on the `GDNET.Tests` workspace are reccomended, as they contain instances to test all possible aspects of the GDNET library, excluding the ones that are tested alongside other modules of the wrapper.
### Requirements
* A platform with the [.NET Cre 3.1 SDK](https://dotnet.microsoft.com/download/dotnet-core), or above installed.
* An IDE, such as [JetBrains Rider](https://www.jetbrains.com/rider/), or [Visual Studio 2019](https://visualstudio.microsoft.com/) and above, that include a sustainable syntax highlighter and intellisense for easier workability with the GDNET codebase.

## Contributing
Contributions to the GDNET codebase can be made via. pull requests to it's repository.

Good ideas on what to contribute can be found on the [list of opened issues](https://github.com/GDdotNET/GDNET/issues), primarily those labelled "bugs", or are "suggestions".

While we don't (yet) have a specific set of standards for contributions, we would appreciate those who wish to contribute to always make their tests with the project in the `GDNET.Tests` namespace, in its proper folder. If any issues with the codebase or its general compositon arise, the GDNET development team appreciates all feedback to such.

## License
The codebase and overall project is licensed under the [MIT License](https://opensource.org/licenses/MIT). For more information on the licensing, please view the [LICENSE file](https://github.com/Homurasama/GDNETPrivate/blob/master/LICENSE).