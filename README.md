![License](https://img.shields.io/github/license/decline-cookies/anvil-unity-core?label=License)&nbsp;&nbsp;&nbsp;

# anvil-unity-core

An opinionated collection of systems and utilities that add [Unity](https://unity.com) specific implementations of [Anvil](https://github.com/decline-cookies/anvil-csharp-core) systems and add new common tools and systems that are uniquely useful to Unity development.

Refer to the [anvil-csharp-core](https://github.com/decline-cookies/anvil-csharp-core) for a description of Anvil's purpose and the team's motivations.

### Expectations

See: [anvil-csharp-core](https://github.com/decline-cookies/anvil-csharp-core)

The code is reasonably clean but documentation and examples are sparse. Feel free to [reach out on Twitter](https://twitter.com/declinecookies) or open issues with questions.

⚠️ We welcome PRs and bug reports but making this repo a public success is not our priority. No promises on when it will be addressed!

## Dependencies

- [Unity 2021.2+](https://unity.com/)
  - .NET Standard 2.1 (Required in Player Settings)
  - [Unity.Mathematics](https://docs.unity3d.com/Packages/com.unity.mathematics@1.2/manual/index.html)
- [anvil-csharp-core](https://github.com/decline-cookies/anvil-csharp-core)

## Features

- [ ] TODO: [Issue #49](https://github.com/decline-cookies/anvil-unity-core/issues/49)

## Project Setup

1. Add anvil-unity-core and any [Dependencies](#dependencies) as submodules to your project
2. Make use of [Features](#features) as desired.
3. Done!

This is the recommended Unity project folder structure:

```
- Assets
  - Anvil
    - anvil-csharp-core
    - anvil-unity-core
```

## Common Errors

### Can't Compile - Errors related to Logging or Logging DLLs are recognized as "Native"

> Example:
> ```
> The type or namespace name 'ILogHandler' could not be found
> ```

This usually means that Git LFS hasn't been initialized. Check the size of the DLL. if it's less than a kilobyte then the Git LFS files have not yet been resolved.

Running `git lfs pull` in each submodules will generally fix the issue.
