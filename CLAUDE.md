# Gary's Battle — Battlerite Clone in s&box

## Project Vision

A scrappy, multiplayer arena brawler inspired by **Battlerite**, built in the **s&box engine** (Source 2, C#). The game features top-down camera, WASD movement, mouse aiming, and ability-based combat (projectiles, dashes, AoE, shields, heals). Target: 2v2 or 3v3 PvP rounds with a shrinking arena.

**Design philosophy:** Lean heavily into s&box's built-in components (PlayerController, Rigidbody, Colliders, SkinnedModelRenderer, etc.) rather than building systems from scratch. Keep it scrappy — working is better than perfect.

## User Context

The developer is **new to game development**. They want to stay in the loop and understand what's being built. **Guide step-by-step** rather than writing everything at once. Explain the "why" behind code decisions. The user prefers hands-on learning.

## Engine: s&box

s&box is a C# game engine built on Valve's Source 2. It uses a **Scene → GameObject → Component** architecture (similar to Unity/Godot).

### Key Concepts
- **Components** inherit from `Component`, use `[Property]` to expose fields in the editor
- **Lifecycle**: `OnAwake` → `OnEnabled` → `OnStart` → `OnUpdate`/`OnFixedUpdate` → `OnDisabled` → `OnDestroy`
- **Coordinate system**: X = forward, Y = left, Z = up
- **Networking**: `[Sync]` for synced properties, `[Rpc.Broadcast]` / `[Rpc.Host]` / `[Rpc.Owner]` for RPCs
- **Input**: `Input.Down("action")`, `Input.Pressed("action")`, `Input.AnalogMove`
- **Scenes** are JSON files, fast to load/switch
- **Hotloading**: Code recompiles in milliseconds on save
- **UI**: Razor-based (.razor files with C# + HTML), styled with .scss
- **API sandbox**: Standard .NET IO is blocked. Use `FileSystem.Data`, `Log.Info()` instead of `Console.Log`

### Full Documentation
The file `SBOX_ENGINE_DOCS.md` (293K chars, 27 sections) contains the complete s&box engine reference scraped from 125 official documentation pages. **Read the relevant section before writing code.** Key sections:
- Section 5: Components (lifecycle, interfaces, events, async)
- Section 8: Networking & Multiplayer (sync, RPCs, ownership, dedicated servers)
- Section 9: Input System
- Section 10: UI System (Razor panels, styling)
- Section 27: API Deep Reference (class hierarchy, all attributes, DamageInfo, traces, sound)

### Common Patterns
```csharp
// Custom component
public sealed class MyComponent : Component
{
    [Property] public float Speed { get; set; } = 200f;
    protected override void OnUpdate()
    {
        WorldPosition += WorldRotation.Forward * Speed * Time.Delta;
    }
}

// Damage system
public sealed class Health : Component, Component.IDamageable
{
    [Property, Sync] public float HP { get; set; } = 100f;
    public void OnDamage( in DamageInfo damage )
    {
        HP -= damage.Damage;
        if ( HP <= 0 ) GameObject.Destroy();
    }
}

// Networked RPC
[Rpc.Broadcast]
public void PlayEffect()
{
    Sound.FromWorld( "fx.hit", WorldPosition );
}

// Raycasting
var tr = Scene.Trace.Ray( start, end ).WithoutTags( "player" ).Run();
if ( tr.Hit ) { /* tr.HitPosition, tr.GameObject, tr.Normal */ }

// Spawning prefabs
var go = MyPrefab.Clone( spawnPosition );
go.NetworkSpawn( connection ); // for multiplayer

// Scene queries
var allEnemies = Scene.GetAll<Health>();
var obj = Scene.Directory.FindByName( "Player" ).First();
```

### Cheat Sheet
| Task | Code |
|------|------|
| Log | `Log.Info( $"Hello {name}" );` |
| Position | `go.WorldPosition = new Vector3(10,0,0);` |
| Create GO | `var go = new GameObject();` |
| Destroy | `go.Destroy();` |
| Clone | `var copy = go.Clone();` |
| Add component | `go.AddComponent<ModelRenderer>();` |
| Get component | `go.GetComponent<ModelRenderer>();` |
| Tags | `go.Tags.Add("player");` / `go.Tags.Has("enemy")` |
| Valid check | `if ( go.IsValid() )` |
| Load scene | `Scene.Load( myScene );` |
| Get all | `Scene.GetAll<CameraComponent>()` |
| Input | `Input.Down("jump")`, `Input.Pressed("attack1")` |
| Analog | `Input.AnalogMove`, `Input.AnalogLook` |
| Time | `Time.Delta` (frame), `Time.FixedDelta` (physics) |
| Rotation | `Rotation.FromYaw(90)`, `Rotation.LookAt(dir)` |
| Lerp | `Vector3.Lerp(a, b, 0.5f)` |
| Sound | `Sound.Play("sounds/hit.sound", WorldPosition);` |
| Mouse visible | `Mouse.Visibility = MouseVisibility.Visible;` |
| Screen to world | `camera.PointToScreenPixels( worldPos )` |

### Key Attributes
`[Property]`, `[Sync]`, `[Sync(SyncFlags.Interpolate)]`, `[Sync(SyncFlags.FromHost)]`, `[Change("callback")]`, `[Rpc.Broadcast]`, `[Rpc.Host]`, `[Rpc.Owner]`, `[RequireComponent]`, `[ConCmd]`, `[ConVar]`, `[Group]`, `[Range]`, `[Title]`, `[Icon]`, `[Category]`

## Current State (What's Built)

### Project Structure
```
Code/
  Assembly.cs          — Global usings (Sandbox, System, etc.)
  TopDownCamera.cs     — Top-down camera + mouse aiming
Editor/
  Assembly.cs          — Editor global usings
  MyEditorMenu.cs      — Default editor menu
Assets/scenes/         — Scene files
ProjectSettings/
  Input.config         — Input bindings (WASD, Attack1/2, etc.)
  Collision.config     — Collision layers
Libraries/
  ozmium.oz_mcp/       — MCP server for AI editor integration
SBOX_ENGINE_DOCS.md    — Complete s&box engine documentation
```

### TopDownCamera.cs (current)
```csharp
using Sandbox;
using Sandbox.Citizen;

public sealed class TopDownCamera : Component
{
    [Property] public GameObject Target { get; set; }
    [Property] public float Height { get; set; } = 500f;
    [Property] public float BackwardOffset { get; set; } = 60f;
    [Property] public bool ShowDebugVectors { get; set; } = false;

    protected override void OnUpdate()
    {
        if ( Target is null ) return;
        Mouse.Visibility = MouseVisibility.Visible;
        FollowTarget();
        AimTargetAtMouse();
    }

    private void FollowTarget()
    {
        var targetPos = Target.WorldPosition;
        var offset = Vector3.Backward * BackwardOffset + Vector3.Up * Height;
        WorldPosition = targetPos + offset;
        WorldRotation = Rotation.LookAt( targetPos - WorldPosition );
    }

    private void AimTargetAtMouse()
    {
        var camera = Scene.Camera;
        if ( camera is null ) return;

        // Camera looks at target, so target ≈ screen center
        var screenCenter = Screen.Size / 2f;
        var playerToCursor = Mouse.Position - screenCenter;

        // Camera basis vectors flattened onto ground plane
        var screenRight = camera.WorldRotation.Right.WithZ( 0 ).Normal;
        var screenUp = camera.WorldRotation.Up.WithZ( 0 ).Normal;

        // Change of basis: screen pixels → world ground direction
        var aimDirection = (screenRight * playerToCursor.x - screenUp * playerToCursor.y).Normal;

        if ( aimDirection.Length > 0.1f )
        {
            Target.WorldRotation = Rotation.LookAt( aimDirection );

            // Sync EyeAngles so swim/climb modes move in the facing direction
            var pc = Target.GetComponent<PlayerController>();
            if ( pc is not null )
                pc.EyeAngles = Target.WorldRotation.Angles();
        }

        // Drive animations manually (UseAnimatorControls is off to prevent rotation fighting)
        var renderer = Target.GetComponentInChildren<SkinnedModelRenderer>();
        if ( renderer is not null )
        {
            var pc = Target.GetComponent<PlayerController>();
            if ( pc is not null )
            {
                var helper = new CitizenAnimationHelper();
                helper.Target = renderer;
                helper.WithVelocity( pc.Velocity );
                helper.IsGrounded = pc.IsOnGround;
                helper.IsSwimming = pc.IsSwimming;
                helper.IsClimbing = pc.IsClimbing;
            }
        }
    }
}
```

### Scene: "First Scene"
- **Main Camera** — CameraComponent + TopDownCamera (Target = Player Controller)
- **Player Controller** — PlayerController, Rigidbody, MoveModeWalk, MoveModeSwim, MoveModeLadder, Dresser, tag: "player"
  - Settings: UseCameraControls=false, UseLookControls=false, UseAnimatorControls=false, UseInputControls=true, RotationAngleLimit=360, RotationSpeed=10
  - **Body** — SkinnedModelRenderer (citizen model)
  - **Colliders** — CapsuleCollider + BoxCollider
- **modular-floor** — Prop (the arena ground)
- **Water** — ModelRenderer + BoxCollider (tagged #water)
- **Sun** — DirectionalLight
- Lights (Point, Spot, Directional)

### .sbproj Config
- Type: game
- TickRate: 50
- MaxPlayers: 64
- GameNetworkType: Multiplayer
- StartupScene: scenes/minimal.scene

### Input Bindings (Input.config)
- Movement: W/A/S/D, Jump=Space, Run=Shift, Duck=Ctrl
- Actions: Attack1=mouse1, Attack2=mouse2, Reload=R, Use=E
- Inventory: Slots 1-9
- Other: View=C, Voice=V, Score=Tab, Menu=Q, Chat=Enter

## Roadmap (What's Next)

### Immediate Next Steps
1. **M1 Projectile** — Create `Projectile.cs` component (moves forward, destroys after lifetime). Spawn on `Input.Pressed("attack1")` in the direction the player faces
2. **Health + Damage** — `Health.cs` component implementing `Component.IDamageable`. Projectiles deal damage on collision using `Component.ITriggerListener`
3. **Space: Dash** — Quick movement burst in facing direction with cooldown
4. **HUD** — Razor panel showing health bar and ability cooldowns

### Later Steps
5. **Multiplayer** — Sync positions, abilities, health. Spawn players on connect via `Component.INetworkListener`
6. **More abilities** — M2 (AoE), Q (shield/counter), E (utility), R (ultimate)
7. **Round system** — Win conditions, respawning, shrinking arena
8. **Arena map** — Proper circular arena with walls and obstacles

## Important Gotchas Learned

### Mouse & Screen Coordinates
- **Mouse.Position vs ScreenPixelToRay**: `Mouse.Position` uses window coordinates but `ScreenPixelToRay` expects viewport coordinates. They don't match in the editor (known bug: [sbox-public#2890](https://github.com/Facepunch/sbox-public/issues/2890)). **Solution**: Use `Screen.Size / 2f` as screen center instead of `camera.PointToScreenPixels()`. Since the camera always looks at the target, the target is always at screen center. This avoids the viewport offset issue entirely.
- **Mouse cursor**: Set `Mouse.Visibility = MouseVisibility.Visible` every frame in OnUpdate to keep cursor visible in a top-down game.

### PlayerController Settings (for top-down games)
- **UseCameraControls** → false (we control the camera)
- **UseLookControls** → false (we control aim via mouse, not FPS mouse-look)
- **UseAnimatorControls** → false (bundles animations AND rotation — can't separate them. We drive animations manually via `CitizenAnimationHelper` instead)
- **UseInputControls** → true (WASD movement still handled by PlayerController)
- **RotationAngleLimit** → 360 (prevents rotation capping, especially in swim mode)
- **RotationSpeed** → 10 (fast rotation snap)

### EyeAngles
- When `UseLookControls` is off, `EyeAngles` stays at (0,0,0). But swim/climb modes use `EyeAngles` to determine movement direction. **Solution**: Manually set `pc.EyeAngles = Target.WorldRotation.Angles()` after setting rotation, so movement modes know which way the player is facing.

### Animations Without UseAnimatorControls
- Use `CitizenAnimationHelper` to drive citizen animations manually:
  ```csharp
  var helper = new CitizenAnimationHelper();
  helper.Target = renderer;  // SkinnedModelRenderer
  helper.WithVelocity( pc.Velocity );
  helper.IsGrounded = pc.IsOnGround;
  helper.IsSwimming = pc.IsSwimming;
  helper.IsClimbing = pc.IsClimbing;
  ```
- Constructor takes 0 arguments. Set `Target` as a property, not a constructor param.

### Coordinate Systems
- **s&box world**: X = forward, Y = left, Z = up. `Vector3.Forward = (1,0,0)`, `Vector3.Up = (0,0,1)`.
- **Screen**: X = right (pixels), Y = down (pixels). Screen Y is inverted vs world forward.
- **Camera basis to screen mapping**: Camera's Right/Up vectors have Z components because the camera is tilted. Use `.WithZ(0).Normal` to flatten them to the ground plane when converting screen directions to world directions.
- **Gizmo.Draw**: Has axes flipped vs game world (X axis is inverted). Debug arrows need per-axis negation corrections to match visual screen directions. Don't use Gizmo arrows to reason about game math — they have their own coordinate quirks.
- **UseAnimatorControls flips axis mapping**: When toggling `UseAnimatorControls`, the correct basis vector for screenRight changes between `camera.WorldRotation.Left` and `camera.WorldRotation.Right`. Current (with UseAnimatorControls=false): uses `.Right`.

### General
- **Save scenes often**: Ctrl+S. Property changes in the editor don't persist without saving.
- **Scene properties reset**: If you don't save, properties like UseLookControls/UseAnimatorControls revert to defaults on editor restart.
