# Anvil Unity Core — Claude Code Rules

These rules apply when working with code that uses the `anvil-unity-core` framework.

## MonoBehaviour Classes (inherit from `AbstractAnvilMonoBehaviour`)

All MonoBehaviour classes should inherit from `AbstractAnvilMonoBehaviour`. This provides:
- `IAnvilDisposable` implementation linked to GameObject destruction
- Lazy-initialized `Logger` property
- `Awake` override for setup

```csharp
public class MyMonoBehaviour : AbstractAnvilMonoBehaviour
{
    // 1. Constants / static readonly
    // 2. Static variables
    // 3. Static functions
    // 4-6. Instance variables (public, protected, private - readonly first)
    // 7. Properties (more common in MonoBehaviours due to no constructor)

    // 8. Awake (setup) - override from AbstractAnvilMonoBehaviour
    protected override void Awake()
    {
        base.Awake();
        // Setup logic here
    }

    // Start (only if absolutely necessary - strongly prefer Awake)
    // private void Start() { }

    // Any Init methods if needed

    // 9. DisposeSelf (teardown) - after setup methods
    protected override void DisposeSelf()
    {
        // Cleanup resources here
        base.DisposeSelf();
    }

    // 10. Other methods - grouped by feature, public first
}
```

### Awake vs Start
Strongly prefer `Awake` over `Start` unless absolutely necessary. `Awake` is called when the component is instantiated, while `Start` is called on the first frame the component is enabled — this delay can introduce timing issues.

## Application Entry Point (inherit from `AbstractAnvilUnityApplication`)

The main application class should inherit from `AbstractAnvilUnityApplication`. Override the `Init()` method as the application entry point.

```csharp
public class MyApplication : AbstractAnvilUnityApplication
{
    protected override void Init()
    {
        // Application startup logic here
    }
}
```

## MonoBehaviour Property Usage

MonoBehaviours will have more Properties than regular classes because readonly variables can't be used without a callable constructor. This is expected and acceptable — use Properties for:
- Triggering additional functionality in setters
- Access control (e.g., public get, private set)
- Exposing calculated values
