# anvil-unity-core

Unity Core Library for Anvil

# Project Setup

This project depends on [anvil-csharp-core](https://github.com/scratch-games/anvil-csharp-core) - a few setup steps are necessary to allow these libraries to communicate in Unity.

When adding both libraries to your project (ex. by submodule), place the C# core library in its own folder, and create an Assembly Definition file called "anvil-csharp-core.asmdef" beside it. **Do not** place the Assembly Definition inside the C# core library or commit it to that repo, it is a Unity-specific asset.

_Note: The Unity core library already contains the necessary Assembly Definitions, which will automatically find this new one by name._

Here is the recommended folder structure for this setup:

- Assets
  - Anvil
    - csharp
      - anvil-csharp-core <- C# core library submodule
      - anvil-csharp-core.asmdef <- Assembly Definition you create
    - unity
      - anvil-unity-core <- Unity core library submodule

