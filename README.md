Parameter Editor Generator
=========================

Macros in Umbraco can have parameters, but those parameters can't be made of configured datatypes like the rich text editor, multinode tree picker etc. For those to work, you need to create your own custom editors, including af "default config", which the macro editor uses. This package makes it easy to generate custom parameter editors based on a specific datatype in your Umbraco installation.

To use the package, just right click a data type in Umbraco (or choose the Action menu) and select Generate Parameter Editor. A package.manifest is then generated for you, and you get the option to save it right away to your Umbraco installation. Once that is done, you now have a shiny new parameter editor to use in your macros.


## Installation

1. [**NuGet Package**][NuGetPackage]  
Install this NuGet package in your Visual Studio project. Makes updating easy.

1. [**ZIP file**][GitHubRelease]  
Grab a ZIP file of the latest release; unzip and move the contents to the root directory of your web application.

## Features

- Generates a package.manifest containing a propertyeditor, enabled as a parameter editor with a default config based on a data type.

- Saves the package.manifest for you in the right location

[NuGetPackage]: https://www.nuget.org/packages/skttl.ParameterEditorGenerator
[GitHubRelease]: https://github.com/skttl/ParameterEditorGenerator


## Screenshots
