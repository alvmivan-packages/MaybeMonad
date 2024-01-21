# MaybeMonad

**MaybeMonad** is a simple and lightweight library that provides a Maybe monad implementation for handling nullable values in a functional way in C#.

## Overview

The MaybeMonad library introduces the `MaybeMonad<T>` structure, which represents a monadic type that can either contain a value or be empty (`None`). This can be particularly useful in scenarios where dealing with `null` values can lead to null reference exceptions or unclear code.

## Installation

### Package Manager (Unity)

1. Open the Unity Package Manager from the Window menu.
2. Click on "Add package from git URL..."
3. Enter the following Git URL:
   ```
   https://github.com/alvmivan-packages/MaybeMonad.git#master
   ```

### Package Manifest (Unity)

Add the following line to your project's `Packages/manifest.json` file under the `dependencies` section:

```json
{
  "dependencies": {
    "com.orbitar.maybemonad": "https://github.com/alvmivan-packages/MaybeMonad.git#master"
  }
}
```

Ensure that the Unity version specified in the manifest is compatible with the MaybeMonad library.

## Usage

```csharp
using MaybeMonad;

// Creating MaybeMonad instances
var maybeWithValue = MaybeMonad<int>.Create(42);
var maybeWithNull = MaybeMonad<string>.Create(null);
var maybeNone = MaybeMonad<double>.None;

// Using MaybeExtensions
var result = maybeWithValue
    .Select(value => value * 2)
    .Where(value => value > 50)
    .Do(Console.WriteLine);

// More examples in the documentation...
```

For detailed usage examples and API documentation, please refer to the [Wiki](https://github.com/your-username/your-repository/wiki).

## Contributing

We welcome contributions! If you encounter issues, have feature requests, or want to contribute to the development of MaybeMonad, please [open an issue](https://github.com/alvmivan-packages/MaybeMonad/issues) or [create a pull request](https://github.com/alvmivan-packages/MaybeMonad/pulls).
## License

This library is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.
