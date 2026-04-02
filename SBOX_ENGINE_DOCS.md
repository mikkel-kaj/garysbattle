# s&box Engine — Complete AI Agent Reference

*Comprehensive documentation for AI agents developing games in the s&box engine.*
*Source: https://sbox.game/dev/doc/ — Auto-generated from 125 documentation pages.*

## Quick Reference for AI Agents

**Engine**: s&box is a C# game engine built on Source 2 (Valve). It uses a Scene/GameObject/Component architecture similar to Unity/Godot.

**Language**: C# (.NET). No scripting languages. Hotloading compiles changes in milliseconds.

**Key Architecture**:
- **Scene** → contains **GameObjects** → which have **Components**
- Games are built by writing custom Components
- Scenes are JSON files, fast to load/switch
- `.sbproj` = project file (JSON)

**Core Patterns**:
```csharp
// Custom component
public sealed class MyComponent : Component
{
    [Property] public float Speed { get; set; } = 200f;

    protected override void OnUpdate()
    {
        if ( Input.Down( "forward" ) )
            WorldPosition += WorldRotation.Forward * Speed * Time.Delta;
    }
}
```

**Component Lifecycle**: `OnAwake` → `OnEnabled` → `OnStart` → `OnUpdate`/`OnFixedUpdate` → `OnDisabled` → `OnDestroy`

**Key Attributes**: `[Property]`, `[Sync]`, `[Authority]`, `[Broadcast]`, `[RequireComponent]`, `[ConCmd]`, `[ConVar]`, `[Group]`, `[Range]`, `[Title]`, `[Icon]`, `[Category]`

**Networking**: Authority-based. Use `[Sync]` for synced properties, `[Broadcast]` for RPCs. The host owns objects by default.

**UI**: Razor-based (like Blazor). `.razor` files with C# + HTML. Styled with `.scss` files.

**API Sandbox**: Standard .NET IO is blocked. Use `FileSystem.Data`, `Log.Info()` instead of `Console.Log`.

**Cheat Sheet — Common Operations**:

| Task | Code |
|------|------|
| Log to console | `Log.Info( $"Hello {name}" );` |
| Get/set position | `go.WorldPosition = new Vector3(10,0,0);` |
| Find by name | `Scene.Directory.FindByName("Cube").First();` |
| Create GO | `var go = new GameObject();` |
| Delete GO | `go.Destroy();` |
| Clone GO | `var copy = go.Clone();` |
| Add component | `go.AddComponent<ModelRenderer>();` |
| Get component | `go.GetComponent<ModelRenderer>();` |
| Add tag | `go.Tags.Add("player");` |
| Check tag | `go.Tags.Has("enemy")` |
| Is valid check | `if ( go.IsValid() )` |
| Load scene | `Scene.Load( myScene );` |
| Get all of type | `Scene.GetAll<CameraComponent>()` |
| Read input | `Input.Down("jump")`, `Input.Pressed("attack1")` |
| Analog input | `Input.AnalogMove`, `Input.AnalogLook` |
| Raycast | `var tr = Scene.Trace.Ray(pos, pos + dir * 1000).Run();` |
| Play sound | `Sound.Play("sounds/hit.sound", WorldPosition);` |
| Time delta | `Time.Delta` (frame), `Time.FixedDelta` (physics) |
| Rotation | `Rotation.FromYaw(90)`, `Rotation.LookAt(dir)` |
| Lerp | `Vector3.Lerp(a, b, 0.5f)`, `Rotation.Slerp(a, b, t)` |

**Math Types**: `Vector2`, `Vector3`, `Vector4`, `Vector2Int`, `Vector3Int`, `Rotation` (quaternion), `Angles` (pitch/yaw/roll), `Transform` (pos+rot+scale), `BBox`, `Color`

---

## Table of Contents

- [1. Overview](#1-overview)
- [2. Project Setup](#2-project-setup)
- [3. Scene System](#3-scene-system)
- [4. GameObjects](#4-gameobjects)
- [5. Components](#5-components)
- [6. GameObjectSystem](#6-gameobjectsystem)
- [7. Prefabs](#7-prefabs)
- [8. Networking & Multiplayer](#8-networking--multiplayer)
- [9. Input System](#9-input-system)
- [10. UI System](#10-ui-system)
- [11. Navigation (AI)](#11-navigation-ai)
- [12. File System](#12-file-system)
- [13. ActionGraph](#13-actiongraph)
- [14. Animation](#14-animation)
- [15. Services](#15-services)
- [16. Storage & UGC](#16-storage--ugc)
- [17. Terrain](#17-terrain)
- [18. Assets](#18-assets)
- [19. Editor Development](#19-editor-development)
- [20. Code Basics](#20-code-basics)
- [21. Advanced Topics](#21-advanced-topics)
- [22. VR](#22-vr)
- [23. Movie Maker](#23-movie-maker)
- [24. Game Exporting](#24-game-exporting)
- [25. Clutter](#25-clutter)
- [26. Post Processing](#26-post-processing)
- [27. API Deep Reference](#27-api-deep-reference-from-source-code--community-docs)

---

## 1. Overview

*s&box engine overview and key concepts*

### About
*Source: https://sbox.game/dev/doc/*


S&box is coded in C#. Under the hood, it uses the Source 2 engine (CS2, HL:Alyx, DOTA2) and some of its systems: rendering, resources, physics, and audio.

When you create games and addons in s&box, you will be creating them in C#.

We have developed a hotload system which is capable of compiling & hotloading your changes to code within a few milliseconds, which negates the need for a scripting language.

Scenes

We use a scene system, similar to Godot and Unity. This allows faster iteration, without everything being code-based. The scene system aims to make how everything works more transparent, by being easily visible, and easily accessible.

Future

Our intention is to let you export the things that you make in our engine and release them standalone. We'll let you do this royalty-free.

Reporting Issues

Issues and feature suggestions should be posted in the sbox-public repo.

Please see Reporting Errors.

Getting Started

Please see First Steps.

---

### Creating a project
*Source: https://sbox.game/dev/doc/about/getting-started/development/*

Creating a game in s&box is easy, but you probably want to know how to do things in the right order.

Creating a project

The first step is to create a game project. Open the s&box Game Editor and the project window will appear. Simply click on New Game Project and fill out the wizard.

The Scene System

We use a scene system to create our games in s&box. We feel this is the easiest system for people to pick up, while still being powerful.

Scenes

A Scene is your game world. Everything that renders and updates in your game at one time should be in a scene. Scenes can be saved and loaded to disk.

GameObject

A scene contains multiple GameObjects. The GameObject is a world object which has a position, rotation and scale. They can be arranged in a hierarchy, so that children GameObjects move relative to their parents.

Component

GameObjects can contain Components. A component provides modular functionality to a GameObject. For example, a GameObject might have a ModelRender component - which would render a model. It might also have a BoxCollider component - which would make it solid.

The game developer ultimately creates games by programming new Components and configuring scenes with GameObjects and Components.

---


## 2. Project Setup

*Creating and configuring projects*

### Play Testbed
*Source: https://sbox.game/dev/doc/about/getting-started/first-steps/*

An engine can seem huge and complicated when you don't know it. The s&box engine is relatively simple.

Here's how we think you should get started exploring the engine.

Getting the s&box editor

The s&box editor and game are avaliable to everyone through the developer preview, you can obtain it here.

To install the s&box editor click here or:

Open Steam
Click on the Library tab
Search for s&box
Install both the game and the editor apps

You can either launch through Steam, use a shortcut to sbox-dev.exe or open your .sbproj flle directly.

Play Testbed

Start s&box (not the editor) and find the game called testbed, this game is used by us to exhibit and test certain engine features, to make sure they work and keep working.

When you enter the game you'll find a menu of scenes. Each scene tests a different engine feature.

Click a scene to enter it, have a play around, press escape to return to the main menu. You can hold escape to completely leave any game.

So here's what you just saw. The menu uses our UI system, which is like HTML with c# inside. It's basically blazor, if you've ever heard of that.

When you clicked on a title, you entered a Scene. Our engine is Scene based, rather than map based like the regular Source Engine. Scenes are json files on disk, and are very fast to load and switch between - just like you experienced.

You probably saw a bunch of cool stuff. Here's some else cool, you can download the source for that game here, which includes all the scenes. Once you download it just open the .sbproj file to open it in the s&box editor, then explore the different scenes in the Asset Browser.

You can edit the scenes and play with them locally to get a feel of how things work.

Create a New Project

The best way to learn is to do. Open the s&box editor and on the welcome screen, choose New Project. Create a Minimal Game project.

Creating Game Objects

Once open you have an empty scene. You can experiment by creating GameObjects by right clicking the tree on the left, and selecting an object type to create.

Creating Components

After that, try to make a GameObject that you can control by creating a custom component by selecting Add Component on the GameObject inspector and typing in a name. The file should open in Visual Studio.

Player Input

Use the Input section of this site to figure out how to read keys, and change the WorldPosition depending on which keys are being pressed.

After that, maybe control the position of the camera too, either by parenting it to your object, or by setting the position directly using Scene.Camera.WorldPosition.

Congratulations - you just learned the basics of GameObjects and Components. You're a game developer now.

Ask questions

If you don't understand something, please ask on the forums or on Discord in the beginner's channel.

The more you ask questions, the more we'll realise that something is confusing, and the more likely we'll be to create documentation or make it simpler.

We can only know if we're doing something shit if you tell us. Please tell us what we're doing wrong.

---


### Addon Project
*Source: https://sbox.game/dev/doc/about/getting-started/project-types/addon-project/*

An addon project adds to a Game Project. The addon project isn't published directly, you create assets and publish those individually.

The general idea is that you are able to use the components and assets from a target game to create assets for that game. The asset could be a map, a model, a material or even a custom resource defined by that game.

You can't make addon projects that contain code yet - but you can use actiongraph

Game Target

In the project settings you're able to select the target game. If you change this game then you must restart the editor for the changes to apply.

Publishing

To publish something made in an Addon Project, you would find it in the asset browser and publish it from there.

From there your map, or model, or whatever will get its own page on sbox.game, and you will be able to configure it.

---


## 3. Scene System

*Scenes, loading, and scene utilities*

### Getting the Current Scene
*Source: https://sbox.game/dev/doc/scene/scenes/*

A scene is a collection of [GameObjects].

Getting the Current Scene

To get the current scene you can use the Scene accessor on any GameObject, Component, or Panel. You can also access it via the static Game.ActiveScene.

Loading a New Scene

To load a new Scene, you can do any of the following:

// Replaces the current scene with the specified scene
Scene.Load( myNewScene );
Scene.LoadFromFile( "scenes/minimal.scene" );

// Additively loads the specified scene on top of the current scene
var load = new SceneLoadOptions();
load.SetScene( myNewScene );
load.IsAdditive = true;
Scene.Load( load );

Directory

All scenes have a GameObject Directory. This holds all the GameObjects in the scene indexed by guid, and enforces that every object's guid is unique.

This also allows fast object lookups if you know the guid of the object.

var obj = Scene.Directory.FindByGuid( guid );

GetAll / Get

The scene also holds a fast lookup index of every component. This allows you to quickly get every component of a certain type without having to keep your own lists, or iterate the entire scene.

// Tint all models a random colour
foreach ( var model in Scene.GetAll<ModelRenderer>() )
{
	model.Tint = Color.Random;
}

// Grab your singleton
var game = Scene.Get<GameManager>();

---

### Simplest Trace
*Source: https://sbox.game/dev/doc/scene/scenes/tracing/*

Scenes can be traced against using Scene.Trace - which uses a builder pattern to make construction a bit easier. At a minimum, traces have a shape, start, and end. You can also filter which specific tags will be hit or ignored, or opt-in to using your project's collision rule matrix by calling WithCollisionRules(tag).

Here are some examples.

Simplest Trace
SceneTraceResult tr = Scene.Trace.Ray( startPos, endPos ).Run();

if ( tr.Hit )
{
	Log.Info( $"Hit: {tr.GameObject} at {tr.EndPosition}" );
}

Use Collision Rules

This will fire a ray using the collision rules of a bullet tag, as configured in your project's Collision settings.

var tr = Scene.Trace
	.Ray( startPos, endPos )
 	.WithCollisionRules( "bullet" ) // Hits everything that a bullet would hit
 	.Run();

Sphere Trace
var tr = Scene.Trace
	.Sphere( 32.0f, startPos, endPos ) // 32 is the radius
	.WithoutTags( "player" ) // ignore GameObjects with this tag
	.Run();

Box Trace
var tr = Scene.Trace
	.Ray( start, end )
	.Size( new BBox( -5, 5 ) ) // size of the aabb
	.UseHitboxes( true ) // hit hitboxes too!
	.Run();

---


## 4. GameObjects

*GameObjects, transforms, tags, hierarchy*

### Transform
*Source: https://sbox.game/dev/doc/scene/gameobject/*

A GameObject represents an object in the scene world. It contains a few different elements.

Transform

Represents where the GameObject is in the scene. Its positioning, its rotation and its scale.

If it has a parent then its transform is held relative to them, so when their parent moves, so does the child.

Here's how you can interact with them in code

// Set world position
GameObject.WorldPosition = new Vector3( 100, 100, 100 );

// Set position relative to parent
GameObject.LocalPosition = new Vector3( 100, 100, 100 );

// Set world transform
GameObject.WorldTransform = new Transform( Vector3.Zero, new Angles( 90, 90, 180 ), 2.0f )

Tags

The GameObject's tags are used for multiple things. They're used to group physics objects to decide what should collide with each other. They can be used by cameras to decide which objects should and shouldn't render. And they can be used by programmers to do whatever they want.

if ( GameObject.Tags.Has( "enemy" ) )
{
	GameObject.Destroy();
}

GameObject.Tags.Add( "enemy" );
GameObject.Tags.Set( "enemy", isEnemy );
GameObject.Tags.Remove( "enemy" );

Tags are inherited. If a parent has the tag, then so does the child. The only way to remove the tag from the child is to remove it from the parent.

Children

GameObject children are available via GameObject.Children. This is just a list of GameObjects.

We should really lock this down a bit more. Make it readonlylist or something.

Components

GameObjects implement functionality using Components.

---


## 5. Components

*Component system - the core of s&box game development*

### Adding Components
*Source: https://sbox.game/dev/doc/scene/components/*

A Component is added to a GameObject to provide functionality. This functionality can vary wildly.

You could add a component that renders a model at the GameObject's position. Or you could add a component that created a physics object at the GameObject's position.

You can create your own components too. This is how games are programmed. For example, you could write a component that moved an object forward when the Forward key is held down.

Adding Components

To add a component to a GameObject in editor, select the GameObject and then click on Add Component in the inspector.

To add a component to a GameObject in code

// Create
var modelRenderer = go.AddComponent<ModelRenderer>();

// Set up
modelRenderer.Model = Model.Load( "models/dev/box.vmdl" );
modelRenderer.Tint = Color.Red;

You can also GetOrAddComponent if you want it to exist if it doesn't already.

// Get or create
var modelRenderer = go.GetOrAddComponent<ModelRenderer>();

// Set up
modelRenderer.Model = Model.Load( "models/dev/box.vmdl" );
modelRenderer.Tint = Color.Green;

Querying Components

You can query a GameObject for components in multiple different ways.

// Get a multiple components of the same type
var x = go.GetComponents<ModelRenderer>();

// Get a single component from a gameobject
var x = go.GetComponent<ModelRenderer>();

// Get a single component from a gameobject, and its children
var x = go.GetComponentInChildren<ModelRenderer>();

// Get all components from a gameobject, and its children
var x = go.GetComponentsInChildren<ModelRenderer>();

// Get all components from a gameobject's ancestors and itself
var x = go.Components.GetComponentsInParent<ModelRenderer>();

// Get a single component from a gameobject's ancestors and itself
var x = go.Components.GetComponentInParent<ModelRenderer>();

Specialized Queries

If you're wanting to go even more granular:

// Get disabled components in ancestors
var x = go.Components.Get<ModelRenderer>( FindMode.Disabled | FindMode.InAncestors );

// Get all enabled components in ancestors and self
var x = go.Components.GetAll<ModelRenderer>( FindMode.Enabled | FindMode.InAncestors | FindMode.InSelf );

// Get all components on a gameobject
var x = go.Components.GetAll();

Component References

You can get component references as variables in two main ways.

// Creates a property in the inspector, you can drag any ModelRenderer from the scene in to reference it
[Property] ModelRenderer BodyRenderer { get; set; }

// References the first ModelRenderer on the same GameObject as this component, or creates one if none exist
[RequireComponent] ModelRenderer BodyRenderer { get; set; }

Removing Components

To remove a component from a GameObject, you call DestroyComponent(). You cannot reuse this component - at this point it is destroyed forever and you should stop using it.

var depthOfField = GetComponent<DepthOfField>();
dephOfField.Destroy();

Destroying GameObject from Component

DestroyGameObject(), nice and easy. You can also use GameObject.Destroy() if you want.

---

### OnLoad (async)
*Source: https://sbox.game/dev/doc/scene/components/component-methods/*

When creating a component there are a number of methods you can override and implement.

Note that for the component be enabled, its GameObject and all of their ancestors need to be enabled too. The GameObject will be considered disabled if one of its ancestor GameObjects is not enabled.

OnLoad (async)

This is called after deserialization and is meant for a place for the component to "load". When loading a scene, the loading screen will stay open and the game won't start until all components OnLoad tasks are complete.

If your component is doing something special, such as generating a procedural level, you can override this on your component to do this in the loadscreen.

protected override async Task OnLoad()
{
	LoadingScreen.Title = "Loading Something..";
	await Task.DelayRealtimeSeconds( 1.0f );
}

Internally this is where the Map component downloads and loads the map.

OnValidate

Called whenever a property is changed in the editor, and after deserialization.

A good place to enforce property limits etc.

OnAwake

Called once when the component is created, but only if our parent GameObject is enabled. This is called after deserialization and loading.

OnStart

Called when the component is enabled for the first time. Should always get called before the first OnFixedUpdate.

OnEnabled

Called when the component is enabled.

OnUpdate

Called every frame

OnPreRender

==This method is not called on dedicated servers.==

Called every frame, right before rendering is about to take place.

This is called after animation bones have been calculated, so it usually a good place to do things that count on that.

OnFixedUpdate

Called every fixed timestep.

In general, it's wise to use a fixed update for things like player movement (the built in Character Controller does this). This reduces the amount of traces a client is doing every frame, and if your client is too performant, the move deltas per frame can be so small that they create problems.

OnDisabled

Called when the component is disabled.

OnDestroy

Called when the component is destroyed.

---

### ExecuteInEditor
*Source: https://sbox.game/dev/doc/scene/components/component-interfaces/*

There are various interfaces that can be given to components for specific purposes.

ExecuteInEditor

A component marked with ExecuteInEditor will also execute these methods in edit mode:

OnAwake
OnEnabled
OnDisabled
OnUpdate
OnFixedUpdate
Sample code
public sealed class ExecuteInEditorSample : Component, Component.ExecuteInEditor
{
	protected override void OnEnabled()
	{
		base.OnEnabled();

		if ( Game.IsEditor )
		{
			Log.Error( "OnEnabled is also executed in editor" );
		}
	}
}

ICollisionListener

A component with this interface can react to physics collisions.

Method	Description
OnCollisionStart	Called when this collider/rigidbody starts touching another collider.
OnCollisionUpdate	Called once per physics step for every collider being touched.
OnCollisionStop	Called when this collider/rigidbody stops touching another collider.
Sample code
public sealed class CollisionListenerSample : Component, Component.ICollisionListener
{
	public void OnCollisionStart( Collision other )
	{
		Log.Error( "Collision started with: " + other.Other.GameObject );
	}

	public void OnCollisionUpdate( Collision other )
	{
		Log.Error( "Collision continued with: " + other.Other.GameObject );
	}

	public void OnCollisionStop( CollisionStop other )
	{
		Log.Error( "Collision stopped with: " + other.Other.GameObject );
	}
}

ITriggerListener

A component with this interface can react to trigger interactions.

Method	Description
OnTriggerEnter	Called when a collider enters the trigger.
OnTriggerExit	Called when a collider stops touching the trigger.
Sample code
public sealed class TriggerListenerSample : Component, Component.ITriggerListener
{
	public void OnTriggerEnter( Collider other )
	{
		Log.Error( "Trigger entered with: " + other.GameObject );
	}

	public void OnTriggerExit( Collider other )
	{
		Log.Error( "Trigger exited with: " + other.GameObject );
	}
}

IDamageable

A helper interface to mark components that can be damaged by something.

Method	Description
OnDamage	The method you invoke when damaging something marked with IDamageable
Sample code
public sealed class SampleDamageable : Component, Component.IDamageable
{
	public void OnDamage( in DamageInfo damage )
	{
		Log.Error( $"I got damaged for {damage.Damage} by {damage.Attacker}" );
	}
}

public sealed class ClickToDamage : Component
{
	protected override void OnUpdate()
	{
		base.OnUpdate();

		if ( Input.Pressed( "attack1" ) )
		{
			var ray = Components.Get<CameraComponent>().ScreenPixelToRay( Mouse.Position );
			var trace = Scene.Trace.Ray( ray, 5000f ).Run();
			if ( trace.Hit )
			{
				var damageable = trace.GameObject.Components.Get<IDamageable>();

				if ( damageable != null )
				{
					damageable.OnDamage( new DamageInfo()
					{
						Damage = 12,
						Attacker = GameObject,
						Position = trace.HitPosition,
					} );
				}
			}
		}
	}
}

INetworkListener

A component with this interface can react to 📆 Network Events.

INetworkSpawn

A component with this interface can react to 📆 Network Events

---

### Flowchart
*Source: https://sbox.game/dev/doc/scene/components/execution-order/*

You should not rely on the order in which the same callback methods get invoked for different GameObjects, it is not predictable. If you need more control, you should use a [GameObjectSystem].

Flowchart

The flow chart shows the order of execution for a Scene, component methods are executed at the same time for all components. There are some internal methods added to make context clearer.

---

### Event Interface
*Source: https://sbox.game/dev/doc/scene/components/events/*

You can broadcast and listen to events in your scene using interfaces.

These events aren't sent over the network. They are sent to active Components and GameObjectSystems in your scene.

Event Interface

An event class is just a regular interface. You don't need to do anything else.

public interface IPlayerEvent
{
	void OnSpawned( Player player );
}

// Run an event telling everyone that playerThatSpawned has spawned
Scene.RunEvent<IPlayerEvent>( x => x.OnSpawned( playerThatSpawned ) );

You can, however, derive from ISceneEvent<T>. This gives you a bit nicer syntax. Internally this is just calling Scene.Run on the active scene.

public interface IPlayerEvent : ISceneEvent<IPlayerEvent>
{
	void OnSpawned( Player player );
}

IPlayerEvent.Post( x => x.OnSpawned( playerThatSpawned ) );

You can also post events to specific GameObjects with PostToGameObject, instead of every object in the scene.

IPlayerEvent.PostToGameObject( player.GameObject, x => x.OnSpawned( player ) );

If your event interface has many events and you don't want to have to implement them all, you can define defaults.

public interface IPlayerEvent : ISceneEvent<IPlayerEvent>
{
	void OnSpawned( Player player ) { }

	void OnJump( Player player ) { }
	void OnLand( Player player, float distance, Vector3 velocity ) { }
	void OnTakeDamage( Player player, float damage ) { }
	void OnDied( Player player ) { }
	void OnWeaponAdded( Player player, BaseWeapon weapon ) { }
	void OnWeaponDropped( Player player, BaseWeapon weapon ) { }

	void OnCameraMove( Player player, ref Angles angles ) { }
	void OnCameraSetup( Player player, CameraComponent camera ) { }
	void OnCameraPostSetup( Player player, CameraComponent camera ) { }
}

Broadcasting

Scene.RunEvent is the entry point to broadcast an event. This isn't limited to interfaces.. here's some interesting stuff you can do.

// tint all skinnedmodelrenderer's to red
Scene.RunEvent<SkinnedModelRenderer>( x => x.Tint = Color.Red );

// allow all listeners to change a value
float damage = 100.0f;
Scene.RunEvent<IPlayerDamageMesser>( x => x.ModifyDamage( player, damageinfo, ref damage ) );

// collect values
List<Vector3> damagePoints = new ();
Scene.RunEvent<IDamageProvider>( x => x.GetDamagePoint( damagePoints ) );

Listening

To listen, you just implement the interface you want to use. This could be an interface you have created, or could be one of the built in event classes.

// A component with the ISceneLoadingEvents interface, 
// for listening to scene load events.
public class MyComponent : Component, ISceneLoadingEvents
{
	void ISceneLoadingEvents.AfterLoad( Scene scene )
	{
		// Called after scene has loaded
	}
}

//
// A camera component weapon, which listens to IPlayerEvent.
//
public class CameraWeapon : BaseWeapon, IPlayerEvent
{
	void IPlayerEvent.OnCameraMove( Player player, ref Angles angles )
	{
        // If the right mouse button down, stop them moving the view by moving the mouse
        // because we're going to be zooming in and out using right mouse.
        if ( Input.Down( "attack2" ) )
		{
			angles = default;
		}
	}
}

These events aren't available on Panels yet.

---

### Examples
*Source: https://sbox.game/dev/doc/scene/components/events/iscenestartup/*

This event interface allows you to listen to Scene startup events. These events are run in three places:

In editor, when pressing play
In game, when loading a game
In game, when joining a server
public interface ISceneStartup : ISceneEvent<ISceneStartup>
{
	void OnHostPreInitialize( SceneFile scene );
	void OnHostInitialize();
	void OnClientInitialize();
}

OnHostPreInitialize

Called before the scene is loaded on the host. The scene is empty at this point.

Note that you'll only really ever see this called on GameObjectSystems. This is because at this point no Components exist in the scene.

OnHostInitialize

Called after the scene is loaded on the host. You can now access and modify all of the GameObjects and Components in the Scene.

This is a good place for the host to spawn common things. For example, if you have a common Camera prefab, this is a good place to spawn it.

OnClientInitialize

Called after the scene is loaded, on both the host and client. This won't be called on a Dedicated Server.

This is a good place to spawn clientside only things.

Don't forget to mark these things as not networked - because otherwise if your client is also the lobby host, the scene will be snapshotted and sent to clients, along with whatever you spawn here.

We're using the term host here, which can be easily misinterpreted. By host we are referring to the computer in charge of the game. This could be the player in a singleplayer game, a dedicated server host, or a host in a lobby. Basically anyone but a client connected to a server.

Examples
public sealed class MyGameManager : GameObjectSystem<GameManager>, ISceneStartup
{
	public MyGameManager( Scene scene ) : base( scene )
	{
	}

	void ISceneStartup.OnHostInitialize()
	{
		//
		// We don't have a menu, but if we did we could put something in the menu
		// scenes that we'd now be able to detect, and skip doing the stuff below.
		//

		//
		// Spawn the engine scene.
		// This scene is sent to clients when they join.
		//
		var slo = new SceneLoadOptions();
		slo.IsAdditive = true;
		slo.SetScene( "scenes/engine.scene" );
		Scene.Load( slo );

		// If we're not hosting a lobby, start hosting one
		// so that people can join this game.
		Networking.CreateLobby();
	}
}

---

### Iscenephysicsevents
*Source: https://sbox.game/dev/doc/scene/components/events/iscenephysicsevents/*

This interface allows you to wrap logic around the physics step. This can be useful when you have an object, or a system, that works closely with the physics system.

public interface IScenePhysicsEvents : ISceneEvent<IScenePhysicsEvents>
{
	/// <summary>
	/// Called before the physics step is run. This is called pretty much
	/// right after FixedUpdate.
	/// </summary>
	void PrePhysicsStep() { }

	/// <summary>
	/// Called after the physics step is run
	/// </summary>
	void PostPhysicsStep() { }
}

---

### Igameobjectnetworkevents
*Source: https://sbox.game/dev/doc/scene/components/events/igameobjectnetworkevents/*

Allows a GameObject's Components to listen to changes in ownership state.

public interface IGameObjectNetworkEvents : ISceneEvent<IGameObjectNetworkEvents>
{
	/// <summary>
	/// Called when the owner of a network GameObject is changed
	/// </summary>
	void NetworkOwnerChanged( Connection newOwner, Connection previousOwner ) { }

	/// <summary>
	/// We have become the controller of this object, we are no longer a proxy
	/// </summary>
	void StartControl() { }

	/// <summary>
	/// This object has become a proxy, controlled by someone else
	/// </summary>
	void StopControl() { }
}

This event is only targetted at a single GameObject - the one that is changing.

---

### Using Async
*Source: https://sbox.game/dev/doc/scene/components/async/*

By default, async tasks in s&box run on the main thread. They operate a lot like coroutines, making them the perfect replacement.

Using Async

To make a method asynchronous, you do it like this..

async Task PrintSomething( float waitSeconds, string message )
{
	// wait for this amount of seconds
	await Task.DelaySeconds( waitSeconds );

	// Print it
	Log.Info( message );
}

Components have a special Task property with some extra helper functions (like DelayRealtimeSeconds).

As you can see, if a task is async, you can await it.

async Task LerpSize( float seconds, Vector3 to, Easing.Function easer )
{
	TimeSince timeSince = 0;
	Vector3 from = WorldScale;

	while ( timeSince < seconds )
	{
		var size = Vector3.Lerp( from, to, easer( timeSince / seconds ) );
		WorldScale = size;
		await Task.Frame(); // wait one frame
	}
}

await LerpSize( 3.0f, Vector3.One * 3.3f, Easing.BounceOut );
await LerpSize( 1.0f, Vector3.One * 4.0f, Easing.EaseInOut );
await LerpSize( 1.0f, Vector3.One * 3.0f, Easing.EaseInOut );

Multiple Async

Tasks can orchestrate and do multiple tasks at once. This feels like multithreading but it's not.

async Task DoMultipleThings()
{
	// notice no await here
	Task taskOne = PrintSomething( 2.0f, "One" );
	Task taskTwo = PrintSomething( 3.0f, "Two" );

	// wait for both these tasks
	await Task.WhenAll( taskOne, taskTwo );
}

Returning Values

Async Tasks can return values too.

async Task<string> GetKanyeQuote()
{
	string kanyeQuote = await Http.RequestStringAsync( "https://api.kanye.rest/" );

	kanyeQuote = kanyeQuote.Replace( "music", "poosic" );

	return kanyeQuote;
}

async Task PrintKanyeQuote()
{
	string quote = await GetKanyeQuote();
	Log.Info( $"KANYE SAID: {quote}" );
}

Cooperating with synchronous code

This is all cool, but how do you call these async functions from your regular functions?

protected override void OnEnabled()
{
	// here the _ just tells the compiler that we don't care about the task
	_ = DoMultipleThings();
}

But what if from synchronous code you want to use the value?

protected override void OnEnabled()
{
    // Will run async and run this Action when the task finishes
    GetKanyeQuote().ContinueWith( task => Log.Info( $"Kanye: {task.Result}" ) );
}

But what if I want to do it more stupidly?

Task<string> getQuoteTask;

protected override void OnEnabled()
{
	getQuoteTask = GetKanyeQuote();
}

protected override void OnUpdate()
{
	if ( getQuoteTask is not null && getQuoteTask.IsCompletedSuccessfully )
	{
		Log.Info( $"Kanye: {getQuoteTask.Result}" );
		getQuoteTask = null;
	}
}

Being Responsible

Something to be thinking of is what happens when your GameObject is destroyed or disabled while you're waiting.

When implementing things yourself you should be considerate of this.. the async method isn't guaranteed to stop just because the GameObject or Component is gone.

We do somewhat handle this internally, when awaiting a method in Component.Task we will automatically cancel the task if the GameObject turns invalid.

Common Errors

A common async error is letting tasks stack up.

For example, if you have a system where a user presses space, it waits a second, then shoots a button.. you need to handle a user pressing that button multiple times during that second. You need to handle the user dying during that second.

Maybe you want to not launch a new async task if the user firing task is running. Maybe you want to cancel the firing task and start it again (use a CancellationToken maybe).

---


## 6. GameObjectSystem

*Scene-level systems for batch processing*

### Implementation
*Source: https://sbox.game/dev/doc/scene/gameobjectsystem/*

A scene can contain systems that need to do work in specific places during the frame.

For example, one of these systems is the SceneAnimationSystem, which finds all SkinnedModelRenderer components and works out all of their animations in parallel. This is faster than doing it in Update() and avoids weird out of order problems. It gives us a single point in the frame where we know for sure that all of the bone positions are up to date.

Implementation

To create your own system, you just define a new class that derives from GameObjectSystem.

public class MyGameSystem : GameObjectSystem
{
	public MyGameSystem( Scene scene ) : base( scene )
	{
		Listen( Stage.PhysicsStep, 10, DoSomething, "DoingSomething" );
	}

	void DoSomething()
	{
		Log.Info( "Did something!" );

        var allThings = Scene.GetAllComponents<MyThing>();

        // do something to all of the things
	}
}

When a scene is created, a copy of every defined GameObjectSystem is created and added to it, you don't need to do anything else.

Access

You can access a GameObjectSystem in a number of ways. One way is using Scene.Get<MyGameSystem>()

You can also inherit from GameObjectSystem<T> which adds a static T Current property to your system.

public class MyGameSystem : GameObjectSystem<MyGameSystem>
{
  	public MyGameSystem( Scene scene ) : base( scene )
	{
	}

    public void MyMethod()
    {
        Log.Info( "Hello, World!" );
    }
}

Then you can use them like…

MyGameSystem.Current.MyMethod();

Stages & Order

Stages are based around certain events. For example, the PhysicsStep stage is called when ticking the physics, during FixedUpdate.

The order defines where to call your method during that event. -1 would call it before, +1 would call it after. You will be defining the systems, so getting them in an order you can live with is your business.

Configuration

GameObjectSystems have two methods of configuration. You can either globally configure them, or edit them per scene.

In Project Settings, hit Systems — and you can configure them there.

Any property marked with [Property] will be configurable in project settings and saved.

---


## 7. Prefabs

*Reusable GameObject templates*

### Assets
*Source: https://sbox.game/dev/doc/scene/prefabs/*

A prefab is a GameObject that can be used in multiple places. They're usually used to contain an object that is used across multiple scenes, or needs to be instantiated at runtime.

Assets

Prefabs are saved to disk as [PrefabFile]. These assets can be referenced in Components anywhere a GameObject can be referenced. When the PrefabFile is updated, all instantiations of the prefab in scenes are updated, too.

To create a PrefabFile, right-click on a GameObject in the scene and select Convert to Prefab.

In Scene

In the scene view, GameObjects that are instantiations of Prefabs are made obvious by their colour.

When in their Prefab Instantiation state, they can't be edited significantly. You can't view or select objects in their hierarchy.

If you want to edit a prefab in the scene you can right-click it and choose Unlink from Prefab to change it to a bunch of normal GameObjects.

In Code

To spawn Prefabs at runtime via code, you treat them like a regular GameObject. A GameObject property on your Component can be populated by dragging a PrefabFile into it.

public sealed class MyGun : Component
{
	[Property] 
	GameObject BulletPrefab { get; set; }

	protected override void OnUpdate()
	{
        // throw an error if BulletPrefab wasn't defined
        Assert.NotNull( BulletPrefab );

		if ( Input.Pressed( "Attack1" ) )
		{
            // create a new instance of the bullet prefab at the gun's position
			GameObject bullet = BulletPrefab.Clone( WorldPosition );

			// bullet is now in the current scene, what do you want to do with it?
			// maybe get components and set the velocity or something?
		}
	}
}

Note that a cloned prefab will still be linked to the prefab. You can call bullet.BreakFromPrefab() to remove that link and have it appear as a normal stack of GameObjects if you want to.

---

### Visual Indicators
*Source: https://sbox.game/dev/doc/scene/prefabs/instance-overrides/*

Prefab instance overrides allow you to customize individual instances of a prefab without affecting the original prefab or other instances. This lets you create variations of the same prefab while maintaining the connection to the original template.

When you modify a property, add a component, or change the hierarchy of a prefab instance, these changes are stored as overrides. The instance remembers what's different from the original prefab while still receiving updates when the prefab itself changes.

Visual Indicators

In the scene hierarchy, prefab instances with overrides are clearly marked to show their modified state.

Overridden properties and components are highlighted in the inspector, making it easy to see what's been customized on each instance.

Types of Overrides
Property Overrides

Change any property value on GameObjects or Components within the prefab instance. Position, rotation, scale, component properties, and GameObject settings can all be overridden.

Component Additions

Add new components to GameObjects within the prefab instance. These components only exist on this specific instance.

GameObject Additions

Add new child GameObjects to the prefab instance hierarchy. These children are unique to this instance.

Managing Overrides

The inspector and scene hierarchy provides controls to manage overrides on individual properties and objects:

Reverting Overrides

Right-click on any overridden property or object and select Revert Override to restore the original prefab value. You can also revert all overrides on a GameObject or the entire prefab instance.

Applying Overrides

To make your instance changes permanent, right-click and select Apply to Prefab. This updates the original prefab with your changes, affecting all other instances.

Nested Prefabs

When working with prefabs that contain other prefabs (nested prefabs), overrides work hierarchically. Changes to nested prefab instances are stored on the outermost prefab instance.

This ensures that all override data is centralized and properly managed even in complex prefab hierarchies.

---

### Prefab Templates
*Source: https://sbox.game/dev/doc/scene/prefabs/prefab-templates/*

If you want to add your own templates to the GameObject Create menu, it's as simple as enabling "Show In Menu" on a Prefab File:

Now you'll see your template as one of the options in the Create menu

You can make it look nicer by fiddling with the other variables, and even throw it in a sub-menu:

The final option is whether or not the prefab should be treated as a template. Templates will always break the prefab when created, otherwise the prefab reference will be maintained:

---


## 8. Networking & Multiplayer

*Multiplayer, sync, RPCs, authority, dedicated servers*

### Overview
*Source: https://sbox.game/dev/doc/systems/networking-multiplayer/*

The networking system in s&box is purposefully simple and easy. Our initial aim isn't to provide a bullet proof server-authoritative networking system. Our aim is to provide a system that is really easy to use and understand.

Overview

Here's a quick cheat sheet for the network system, to get you started.

Create a new lobby
Networking.CreateLobby( new LobbyConfig()
{
  MaxPlayers = 8,
  Privacy = LobbyPrivacy.Public,
  Name = "My Lobby Name"
} );

List all available lobbies
list = await Networking.QueryLobbies();

Join an existing lobby
Networking.Connect( lobbyId );

Enable GameObject Networking

Destroy Networked GameObject
go.Destroy();

Instantiating a Networked GameObject
var go = PlayerPrefab.Clone( SpawnPoint.Transform.World );
go.NetworkSpawn();

RPCs
[Rpc.Broadcast]
public void OnJump()
{
	Log.Info( $"{this} Has Jumped!" );
}

---


### Supported Types
*Source: https://sbox.game/dev/doc/systems/networking-multiplayer/sync-properties/*

Sync Attribute

Adding the [Sync] attribute to a property on a Component will have its latest value sent to other players each time it changes.

public class MyComponent : Component
{
  [Sync] public int Kills { get; set; }
}

These properties are controlled by the owner of the object, therefore only the owner of the object can change them.

Supported Types

[Sync] properties support unmanaged types, and string. You can't synchronize every class with them, but any value type including structs will be fine. int, bool, Vector3, float are all examples of valid types. We also support serializing specific classes such as GameObject, Component, GameResource.

Detecting Changes

You can detect changes to a [Sync] property by also applying a the [Change] attribute to it. With this attribute you can specify the name of a callback method that will be invoked when the value of the property has changed.

Right now the [Change] attribute will not invoke the callback when a collection has changed. The callback will only be invoked when the property itself is assigned to something different.

public class MyComponent : Component 
{
  [Sync, Change( "OnIsRunningChanged" )] public bool IsRunning { get; set; }

  private void OnIsRunningChanged( bool oldValue, bool newValue )
  {
    // The value of IsRunning has changed...
  }
}

Sync Flags

You can customize the behaviour of a synchronized property with SyncFlags.

Flag	Description
SyncFlags.Query	Enables Query Mode for the property. See the Query Mode section below.
SyncFlags.FromHost	The host has ownership over the value, instead of the owner of the networked object. Only the host may change the value.
SyncFlags.Interpolate	The value of the property will be interpolated for other clients. The value is interpolated over a few ticks.
Collections

Sometimes you want to network collections such as an entire list or a dictionary. We provide special classes to do that.

public enum AmmoCount
{
  Pistol,
  Rifle
}

public class MyComponent : Component 
{
  [Sync] public NetList<int> List { get; set; } = new();
  [Sync] public NetDictionary<AmmoCount,int> Dictionary { get; set; } = new();
}

You can initialize each in the declaration with new() or you can initialize the lists elsewhere, so long as you're doing so on the Owner of the network object. It doesn't matter if they are null for anyone else because they'll get created when they are networked if they need to be.

You can use NetList<T> and NetDictionary<K,V> like their regular counterparts. They contain indexers, Add, Remove and other methods you'd expect.

NetList and NetDictionary do not currently support the [Property] attribute.

Query Mode

By default the properties are automatically marked dirty when set, via codegen magic.. meaning that when you set a property, if it's different we'll send the updated value to everyone.

[Sync]
public Vector3 Velocity
{
	get;
	set;
}

No Query Mode needed. The only way to change Velocity is via the setter, which when called will mark it as changed using invisible codegen magic on the setter.

Vector3 _velocity;
[Sync]
public Vector3 Velocity
{
	get => _velocity;
	set => _velocity = value;
}

Again - no Query Mode needed. The only way we're setting _velocity is via the setter - so it can never get out of date.

Vector3 _velocity;
[Sync( SyncFlags.Query )]
public Vector3 Velocity
{
	get => _velocity;
	set => _velocity = value;
}

void SetVelocity( Vector3 val)
{
    _velocity = val;
}

Query Mode needed - because when you call SetVelocity it changes _velocity and the network system doesn't know that the Velocity value has changed. This could be avoided in that case by setting Velocity instead of _velocity.

With SyncFlags.Query, the variable is instead checked for changes every network update, and sent if changed. This is marginally slower than non-query mode, but it means that you can sync special stuff like this.

---

### Example
*Source: https://sbox.game/dev/doc/systems/networking-multiplayer/rpc-messages/*

Components can contain RPCs. An RPC is a function that when called, is called remotely too.

Supported RPC arguments are the exact same as Sync properties.

Example

Imagine your game has a button, and you want it to make a bing noise when it's pressed. You could have a function like this.

void OnPressed()
{
	Sound.FromWorld( "bing", WorldPosition );
}

The problem here is, that sound is only played on the host, or on the client where OnPressed is called. You want everyone to hear that sound. So you instead do something like this.

void OnPressed()
{
	PlayOpenEffects();
}

[Rpc.Broadcast]
public void PlayOpenEffects()
{
	Sound.FromWorld( "bing", WorldPosition );
}

The attribute [Rpc.Broadcast]makes it so when you call that function, it broadcasts a network message to everyone to call that function too.

Static RPC

Static methods can be RPCs, too. A static RPC does not need to exist on a Component but can exist as a method on any static class.

[Rpc.Broadcast]
public static void PlaySoundAllClients( string soundName, Vector3 position )
{
	Sound.Play( soundName, position );
}

Rpc.Owner

Unlike [Rpc.Broadcast] which calls the function for everybody, you can use [Rpc.Owner] instead which means that the function will only be called for the Owner of the networked object or the host if the object has no owner.

Rpc.Host

Similarly to Rpc.Owner, adding this will mean the function is only called on the Host.

Flags

When defining an RPC, you can define a number of flags.

[Rpc.Broadcast( NetFlags.Unreliable | NetFlag.OwnerOnly )]
public static void PlaySoundAllClients( string soundName, Vector3 position )
{
  // ...
}

Name	Description
NetFlags.Unreliable	Message will be sent unreliably. It may not arrive and it may be received out of order. But chances are that it will arrive on time and everything will be fine. This is good for sending position updates, or spawning effects. This is the fastest way to send a message. It is also the cheapest.
NetFlags.Reliable	This is the default, so you don't need to specify this. Message will be sent reliably. Multiple attempts will be made until the recipient has received it. Use this for things like chat messages, or important events. This is the slowest way to send a message. It is also the most expensive.
NetFlags.SendImmediate	Message will not be grouped up with other messages, and will be sent immediately. This is most useful for things like streaming voice data, where packets need to stream in real-time, rather than arriving with a bunch of other packets.
NetFlags.DiscardOnDelay	Message will be dropped if it can't be sent quickly. Only applicable to unreliable messages.
NetFlag.HostOnly	This RPC can only be called from the Host.
NetFlag.OwnerOnly	This RPC can only be called from the owner of the object it's being called on.
Arguments

You can pass arguments to the RPC like any other method, and they'll get passed magically.

void OnPressed()
{
	PlayOpenEffects( "bing", WorldPosition );
}

[Rpc.Broadcast]
public void PlayOpenEffects( string soundName, Vector3 position )
{
	Sound.FromWorld( soundName, position );
}

Filtering

You can filter the recipients of a Broadcast RPC. This allows you to exclude specific connections from receiving the RPC, or only include specific connections.

// Don't send the RPC to player's called Harry (sorry Harry!)
using ( Rpc.FilterExclude( c => c.DisplayName == "Harry" ) )
{
	PlayOpenEffects( "bing", WorldPosition );
}

// Only send the RPC to player's called Garry.
using ( Rpc.FilterInclude( c => c.DisplayName == "Garry" ) )
{
	PlayOpenEffects( "bing", WorldPosition );
}

Caller Information

You can check which connection called the method using the Rpc.Caller class.

void OnPressed()
{
	PlayOpenEffects( "bing", WorldPosition );
}

[Rpc.Broadcast]
public void PlayOpenEffects( string soundName, Vector3 position )
{
	if ( !Rpc.Caller.IsHost ) return;

	Log.Info( $"{Rpc.Caller.DisplayName} with the steamid {Rpc.Caller.SteamId} played open effects!" );
	Sound.FromWorld( soundName, position );
}

---

### Default Owners
*Source: https://sbox.game/dev/doc/systems/networking-multiplayer/ownership/*

Networked GameObjects can be owned by a connection.

Objects that are owned by a connection are simulated by that connection. Their position and their variables are all controlled by the controlling client.

If an object is unowned by a connection then it is simulated by the host.

Owner Transfer

By default, a networked object's owner can only be changed by the host. However, the current owner of the object can change that behavior by setting its OwnerTransfer value.

// Make it so anyone can change the owner of this networked object.
go.Network.SetOwnerTransfer( OwnerTransfer.Takeover );

Type	Behaviour
OwnerTransfer.Takeover	Anyone can change the owner
OwnerTransfer.Fixed (default)	Only the host can change the owner
OwnerTransfer.Request	A request must be made to the host to change the owner
Getting the Owner

You can find the owner of a GameObject by checking Network.OwnerId.

public override void Update()
{
	Log.Info( $"Owner is {Network.OwnerId}" );
}

In reality, day to day, you won't really be interested in the particular owner. You only care about whether you're meant to be simulating it or not. You do that by checking IsProxy - which is true if the GameObject is being simulated by another client (or the server).

public override void Update()
{
    // this is controlled by someone else
    if ( IsProxy ) return;

    // if we pressed E, try to pick something up
    if ( Input.Pressed( "use" ) )
    {
        TryPickup();
    }
}

Taking Ownership

You can take ownership of an object, which makes you the simulator.

void TryPickup()
{
	// are we looking at anything?
	var tr = Physics.Trace.WithoutTags( "player" )
			.Sphere( 16, EyePos, EyePos + LookDir.Forward * 100 )
			.Run();

	if ( !tr.Hit ) return;

	if ( tr.Body.GameObject is not GameObject go )
		return;

	if ( !go.Tags.Has( "pickup" ) )
		return;

    // You're my wife now
	go.Network.TakeOwnership();

    // Store that we're carrying it 
	Carrying = go;
}

So for example, if a player picks up an object, the player could take ownership of that object. That way the position is controlled by that player.

If a player gets in a car, you could make the player the owner of that car - then its position will be controlled by that player.

And of course, the player is generally controlled by its own connection.

Dropping Ownership

You can also stop owning an object. At that point the object becomes owned by the server.

void ThrowObject()
{
	if ( !Carrying.IsValid() )
		return;

	// Stop owning this
	Carrying.Network.DropOwnership();
	Carrying = null;
}

Default Owners

If you make an object in the scene editor networked, by default it will not have an owner. It will be simulated by the host.

When a client creates a network object via GameObject.NetworkSpawn(), they will be the owner of that object.

Disconnection

By default all networked objects that a client owns will be destroyed for everyone when they disconnect. You can change this by setting the Orphaned Mode for that networked object. Only the current owner can change the Orphaned Mode.

Simply call GameObject.Network.SetOrphanedMode with one of the following options:

Type	Behaviour
NetworkOrphaned.Destroy (default)	The object will be destroyed when the owner disconnects
NetworkOrphaned.Host	The host will be assigned ownership when the current owner disconnects
NetworkOrphaned.Random	A random client will be assigned ownership when the current owner disconnects
NetworkOrphaned.ClearOwner	The object will remain but ownership will be cleared (the host will simulate the object)

---

### Spawning Objects
*Source: https://sbox.game/dev/doc/systems/networking-multiplayer/connection-permissions/*

The host can change some permissions for a specific Connection. The ideal place to set these permissions would be in the OnActive network event.

Spawning Objects

You can set Connection.CanSpawnObjects to allow or disallow a specific connection to create their own networked objects. By default this is true.

Refreshing Objects

By default only the host can send network refresh updates for networked objects. This can be changed to allow the owner of a networked object to also send these updates with Connection.CanRefreshObjects.

---


### Example
*Source: https://sbox.game/dev/doc/systems/networking-multiplayer/network-events/#h-inetworklistener/*

Your games will likely want to react to people joining and leaving the game. To help with this you can implement Component.INetworkListener or Component.INetworkSpawn on a component and place it in the scene.

Example

Here's an example, where on joining the game a player object is created and the incoming player is assigned as the owner.

public sealed class GameNetworkManager : Component, Component.INetworkListener
{
	[Property] public GameObject PlayerPrefab { get; set; }
	[Property] public GameObject SpawnPoint { get; set; }

	/// <summary>
	/// Called on the host when someone successfully joins the server (including the local player)
	/// </summary>
	public void OnActive( Connection connection )
	{
		// Spawn a player for this client
		var player = PlayerPrefab.Clone( SpawnPoint.Transform.World );

		// Find the NameTag component and set their name correctly
		var nameTag = player.Components.Get<NameTagPanel>( FindMode.EverythingInSelfAndDescendants );
		if ( nameTag is not null )
		{
			nameTag.Name = connection.DisplayName;
		}

		// Spawn it on the network, assign connection as the owner
		player.NetworkSpawn( connection );
	}
}

INetworkListener

The interface INetworkListener has multiple methods that you can optionally override.

Method	Description
OnConnected	The client has connected to the server. They're about to start handshaking, in which they'll load the game and download all the required packages.
OnDisconnected	The client has disconnected from the server.
OnActive	The client is fully connected and completely the handshake. After this call they will close the loading screen and start playing.
INetworkSpawn

The interface INetworkSpawn has a method to react to objects spawning on the network.

Method	Description
OnNetworkSpawn	Called when this object is spawned on the network.

---

### Creating a server
*Source: https://sbox.game/dev/doc/systems/networking-multiplayer/network-helper/*

To make a multiplayer game you need to take care of a few things. There's a special component that helps with those things, called NetworkHelper. This is a simple component that fits a lot of situations, but can be used as an example to code your own network component.

Creating a server

If the StartServer property is enabled, a server will automatically be created when the scene is loaded. That is unless the network system is already active (because you're joining a server using this scene).

Player Spawning

When a player enters a server you need to create an object for them to control. If it's a racing game, you'd spawn them a car to control. If it's a shooter game, you'd spawn them a player.

Generally this is done using a prefab. You define your player gameobject and create a prefab, then you can drag the prefab object into the PlayerPrefab property on the component.

You can also define a list spawnpoint GameObject's. The player will spawn randomly on one of them. If you don't define any spawn points, they will spawn at the location of the NetworkHelper object.

Player Object

Your player object will usually contain a component with a function like this, which controls the GameObject if it isn't a Proxy.

	protected override void OnUpdate()
	{
		// If we're a proxy then don't do any controls
		// because this client isn't controlling us!
		if ( IsProxy )
			return;

		// direction keys are pressed
		if ( !Input.AnalogMove.IsNearZeroLength )
		{
			WorldPosition += Input.AnalogMove.Normal * Time.Delta * 100.0f;	
		}

		// position the camera
		var camera = Scene.GetAllComponents<CameraComponent>().FirstOrDefault();
		camera.WorldRotation = new Angles( 45, 0, 0 );
		camera.WorldPosition  = WorldPosition  + camera.WorldRotation.Backward * 1500;
	}

Under The Hood

NetworkHelper works by implementing Component.INetworkListener. This interface contains a method that is called when a connection becomes active on the server.

In this method we create an instance of the PlayerPrefab and set the new client as its owner. The client receives the information about this new object, and the fact that they're the owner, and takes over from there.

---

### Network Visibility & Culling
*Source: https://sbox.game/dev/doc/systems/networking-multiplayer/network-visibility/*

Network Visibility & Culling

Network Visibility controls whether a networked object should be visible for a specific player (Connection). Visibility determines whether the object receives ongoing network updates — such as Sync Vars and Transform updates — for that client.

By default, all networked objects always transmit to all Connections, unless you explicitly disable this behaviour.

Always Transmit

Every networked object has flag called Always Transmit.

Default: AlwaysTransmit = true
When true, the object never gets culled and is visible to every player.

This is the simple default for beginners, but for larger or more complex games, disabling Always Transmit can enable performance benefits by culling objects that the player should not receive updates for.

INetworkVisible Interface

You can take control of visibility by attaching a Component to the root GameObject of a networked object that implements Component.INetworkVisible.

Only the owner of a networked object decides visibility for each connection.

public class MyVisibilityComponent : Component, INetworkVisible
{
    public bool IsVisibleToConnection( Connection connection, in BBox worldBounds )
    {
        // Your visibility logic here…
        return true;
    }
}

IsVisibleToConnection Parameters
Parameter	Description
Connection connection	The target player being tested.
BBox worldBounds	The object's world-space bounding box. Helpful for distance or frustum checks.

Return true if the object should be visible to that connection; false if it should be culled.

To enable this behaviour, disable AlwaysTransmit on the root network object.

Hammer PVS Integration

If no component implementing INetworkVisible exists on the root GameObject and the map is a Hammer map with VIS compiled:

The engine automatically falls back to PVS (Potentially Visible Set).
Visibility is determined using Hammer's visibility data.

This is an ideal default for static world objects on Hammer maps.

What Happens When an Object Is Culled?

When the owner decides an object is not visible to a connection:

The following stop being sent:
Sync Var updates
Transform updates
The following still occur:
The object still spawns for the client.
The object becomes Disabled locally for that client while hidden.
RPCs are still delivered.

Invisible objects remain known to the client, just not updated.

---

### Writing Snapshot Data
*Source: https://sbox.game/dev/doc/systems/networking-multiplayer/custom-snapshot-data/*

In some circumstances you may want to add additional data to the network snapshot that gets sent to clients when they join a server. One example of this might be serializing and deserializing voxel world data.

Components in the Scene can write and read their own data during the snapshot sending and receiving process by implementing Component.INetworkSnapshot.

Writing Snapshot Data

To write snapshot data for a Component you can simply override the WriteSnapshot method on a component.

  	private byte[] MyVoxelData { get; set; }

	void INetworkSnapshot.WriteSnapshot( ref ByteStream writer )
	{
        writer.Write( MyVoxelData.Length );
		writer.WriteArray( MyVoxelData );
	}

Reading Snapshot Data

Reading snapshot data can be done by overriding ReadSnapshot on a component. You can return a Task here to have the loading screen wait for this to be done before continuing.

	void INetworkSnapshot.ReadSnapshot( ref ByteStream reader )
	{
        var length = reader.Read<int>();
        MyVoxelData = reader.ReadArray<byte>( length ).ToArray();
	}

    protected override Task OnLoad()
    {
        await LoadVoxelWorld( MyVoxelData );
    }

	private Task LoadVoxelWorld( byte[] data )
	{
		// ...
	}

---

### Connection and Messaging
*Source: https://sbox.game/dev/doc/systems/networking-multiplayer/websockets/*

s&box allows you to use WebSockets to interface with an external server. Common usages include custom networking, or persistent data outside of a local filesystem.

Connection and Messaging

Adding this Component to a GameObject in the Scene will ensure the connection is started when the game starts.

public sealed class Server : Component
{
	// Example: wss://host.example:443/ws
	[Property] public string ConnectionUri { get; set; }
	public WebSocket Socket { get; set; }

	protected override void OnStart()
	{
		Socket = new WebSocket();
		Socket.OnMessageReceived += HandleMessageReceived;
		_ = Connect();
	}

	// Connect to the server and send a message
	private async Task Connect()
	{
		await Socket.Connect( ConnectionUri );
		await SendMessage( "Hello!" );
	}

	private async Task SendMessage( string message )
	{
		await Socket.Send( message );
	}

	// Log our received messages
	private void HandleMessageReceived( string message )
	{
		Log.Info( message );
	}
}

Using Auth Tokens

You could attach an Auth Token to your WebSocket request header like so:

var token = await Sandbox.Services.Auth.GetToken( "YourServiceName" );

if ( string.IsNullOrEmpty( token ) )
{
	// Unable to fetch a valid session token
	return;
}

var headers = new Dictionary<string, string>()
{
	{ "Authorization", token }
};

await socket.Connect( "ws://localhost:8080", headers );

---

### Http Requests
*Source: https://sbox.game/dev/doc/systems/networking-multiplayer/http-requests/*

s&box provides a static Http class, this lets you easily create asynchronous HTTP requests of different methods (GET, POST, DELETE, etc…) providing JSON content and parsing JSON responses.

Allowed URLs

You can only use http orhttps URLs to domains (no IP addresses), and to prevent abuse, localhost is permitted only on ports 80/443/8080/8443.

The command line switch -allowlocalhttp will let you access any local URL from the server.

Cheat Sheet

Some common things you might want to do…

// GET request that returns the response as a string
string response = await Http.RequestStringAsync( "https://google.com" );

// POST request of JSON content ignoring any response
await Http.RequestAsync( "https://api.facepunch.com/my/method", "POST", Http.CreateJsonContent( playerData ) );

---

### New Instance
*Source: https://sbox.game/dev/doc/systems/networking-multiplayer/testing-multiplayer/*

The number one best way to test multiplayer is to have someone join your game.. but that's obviously not always possible.

New Instance

For this reason you can spawn another instance of the game, which will join your currently running session.

To do this, click on the network status icon in the header bar, and select Join via new instance.

A new instance of the game will appear and join your game.

Iterating

You can continue to code on your main instance, with the game running and the other instance joined. The code changes will be mirrored to the other client. In fact, they'll be mirrored to all clients - so even if you have a friend join, their game will update.

Reconnect

If you need to reconnect, you can do this via the reconnect command.

Joining manually

You can open an instance and manually join your local editor session by running connect local in the console.

---

### Installation
*Source: https://sbox.game/dev/doc/systems/networking-multiplayer/dedicated-servers/*

Installation

You can find information about how to install and use SteamCMD here https://developer.valvesoftware.com/wiki/SteamCMD to install the s&box Dedicated Server.

Once SteamCMD has been installed, you can install or update the s&box Server by running the following command in Windows Terminal from the directory you installed SteamCMD.

./steamcmd +login anonymous +app_update 1892930 validate +quit

You can use -beta staging to host a server on the staging branch, this might not be playable by everyone though.

Running the Server

Once installed, the default directory would be steamcmd/steamapps/common/Dedicated Server and you can create a simple .bat file there that will start your server. Here's an example, you could create a file called Run-Server.bat that looks like this:

echo off
sbox-server.exe +game facepunch.walker garry.scenemap +hostname My Dedicated Server

When run, this will load the facepunch.walker game with the garry.scenemap map and the title would be My Dedicated Server.

Configuration

You can run the server with the following available command line parameters. These are just essentially a ConVar or ConCmd that is run when the server boots up.

You can pass a path to a .sbproj file to load a local project on a Dedicated Server. Connected clients will receive code changes and hotload them.

Switch	Arguments	Description
+game	<packageIdent> [mapPackageIdent]	The game package to load and optionally a map package.
+hostname	<name>	The server title that players will see.
+net_game_server_token	<token>	**This is not required and is only available as an option once s&box is released.**Visit https://steamcommunity.com/dev/managegameservers to generate a token associated with your Steam Account. You can use this token to ensure your Dedicated Server always has the same Steam ID for other players to connect to it. You don't need this, but otherwise every time you load the server it will generate a new Steam ID.

---

### What is it?
*Source: https://sbox.game/dev/doc/systems/networking-multiplayer/dedicated-servers/serverside-code/*

Serverside Code only works when running local projects - please read that page first.

What is it?

When compiling a project, if we're a dedicated server, we can run code in if #SERVER blocks.

protected override void OnUpdate()
{
#if SERVER
  Log.Info( $"This is a server update!" );
#else
  Log.Info( $"This is a client update!" );
#endif
}

If we're running on a dedicated server, we'll get This is a server update! spamming our console.

This is especially useful for omitting potentitally sensitive code from client builds, like contacting an API server, or a database.

.Server.cs files

Say you have a GameManager.cs file, which has a partial class, you can accompany it with a GameManager.Server.cs file which is safely omitted from client builds and wraps the whole file in an '#if SERVER block - it's a nice bit of sugar that saves you having a bunch of preprocessor blocks in your code.

What if I publish my game?

All serverside code is stripped out of published games by default - so there's no worries there.

---

### What about Serverside Code?
*Source: https://sbox.game/dev/doc/systems/networking-multiplayer/dedicated-servers/running-local-projects/*

When running a dedicated server - you have the option of running an entirely local project as the game. I.e:

+game c:/git/sbox-mygame/.sbproj

Assets will still be fetched from the cloud version of the game - and stuff that is missing will be sent directly from the server to clients, including code.

What about Serverside Code?

This is the only opportunity for running serverside code which can be safely hidden from clients. When publishing a game, we'll strip out serverside code - this gives you the control, via your own servers.

---

### User Permissions
*Source: https://sbox.game/dev/doc/systems/networking-multiplayer/dedicated-servers/user-permissions/*

On a Dedicated Server, you can specify who is an admin of your server, or customize permissions even further with claims for individual Steam accounts.

Users

On your Dedicated Server, you can edit the users/config.json file to add specific permissions per user. The default config demonstrates how these are set up.

[

	/* You can give a Steam account specific permissions here using their Steam Id. */

	{
		"SteamId": "00000000000000000",
		"Claims": [ "kick", "ban", "restart" ],
		"Name": "Example"
	},

	{
		"SteamId": "00000000000000000",
		"Claims": [ "kick", "ban", "restart" ],
		"Name": "Example"
	}

]

Claims

Claims are strings which describe actions that a user can take. You can add your own custom claims, as they're just strings.

The host can check if a specific Connection has a permission with Connection.HasPermission( string ). By default, the host has all permissions.

Permissions are not networked right now, so only the host can check if a connection has a specific permission.

---


## 9. Input System

*Keyboard, mouse, controller input*

### Custom Keys
*Source: https://sbox.game/dev/doc/systems/input/*

Input is accessed using the… Input class!

public sealed class MyPlayerComponent : Component
{
	protected override void OnUpdate()
	{
		if ( Input.Down( "jump" ) )
		{
			WorldPosition += Vector3.Forward * Time.Delta;
		}
	}
}

Here you can see that when jump is pressed down, the GameObject will move forward. We multiply forward by the time delta to make it frame rate independent.

There are a few different ways to query buttons..

Input.Down( "jump" ) // key is held down (button name is case insensitive)
Input.Pressed( "jump" ) // key was just pressed this frame
Input.Released( "jump" ) // key was just released this frame

Input.AnalogMove // joystick "move" input Vector3 (or wsad)
Input.AnalogLook // Joystick "look" input Vector3 (mouse look)

Custom Keys

You can customize the keys for your game in the Project Settings.

Escape Key

By default, s&box will show the pause menu when a player presses the ESC key in-game.
You can override this functionality:

// In Update() on one of your components
if ( Input.EscapePressed )
{
	Input.EscapePressed = false;
	// handle escape pressed in your game
}

If you override the escape button it'll be up to you to let the user access settings etc in your own menu system.
n

---

### Analog Inputs
*Source: https://sbox.game/dev/doc/systems/input/controller-input/*

Some Input methods are specific to Controllers/Gamepads, and are useful to know for making sure your player experience caters to those who don't play on a Keyboard + Mouse.

To see if the player is using a Controller, you can check Input.UsingController

Analog Inputs

You can get direct analog inputs from the Left Stick, Right Stick, Left Trigger or Right Trigger. Keep in mind that not all Controllers have analog triggers.

float moveX = Input.GetAnalog( InputAnalog.LeftStickX );
float aimAmount = Input.GetAnalog( InputAnalog.LeftTrigger );

There is also Input.AnalogMove and Input.AnalogLook which are Vector3 and Angles respectively that automatically map to both the Keyboard + Mouse and the Left and Right Stick.

Haptics/Rumble

You can directly set the vibration intensity of each motor (and how long the vibration should last)

float leftMotor = 0.5f; // 50% intensity
float rightMotor = 0.7f; // 70% intensity
float leftTrigger = 0.2f; // 20% intensity
float rightTrigger = 0f; // No vibration
int duration = 1000; // 1000 milliseconds == 1 second

Input.TriggerHaptics( leftMotor, rightMotor, leftTrigger, rightTrigger, duration );

Or you can use one of a few presets, specifying the length, frequency and intensity

// These are optional inputs
float lengthScale = 1.2f; // 1.2x longer vibration time
float frequencyScale = 2f; // 2x vibration frequency
float amplitudeScale = 0.5f; // 0.5x as intense of a rumble

Input.TriggerHaptics( HapticEffect.HardImpact, lengthScale, frequencyScale, amplitudeScale );

You can also force all haptics to stop using Input.StopAllHaptics()

Motion Controls

If the Controller has a gyroscope or an accelerometer, then you can get motion data, otherwise it will be null.

InputMotionData motionData = Input.MotionData;
if(motionData is not null)
{
  Vector3 acceleration = motionData.Acceleration; // Accelerometer
  Vector3 angularVelocity = motionData.AngularVelocity; // Gyroscope
  // Process the data as needed...
}

Local Multiplayer

You can see how many Controllers are currently connected using Input.ControllerCount

With that count you could create a Player GameObject for each index, and then query their Inputs as normal

int playerIndex = 0; // The index of the controller this player is using

using( Input.PlayerScope( playerIndex ) )
{
  // Any input handling within this code block will only query Controller index 0
  if( Input.Pressed( "Jump" ) )
  {
    // Make Player 1 jump (or whichever index)
  }
}

---

### Raw Input
*Source: https://sbox.game/dev/doc/systems/input/raw-input/*

We very much recommend that you only use this if you can't use input actions for whatever reason. You can't rebind these at all.

We have the ability to access keyboard inputs directly, bypassing input actions.

//
// Is the W key down this frame?
//
if ( Input.Keyboard.Down( "W" ) )
{
    Log.Info( "W is down!" );
}

//
// Was the W key pressed this frame?
//
if ( Input.Keyboard.Pressed( "W" ) )
{
    Log.Info( "W was pressed!" );
}

//
// Was the W key released this frame?
//
if ( Input.Keyboard.Released( "W" ) )
{
    Log.Info( "W was released!" );
}

Most of the keys on the keyboard should work, here's an exhaustive list of them.

Key	Name
"0" - "9"	0 - 9
"a" - "z"	A - Z
"KP_0" - "KP_9"	Numpad 0 - Numpad 9
"KP_DIVIDE"	Numpad /
"KP_MULTIPLY"	Numpad *
"KP_MINUS"	Numpad -
"KP_PLUS"	Numpad +
"KP_ENTER"	Numpad Enter
"KP_DEL"	Numpad Delete
"<"	Less Than
">"	More Than
"["	Left Bracket
"]"	Right Bracket
"SEMICOLON"	Semicolon
" ' "	Apostrophe
" ` "	Backtick / Console Key
","	Comma
"."	Period
"/"	Slash
"\\"	Backslash
"-"	Hyphen / Minus
"="	Equals
"ENTER"	Enter
"SPACE"	Space
"BACKSPACE"	Backspace
"TAB"	Tab
"CAPSLOCK"	Caps Lock
"NUMLOCK"	Num Lock
"ESCAPE"	Escape
"SCROLLLOCK"	Scroll Lock
"INS"	Insert
"DEL"	Delete
"HOME"	Home
"END"	End
"PGUP"	Page Up
"PGDN"	Page Down
"PAUSE"	Pause
"SHIFT"	Left Shift
"RSHIFT"	Right Shift
"ALT"	Left Alt
"RALT"	Right Alt
"UPARROW"	Up Arrow
"LEFTARROW"	Left Arrow
"RIGHTARROW"	Right Arrow
"DOWNARROW"	Down Arrow

---

### Glyphs
*Source: https://sbox.game/dev/doc/systems/input/glyphs/*

Input glyphs are an easy way to show users which buttons to press for actions, they automatically adjust for whatever device you're using and return appropriate textures.

Texture JumpButton = Input.GetGlyph( "jump" );

Glyphs can change from users rebinding keys, or switching input devices - so it's worth it just grabbing them every frame.

You can also choose between the default and outlined versions of glyphs, like so:

Texture JumpButton = Input.GetGlyph( "jump", true );

To use these quickly and easily in razor, you can use the resulting texture directly in an <Image> panel:

<Image Texture="@Input.GetGlyph( "jump", InputGlyphSize.Medium, true )" />

Examples

---


## 10. UI System

*Razor panels, styling, HUD*

### Making a C# Panel
*Source: https://sbox.game/dev/doc/systems/ui/*

Our UI system is structured around Panels. A Panel is a c# class that can have parent and child panels.

The panels use a stylesheet and flex system for layout and rendering.

They can be created directly in code or via Razor files with HTML/CSS syntax.

Making a C# Panel

Here's a basic example of a Panel that simply displays Time.Now:

public class MyPanel : Panel
{
  public Label MyLabel { get; set; }

  public MyPanel()
  {
    MyLabel = new Label();
    MyLabel.Parent = this;
  }

  public override void Tick()
  {
    MyLabel.Text = $"{Time.Now}";
  }
}

Using a Panel

Once you've create a Panel, it can be used in two different ways. The first is done identically to how we added a Label in the example above:

var myPanel = new MyPanel();
myPanel.Parent = this;

The other is by using the following syntax within a [Razor Panels] file:

<MyPanel />

Creating a C# Root Panel

To draw your UI to either the Screen or to a World Panel, you'll need to create a PanelComponent. A PanelComponent acts as the root of all UI, and is added to any GameObject with either a ScreenPanel or WorldPanel component. Here's an example of how you can create a basic one including the above panel:

public sealed class MyRootPanel : PanelComponent
{
	MyPanel myPanel;

	protected override void OnTreeFirstBuilt()
	{
		base.OnTreeFirstBuilt();

		myPanel = new MyPanel();
		myPanel.Parent = Panel;
	}
}

Scaling

By default, ScreenPanels will rescale all UI based on a 1080p target height automatically. If you wish to disable this, or change the scaling to target the Desktop Resolution, you can change the following:

---

### Creating a new PanelComponent
*Source: https://sbox.game/dev/doc/systems/ui/razor-panels/*

UI can also be created using Razor, which allows you to use web-like HTML/CSS to create and style each Panel while also being able to leverage C#.

The panels are created and rendered exactly the same way. They don't use a HTML renderer. This syntax is a convenience.

Creating a new PanelComponent

Every single UI consists of a PanelComponent at the root, with normal Panels being the children/content within. The PanelComponent can be added to any GameObject that also has either a ScreenPanel or WorldPanel component, and it will be drawn to whichever it has.

When you create a new Component, select "New Razor Panel Component" to get the following:

@using Sandbox;
@using Sandbox.UI;
@inherits PanelComponent

<root>
	<div class="title">@MyStringValue</div>
</root>

@code
{

	[Property, TextArea] public string MyStringValue { get; set; } = "Hello World!";

	/// <summary>
	/// the hash determines if the system should be rebuilt. If it changes, it will be rebuilt
	/// </summary>
	protected override int BuildHash() => System.HashCode.Combine( MyStringValue );
}

HTML/Razor

Anything within the <root> tags is treated as HTML, allowing you to inject C# as-needed, like so:

<root>
  @foreach(var player in Player.All)
  {
    <div class="player">
      <label>@player.Name</label>
      @if(player.IsDead)
      {
        <img src="ui/skull.png" />
      }
    </div>
  }
</root>

If no <root> element is present, then any and all elements will be a child of your Panel's root automatically.

You can also return early in Razor if you don't want it to render anything beyond the specified line.

BuildHash

A Panel's contents will only be rebuilt if the value returned from BuildHash() has changed from the previous value, so make sure to include certain values here to ensure the Panel updates when necessary.

A Panel's contents will also rebuild if if has pointer-events and the cursor enters/exits/clicks the Panel.

You can also force a rebuild by calling StateHasChanged(). This will queue the rebuild for the next frame.

Creating a new Panel

Since a PanelComponent is the root, any child elements are instead Panels. These are created in the exact same way PanelComponents are, but instead inheriting the Panel class (which .razor files do by default):

@using Sandbox;
@using Sandbox.UI;

<root>
  <div class="health">HP: @Health</div>
  <div class="armor">Armor: @Armor</div>
</root>

@code
{
  public int Health { get; set; } = 100;
  public int Armor { get; set; } = 100;

  protected override int BuildHash() => System.HashCode.Combine( Health, Armor );
}

How to Use

Back in your PanelComponent (or any other Panel), you can simply use the panel like any other element:

<MyChildPanel />

And can even pass variables to the Panel like so:

<MyChildPanel Health=@(30) Armor=@(75) />

How to store a Panel Reference

If you wish to store a reference to MyChildPanel so you can modify its properties in code, you can do this:

<root>
  <MyChildPanel @ref="PanelReference" />
</root>

@code
{
  MyChildPanel PanelReference { get; set; }

  protected override void OnStart()
  {
    PanelReference.Health = Player.Local.Health;
    PanelReference.Armor = Player.Local.Armor;
  }
}

Two Way Binds

Sometimes you want to bind a variable to a control, and if it changes, sync the value back. That's what this is.

You create a two way bind using :bind after the attribute name:

<SliderEntry min="0" max="100" step="1" Value:bind=@IntValue></SliderEntry>

@code
{
	public int IntValue{ get; set; } = 32;
}

The example above will update the Slider when IntValue changes, and IntValue when the Slider changes

Differences between Panel and PanelComponent

Since a Panel is not a Component, you cannot override OnStart, OnUpdate, ect.
Instead Panel has OnAfterTreeRender(bool firstTime) and Tick().

Since PanelComponent is not a Panel, you have to access it's Panel via the PanelComponent.Panel accessor.
This means where you can do Style.Left = Length.Auto on a Panel, you have to do
Panel.Style.Left = Length.Auto on a PanelComponent.

This also means you cannot do <MyPanelComponent /> within another Panel/PanelComponent, they must be added to a GameObject with a ScreenPanel or WorldPanel.

---

### Creating a Razor Component
*Source: https://sbox.game/dev/doc/systems/ui/razor-panels/razor-components/*

Razor Component allow you to define some content to show inside another razor file. This makes it easy to create Panels that are more agile and reusable.

Creating a Razor Component

Here's an example called InfoCard.razor:

<root>
    <div class="header">@Header</div>
    <div class="body">@Body</div>
    <div class="footer">@Footer</div>
</root>

@code
{
    public RenderFragment Header { get; set; }
    public RenderFragment Body { get; set; }
    public RenderFragment Footer { get; set; }
}

Using a Razor Component

We can now very quickly and easily create Info Cards, and even add events/functions that interact with the parent Panel:

<root>
	<InfoCard>
        <Header>My Card</Header>
        <Body>
            <div class="title">This is my card</div>
        </Body>
        <Footer>
            <div class="button" onclick="@CloseCard">Close Card</div>
        </Footer>
    </InfoCard>
</root>

@code 
{
    void CloseCard()
    {
        Delete();
    }
}

Elements inside Panels

All Panels have a render fragment called ChildContent, so you can add elements inside of any Panel without adding your own fragments:

<InfoCard>
  <ChildContent>
    <label>Something inside of InfoCard</label>
  </ChildContent>
</InfoCard>

Razor Components With Arguments

RenderFragment<T> is used to define a fragment that takes an argument. Here's PlayerList.razor:

@using Sandbox;

<root>
    @foreach ( var player in PlayerList )
    {
        <div class="row">
            @PlayerRow( player )
        </div>
    }
</root>

@code
{
    public List<Player> PlayerList { get; set; }
    public RenderFragment<Player> PlayerRow { get; set; }
}

As you can see, we're now defining PlayerRow which takes an argument. To call it we just call it like a function, with the Player argument.

Now to use it we do this:

@using Sandbox;

<root>
    <PlayerList PlayerList=@MyPlayerList>
        <PlayerRow Context="item">
            @if (item is not Player player)
                return;

            <div class="row">@player.Name</div>
            <div class="row">@player.Health</div>
        </PlayerRow>
    </PlayerList>
</root>

@code 
{
    List<Player> MyPlayerList;
}

In the PlayerRow, we add the special attribute Context, which tells our code generator that this is going to need a special object context.

Then in the fragment, we can convert this object into our target type. Then you're free to use it as you wish.

Generics

You can make a generic Panel class by using @typeparam to define the name of the parameter

@typeparam T

<root>
    This is my class for @Value
</root>

@code
{
    public T Value { get; set; }
}

You will then be able to use this like a regular generics class in C#, like PanelName<string> ect.

In Razor, you will need to add a T attribute to define the type like so:

<root>
	<MyPanel T="string"></MyPanel>
</root>

---

### Using Stylesheets
*Source: https://sbox.game/dev/doc/systems/ui/styling-panels/*

Using Stylesheets

It is common to have a file system like this:

Health.razor
Health.razor.scss

If you do this, the scss file is automatically included by your Health.razor panel.

If you want to specify a different location for the loaded Stylesheet, you can add this to your Panel class:

[StyleSheet("main.scss")]

You can also import a stylesheet from within another stylesheet like so:

@import "buttons.scss";

Styling Directly

There are a few ways to style your Panels without a stylesheet. It's really recommended that you use a stylesheet to keep things organized, but there are also valid reasons to use the following methods.

Styling the Element

You can directly style any element just like you can in HTML, but can inject C# when necessary:

<label style="color: red">DANGER!</label>
<div class="progress-bar">
  <div class="fill" style="width: @(Progress * 100f)%"></div>
</div>

Style Block

Before or after your <root> element, you can add a <style> block that is read just like a Stylesheet:

<style>
  MyPanel {
    width: 100%;
    height: 100%;
  }
  .hp { color: red; }
  .armor { color: blue; }
</style>

Styling in Code

You can a Panel's Style directly and modify the values however you'd like via Tick() or OnUpdate():

myPanel.Style.Width = Length.Percent(Progress * 100f);

---

### Style Properties
*Source: https://sbox.game/dev/doc/systems/ui/styling-panels/style-properties/*

We try to keep as close to standard web styles as possible - but not every property is implemented. We'll use this page to highlight any differences.

Anything marked in bold* may behave differently to how you'd expect on web.

Common Types
Name	Description	Examples
Float	A standard float	flex-grow: 2.5;
String	A string with or without quotes	font-family: Poppins;,content: "Back to Menu";
int	A standard int	font-weight: 600;
Color	Can have alpha	color: #fff;,color: #ffffffaa;,color: rgba(red, 0.5);
Length	A dimension, pixel or relative.	left: 10px;,left: 10%;,left: 10em;,left: 10vw;,mask-angle: 10deg;
Custom Style Properties
Name	Parameters	Examples / Notes
aspect-ratio	Float, Float (Optional)	aspect-ratio: 1;,aspect-ratio: 16/9;
background-image-tint	Color	Multiplies the background-image by this Color. Not a replacement for filter or backdrop-filter.
border-image-tint	Color	Multiplies the border-image by this Color.
mask-scope	default / filter	default will apply the mask normally, filter will use the mask to blend between unfiltered and filtered.
sound-in	String	The name of the sound to play when this style is applied to an element. This is useful to put on a :hover or :active style to play a sound on hover/click
sound-out	String	The name of a sound to play when this style is removed from an element.
text-stroke	Length, Color	This will put an outline
text-stroke-color	Color	
text-stroke-width	Length	
Supported Style Properties
Name	Parameters	Examples / Notes
align-content	autoflex-endflex-startcenterstretchspace-betweenspace-aroundbaseline	
align-items	Same as align-content	
align-self	Same as align-content	
animation	Fills in the properties below	
animation-delay	Float	
animation-direction	normal (default) reverse alternate alternate-reverse	
animation-duration	Float	
animation-fill-mode	none forwardsbackwardsboth	
animation-iteration-count	int / infinite	
animation-name	String	
animation-play-state	runningpaused	
animation-timing-function	linear (default) ease ease-in ease-out ease-in-out	
backdrop-filter	blur(Length) <br>saturate(Length) <br>contrast(Length) <br>brightness(Length) <br>grayscale(Length) <br>sepia(Length) <br>hue-rotate(Length) <br>invert(Length) <br>border-wrap(Length, Color)	backdrop-filter: blur(10px) saturate(80%);
backdrop-filter-blur	Length	
backdrop-filter-brightness	Length	
backdrop-filter-contrast	Length	
backdrop-filter-hue-rotate	Length	
backdrop-filter-invert	Length	
backdrop-filter-saturate	Length	
backdrop-filter-sepia	Length	
background	Fills in the properties below	
background-angle	Length	
background-blend-mode	normal lighten multiply	
background-color	Color	
background-image	url(string) <br>linear-gradient(Color, Color) <br>radial-gradient(Color, Color) <br>conic-gradient(Color, Color)	
background-position	Length, Length (optional)	background-position: 10px``background-position: 10px 15px
background-position-x	Length	
background-position-y	Length	
background-repeat	no-repeat repeat-x repeat-y repeat	
background-size	Length, Length (optional)	background-size: 10px background-size: 10px 15px
background-size-x	Length	
background-size-y	Length	
border	border-width, border-style, border-color	border: 10px solid black;
border-bottom	Same as border	
border-bottom-color	Color	
border-bottom-left-radius	Length	
border-bottom-right-radius	Length	
border-bottom-width	Length	
border-color	Color	
border-image	Same as background-image	
border-image-tint	Color	
border-image-width-bottom	Length	
border-image-width-left	Length	
border-image-width-right	Length	
border-image-width-top	Length	
border-left	Same as border	
border-left-color	Color	
border-left-width	Length	
border-radius	Length	border-radius: 8px;<br>border-radius: 8px 0px 8px 8px;
border-right	Same as border	
border-right-color	Color	
border-right-width	Length	
border-top	Same as border	
border-top-color	Color	
border-top-left-radius	Length	
border-top-right-radius	Length	
border-top-width	Length	
border-width	Length	
bottom	Length	
box-shadow	Length,<br>Length (optional),<br>Length (blur, optional),<br>Length (spread, optional),<br>Color	box-shadow: 2px 2px 4px black;
color	Color /<br>linear-gradient(Color, Color) /<br>radial-gradient(Color, Color) /<br>conic-gradient(Color, Color)	
column-gap	Length	
content	string	Sets the text of a Label.<br>eg. content: "Loading…";
cursor	none / pointer / progress / wait / crosshair / text / move / not-allowed / any custom cursors	
display*	flex (default) / none	Everything is flex by default
filter	Same as backdrop-filter	
filter-blur	Length	
filter-border-color	Color	
filter-border-width	Length	
filter-brightness	Length	
filter-contrast	Length	
filter-drop-shadow	Same as box-shadow	
filter-hue-rotate	Length	
filter-invert	Length	
filter-saturate	Length	
filter-sepia	Length	
filter-tint	Length	
flex-basis	Length	
flex-direction	row (default) / row-reverse / column / column-reverse	
flex-grow	Float	
flex-shrink	Float	
flex-wrap	nowrap (Default) / wrap / wrap-reverse	
font-color	Color	
font-family*	String	Specify a single font, based on the name of the font itself, not the filename.<br>eg. font-family: Comic Sans MS;
font-size	Length	
font-smooth	auto / always / never	never is good for pixel fonts
font-style	normal (default) / italic	
font-variant-numeric	normal / tabular-nums	
font-weight	normal (default) / bold / light / bolder / lighter / black / int	font-weight: bold;,font-weight: 300;
gap	Length, Length (optional)	Shorthand for row-gap and column-gap, specified the size of gutters.
height	Length	
image-rendering	auto (default) / anisotropic / bilinear / trilinear / point / pixelated / nearest-neighbour	
justify-content	Same as align-content	
left	Length	
letter-spacing	Length / normal	
line-height	Length	
margin	Fills in the properties below	
margin-bottom	Length	
margin-left	Length	
margin-right	Length	
margin-top	Length	
mask	Shorthand for other mask properties	
mask-angle	Length	
mask-image	Same as background-image	
mask-mode	luminance / alpha	
mask-position	Length, Length (optional)	
mask-position-x	Length	
mask-position-y	Length	
mask-repeat	same as background-repeat	
mask-size	Length, Length (optional)	
mask-size-x	Length	
mask-size-y	Length	
max-height	Length	
max-width	Length	
min-height	Length	
min-width	Length	
mix-blend-mode	normal / lighten / multiply	
opacity	Float	
order	int	
overflow	visible (default) / hidden / scroll	
overflow-x	Same as overflow	
overflow-y	Same as overflow	
padding	Fills in the properties below	
padding-bottom	Length	
padding-left	Length	
padding-right	Length	
padding-top	Length	
perspective-origin	Length, Length (optional)	
perspective-origin-x	Length	
perspective-origin-y	Length	
pointer-events	none (default) / all / auto	
position*	static (default) / relative / absolute	See how it works: https://yogalayout.com/docs/absolute-relative-layout/
right	Length	
row-gap	Length	
text-align	left (default) / center / right	
text-background-angle	Length	
text-decoration	Color / Length / LineStyle, Line	Properties can be in any order and you can have multiple lines.
text-decoration-color	Color	
text-decoration-line	underline / line-through / overline	Multiple properties can be set. eg. text-decoration-line: overline underline;
text-decoration-line-through-offset	Length	
text-decoration-overline-offset	Length	
text-decoration-skip-ink	all / none	Decides whether the line decoration should draw above glyphs or not
text-decoration-thickness	Length	
text-decoration-underline-offset	Length	
text-overflow	clip / ellipsis	
text-shadow	Same as box-shadow	
text-transform	none (default) / capitalize / lowercase / uppercase	
top	Length	
transform	Fills in the properties below	
transform-origin	Length, Length, Length (optional)	
transform-origin-x	Length	
transform-origin-y	Length	
transition	Fills in the properties below	transition: all 0.1s ease;,transition: opacity 0.1s transform 0.2s ease-out;
transition-delay	Float	
transition-duration	Float	
transition-property	String	
transition-timing-function	linear (default) / ease / ease-in-out / ease-out / ease-in	
white-space	normal / nowrap / pre	Use pre to format tabs and newlines.
width	Length	
word-break	normal / break-all	
word-spacing	Length	
z-index	int	
Custom Pseudo-Classes

These are some s&box-specific pseudo-classes which are useful for applying transitions when an element is created or deleted.

:intro is removed when the element is created, things will transition away from this
:outro is added when Panel.Delete() is called. The Panel waits for all transitions to finish before actually deleting itself.
MyPanel {
	transition: all 2s ease-out;
	transform: scale( 1 );

	// When the element is created make it expand from nothing.
	&:intro {
		transform: scale( 0 );
	}

	// When the element is deleted make it double in size before being deleted.
	&:outro {
		transform: scale( 2 );
	}
}

---

### Hudpainter
*Source: https://sbox.game/dev/doc/systems/ui/hudpainter/*

Each camera has a HudPainter that can be used to draw onto the HUD. You do this every frame, in any Update function.

This is more efficient than using actual UI panels because there's no layout, stylesheets or interactivity.. you're doing all that stuff yourself.

If your UI is relatively simple, you can do it this way to keep things easy.

	protected override void OnUpdate()
	{
		if ( Scene.Camera is null )
			return;

		var hud = Scene.Camera.Hud;

		hud.DrawRect( new Rect( 300, 300, 10, 10 ), Color.White );

		hud.DrawLine( new Vector2( 100, 100 ), new Vector2( 200, 200 ), 10, Color.White );

		hud.DrawText( new TextRendering.Scope( "Hello!", Color.Red, 32 ), Screen.Width * 0.5f );
	}

---

### Tokens
*Source: https://sbox.game/dev/doc/systems/ui/localization/*

When displaying text such as Hello World, you should instead use a localization token like #menu.helloworld. This will automatically replace the text with the corresponding language set by the user when set on a Label, allowing you to easily support multiple languages.

Tokens

In UI, any displayed string that begins with a # will be recognized as a token, which means it will look for it's real value in the localization system.

Example

Filename: Localization/en/sandbox.json

{
  "menu.helloworld": "Hello World",
  "spawnmenu.props": "Models",
  "spawnmenu.tools": "Tools"
  "spawnmenu.cloud": "sbox.game",
}

Then it's as easy as doing the following:

<label>#spawnmenu.props</label>

Languages
English Name	API Language Code
Arabic	ar
Bulgarian	bg
Simplified Chinese	zh-cn
Traditional Chinese	zh-tw
Czech	cs
Danish	da
Dutch	nl
English	en
Finnish	fi
French	fr
German	de
Greek	el
Hungarian	hu
Italian	it
Japanese	ja
Korean	ko
Norwegian	no
Pirate	en-pt
Polish	pl
Portuguese	pt
Portuguese-Brazil	pt-br
Romanian	ro
Russian	ru
Spanish-Spain	es
Spanish-Latin America	es-419
Swedish	sv
Thai	th
Turkish	tr
Ukrainian	uk
Vietnamese	vn

---

### Razor
*Source: https://sbox.game/dev/doc/systems/ui/virtualgrid/*

VirtualGrid is a Panel that allows you to create a grid of items virtually. What this means is that if you have 1 million items, it won't render them and try to lay them all out at the same time. It'll just pick the few that are visible and create them. When you scroll down, it'll delete the ones it can no longer see and create the new visible ones.

Razor

You can use it in Razor like this

<VirtualGrid Items=@Items ItemSize=@(120)>
    <Item Context="item">
        @if (item is Package entry)
        {
            <SpawnButton Icon="@entry.Thumb" Action="@(() => Spawn(entry))"></SpawnButton>
        }
    </Item>
</VirtualGrid>

@code
{
    public Package[] Items{ get; set; }
}

Here you can see that it sets the Items property (which takes any IEnumerable<T>) and the ItemSize.

Item defines the cell contents. The cell has the class "cell" and will contain whatever you put in between the <Item> elements.

ItemSize is a Vector2. The cell size will be scaled up so it fits into the parent container flush. It'll keep the aspect ratio of the ItemSize.

Gotchas

You need to make sure that VirtualGrid has a size in your styles. I usually make it width and height 100%.

All items obviously need to be the same size.

You can add spacing between elements using the gap styles on the VirtualGrid element.

---


## 11. Navigation (AI)

*NavMesh, agents, pathfinding*

### What is a NavMesh?
*Source: https://sbox.game/dev/doc/systems/navigation/*

We implement Recast Navigation in s&box, the industry standard for navmesh generation and navigation agents. It's used in Unreal, Unity and Godot. So if it seems familiar, that's why.

What is a NavMesh?

A NavMesh is a simplified map of traversable areas in a game world, designed to help AI with pathfinding and movement. NavMeshes are created from the game's PhysicsWorld, which defines where AI characters are allowed to move.

Understanding NavMesh Limitations

A NavMesh is not a detailed or precise representation of the game world; rather, it is a simplified abstraction focused solely on navigable areas. It lacks exact height information or precise ground geometry. Use the PhysiscsWorld alongside the NavMesh, for interactions that require specific physical details, such as placing the game object on the ground.

Creating a NavMesh

To create a NavMesh in your scene, just click the Enable NavMesh button in the header.

You can toggle viewing the generated mesh by clicking the button next to it.

The NavMeshis split up into multiple smaller tiles. The ==yellow== lines represent regular polygon boundaries, while the ==blue== lines represent polygon borders that are also tile boundaries.

NavMesh Settings

You can edit further NavMesh settings by clicking the pencil and paper icon in the NavMesh group in the header.

This allows you to adjust the properties of the mesh, like how steep slopes can be. You can also filter which physics objects are used when generating the mesh.

Code

The navmesh is accessible at Scene.NavMesh.

// Get a random position anywhere the navmesh
var pos = Scene.NavMesh.GetRandomPoint();

// Get a random position within radius of testposition
var pos = Scene.NavMesh.GetRandomPoint( testposition, radius );

// Get the closest point on the navmesh from another position
var pos = Scene.NavMesh.GetClosestPoint( testposition );

// Get the closest edge of the navmesg from this position
var pos = Scene.NavMesh.GetClosestEdge( testposition );

// Get a path from one position to another
var path = Scene.NavMesh.CalculatePath( new CalculatePathRequest()
{
	Agent = Agent, // Optional agent to use for parameters
    Start = WorldPosition,
    Target = TargetWorldPosition
} );

// Path.Status is either Complete or Partial
if ( path.IsValid() ) // ...

// Mark the navmesh dirty, so it will be rebuilt in the background
Scene.NavMesh.SetDirty();

---

### Component
*Source: https://sbox.game/dev/doc/systems/navigation/navmesh-agent/*

A NavMesh Agent will move from position to position on the NavMesh, automatically. It features crowd control features, so they will try to avoid bumping into each other if possible.

Component

The NavMeshAgent is implemented as a component. When you add it to your GameObject it can take over control of the position and rotation.

You would usually create another component, for your main AI logic, which will set positions and animate a model based on its velocity. It's not unusual for this custom AI component to change the max speed and acceleration as it reaches goals etc.

Code
NavMeshAgent agent = ...

// Sets the target position for the agent. It will try to get there
// until you tell it to stop.
agent.MoveTo( targetPosition );

// Stop trying to get to the target position.
agent.Stop();

// The agent's actual velocity
var velocity = agent.Velocity;

---

### Limitations
*Source: https://sbox.game/dev/doc/systems/navigation/navmesh-areas/*

NavNesh Areas can affect NavNesh generation and agent behavior/pathing.

The NavMeshArea component is used to define the location, shape and type of an area.

You can also specify the Area for a link component.

The NavMeshAreaDefinition resource is used to define properties of the Area.

Limitations
Currently there is a limit of 24 NavMeshAreaDefinition, but you can assign them to as many Area Components as you like
Static areas are basically free.
Moving areas are a bit more expensive, but you should be able to have at least a couple of dozens of them.

---

### Costs
*Source: https://sbox.game/dev/doc/systems/navigation/navmesh-areas/costs-filters/*

Areas can be used:

to mark a section as more costly to traverse than others (e.g. shallow water)
to prevent certain agents from entering a specific area

Nav Area impacting agent Pathing behavior

Costs

Agent preferring the cheap area (green) over the expensive area (orange).

Filters

By default an agent can traverse any area.

But, you can also specify which areas an agent is allowed to travers and which not.

Maze made of Forbidden areas, one agent can take shortcuts in the maze.

Regular Agent

Shortcutter Agent

---

### Obstacles
*Source: https://sbox.game/dev/doc/systems/navigation/navmesh-areas/obstacles/*

Areas can be used to block of certain areas of the NavNesh both in editor and at runtime. This will completely omit an area from navmesh generation.

Obstacles can be created by creating a NavMesh Area Component, enabling the IsBlocker flag.

Examples

---

### Creating Links
*Source: https://sbox.game/dev/doc/systems/navigation/navmesh-links/*

Creating Links

You can create links by adding a NavMeshLink Component to an GameObject.

Agent Link Traversal
Default Traversal

By default agents will linearly interpolate between start/end locations to travers the links.

We provide virtual functions you can override and events you can subscribe to in order to be informed about agent traversal.

Custom NavMeshLinkComponent:

public sealed class CustomLink : NavMeshLink
{
	protected virtual void OnLinkEntered( NavMeshAgent agent )
	{

	}

	protected virtual void OnLinkExited( NavMeshAgent agent )
	{

	}
}

NavMeshLink & NavMeshAgent Events:

public class NavMeshLink
{
    public Action<NavMeshAgent> LinkEntered;
    public Action<NavMeshAgent> LinkExited;
}

public class NavMeshAgent
{
    public Action LinkEnter;
    public Action LinkExit;
}

Custom Links

In most cases the default traversal wont suffice. You will likely want to move the game object from start to end differently depending on the link that is being traversed.

If you want to handle the traversal yourself you first need to disable the default traversal on the agent.

Agent.AutoTraverseLinks = false;

Afterwards there are couple options to take over the link traversal and handle it yourself:

By Overriding virtual void NavMeshLink.LinkEntered()
By subscribing to the Action<NavMeshAgent> NavMeshLink.LinkEntered event
By subscribing to the Action NavMeshAgent.LinkEnter event
Constantly checking for bool NavMeshAgent.IsTraversingLink in an OnUpdate method

In some cases you will want the link to handle the traversal, in others you may want to handle it on the agent.

Regardless, of which you choose, after the traversal is finished make sure to call NavMeshAgent.CompleteLink() to continue the regular agent navigation.

In the following we provide a few examples for custom traversal, that can be modified to your needs.

Basic Jump

Agent.CurrentLinkTraversal provides information about the link an agent is currently traversing.

The positional values can be used to drive custom movement and animations.

public record struct LinkTraversalData
{
	public Vector3 LinkEnterPosition;

	public Vector3 LinkExitPosition;

	public Vector3 AgentInitialPosition;

	public NavMeshLink LinkComponent;
}

In this example we crate a custom link component that overrides OnLinkEntered. The component then drives a simple parabolic jump for the agent using the data from Agent.CurrentLinkTraversal.

Note, how we manually update the Agents position every frame using Agent.SetAgentPosition().

public sealed class JumpLink : NavMeshLink
{
    protected override void OnLinkEntered( NavMeshAgent agent )
    {
    	ParabolicJump( agent );
    }

    private async void ParabolicJump( NavMeshAgent agent )
    {
      	var start = agent.CurrentLinkTraversal.Value.AgentInitialPosition;
      	var end = agent.CurrentLinkTraversal.Value.LinkExitPosition;

      	// Calculate peak height for the parabolic arc
      	var heightDifference = end.z - start.z;
      	var peakHeight = MathF.Abs( heightDifference ) + 25f;

      	var mid = (start + end) / 2f;

      	// Estimate prabolic duration size using a triangle /\ between start, mid, end 
      	var startToMid = mid.WithZ( peakHeight ) - start;
      	var midToEnd = end - mid.WithZ( peakHeight );
      	var duration = ( startToMid + midToEnd ).Length / agent.MaxSpeed;
      	duration = MathF.Max( 0.75f, duration ); // Ensure minimum duration

      	TimeSince timeSinceStart = 0;

      	while ( timeSinceStart < duration )
      	{
      		var t = timeSinceStart / duration;

      		// Linearly interpolate XY positions
      		var newPosition = Vector3.Lerp( start, end, t );

      		// Apply parabolic curve to Z position using a quadratic function
      		var yOffset = 4f * peakHeight * t * (1f - t);
      		newPosition.z = MathX.Lerp( start.z, end.z, t ) + yOffset;

      		agent.SetAgentPosition( newPosition );

      		await Task.Frame();
      	}

      	agent.SetAgentPosition( end );
      	agent.CompleteLinkTraversal();
    }
}

Ladders

Ladders follow a similar pattern. But the animated movement is different, it's divided into 3 parts:

Align with bottom of the ladder
Vertical Movement only to reach the ladder top
Walk off the ladder
public sealed class LadderLink : NavMeshLink
{
    protected override void OnLinkEntered( NavMeshAgent agent )
    {
    	ClimbLadder( agent );
    }

    private async void ClimbLadder( NavMeshAgent agent )
    {
    	var initialPos = agent.CurrentLinkTraversal.Value.AgentInitialPosition;

    	var start = agent.CurrentLinkTraversal.Value.LinkEnterPosition;
    	var endVertical = start.WithZ( agent.CurrentLinkTraversal.Value.LinkExitPosition.z );
    	var end = agent.CurrentLinkTraversal.Value.LinkExitPosition;

    	var climbSpeed = 100f;

    	var startDuration = (start - initialPos).Length / climbSpeed;
    	var climbDuration = (endVertical - start).Length / climbSpeed;
    	var endDuration = (end - endVertical).Length / climbSpeed;

    	var totalLadderTime = startDuration + climbDuration + endDuration;

    	TimeSince timeSinceStart = 0;

    	while ( timeSinceStart < totalLadderTime )
    	{
    		Vector3 newPosition = start;

    		// 1. Make sure we are positioned at the link start
    		if ( timeSinceStart < startDuration )
    		{
    			newPosition = Vector3.Lerp( initialPos, start, timeSinceStart / startDuration );
    		}
    		// 2. Vertical Movement
    		else if ( timeSinceStart < startDuration + climbDuration )
    		{
    			newPosition = Vector3.Lerp( start, endVertical, (timeSinceStart - startDuration) / climbDuration );
    		}
    		// 3. Move off ladder to link end position
    		else
    		{
    			newPosition = Vector3.Lerp( endVertical, end, (timeSinceStart - startDuration - climbDuration) / endDuration );
    		}

    		agent.SetAgentPosition( newPosition );

    		await Task.Frame();
    	}

    	agent.SetAgentPosition( end );

    	agent.CompleteLinkTraversal();
    }
}

Physics Based Jump

You can also let the physics system drive the jump for you.

For this one we will switch it up and implement the traversal on the agent rather than the link.
We create a new component that we will attach to the GameObject our NavMeshAgent is on.
In addition, we will also need a RigidBody and Collider.

To perform a jump we simply apply a velocity to the RigidBody.
Physics jumps are usually hard to get right consistently, because they can fail in the same way a player jump can fail.
For example, jump was initiated to early/late or the jump was blocked by something mid air.

public sealed class NavigationLinkTraversal : Component
{
	[RequireComponent]
	NavMeshAgent Agent { get; set; }

	[RequireComponent]
	Rigidbody Body { get; set; }

	[RequireComponent]
	public SkinnedModelRenderer Model { get; set; }

	protected override void OnEnabled()
	{
		Agent.AutoTraverseLinks = false;
		Agent.LinkEnter += OnNavLinkEnter;
	}

	protected override void OnDisabled()
	{
		Agent.LinkEnter -= OnNavLinkEnter;
	}

	private void OnNavLinkEnter()
	{
		PhysicsJump();

	}

	private async void PhysicsJump()
	{
		Model.Set( "b_grounded", false );
		Model.Set( "b_jump", true );

		// Physiscs will drive our jump so disable game object position sync
		Agent.UpdatePosition = false;

		var start = Agent.CurrentLinkTraversal.Value.AgentInitialPosition;
		var end = Agent.CurrentLinkTraversal.Value.LinkExitPosition;

		var xyVelocity = Agent.MaxSpeed * (end.WithZ( 0 ) - start.WithZ( 0 )).Normal * 1.25f; 

		var velocity = xyVelocity + Vector3.Up * Math.Max( 500f, (end.z - start.z) * 8f );
		// Launch the agent into the air
		Body.Velocity = velocity;

		TimeSince timeSinceStart = 0;

		while ( true )
		{
			Agent.SetAgentPosition( WorldPosition );

			// Try to find ground
			var tr = Scene.Trace.Ray( WorldPosition + Vector3.Up * 0.1f, WorldPosition + Vector3.Down * 1000 )
				.IgnoreGameObjectHierarchy( GameObject )
				.Run();

			if ( tr.Hit && timeSinceStart > 0.5f && tr.Distance < 4f )
			{
				break;
			}

			await Task.Frame();
		}

		Agent.SetAgentPosition( WorldPosition );

		// Hand back position control to the agent
		Agent.UpdatePosition = true;

		Model.Set( "b_grounded", true );
		Model.Set( "b_jump", false );

		Agent.CompleteLinkTraversal();
	}
}

Full Example on testbed

You can find a full example that implements those 3 traversal types in our testbed:

https://github.com/Facepunch/sbox-scenestaging/blob/main/Code/ExampleComponents/NavigationTargetWanderer.cs

---


## 12. File System

*Reading and writing files in the sandbox*

### File Systems
*Source: https://sbox.game/dev/doc/systems/file-system/*

Standard .NET file access is restricted to prevent rogue access to your files, this means you can not use System.IO.File or variants directly.

Instead, s&box provides a BaseFileSystem for several virtual filesystems that can only access files within specific game directories.

File Systems

There are a few different File Systems available to use in your game. Each one writes and reads from a different location.

Data File System

FileSystem.Data is used to read and write data to a sub-directory specifically for the game that is currently running, i.e. C:\steam\steamapps\common\sbox\data\org\game\

Mounted File System

FileSystem.Mounted is an aggregate filesystem of all mounted content from the core game, the current game and its dependencies.

Organization File System

FileSystem.OrganizationData is a place to store user data across several games in your organization, i.e.

C:\steam\steamapps\common\sbox\data\org\

Reading and Writing Text
// If the file doesn't exist already then write some data to it
if ( !FileSystem.Data.FileExists( "player.txt" ) )
    FileSystem.Data.WriteAllText( "player.txt", "Hello, world!" );

// Read the text back into a variable from the file
var hello = FileSystem.Data.ReadAllText( "player.txt" );

Reading and Writing Json

WriteJson and ReadJson will only work with Properties of your class unless directed not to - make sure the things you want to save are Properties!

public class PlayerData
{
  	public int Level { get; set; } // Will be serialized
	public int MaxHealth { get; set; } // Will be serialized
	public string Username; // Won't be serialized

	public static void Save( PlayerData data )
	{
		FileSystem.Data.WriteJson( "player.json", data );
	}

	public static PlayerData Load()
	{
		return FileSystem.Data.ReadJson<PlayerData>( "player.json" );
	}
}

---


## 13. ActionGraph

*Visual scripting system*

### Intro To Actiongraphs
*Source: https://sbox.game/dev/doc/systems/actiongraph/intro-to-actiongraphs/*

ActionGraph is a visual scripting language. Each node describes an action or expression, and links between nodes carry values or signals.

Creating a new ActionGraph

To start using ActionGraph in your project, you can either:

Use the built-in Component actions
Use the Component Editor to create methods in a custom component
Add a Delegate-type property to a custom component in C#

Following either guide above will get you into the ActionGraph editor.

Nodes

Nodes appear as rectangles in the ActionGraph editor with a name (or symbol), and input or output sockets. You create a node by right-clicking in any empty space, or dragging a link from an output to get a list of possible nodes that are specific to the link value type.

Root Node

The root node is the entry point of your graph. It'll be the only node in a new graph, and can't be deleted. It has only output sockets: one signal socket at the top, and optionally some value sockets if your graph accepts parameters.

When the graph runs, a signal is fired from the output signal socket of your root node. This will follow any links to other action nodes, causing them to run too.

Expression Nodes

Expression nodes (green) perform a calculation based on their inputs, without changing any state elsewhere. They don't have any input or output signal sockets, and will evaluate when any of their output values are used by another node.

Action Nodes

Action nodes (blue) are any nodes with the white, arrow-shaped signal sockets. These nodes trigger things to happen when receiving a signal.

Most action nodes will have one input and one output signal socket in their title bar. The input will trigger the node to run, and the output will fire when the node has finished performing its action.

Some control flow nodes like If, While, For Each, or For Range, will have extra output signal sockets that will fire if certain conditions are met. This allows you to do loops, or branch based on some condition.

Links

Links connect an output of one node to an input of another. A link between signal sockets (white, arrow-shaped) will carry a signal, and any other link will carry a value. Links are created by dragging from a node's output.

If your graph is becoming messy because of links travelling a long distance, you can use variables to help clean things up. You can also use reroute nodes (shift-click on an existing link) to help organize your links.

Your graph can't be linked in a way that leads to a cycle: it shouldn't be possible to follow links from outputs to inputs and arrive back at the same node. You can use special action nodes in the Control Flow category to perform loops, rather than trying to connect your nodes in a cycle with links.

Constants

If you want to use a hard-coded value in an input, you can directly set it in the Properties panel when the node is selected.

Next Steps

Now you know the basics, the next things to learn are:

How to use variables
Creating your own custom nodes

---

### Component Actions
*Source: https://sbox.game/dev/doc/systems/actiongraph/component-actions/*

The easiest way to add an ActionGraph to your scene is through the Actions Invoker component. Just click Add Component on an object, then you can find it under Actions.

You can also find action properties in a few other built-in components, like Colliders when Is Trigger is checked.

Each of these start out with an "Empty Action". Simply click on any of those to create a new ActionGraph and start editing it!

Next Steps

Take a look at the Intro to ActionGraphs guide to learn how to edit your new graph.

---


### Examples
*Source: https://sbox.game/dev/doc/systems/actiongraph/examples/*

Here's how to do some common things. Each has a code you can copy to your clipboard, then Ctrl+V to paste in the ActionGraph editor.

Loop Through Child Objects

Clipboard Code
actiongraph:H4sIAAAAAAAACp1UW0+DMBR+X7L/0OArQ5huGb4ZTcyi0SVeXoxZCpyxKrSkLWbLwn+3pbjVMc3w6XBu3/nOhW76PYScW0IT5wI5l7EkjDpubXzBnOAoA6E8r2/Gds8So2sNoY0RyjHV+aOJuzU8rQvQkCIGCl4KkkXvEEtnF/EsgF9jiVXUFkeZZ0yQmoVKHozGvjs4G0+c74DKfFTuYQJhm0DBWQFcrjUHq/rMmEndjl1/LpvER0yTiK28G5zDqa087HWik6iy66SrJckSDnRHuFvD536nhtV4Wg0vGAccLztOOgiHnQoH7cIZSz1CF8yqPKVFKfcnrAjmWG6nhU42QaUi/OqfQwuGk8PUtWgO947QD+twG4F+nOycgygza7H2OUnM9QEZ3YDaMOEfMPaSgJY5cP1b/QZlR0csWVs41szngqQUZ8eCeJ84K+EwlGpM7Dz+sYjqyYDVMYhBg1hvo9+rvgCTVPEacwQAAA==

Move With Velocity

Clipboard Code
actiongraph:H4sIAAAAAAAACr1U30+DMBB+X7L/gfDMENjGmG8mJsZodIlzL8YsBaqpQkvabnFZ9r/b0g2qDALzR3govet9333t3W37PcMwbxCOzXPDvIg4Iti0cuMCUATCBDLheXpWtjsSq73cGcZWLcJxLeP9sVUY5psMSkgWQQztV8hJ+AYjbpYnHhmkl4ADcarAEeYZYSjPQgQPxl5gDRzz4N6pn511nN6v0meUZJDyjcxA454pM8rF6OxLvg98ADgOyYd9BVJ4pm/uv+mQQVjYZdCcAsxeCE3LjLvpHfpd9E4a9LLf0ltqOiq5SP80xZ4TWK31Bv/xvn+rd+D6juWP2iqeVhWTzAZx3K2PRoHlekFL0olTJeUohXYME8HSsaCHfhdq96jedJVwlCUbs/tVu1OvLbdX5Y4IZtxew2jYprYOJbKACYkQ19IVzjVIVrnXdRxLfCdPCHGhnlctILnsh/Qtwu/akN4vxpfxvKSQiVstU9SHJwdUtpPaK1Adxm+C0WbST2CC1jBBE4zWP6AWQTvUqEc9YQ2K3jUVFL2ua/PQDzUqCWsRtBJuzKFAyKum39t9Ai3avLMHCAAA

---

### Using With C
*Source: https://sbox.game/dev/doc/systems/actiongraph/using-with-c/*

Here's how you can use ActionGraph with your game code written in C#.

public class ExampleComponent : Component
{
	[Property]
	public Action ExampleAction { get; set; }

	protected override void OnUpdate()
	{
		if ( Input.Down( "attack1" ) )
		{
			ExampleAction?.Invoke();
		}
	}
}

Declaring the Property
[Property]
public Action ExampleAction { get; set; }

This will add a property that we can see in the inspector. For a property to be usable as an ActionGraph it must have a Delegate type. We're using the type Action here, which takes in no parameters and returns nothing. Once we do this we can press the "+" to add as many callbacks to the Action as we want.

If you only intend to have one callback for your Delegate, you can add the [SingleAction] attribute and it will show only the one action in-line without having the option to add/remove others.

Clicking on the Example Action or Empty Action will open the ActionGraph editor and start editing it.

Adding Parameters

If you want to be able to pass in some parameters, you can use a different delegate type:

[Property]
public Action<int, string, GameObject> ExampleAction { get; set; }

Opening up the graph, you'll see the root node has a socket for each parameter type, in the order you declared them.

Renaming Parameters

There's a couple of ways to rename the parameters, since "Arg 1", "Arg 2" isn't very helpful. First, you can click on the root node in the editor, and change the socket names and descriptions in the Properties panel.

The other way is to use a custom delegate type:

public delegate void ExampleDelegate( int someNumber, string someText, GameObject someObject );

[Property]
public ExampleDelegate ExampleAction { get; set; }

This will automatically fill in the names of each parameter like above, and you can re-use the same delegate type for any property that would take in the same set of parameters.

Running an ActionGraph

You can call the property like any other method directly:

ExampleAction();

However, if the action hasn't been created in the editor yet, this will throw a NullReferenceException. A safer way to run it is to check for null like this:

ExampleAction?.Invoke();

This is shorthand for:

if ( ExampleAction is not null ) ExampleAction();

Passing in Arguments

If you've declared some parameters, you can pass in values when running the graph like this:

ExampleAction( 123, "Hello, World!", GameObject.Parent );

Or if you're checking for null:

ExampleAction?.Invoke( 123, "Hello, World!", GameObject.Parent );

Returning Values

Delegates with return types are supported too.

[Property]
public Func<int> TestExpression { get; set; }

Graphs implementing delegates like this will have an Output node, which you'll need to connect to the input without going through any async nodes (like delays).

Async Graphs

If you want to wait for a graph with delays to finish running, it needs to be declared with a delegate type that returns a Task.

[Property]
public Func<Task> ExampleAction { get; set; }

With parameters:

[Property]
public Func<int, string, GameObject, Task> ExampleAction { get; set; }

As a custom delegate type:

public delegate Task ExampleDelegate( int someNumber, string someText, GameObject someObject );

[Property]
public ExampleDelegate ExampleAction { get; set; }

When you call it you can either await the returned task, or use ContinueWith outside of async code.

await ExampleAction( 123, "Hello, World!", GameObject.Parent );

ExampleAction?.Invoke( 123, "Hello, World!", GameObject.Parent )
    .ContinueWith( task => { } );

Next Steps

Take a look at the Intro to ActionGraphs guide to learn how to edit your new graph.

---

### Custom Nodes
*Source: https://sbox.game/dev/doc/systems/actiongraph/custom-nodes/*

There are two ways to create your own node types:

Action Resources
C# Method Nodes

---

### C Method Nodes
*Source: https://sbox.game/dev/doc/systems/actiongraph/custom-nodes/c-method-nodes/*

Making a custom node in C# is as easy as writing a static method with a special [ActionGraphNode( id )] attribute. You should also add [Pure] if you want your node to be an expression node - a node without signals.

/// <summary>
/// Increments the value by 1.
/// </summary>
[ActionGraphNode( "example.addone" ), Pure]
[Title( "Add One" ), Group( "Examples" ), Icon( "exposure_plus_1" )]
public static int AddOne( int value )
{
	return value + 1;
}

/// <summary>
/// Waits for one second.
/// </summary>
[ActionGraphNode( "example.waitone" )]
[Title( "Wait One" ), Group( "Examples" ), Icon( "hourglass_bottom" )]
public static async Task WaitOne()
{
	await Task.Delay( TimeSpan.FromSeconds( 1d ) );
}

---


## 14. Animation

*Animgraph, skeletal animation*

### Getting Started
*Source: https://sbox.game/dev/doc/systems/movie-maker/skeletal-animation/*

You can animate skinned characters directly in the editor using Movie Maker. This tutorial covers:

Enabling the skeletal animation tools
Importing animation sequences from a model
Generating bone animation track data using AnimGraph
Recording bone animations from gameplay
Upgrading legacy procedural bone object animations
Using IK to manipulate poses
Getting Started

Movie Maker's skeletal animation tools are enabled if you have at least one SkinnedModelRenderer in your movie with a Bones sub-track. When enabled for a renderer, you'll see its skeleton.

You'll also need to be in the Motion Editor to do the modifications described in this tutorial.

Base Animations

Animating skinned objects from scratch can be time consuming, so you'll usually want to start with an imported or generated base-level animation that you can tweak later.

Importing Animation Sequences

For simple sequences you can just load an animation from your model.

Make sure you're in Motion Editor mode
In the timeline, select a time range that you want to import an animation for
Right-click on a selected region of track that belongs to the object you want to animate
Select Import Anim Sequence, and pick a clip
Drag the imported sequence around or trim the start / end times
Click the green Apply button to save the modification

Root motion will be included by default, you can disable it by locking the LocalPosition / LocalRotation tracks of the object you're animating.

Generating Using AnimGraph

You can save a lot of time by letting AnimGraph generate an animation for you. You'll just need LocalPosition and LocalRotation tracks describing how your character moves. These can be created in any way you like: keyframes, recording, or motion editing.

Rotate With Motion

If you've only got a LocalPosition track, you can generate a LocalRotation track that always looks in the direction of movement. Just select the time range you want to generate rotations for in the motion editor, then right-click and select Rotate with Motion in the context menu, and apply if you're happy with it.

Before moving on to the next step, feel free to tweak the generated LocalRotation track if you want your character to side-step or walk backwards at any point.

Motion to AnimGraph Parameters

When you have LocalPosition and LocalRotation tracks for your SkinnedModelRenderer, you can generate AnimGraph parameter tracks based on that motion. Select the time range you want to generate the track data for, and select Motion to AnimGraph Parameters in the context menu.

You can find the generated tracks nested under Parameters in the track list, and again they can be tweaked using keyframes or motion editing before moving to the next step.

AnimGraph Parameters to Bones

If you want to have more fine-grained control over how your character moves, you'll need to bake its animation into bone tracks. As always, select the time range you want to bake, then select AnimGraph Parameters to Bones in the context menu. Now you're ready to move on to the Manipulating Poses section.

Recording Gameplay

Another way to generate initial bone track data is to record it in play mode. You'll just need to create tracks for the bones you want to record before you start. We've included a sub-track preset for the built-in player controller as an example.

Upgrading Legacy Bone Object Animations

Before we had dedicated bone track support, the best you could do was to enable Create Bone Objects on a SkinnedModelRenderer and add all the created objects to your movie. If you have an existing animation using that method, you can upgrade it to the new-style bone tracks with this context menu option. After doing the upgrade, you can manually delete the old tracks and disable Create Bone Objects.

Manipulating Poses

We have some basic tools to help you tweak how your characters are posed during your movie. This is fairly minimal currently, but should be enough to do things like layer upper body motion on top of a walk animation generated using the above methods.

You should know the basics of Motion Editing before getting started here.

Moving Bones

First, select a time region in your movie that you want to animate a pose change for. Next, click and drag from a joint. We'll simulate an IK chain going from the dragged bone to the root to try to find a sensible pose. While dragging, you can press Esc to cancel. Press the green Apply button when you're happy to commit the change to your animation.

Rotating Bones

When dragging a bone, you can hold the E key to start rotating it in place.

Locking Bones

Bones can be locked or unlocked in the context menu, or by holding Shift and clicking on them. This can be especially useful for things like finger posing by locking the hand.

Currently this will only animate correctly if you lock an ancestor of whichever bone you're manipulating. You can lock descendant bones like in the example below, but the lock is only considered for the current playhead time while posing.

---


## 15. Services

*Achievements, leaderboards, stats, auth*

### Listing
*Source: https://sbox.game/dev/doc/systems/services/achievements/*

Your game can have multiple achievements for players to unlock.

Score

Each achievement can have a different score. The score is usually between 5 and 100, which is really something you need to choose based on the achievement. Generally, give low values to easy-to-achieve things and a high value to hard-to-achieve things. Have many low value, and few high value.

When a player unlocks the achievement, the score is added to their global score.

You have full control over choosing the score but the total combined for your game cannot exceed 1000.

Icons

The icon is automatically resized to 128x128. It will generally be rendered quite small, next to the name of the achievement, so should be treated more like an icon than a picture.

Stat Based

When your achievement unlock mode is set to "Stat" it will automatically be unlocked for you. All you need to do is make sure the stat is set up properly.

An example of a stat based achievement would be the "100 coins".

You would do Stat.Increment( "coins", 1 ) in your code every time you collected a coin, then you can set your achievement to this..

Property	Value	Explanation
Target Stat	"coin"	
Aggregation	Sum	
Min Value	0	
Max Value	100	
Show Progress	true	
Target Stat: "coin" (the name of the stat)
Aggregation: Sum (you want to add the 1's values together)
Min Value: 0, Max Value: 100 (unlocks at 100)
Show Progress - yes - will show progress between 0 and 100 as a percentage
Manual

If the stat is set to manual, then you can unlock it in your code like this:

Sandbox.Services.Achievements.Unlock( "statname" );

Listing

You can get the list of achievements at any time in your game

foreach ( var a in Sandbox.Services.Achievements.All )
{
    // do something
}

Or from outside the game, you can access it via the API

https://services.facepunch.com/sbox/achievement/list?package=facepunch.testbed

---

### Leaderboards
*Source: https://sbox.game/dev/doc/systems/services/leaderboards/*

Leaderboards are just stats, aggregated and ordered.

Basic Example

Here's how to get a leaderboard. We want to get a leaderboard to show who has killed the most zombies, globally.

var board = Sandbox.Services.Leaderboards.GetFromStat( "facepunch.ss1", "zombies_killed" );
await board.Refresh();

foreach ( var entry in board.Entries )
{
	Log.Info( $"{entry.Rank} - {entry.DisplayName} - {entry.Value}" );
}

By default, leaderboards are aggregated using the sum of all the stats and ordered descending.

By Country

You can filter a leaderboard by country. This will only show stats that were created in that country.

var board = Sandbox.Services.Leaderboards.GetFromStat( "facepunch.ss1", "zombies_killed" );
board.SetCountryCode( "gb" );
await board.Refresh();

foreach ( var entry in board.Entries )
{
	Log.Info( $"{entry.Rank} - {entry.DisplayName} - {entry.Value} [{entry.CountryCode}]" );
}

If you pass countrycode as "auto" it will use the current player's location

By Date

You can filter leaderboards by date, allowing yearly, weekly, monthly or daily leaderboards.

var board = Sandbox.Services.Leaderboards.GetFromStat( "facepunch.ss1", "zombies_killed" );
board.FilterByMonth();
board.SetDatePeriod( new System.DateTime( 2024, 8, 1 ) );
await board.Refresh();

foreach ( var entry in board.Entries )
{
	Log.Info( $"{entry.Rank} - {entry.DisplayName} - {entry.Value} [{entry.CountryCode}]" );
}

If you don't set a date period, it'll use the current date

Centering

You can focus the leaderboard on a certain player. This will show the results around that player. It is nice to show a player's contemperies rather than showing the top 20 all the time.

var board = Sandbox.Services.Leaderboards.GetFromStat( "facepunch.ss1", "zombies_killed" );
board.CenterOnSteamId( 76561197960279927 );
await board.Refresh();

foreach ( var entry in board.Entries )
{
	Log.Info( $"{entry.Rank} - {entry.DisplayName} - {entry.Value}" );
}

You can also call .CenterOnMe() to center on the local player.

Aggregation and Sorting

So maybe instead of the sum of something, we want to show people's shortest result. Like in this example, we're showing a list of the fastest win times.

var board = Sandbox.Services.Leaderboards.GetFromStat( "facepunch.ss1", "victory_elapsed_time" );

board.SetAggregationMin(); // select the lowest value from each player
board.SetSortAscending(); // order by the lowest value first
board.FilterByMonth(); // only show results from this month
board.CenterOnMe(); // offset so I'm in the middle of the results
board.MaxEntries = 100; 

await board.Refresh();

foreach ( var entry in board.Entries )
{
	Log.Info( $"{entry.Rank} - {entry.DisplayName} - {entry.Value} [{entry.Timestamp}]" );
}

You can aggregate by sum, min, max, avg or last.

The entry timestamp holds the time of the selected result

---

### Example
*Source: https://sbox.game/dev/doc/systems/services/leaderboards/web-api/*

The API for leaderboards is publically accessible. Have fun!

https://services.facepunch.com/sbox/package/leaderboard/{package_ident}/

stat

The name of the stat to base the leaderboard on. This does not need to have been registered formally.

count (default 10)

The amount of results to return

offset (default 0)

If higher than 0 then the results will start at this number. If lower than 0, we'll start at the bottom of the results. For example, a value us -1 will return the very last page of results.

aggregation (default sum)

Describes how to aggregate the stats. This is fundamental to getting the leaderboard in a format that makes sense.

sum	Adds all values together. This is useful for things like the most kills, where you have a stat that is incremented on each kill.
max	The maximum single value every submitted. An example would be for high scores.
min	The lowest value every submitted. You could use this for fastest time etc.
avg	The average value from every value submitted. I can't think of a use case.
last	The last value submitted. I can't think of a use case.
sort (default desc)

How the leaderboard values should be sorted.

desc	Highest values first
asc	Lowest values first
datefilter (default none)

Allows querying a specific time period for a leaderboard. This allows creating things like weekly leaderboards.

none	Do not limit by time period.
year	Limit by year
month	Limit by month
week	Limit by week
day	Limit by day
date(default now)

Allows specifying a date to get for datefilter. You don't need to be exact with the date, it just has to be within the period you want to get. If unset, will default to now - which will get the current leaderboard for any datefilter.

This does nothing if datefilter is none.

centersteamid(default 0)

Center the results on a certain SteamId within the board. Offset will offset from this position.

country(default none)

Limit results to a specific country code. If set to auto this will return the current user's country, if logged in.

friends (default false)

Limit results to the current logged user's friends.

include[] (default null)

Allows a list of steamids to always show on the leaderboard, even if they're not in current subset. This is useful for situations where you want to show the top 20, but want the current player's score to be present too.

Example
https://services.facepunch.com/sbox/package/leaderboard/facepunch.ss1/?stat=zombies_killed

{
  Stat: "zombies_killed",
  TotalEntries: 2588,
  Entries: 
  [
    {
      Rank: 1,
      Value: 645806,
      SteamId: 76561198043785940,
      CountryCode: "us",
      DisplayName: "puxorb",
      Timestamp: "2024-08-23T03:44:17.0174014+00:00",
      Data: { }
    },
    {
      Rank: 2,
      Value: 310250,
      SteamId: 76561198053736750,
      CountryCode: "au",
      DisplayName: "Rin",
      Timestamp: "2024-08-19T18:28:10.6930596+00:00",
      Data: { }
    }
  ],
  DateDescription: null,
}

---

### Recording Stats
*Source: https://sbox.game/dev/doc/systems/services/stats/*

Your game can record stats for each player that plays it. Here are some examples of things stats can record.

Number of zombies killed
Fastest time
Total meters walker
Coins collected
Longest time
Bullets fired
Kills per weapon

You can query and display these stats, or use them in any other way you want. You can query stats from individual players, and you can get the compound stats globally.

Recording Stats

Depending on what you're doing, you either want to increment your stat..

public void OnZombieKilled()
{
    Sandbox.Services.Stats.Increment( "zombies-killed", 1 );
}

Or just set it directly..

public void OnGameFinished()
{
    Sandbox.Services.Stats.Increment( "wins", 1 );
    Sandbox.Services.Stats.SetValue( "win-time", SecondsTaken );
}

You can call these apis as often as you like. We batch the stats and send them when we're ready.

You can also add data when calling SetValue. This is in the format of a Dictionary<string, object> and this data is available when querying leaderboards.

Reading Stats

Stats are available in two forms. Either global, or player.

// get global zombie kill count
var zombies = Sandbox.Services.Stats.Global.Get( "zombies_killed" );

Log.Info( $"there have been {zombies.Sum} zombies killed by {zombies.Players} players!" );

// Get local player zombie kill count
var zombies = Sandbox.Services.Stats.LocalPlayer.Get( "zombies_killed" );

Log.Info( $"You have killed {zombies.Sum} zombies!" );

// Get stats for Garry in SS1
var stats = Sandbox.Services.Stats.GetPlayerStats( "facepunch.ss1", 76561197960279927 );

// wait for them to download
await stats.Refresh();

var zombies = stats.Get( "zombies_killed" );

Log.Info( $"Garry has killed {zombies.Sum} zombies!" );

---

### Generating Tokens
*Source: https://sbox.game/dev/doc/systems/services/auth-tokens/*

If you are using HTTP requests or WebSockets in your game, you can use Auth Tokens to validate that the requests were sent from a valid Steam user in a s&box game session. This is useful if you want to tie data to a specific Steam account, or prevent botting.

Generating Tokens

You can generate a new token with Sandbox.Services like so:

var token = await Sandbox.Services.Auth.GetToken( "YourServiceName" );

Validating Tokens

To validate a token on your backend, you need to make a call to the services.facepunch.com/sbox API using the auth/token endpoint.

Here is an example of how to validate a token in C# using System.Net.Http

private class ValidateAuthTokenResponse
{
	public long SteamId { get; set; }
	public string Status { get; set; }
}

public static async Task<bool> ValidateToken( long steamId, string token )
{
	var http = new System.Net.Http.HttpClient();
	var data = new Dictionary<string, object>
	{
		{ "steamid", steamId },
		{ "token", token }
	};
	var content = new StringContent( JsonSerializer.Serialize( data ), Encoding.UTF8, "application/json" );
	var result = await http.PostAsync( "https://services.facepunch.com/sbox/auth/token", content );

	if ( result.StatusCode != HttpStatusCode.OK ) return false;

	var response = await result.Content.ReadFromJsonAsync<ValidateAuthTokenResponse>();
	if ( response is null || response.Status != "ok" ) return false;

	return response.SteamId == steamId;
}

At some point when receiving the token from the client on your backend you can then validate it as such:

var isValidToken = await ValidateToken( steamId, token );

if ( isValidToken )
{
	Console.WriteLine( "Success!" );
}

---


## 16. Storage & UGC

*User-generated content, Steam Workshop*

### Steam Workshop
*Source: https://sbox.game/dev/doc/systems/storage-ugc/*

The Storage system provides a simple, unified way to manage user-generated content in your game. Whether you're saving game progress, storing player creations, or anything else, Storage handles everything from local file management to Steam Workshop integration.

Each storage entry is a self-contained folder with its own filesystem, metadata, and optional thumbnail, making it easy to organize, share, and distribute user content.

Storage entries are categorized by a string type (like "save" or "dupe"), automatically maintain metadata, and can be seamlessly published to or downloaded from the Steam Workshop. The API is designed to be straightforward: create entries, read/write files, and optionally share them with the community.

Creating a New Entry

To create a new storage entry, use Storage.CreateEntry() with a type identifier. The entry provides a Files property (a BaseFileSystem) that you use to read and write files.

// Create a new save game entry
var saveEntry = Storage.CreateEntry( "save" );

// Write game data to files
saveEntry.Files.WriteAllText( "player.json", playerJson );
saveEntry.Files.WriteAllBytes( "world.dat", worldData );

// You can also use JSON serialization
saveEntry.Files.WriteJson( "config.json", gameConfig );

// Set metadata for searching and display
saveEntry.SetMeta( "playerName", "John Doe" );
saveEntry.SetMeta( "level", 42 );
saveEntry.SetMeta( "playtime", 3600 );

// Create and set a thumbnail
var thumbnail = CreateThumbnailBitmap(); // Your method to create a Bitmap
saveEntry.SetThumbnail( thumbnail );

Entry Properties

Each entry has the following public properties:

**Id** - A unique identifier (GUID) for this entry
**Type** - The category type (e.g., "save", "dupe")
**Created** - When this entry was created
**Files** - A BaseFileSystem for reading and writing files
**Thumbnail** - The thumbnail image as a Texture
Working with Files

The Files property is a full-featured filesystem that supports:

// Write operations
entry.Files.WriteAllText( "notes.txt", "Player notes..." );
entry.Files.WriteAllBytes( "screenshot.png", imageBytes );
entry.Files.WriteJson( "data.json", myObject );

// Read operations
string notes = entry.Files.ReadAllText( "notes.txt" );
byte[] screenshot = entry.Files.ReadAllBytes( "screenshot.png" ).ToArray();
MyData data = entry.Files.ReadJson<MyData>( "data.json" );

// File queries
bool exists = entry.Files.FileExists( "notes.txt" );
var allFiles = entry.Files.FindFile( "/", "*", recursive: true );

// Directories
entry.Files.CreateDirectory( "levels" );
entry.Files.WriteAllText( "levels/level1.json", levelData );

Listing Entries

To retrieve all storage entries of a specific type from disk:

// Get all saved games
var allSaves = Storage.GetAll( "save" );

foreach ( var save in allSaves )
{
    // Access entry properties
    Log.Info( $"Save: {save.Id}" );
    Log.Info( $"Created: {save.Created}" );

    // Read metadata
    var playerName = save.GetMeta<string>( "playerName" );
    var level = save.GetMeta<int>( "level" );

    // Access the thumbnail
    var thumbnail = save.Thumbnail;

    // Read files from the entry
    if ( save.Files.FileExists( "player.json" ) )
    {
        var playerJson = save.Files.ReadAllText( "player.json" );
        // Load your game...
    }
}

Deleting Entries

To remove an entry from disk:

entry.Delete();

This permanently removes the entry's folder and all its contents.

Steam Workshop

Publishing to Steam Workshop is made very simple:

saveEntry.Publish();

This pops open a dialog for the client, where they can name, add a description and change visibility of the entry.

Querying the Steam Workshop

To search for content on the Steam Workshop, create and configure a Query, then run it:

var query = new Storage.Query
{
	// Search for items with specific tags
	TagsRequired = { "save", "adventure" },

	// Exclude certain tags
	TagsExcluded = { "adult" },

	// Key-value filters (set during publish)
	KeyValues = { ["type"] = "save", ["package"] = "facepunch.sandbox" },

	// Text search
	SearchText = "epic quest",

	// Sort order
	SortOrder = Storage.SortOrder.RankedByVote,
};

// Run the query
var result = await query.Run();

Log.Info( $"Found {result.TotalCount} total items" );
Log.Info( $"Returned {result.ResultCount} items" );

foreach ( var item in result.Items )
{
	Log.Info( $"{item.Title} by {item.Owner.Name}" );
	Log.Info( $"Rating: {item.VotesUp}/{item.VotesDown}" );
	Log.Info( $"Created: {item.Created}" );
	Log.Info( $"Tags: {string.Join( ", ", item.Tags )}" );
}

Available Sort Orders

The SortOrder enum provides various ranking options:

RankedByVote - Most popular items
RankedByPublicationDate - Newest first
RankedByTrend - Currently trending
RankedByTotalPlaytime - Most played
RankedByTextSearch - Best search matches
And many more...
KeyValues

Any storage published automatically have two keyvalues

package - the name of the package that published it
type - the name of the storage type

These are obviously useful when you only want to find packages from a specific game

Installing from Workshop

To download and use content from the Steam Workshop, just call Install on the Storage.QueryItem

// From a query result
var query = new Storage.Query { TagsRequired = { "save" } };
Storage.QueryResult result = await query.Run();
Storage.QueryItem chosenItem = result.Items.First();

// Install the workshop item
Storage.Entry entry = await chosenItem.Install();
if ( entry == null ) throw new Exception( "Failed to install the chosen item." );

// The entry is now available locally
Log.Info( $"Installed: {entry.Type} - {entry.Id}" );

// Read its metadata
var playerName = entry.GetMeta<string>( "playerName" );

// Access its files (read-only)
if ( entry.Files.FileExists( "player.json" ) )
{
	var data = entry.Files.ReadAllText( "player.json" );
	// Load the save...
}

// Workshop entries are read-only
if ( entry.Files.IsReadOnly )
{
	Log.Info( "This is a workshop item (read-only)" );
}

Important Notes
Workshop entries are read-only.

---


## 17. Terrain

*Terrain system*

### Terrain
*Source: https://sbox.game/dev/doc/systems/terrain/*

The Terrain System allows you to create expansive outdoor environments quickly and easily in your scenes.

The pages in this section explain how to get started and all the various built-in options for Terrain and how to use them.

The Terrain System is a work in progress, here's what is supported and what's not right now.

Feature Name	Supported	Not Supported	Notes
Real-time editing	✅		Basic sculpting tools & 2 layer texture painting
Import Heightmaps	✅		Import from World Machine, GeoGen, Gaia. Anything that can export a 16 bit heightmap.
Level of Detail	✅		Very large landscapes possible
Multi-Layer PBR Materials	✅		Support up to 32 materials.<br>Can import multiple splatmaps.
Material Height Blending	✅		
Material Anti-Tiling	✅		
Physics Materials	✅		Physics properties are applied per material used: footstep sounds, bounciness, etc.
Foliage	✅		Grass & scatter meshes
Trees	✅		
Holes	✅		
Nav Mesh	✅		
Displacement	✅		Vertex displacement

---

### Creating Terrain
*Source: https://sbox.game/dev/doc/systems/terrain/creating-terrain/*

To add Terrain to your Scene, right click in the Hierarchy and select 3D Object → Terrain from the menu.
The Terrain inspector will then prompt you to create, import or link an existing Terrain asset.

Create New Terrain

Once you've created a Terrain GameObject it will be blank, you need to create the Terrain asset which stores all the height maps, control maps, materials, etc. It can then be reused across multiple scenes.

In the inspector window with the Terrain GameObject selected you will be asked how large you want your terrain to be. Pressing Create New Terrain will prompt you where to save your terrain asset.

Heightmap Size	The size of your heightmap.Higher values increase VRAM usage drastically for combined heightmap and control maps:2048 x 2048 = 24MB4096 x 4096 = 96MB8192 x 8192 = 384MB
World Scale	How many inches per heightmap texel.A smaller scale gives more precision at the reduction of overall size.<br>A larger scale gives more overall size at the reduction of precision.39 inches ~= 1 meter is a good default
Max Height	The maximum height your terrain will be in inches.The higher this is the less precision you get at lower values.

Link Existing Terrain

If you already have a terrain created in one scene that you want to reuse you can either drag the terrain asset in to automatically create the object, or press link existing and select an existing Terrain asset.

---

### Terrain Materials
*Source: https://sbox.game/dev/doc/systems/terrain/terrain-materials/*

A Terrain Material is an Asset that defines a set of PBR textures, physics surfaces, detail meshes and other properties that the Terrain uses to render.

Because they are Assets you can easily reuse them on multiple Terrains in different Scenes easily, transfer them between projects, or use them from the cloud.

These are similar but different to standard Materials because Terrain uses specialized shaders for rendering to deliver performant landscape rendering with LOD support and material blending.

The first Terrain Material you apply to a Terrain automatically becomes the base layer, and spreads over the whole landscape. You can then paint areas with other Terrain Materials blending them together.

Creating Terrain Materials

To create a new Terrain Material, select your Terrain GameObject and look at the Inspector, under Terrain Materials press New Terrain Material… Pick a save location and it will be automatically added to your Terrain Material list.

A resource editor window will open allowing you to select and modify the properties of the Terrain Material.

You can also create a Terrain Material that isn't automatically associated with a Terrain, by rick-clicking the Asset Browser and selecting New Asset → New Terrain Material…

Adding Terrain Materials

Existing Terrain Material assets can be dragged from the Asset Browser into the Terrain Material list - or onto the Terrain itself.

Terrain Materials can be found on the cloud by clicking the Browse… button, or filtering @cloud ext:tmat.

There is a limit of 4 Terrain Materials currently, this is planned to be resolved.

Terrain Material Properties

This is where I describe each property in an overwhelming amount of detail.

Painting Terrain Materials

The first material is applied across the entire Terrain by default. You can paint subsequent Terrain Materials using the Paint Texture tool and selecting the Terrain Material in the list in the inspector.

---


## 18. Assets

*Asset management, citizen characters, weapons*

### How to use them?
*Source: https://sbox.game/dev/doc/assets/ready-to-use-assets/citizen-characters/*

The Citizen character is a default player model provided by Facepunch.

How to use them?

This section needs to be rewritten with an explanation of all the code, libraries, etc.

Character source files

All source files come with the game: VMDLs, FBXs, animgraphs, etc.

You can find them right in the game folder, under addons/citizen/models/citizen. These files are synced straight from our source folders; when new source files are added on our end, they'll show up in that folder for you too!

If you would like to make animations for the Citizen, here is a community-made control rig to get you started: https://github.com/Ali3nSystems/Ecodelia-Tools-for-Facepunch-Assets

Procedural helper bones & constraints

These helper bones are set up to be procedural helpers driven entirely by constraints configured in ModelDoc. Open up citizen.vmdl and look at the AnimConstraintList prefab to see what makes them tick!

arm_upper_[R/L]_twist[0/1] (from shoulder to biceps)
arm_elbow_helper_[R/L] (better deformation for extreme bend angles on the elbow)
arm_lower_[R/L]_twist[0/1] (from forearm to wrist)
leg_upper_[R/L]_twist[0/1] (from pelvis to thigh)
leg_knee_helper_[R/L] (better deformation for extreme bend angles on the kneecap)
leg_lower_[R/L]_twist[0/1] (from calf to foot)
neck_clothing (reduces the twist of the neck by two-thirds; some clothing items use it)

Because they're procedural, animation data doesn't need to be exported from your 3D program for these; in fact, it's set up to be ignored. Our own animation FBX files usually don't have them exported, which has the added benefit of slightly saving on file size.

If you're making a model (e.g. clothing) to be bonemerged on top of the citizen, you don't actually need to skin your mesh to these bones... but it's better if you do!

The limb bone is a sort of "container" for its *_twist{0/1} bones. The limb bones aren't skinning anything and should never skin anything; the twist bones are. Think of them as the "upper part" and the "lower part" of each limb respectively. This is how the height scaling feature works.

Adding new animations

If you only need to add a few animations to the VMDL, you can try using the "Base Model" feature. This is similar to the Source 1 "$includemodel" feature. You can only add animations to an existing model using this feature; you can't add anything else. You only need to reference the official citizen.vmdl file in the "Base Model" field, above the "Add" button in ModelDoc.

Name your new animations something unique (maybe by giving them all a prefix); bad things could happen if animation names collide.

You could also simply make your own "fork" of citizen.vmdl while keeping the references to prefabs intact, but you might have to sync some changes manually.

IK bones

These bones are used to drive IK constraints in-engine. They need animation data, and the default graph assumes it is present. This data is baked by the 3D animation program during the exporting process.

root_IK is the parent to all *_IK_target bones. These are effectively "model-space" IK targets.

Why are we doing it this way?

Because the position of IK targets need to be kept in a different space that won't be affected by any layers, weightlists, etc. that animation compositing is touching, otherwise we can't restore the positions through IK afterwards! Think of them as a way to keep the pos/rot data of hands and feet intact, separately.

root_IK is, in the control rig, constrained to follow X & Y of the pelvis, but with Z (height) forced to 0. It also needs to always point forwards (zero rotation), otherwise funky things can happen. Similarly, the hand/feet target bones are pos/rot constrained to their respective hand/feet bones in the control rig.

Additionally, there are two special IK bones, which are used to reproduce the ikrule touch feature from Source 1.

hand_L_to_R_ikrule (child of right hand, constrained to the left hand)
hand_R_to_L_ikrule (child of left hand, constrained to the right hand)

These are used as a way to keep the position and orientation of hands the same relative to each other (in their local space), even after applying layers, weightlists etc. and they are pos/rot constrained in the control rig.

In the animgraph, this is most often how the left hand is made to stick to a two-handed weapon that's driven by the right hand! It's not sticking to the gun itself, it's made to stick back to its original position relative to the right hand, even after all the aim matrices, breathing additive motion, etc. has been applied.

There may be different IK solutions implemented in the future.

LOD reference & guidelines

To give you an idea of how we use LODs on the Citizen... here are the figures for the base meshes.

LOD0: 6.6k tris (+ 7.2k head).
LOD1: 4.2k tris (+ 7.2k head) @ distance of 5, so it happens fairly close and very fast. This level is used to trim the low-hanging fruit that has an almost-zero impact to visuals, as soon as possible. Here, we only use LOD1 to trim the poly density of the feet and fingers. The body and leg meshes remain the exact same; this means far fewer headaches with not needing to sync with underlying topology there. The head remains the same as LOD0(*).
LOD2: 1.7k tris (+ 1.0k head) @ distance of 20. We are at a medium distance, slightly on the side of long. Helper bones are culled.
LOD3: 1.0k tris (+ 0.4k head) @ distance of 40. Long distance. Even more bones are culled.
LOD3: 1.0k tris (+ 0.2k head) @ distance of 70. Longest distance. This only reduces the head further, cutting down a bit more of its geometry, but most importantly removes its eyes, which cuts down the material count (and therefore draw calls) by one.

(*) For now. Whenever our tech & tools allow us to start maintaining a lower-density head with no headaches with regards to transferring morphs etc., we'll probably try to target a 4-5k tris head for LOD1.

Some clothing items might shift LOD1 back to a distance of 10 instead of 5, but this is only applicable when they are displayed on their own, away from the player; s&box implements a sort of "LOD sync", which makes bonemerged models follow the LOD level of their parent.

Here's a decent rule of thumb for telling at which distances your model needs to switch down a LOD level. Zoom out slowly, and look at when the wireframe starts looking like "solid green". Of course, it doesn't tell the whole story, but it's a good place to start from.

If you are using more than one material, and can cull the number of total materials back to one in your LOD meshes, you should do so!

Stretching limbs

The Citizen has "support" for stretching and squashing the lengths of arms and legs while still looking reasonably good, thanks to the elbow & kneecap helpers. This is NOT actual "per-bone" scaling; the animations don't store scaling values. However, animations are, in a sense, fundamentally just a collection of lists of position & orientation coordinates for X bones at Y times. For example, the lower arm, being a child of the upper arm, sits in its parent space at a position of X = 10. There's nothing that prevents an animation from saying "in my animation, that X position offset is actually 12", meaning a 20% stretch.

Likewise, there's nothing that prevents any bone from changing its position in their parent's local space. Human anatomy doesn't work that way, of course, but the Citizen is not a photorealistic character, and pushing bones around in various ways allows to get more interesting poses! Almost all of the "long idle" standing poses do this to various extents.

About source file scaling

All source files are in centimeters. But because just about everything else is using inches, the model is scaled down using the ScaleAndMirror modifier in ModelDoc. (We're NOT doing it at the import level on each mesh node.)

Therefore, if you want to create something for the character (clothing, etc.), you should also work in centimeters, with the provided sources, and in your VMDL, you'll apply the same ScaleAndMirror modifier with the same value of 0.3937. (4 decimals only is an arbitrary choice on our part.)

Why?

This way, no one has to do any scaling using 3D packages, which are notoriously unreliable and inconsistent in how they choose to perform the scaling, especially during export steps. Also, the ability to flawlessly and arbitrarily scale any 3D model, even animated ones, including all their data, is extremely valuable to have, and important enough to be dogfooding.

---

### How to use our weapons
*Source: https://sbox.game/dev/doc/assets/ready-to-use-assets/first-person-weapons/*

Facepunch provides some ready-to-use first-person weapons for you!

➡️ https://sbox.game/facepunch/sboxweapons

These first-person resources are "open-source". If downloaded from the editor, they will come bundled with their source files: VMDL, FBX, animgraphs & subgraphs…

… But source files are currently NOT included with downloads, while the feature is getting re-implemented in a better way.

Our first-person arms are also in the collection; here's a direct link for convenience. By themselves, they contain and are assigned the "punching" animgraph, for all of your barehanded melee needs.

How to use our weapons

You may be familiar with the classic Source 1 setup of bonemerging separate weapon models onto a single common arms model. However, our weapon assets require the opposite.

You must bonemerge the arms → onto → the weapons.

The recommended maximum FOV for these is 80° (horizontal) at 16:9. I try to keep them looking OK in 4:3 and at higher FOVs, but this is not a guarantee.

Common animgraph parameters

Now that you've bonemerged arms onto our weapons, you can send animgraph inputs to the weapon just like you would with any other model.

As a reminder, ♻️ self-resetting parameters set themselves immediately back to default after one frame. For example, you don't need to set b_attack back to false yourself; it does that on its own.

Movement
Parameters	Type & values	Description
b_grounded	☑️ bool	Behaves like on characters; can be replicated as-is from third-person to first-person.
b_jump	☑️ bool, ♻️ self-resets	Behaves like on characters; can be replicated as-is from third-person to first-person.
b_sprint	☑️ bool	Controls sprinting stance. I recommend to set to true only if sprint key held and player is moving.
move_bob	🎚️ float, 0.0↔1.0	Intensity of movement animations sway/bob (equivalent to move_groundspeed in 3P).
move_bob_cycle_control	🎚️ float, 0.0↔1.0	Manual control of movement animation phase. Think of it like scrubbing through the animation yourself. Active if ≠ 0.0. If = 0.0, auto-resumes normal behaviour after 100ms.
move_x, move_y, move_z	🎚️ float, -1.0↔1.0	Normalized movement input; unused but reserved for future, and should ideally be set anyway.
Weapon mechanics, actions, and states
Parameters	Type & values	Description
b_attack	☑️ bool, ♻️ self-resets	Plays the gun's fire animation, or throws a punch.
b_attack_dry	☑️ bool	Use this instead of b_attack when the gun is empty.
b_attack_hit	☑️ bool	Set to true if the attack connects (used for melee hit/miss animation variations).
attack_hold	🎚️ float, 0.0↔1.0	Staggered recoil for continuous fire; blend toward 1 when holding fire and continuously firing.
b_reload	☑️ bool, ♻️ self-resets	Triggers reload animation.
b_empty	☑️ bool	Set to true if magazine/clip is empty; is used to switch to different reload animations and affect weapon visuals (e.g. slide pulled back).
ironsights	🗂️ enum, 1 = ADS	Trigger "aim down sights" stance. The animation is only in charge of aligning the gun in its default state, additional offsets (for attachments) are up to you in code.
ironsights_fire_scale	🎚️ float, 0.0↔1.0	Scale down strength of fire animations while aiming down sights.
firing_mode	🗂️ enum	Reflects firing mode selector on the weapon. Values vary. Usually: 0 = safety/off, 1 = single, 2 = burst, 3 = auto.
b_deploy_skip	☑️ bool	Skip the deploy animation when the animgraph initializes.
b_twohanded	☑️ bool	Toggle between one-handed and two-handed animation sets. Only supported by some weapons.
b_lower_weapon	☑️ bool	Aim the weapon away from center and lower it (HL2-style ally-friendly aim posture).
b_holster	☑️ bool	Holster the weapon. Unticking will trigger a re-deploy, but it's not guaranteed to be ideal. Recommendation: once your code detects the 🏷️holster_finished tag, destroy the GameObject. Recreate it if the same weapon is drawn again.
weapon_pose	🗂️ enum	Adjust pose for attachments. Used by some weapons; refer to the section below.
b_grab	☑️ bool	Trigger the "grab stance" (left hand ready, towards the center of the screen).
grab_action	🗂️ enum, ♻️ self-resets	Trigger a "grab gesture". 1 = sweep down, 2 = sweep right, 3 = sweep left, 4 = push button.
deploy_type, reload_type	🗂️ enum	Used by some weapons; refer to the section below.
Others
Parameters	Type & values	Description
camera_position_scale<br>camera_rotation_scale	🎚️ float, 0.0↔2.0	Control the strength of camera animations. Setting the float above 1.0 makes them stronger (but only up to 2.0).
Speed scaling

You can change these 🎚️ floats at any time, including in the middle of the animations they affect!

Parameters	What's affected
speed_reload	Reload animations
speed_deploy	Deploy & holster animations
speed_ironsights	Ironsight transitions
speed_grab	Grab stance & grab gestures
Aim modifiers

The aim_pitch_inertia and aim_yaw_inertia parameters (🎚️ floats, exploitable range of -45↔45) control an animated, bouncy "lag" of the weapon when looking around (or other parts, e.g. the left arm when "grab stance" is active).

Parameters & behaviours specific to certain weapons
v_first_person_arms (🤜 punching animgraph)
(todo…)
v_crowbar (λ)
Swings every 400-450ms are recommended.
The implementation of a "viewpunch"-style camera rotation kick when you hit something is, as of the time of writing, up to you.
b_attack_has_hit is used.
v_m4a1 (assault rifle)
weapon_pose (🔢 int) should be set to 1 if the handguard covers bodygroup is active.
reload_type (🗂️ enum) can be set to 1 for a "pull" animation, instead of tossing the magazine.
deploy_type (🗂️ enum) can be set to 1 for a faster variant.
v_m700 (sniper rifle)
b_reload_bolt (☑️ bool, ♻️ self-resets) triggers a bolt action. This can be done at any time. The firing pin (striker) will be sent back to its ready position only after using this parameter.
deploy_type (🗂️ enum) can be set to 1 for a faster variant.
v_mp5 (submachine gun)
deploy_type (🗂️ enum) can be set to 1 for a faster variant.
v_physgun
b_button (☑️ bool) makes the right hand push down the big button on the iron part.
brake (🎚️ float, 0.0↔1.0) makes the left hand squeeze the bicycle brake.
stylus (🎚️ float, -1.0↔1.0) makes the record player stylus needles go in/out.
v_recoillessrifle (🚀 rocket launcher)
deploy_type & reload_type (🗂️ enums) can be set to 1 for faster variants.
v_spaghellim4 (shotgun, automatic)
b_reload is NOT self-resetting. Toggle it on to start reloading, toggle it off to end. This is the "simple" way to handle reloads. See below for advanced controls.
b_reloading (☑️ bool) activates a "reloading stance".
During this, you can fire b_reloading_shell (☑️ bool ♻️ self-resets) at any pace.
The animation for inserting the first shell through the carrier can be triggered with b_reloading_first_shell (☑️ bool ♻️ self-resets) regardless of the other parameters.

↳ You can trigger these parameters in (mostly) any order, at (mostly) the pace that you want.

deploy_type (🗂️ enum) can be set to 1 for a faster variant.
The 🏷️reload_bodygroup tag should be used as a hint to display the shell bodygroup when it's active, and to keep it disabled whenever it's not.
v_toolgun
b_twohanded (☑️ bool) is supported.
trigger_press (🎚️ float, 0.0↔1.0) will make the hand visually squeeze the trigger. This is because the toolgun is not a "conventional" weapon, and it needs to be able to "fire" without that actually being an "attack".
b_joystick (☑️ bool) activates a "joystick stance".
During this, use joystick_x and joystick_y (🎚️ floats, -1.0↔1.0) to make the right thumb control the joystick. with Note that diagonal directions would be roughly ±0.71 in both axises (because the joystick is a circle, not a square).
firing_mode is assigned to the state of the side switch. 0 = middle, 1 = up, 2 = down.
coil (🎚️ float, 0.0↔1.0) controls the orientation of the coil, so it can be spun.
v_trenchknife & v_m9bayonet (🔪 knives)
Melee attacks automatically chain into different swing animations if b_attack is called again within ≈500ms. The animations & animgraph were made with swings every 350ms in mind.
b_backstab (☑️ bool) activates a "backstab stance". During this stance:
Attacks will change to be "heavy" attacks, similar to Counter-Strike games.
You can toggle this stance at any time, but note that doing so during an attack will interrupt it.
b_attack_has_hit is used during this state.
v_usp (🔫 pistol)
b_twohanded (☑️ bool) is supported.
deploy_type (🗂️ enum) can be set to 1 for a different animation, without the safety being turned off.
💣 Throwables

These include: v_flash_grenade, v_decoy_grenade, v_he_grenade, v_molotov, and v_incendiary_grenade.

b_charge (☑️ bool, ♻️ self-resets) triggers the "ready to throw state".

↳ then b_attack will play the "throw" animation at any time (but preferably during this state).

b_pin_remove (☑️ bool) will instantly remove the pin from all animations (and should therefore be set as soon as the graph initializes). This is useful if you set charge_type to 1 (so that it doesn't look like the player is throwing a grenade that still has its pin).
charge_type (🗂️ enum) determines which type of "readying" is performed: pulling the pin or not. (Has no effect on v_molotov.)
throw_blend(🎚️ float, 0.0↔1.0) blends the left hand between far (0) & near (1) poses in the "ready to throw" state, similar to Counter-Strike.
throw_type (🗂️ enum) determines the throw animation. 0 = far, 1 = near.

Once a throw animation has finished, you must trigger b_reload to bring up a new throwable.

With these parameters at your disposal, there are three ways you can implement grenades:

Counter-Strike style: deploy normally → charge_type = 0 (pull pin) → throw.
Faster style: deploy with b_pin_remove = true → charge_type = 1 (lift) → throw.
Cook style: deploy with b_pin_remove = true → trigger b_lever_release = true → charge_type = 1 (lift) → throw.
Tags

Animgraphs can use "Internal Tags" for various purposes (letting parts of the graph communicate with one another without spaghetti wiring) — but there are also "Event Tags" that are sent to the game code to let it know about various events. The most common example is changing the bodygroup of a mesh mid-reload animation, so that a held empty magazine becomes full again.

See OnAnimTagEvent. Tags are passed as a string, as-is; they are effectively "hints" for the code, they don't contain any other data than their name. You won't have to hard-code lengths, timings, etc. and handle logic that scales those timings based on speed scaling parameters. All you have to do is listen for these tags!

Standard "Event Tags"
🏷️attack_discouraged: you probably shouldn't let the player attack/fire right now (because it would look weird or off), but there's nothing stopping you; this is just a hint. For example, when it comes to reload animations, this tag will let you know when the weapon will be "ready" again (aiming at the crosshair).
🏷️holster_finished: fired once the holster animation is done playing; this lets you know when it's safe to switch without interrupting the animation (which might feel weird, as some weapons significantly animate the camera's position).
🏷️reload_bodygroup: the clip/magazine/etc. went off-screen during the reload animation, and now's the time for code to swap something to a different bodygroup (e.g. from empty to full).
🏷️reload_increment: lets you know exactly when in the animation a mag/clip/etc. has been inserted, and you can change the ammo counter.
🏷️melee_swing: active across an entire melee swing (in case you want some sort of continuous hit detection)
🏷️melee_hit: the actual time when a melee swing connects with its target, in the center of the screen. Swinging takes a bit of time, so you could use this to delay sounds, particle effects, etc.
🏷️melee_plant: the time range during which a melee weapon stays planted inside something or someone (e.g. a knife in someone's back). Example use case: during this tag, freeze player movement, freeze the camera, or freeze the viewmodel rotation relative to the player view + soft-clamp player look to ±10°…
🦴 Camera bone

The camera is animated through the 🦴camera bone. Its position (relative to the first-person 0,0,0) and orientation (relative to +X forwards, +Y left, +Z up) should add onto your in-game camera.

Our animgraphs automatically "weaken" this camera animation by 50% while moving (this is mapped to the use of the move_bob float).

The "positional" part of these animations should always be used. As for rotations, feel free to offer a toggle for players prone to motion sickness, or to simply not play it back.

Replacing weapons with your own

You might want to use your own weapon meshes. You can hide them, and then bonemerge (or simply parent) yours on top. For example, as pictured in this image, you could grab v_crowbar, hide the crowbar itself, then add a police baton on top.

Then, instead of manually rotating GameObjects through the viewport, you can correct the grip of the hand(s) with our simple finger adjustment parameters!

They're all floats with a range of -60,60, emulating a 3ds Max CAT-style finger controller setup. The syntax goes like this: FingerAdjustment_{``*L*``|``*R*``}{``*1*``|``*2*``|``*3*``|``*4*``|``*5*``}_{``*Bend*``|``*Curl*``|``*Roll*``|``*Spread*``}

For example: FingerAdjustment_L3_Bend will bend the left hand's middle finger.

This approach has many benefits. Here's one: you can store collections of different finger adjustment parameters through your C# code, and easily switch between them based on conditions. For example, if you were to swap the arms mesh for thick heavy gloves, you might add a slight offset to all curl and spread parameters, to account for the thickness.

Technical details

Each weapon contains its own skeleton. There are three separate root hierarchies: the weapon bones, the arms bones, and the camera.

Under 🦴weapon_root, there's 🦴weapon_root_children, and under that one, different bones for every weapon (as the various mechanical bits of every gun are different).

There are two IK bones under 🦴weapon_root, one for each hand.

The control rig used to create animation has these bones constrained to be at the same position and orientation as the hand bones, and this data is baked out during export, when the exporter "flattens" the animation by making every frame a keyframe. This means that the weapon knows where the hands should be relative to itself at any time — and the hands are always corrected back via IK in their animgraphs.

From there, it becomes very easy to apply various layers and additives (like movement bobbing) and have the hands always stick where they should be. Most of the time, only 🦴weapon_root needs to move… and this means a lot of animations can be shared between guns!

---


## 19. Editor Development

*Custom editors, tools, widgets, apps*

### Creating
*Source: https://sbox.game/dev/doc/editor/editor-project/*

Your game can have an editor component to it. Your game's editor project is special in that it can access both the tools and the game code.

Editor projects are not sandboxed. They are not limited by any whitelists and can run any functions. You should be careful when running code you have received from an untrusted source - because it can do almost anything.

Creating

To create an editor project you simply create a folder named "editor" in your project folder. Any code in this folder will be treated as part of the editor project.

You will get a new project in your IDE called <projectname>.editor.

Why create an Editor Project

Creating an editor project lets you do a few special things.

Create Editor Widgets
Create Editor Tools
Create Custom Inspectors for your Components or GameResources
Create new Control Widgets
Create new Editor Docks
Create Editor Apps
Create Editor Tools
Create Asset Previews

---

### Creating a Widget
*Source: https://sbox.game/dev/doc/editor/editor-widgets/*

Editor UI is built entirely out of Widgets. Widgets are different from Panels, which are used for in-game UI. Widgets can be various elements or components, such as labels, buttons, text boxes, trees, or images.

If a Widget does not have a parent, it is a Root Widget. This Widget will act as a window on the user's OS.

Creating a Widget

Each Widget has a Layout, which can contain child Widgets or sub-Layouts. Widgets can also be styled with CSS similarly to Panels.

public class ExampleWidget : Widget
{
	public ExampleWidget(Widget parent) : base(parent, false)
    {
        // Create a Column Layout
		Layout = Layout.Column();
        // Give it some Margins/Spacing
        Layout.Margin = 4;
        Layout.Spacing = 4;
        // Apply some CSS styling
        SetStyles( "background-color: #303445; color: white; font-weight: 600;" );

        // Add some child Widgets to the Layout
        Layout.Add(new Label("Press the button:", this));
        var btn = Layout.Add(new Button("Click me!", this));
        btn.Clicked += () =>
		{
			Log.Info("You did it!");
		};
	}
}

You can now create this Widget anywhere else in your Editor Project by doing the following:

// Creates the Widget as a new Window since it has no parent
var windowExample = new ExampleWidget(null);
windowExample.Show()

// Creates the Widget as the child of another Widget, then adds it to that Widget's Layout
var childExample = new ExampleWidget(parentWidget);
parentWidget.Layout.Add(childExample);

Creating a Dockable Widget

Creating a Widget with the [Dock] attribute will allow it to be docked within any DockWindow. It will also be added to the View menu so it can be toggled easily.

[Dock("Editor", "Example Editor Dock", "local_fire_department")]

Creating an Asset Editor

Including the [EditorForAssetType] attribute will open your widget upon double-clicking the specified asset. Doing this will invoke AssetOpen() on your Widget through IAssetEditor so you can get any Asset information.

// Supply the file extension of the asset, cannot be more than 8 characters
[EditorForAssetType("item")]
public class ItemEditorExample: Window, IAssetEditor
{
    // Return false if you want the have a Widget created for each Asset opened,
    // Return true if you want only one Widget to be made, calling AssetOpen on the open Widget
	public bool CanOpenMultipleAssets => true;

    Asset MyAsset;
    ItemResource MyItem;
    Label MyLabel;

	public ItemEditorExample()
    {
        // Cannot modify the Layout of a Window, instead we have a Canvas Widget
        Canvas = new Widget( null );
        Canvas.Layout = Layout.Column();
        Canvas.Layout.Spacing = 8;
        Canvas.Layout.Margin = 8;
        MyLabel = Canvas.Layout.Add( new Label( "", this ) );
        var btn = Canvas.Layout.Add( new Button( "Reset Name", this ) );
        btn.Clicked += ResetName;
        Show();
    }

    // From IAssetEditor
	public void AssetOpen(Asset asset)
    {
        // Get the Resource from the Asset, from here you can get whatever info you want
        MyAsset = asset;
        MyItem = MyAsset.LoadResource<ItemResource>();
        MyLabel.Text = MyItem.Name;

		// If CanOpenMultipleAssets returns true, you should refocus this widget
		Focus();
	}

    // From IAssetEditor
    public void SelectMember(string memberName) { }

    void ResetName()
    {
        // If we modify the Resource at all, we can save those changes with SaveToDisk
        MyItem.Name = "N/A";
        MyLabel.Text = MyItem.Name;
        MyAsset.SaveToDisk(MyItem);
    }
}

---

### Creating
*Source: https://sbox.game/dev/doc/editor/editor-apps/*

Editor Apps are apps that run in the editor. They generally have their own window. They're sometimes used to edit specific types of asset.

Examples of editor apps are the ShaderGraph, Material Editor, Model Editor.

Creating

To create an Editor App, you just need to add an [EditorApp] attribute to its main window.

[EditorApp( "Example App", "pregnant_woman", "Inspect Butts" )]
public class MyEditorApp : Window
{
	public MyEditorApp()
	{
		WindowTitle = "Hello";
		MinimumSize = new Vector2( 300, 500 );
	}
}

The app will be available on the App sidebar and the Apps menu.

---

### The Scene
*Source: https://sbox.game/dev/doc/editor/editor-tools/*

You can create your own editor tool to help you create your game. Your tool needs to be created in an editor project.

[EditorTool] // this class is an editor tool
[Title( "Rocket" )] // title of your tool
[Icon( "rocket_launch" )] // icon name from https://fonts.google.com/icons?selected=Material+Icons
[Shortcut( "editortool.rocket", "u" )] // keyboard shortcut
public class MyRocketTool : EditorTool
{
	public override void OnEnabled()
	{

	}

	public override void OnDisabled()
	{

	}

	public override void OnUpdate()
	{

	}
}

This will create a tool that you can select here.

The Scene

The EditorTool has a member called Scene to access the scene.

public override void OnUpdate()
{
	var tr = Scene.Trace.Ray( Gizmo.CurrentRay, 5000 )
					.UseRenderMeshes( true )
					.UsePhysicsWorld( false )
					.WithoutTags( "sprinkled" )
					.Run();

	if ( tr.Hit )
	{
		using ( Gizmo.Scope( "cursor" ) )
		{
			Gizmo.Transform = new Transform( tr.HitPosition, Rotation.LookAt( tr.Normal ) );
			Gizmo.Draw.LineCircle( 0, 100 );
		}
	}
}

Preventing Selection

Depending on how your tool operates, you might want to prevent the user's ability to click to select GameObjects in the scene. To do this you can change AllowGameObjectSelection on your tool.

public override void OnEnabled()
{
	AllowGameObjectSelection = false;
}

Creating Overlay UI

You can create UI on the scene's overlay. This is useful for creating controls and other things.

public override void OnEnabled()
{
    // create a widget window. This is a window that  
    // can be dragged around in the scene view
	var window = new WidgetWindow( SceneOverlay );
	window.Layout = Layout.Column();
	window.Layout.Margin = 16;

    // Create a button for us to press
	var button = new Button( "Shoot Rocket" );
	button.Pressed = () => Log.Info( "Rocket Has Been Shot!" );

    // Add the button to the window's layout
	window.Layout.Add( button );

    // Calling this function means that when your tool is deleted,
    // ui will get properly deleted too. If you don't call this and
    // you don't delete your UI in OnDisabled, it'll hang around forever.
	AddOverlay( window, TextFlag.RightTop, 10 );
}

The UI is created when the tool is activated, and destroyed when it's deactivated.

---

### Defining
*Source: https://sbox.game/dev/doc/editor/editor-tools/component-editor-tools/*

Component Editor Tools work a lot like regular Editor Tools, but they're always active when a specific Component is selected. These tools generally create UI in the scene view, but they can also override input too.

An example of a component tool is the camera preview - which is shown when a GameObject with a CameraComponent is shown.

Defining

To define an EditorTool for your Component, you create a class like this.

public class MyEditorTool : EditorTool<MyComponent>
{

	public override void OnEnabled()
	{

	}

	public override void OnUpdate()
	{

	}

	public override void OnDisabled()
	{

	}

	public override void OnSelectionChanged()
	{
		var target = GetSelectedComponent<MyComponent>();
	}
}

The method OnSelectionChanged is called after the tool is created and registered. It can also be called later if the selection is changed to another component.

The tool is automatically deleted/destroyed when the selection no longer contains the specific component type.

---

### Creating a Custom ControlWidget
*Source: https://sbox.game/dev/doc/editor/custom-editors/*

When creating your own Classes/Structs/Assets/ect, you'll sometimes want custom editors that pair with them. For example, a Gradient Editor so you can visually see what the Gradient looks like instead of editing the Gradient as if it were a Struct with a list of Colours.

Creating a Custom ControlWidget

The above are examples of ControlWidgets. The Inspector will try to find the appropriate one for each type, falling back on a Class/Struct editor (or none if it's not applicable). We can define our own with [CustomEditor]

public class MyClass
{
    public Color Color { get; set; }
    public string Name { get; set; }
}

[CustomEditor(typeof(MyClass))]
public class MyCustomControlWidget : ControlObjectWidget
{
    // Whether or not this control supports multi-editing (if you have multiple GameObjects selected)
    public override bool SupportsMultiEdit => false;

    public MyCustomControlWidget(SerializedProperty property) : base(property)
    {
        Layout = Layout.Row();
        Layout.Spacing = 2;

        // Get the Color and Name properties from the serialized object
        SerializedObject.TryGetProperty(nameof(MyClass.Color), out var color);
        SerializedObject.TryGetProperty(nameof(MyClass.Name), out var name);

        // Add some Controls to the Layout, both referencing their serialized properties
        Layout.Add( Create(color) );
        Layout.Add( Create(name) );
    }

    protected override void OnPaint()
    {
        // Overriding and doing nothing here will prevent the default background from being painted
    }
}

You can also check for certain attributes, so you can have a custom Password string editor that only appears when you've added the [Password] attribute

[CustomEditor(typeof(string), WithAllAttributes = new[] { typeof(PasswordAttribute) })]

Creating a Custom InspectorWidget

While creating Custom ControlWidgets is very powerful, sometimes you might want to replace the entire Inspector. This is especially useful for Editor Tools or Assets, and is done with the [Inspector] attribute.

using static Editor.Inspectors.AssetInspector;

// [CanEdit] is used for editing Assets with a certain file extension, if the thing you wish to
// inspect is NOT an Asset, you can use [Inspector(typeof(MyClass))]
[CanEdit("asset:char")]
public class CharacterInspector : Widget, IAssetInspector
{
    CharacterResource Character;
    ControlSheet MainSheet;

    // If this isn't an Asset Inspector, use CharacterInspector(SerializedObject so) : base(so)
    public CharacterInspector( Widget parent ) : base( parent )
    {
        Layout = Layout.Column();
        Layout.Margin = 4;
        Layout.Spacing = 4;

        // Create a header
        var header = Layout.Add( new Label( "My Inspector!", this ) );
        header.SetStyles( "font-size: 42px; font-weight: 600; font-family: Poppins" );

        // Create a ontrolSheet that will display all our Properties
		MainSheet = new ControlSheet();
		Layout.Add( MainSheet );

        // Add a randomize button below the ControlSheet
        var button = Layout.Add( new Button( "Randomize", "casino", this ) );
        button.Clicked += () =>
        {
        	foreach ( var prop in Test.GetSerialized() )
        	{
        		// Randomize all the float values from 0-100
        		if ( prop.PropertyType != typeof( float ) ) continue;
        		prop.SetValue( Random.Shared.Float( 0, 100 ) );
        	}
        };

        // Fill the remaining space and align everything else to the top
        Layout.AddStretchCell();

        // Populate the ControlSheet
        RebuildSheet();
    }

    // Rebuild the ControlSheet every hotload, so we catch any changes to the Asset
    [EditorEvent.Hotload]
    void RebuildSheet()
    {
        if ( Character is null ) return;
        if ( MainSheet is null ) return;
        MainSheet.Clear( true );

        // Populate the ControlSheet using only `float` Properties
        var so = Character.GetSerialized();
        so.OnPropertyChanged += x =>
        {
          Character.StateHasChanged(); // Mark as dirty so we can save
        };
        MainSheet.AddObject( so, x => x.PropertyType == typeof( float ) );   
    }

    // Only needed if Asset Inspector, and you are implementing IAssetInspector
    public void SetAssetPreview( AssetPreview preview )
    {
        // You can store the AssetPreview if you wish to interact with it later
        // Useful for an animation list that will update the Preview
    }

    // Only needed if Asset Inspector, and you are implementing IAssetInspector
    public void SetAsset( Asset asset )
    {
        Character = asset.LoadResource<CharacterResource>();
        RebuildSheet();
    }
}

---

### String-Specific
*Source: https://sbox.game/dev/doc/editor/property-attributes/*

You can add attributes to your Component's properties in C# to change how they look in the editor/inspector.

[Hide]

Hides the property from the editor. Will still be a property, and will still save and load - but won't be visible.

[RequireComponent]

Added to a property containing a component. The component will be created if it doesn't exist.

[Group( "Helpers" )]

Will create a group box and add this property to that group. Mark multiple properties to add them to the same group.

[ToggleGroup( "UseHelpers" )]

A group that has a checkbox to turn it on and off. The name should match the name of a bool property to hold the state.

[Title( "My New Title" )]

Change the title of the property in editor, instead of using the property name.

[Feature( "Helpers" )]

Adds this property to a separate feature tab. All other properties will be in a tab called General.

[FeatureEnabled( "Helpers" )]

Should be used on a bool property. This allows you to turn the feature on and off by removing or adding the tab.

[Order( 100 )]

Change the order of this property in the editor. Their position in the source usually orders them.

[ShowIf( "PropertyName", 3 )]

Only show this property if another property is equal to the value.

[HideIf( "PropertyName", 3 )]

Hide this property if another property is equal to the value.

[Range( 0, 100 )]

When used on a number property, the control widget will have a min and max value provided, with an optional argument to clamp the value (enabled by default, so the user cannot manually input a number outside of the range) and another to show a slider instead of a number field (enabled by default)

[Step( 10 )]

When used on a number property, the control widget will only increment by the step amount provided when clicked and dragged.

[Space]

Add a space above the property.

[Header( "My Header" )]

Add a header above the property.

[ReadOnly]

Don't allow changing this property.

[Flags]

Indicates that an enum can be treated as a set of flags. Allows for selecting multiple flags at once from the dropdown instead of just selecting one.

[InlineEditor]

Tell the editor to try to display inline editing for this property, rather than hiding it behind a popup.
(useful for custom Classes/Structs)

[WideMode]

Fill the width of the editor with the widget and put the label above instead of to the left of the ControlWidget. Can optionally hide/remove the label entirely.

[Validate( nameof( IsValid ), "Warning Message", LogLevel.Warn )]

n

Specifies a method in the same class to use for validation. The validation result will be shown in the inspector.

String-Specific
[Placeholder( "Your mother's maiden name" )]

When used on a string property, will show this text when it's empty.

[TextArea]

When used on a string property, show a multiline text area instead of a single-line text edit.

[FilePath]

When used on a string property, creates a File Picker allowing you to select any file, with an optional Extension you can use to specify certain file type(s).

[ImageAssetPath]

When used on a string property, creates a ResourceControlWidget allowing you to select an image file, setting the string's value to the path of the image.

[MapAssetPath]

When used on a string property, creates a ResourceControlWidget allowing you to select a vmap file, setting the string's value to the path of the map.

[FontName]

When used on a string property, creates a Font dropdown allowing you to easily select a font by it's name.

[InputAction]

When used on a string property, creates an Input dropdown comprised of all Inputs configured in your Project Settings, allowing you to easily select an Input by it's name.

Curve-Specific
[TimeRange]

Used to specify a default or clamped time/x-range on a curve

[ValueRange]

Used to specify a default or clamped value/y-range on a curve

ActionGraph-Specific
[SingleAction]

When used on any ActionGraph-compatible property, it will only show a single action to edit instead of a list that allows you to create multiple

---

### Asset Previews
*Source: https://sbox.game/dev/doc/editor/asset-previews/*

If you want a specific file/asset type to have a custom Thumbnail/Inspector Preview, you can simply create an AssetPreview. An AssetPreview initializes a SceneWorld and SceneCamera, rendering the Camera to the Preview output, so all you need to do is populate it and/or position the Camera to your liking.

Model Example
[AssetPreview( "mymdl" )]
public class PreviewMyModelResource : AssetPreview
{
	// The speed at which the model rotates. The length of a cycle in seconds is 1 / CycleSpeed
	public override float PreviewWidgetCycleSpeed => 0.2f;

	// This will evaluate a few frames and pick the one with the least alpha and most luminance for the thumbnail
	public override bool UsePixelEvaluatorForThumbs => true;

	public PreviewMyModelResource( Asset asset ) : base( asset )
	{
	}

	public override Task InitializeAsset()
	{
		// Get the resource from the asset
		var resource = Asset.LoadResource<MyModelResource>();

		// Get the Model from the resource
		var model = resource?.Model;
		if ( model is null )
		{
			return Task.CompletedTask;
		}

		// Make sure we are scoped to the preview scene
		using ( Scene.Push() )
		{
			// Create a new GameObject with a SkinnedModelRenderer component 
			PrimaryObject = new GameObject( true, "Model Preview" );
			PrimaryObject.WorldTransform = Transform.Zero;

			var modelRenderer = PrimaryObject.AddComponent<SkinnedModelRenderer>();
			modelRenderer.PlayAnimationsInEditorScene = true;
			modelRenderer.Model = model; // Set the model on the renderer

			// Center the scene around the model bounds, so the camera is positioned correctly
			SceneSize = model.Bounds.Size;
			SceneCenter = model.Bounds.Center;
		}

		return Task.CompletedTask;
	}
}

Texture Example
[AssetPreview( "mytex" )]
public class PreviewMyTextureResource : AssetPreview
{
	// Since we're only previewing a texture, we don't need to bother rendering a video
	public override bool IsAnimatedPreview => false;

	public PreviewMyTextureResource( Asset asset ) : base( asset )
	{
	}

	public override Task InitializeAsset()
	{
		// Get the resource from the asset
		var resource = Asset.LoadResource<MyTextureResource>();

		// Get the Texture from the resource
		var texture = resource?.Texture;
		if ( texture is null )
		{
			return Task.CompletedTask;
		}

		// Make sure we are scoped to the preview scene
		using ( Scene.Push() )
		{
			// Create a new GameObject with a SpriteRenderer component 
			PrimaryObject = new GameObject( true, "Texture Preview" );
			PrimaryObject.WorldTransform = Transform.Zero;

			var spriteRenderer = PrimaryObject.AddComponent<SpriteRenderer>();
			spriteRenderer.Texture = texture; // Set the texture on the renderer
			spriteRenderer.Size = 100;
		}

		return Task.CompletedTask;
	}

	public override void UpdateScene( float cycle, float timeStep )
	{
		base.UpdateScene( cycle, timeStep );

		// Override the Camera settings so the camera is fixed on the Texture
		Camera.Orthographic = true;
		Camera.OrthographicHeight = 100;
		Camera.WorldPosition = Vector3.Backward * 100;
		Camera.WorldRotation = Rotation.LookAt( Vector3.Forward );
	}
}

---

### Creating a Static Shortcut
*Source: https://sbox.game/dev/doc/editor/editor-shortcuts/*

When creating a Tool or Editor Project, it's common to want to be able to trigger certain actions with a key press or combined keystroke (Like O to enter Object Mode, or SHIFT+B to enter the Block Tool).

Editor Shortcuts do exactly that, while also giving the user the option to rebind each shortcut themselves.

Creating a Static Shortcut

Shortcuts are created by adding the [Shortcut] attribute to a function, giving a name and default bind. The function can reside within any class (including static classes).

[Shortcut("scene.toggle-gizmos", "SHIFT+G")]
static void ToggleGizmos()
{
    // Do stuff...
}

Now whenever you press SHIFT+G in the editor, this static function will be run if the primary Editor window is in focus.

Creating a Widget Shortcut

Widget Shortcuts are created just the same, but there's some optional parameters to play with.

[Shortcut("mesh.merge", "M", typeof(SceneViewportWidget), ShortcutType.Widget)]
private void Merge()
{
    // Do stuff...
}

The last 2 arguments are optional.

The first is which type of Widget you need to have in focus to do the shortcut (if none/null, assumes the class the function is defined in is the Widget type)

The second is the ShortcutType, which is either the Widget itself needs to be in focus, a Window containing it needs to be in focus, or the Application as a whole needs to be in focus.

So in this example, Merge() will only be called when pressing M while a SceneViewportWidget is in focus.

---

### Hooking into an EditorEvent
*Source: https://sbox.game/dev/doc/editor/editor-events/*

Editor Events are events that are broadcast globally throughout the editor and can be listened to/fired from any Editor Project. These are useful for creating your own custom Editor Tools and making sure they can work in tandem with existing systems.

Hooking into an EditorEvent

Hooking into an Editor Event allows you to run additional code whenever an event is called. You can control the order at which event hooks are triggered via the Priority variable. Events with a lower Priority run first.

// Hooking into a named event
[Event( "scene.stop", Priority = 100 )]
void OnSceneStop()
{
  Log.Info( "The scene has stopped" );
}

// Shorthands for frequently used events
[EditorEvent.Hotload]
void OnHotload()
{
  Log.Info( "You've hotloaded!" );
}

Just make sure that you have the same arguments that the EditorEvent is looking for. See the table below.

Calling a Custom EditorEvent
// Calling an event with 0-3 arguments
EditorEvent.Run( "customevent.test" ); // No arguments
EditorEvent.Run( "customevent.add", 1, 2); // 2 arguments

// Calling an event via an interface (theoretically unlimited arguments)
// This will run the event on any Editor Widget that implements the custom interface
EditorEvent.RunInterface<ICustomEvent>( x => x.MyCustomEvent(1,2,3,4,5) );

Event Interfaces

While string-based events still exist, we prefer to use event interfaces nowadays. We find that they're more stable than strings. It's more obvious if things are using it, and it's really IntelliSense friendly.

To use the interface, you just implement it. They're coded in a way that means you don't have to implement each member, so you can just implement what you want to listen to.

public class MyCustomWidget : Widget, ResourceLibrary.IEventListener
{
	void ResourceLibrary.IEventListener.OnRegister( GameResource resource )
	{
		Log.Info( $"{resource} has been registered!" );
	}
}

For widgets event listeners will register automatically, if you want to listen to events on a non widget class you will manually need to call EditorEvent.Register( myCustomListenerInstance );

AssetSystem.IEventListener

/// <summary>
/// An asset has been modified
/// </summary>
void OnAssetChanged( Asset asset ) { }

/// <summary>
/// The thumbnail for an asset has been updated
/// </summary>
void OnAssetThumbGenerated( Asset asset ) { }

/// <summary>
/// Changes have been detected in the asset system. We won't tell you what, but
/// you probably need to update the asset list or something.
/// </summary>
void OnAssetSystemChanges() { }

/// <summary>
/// Called when a new tag has been added to the asset system.
/// </summary>
void OnAssetTagsChanged() { }

ResourceLibrary.IEventListener
/// <summary>
/// Called when a new resource has been registered
/// </summary>
void OnRegister( GameResource resource ) { }

/// <summary>
/// Called when a previously known resource has been unregistered
/// </summary>
void OnUnregister( GameResource resource ) { }

/// <summary>
/// Called when the source file of a known resource has been externally modified on disk
/// </summary>
void OnExternalChanges( GameResource resource ) { }

/// <summary>
/// Called when the source file of a known resource has been externally modified on disk
/// and after it has been fully loaded (after post load is called)
/// </summary>
void OnExternalChangesPostLoad( GameResource resource ) { }

Other Default EditorEvents
Editor
Event	Arguments	Invokes
editor.created	EditorMainWindow	When the editor has just started
tool.frame		Every frame
hotloaded		When a hotload occurs
refresh		When assemblies are changed
tools.gamedata.refresh		When assemblies are changed. Runs after refresh
app.exit		When the editor is shutting down
localaddons.changed		When Project Settings are updated
keybinds.update		When Editor Keybinds have been changed
Asset System
Event	Arguments	Invokes
assetsystem.newfolder		When a new folder was created
assetsystem.openpicker	AssetPickerParameters	When opening an Asset Picker
assetsystem.highlight	string	When you click "Show in Asset Browser"
asset.contextmenu	AssetContextMenu	When an asset is right clicked
asset.nativecontextmenu	FolderContextMenu	When a folder is right clicked
content.changed	string	When a file has changed in the "Content" path
compile.shader	string	When a shader starts compiling
open.shader	string	When opening a shader
package.changed	Package	When you update a package
package.changed.installed	Package	When a package is installed
package.changed.uninstalled	Package	When a package is uninstalled
package.changed.favourite	Package	When you favourite a package
package.changed.rating	Package	When you upvote/downvote
Scenes
Event	Arguments	Invokes
scene.open		When a Scene or Prefab is opened
scene.startplay		When you click the Play button
scene.play		When the Scene enters Play Mode
scene.stop		When the Scene exits Play Mode
scene.session.save		Every second a Scene is open
scene.saved	Scene	When a Scene is saved
Widgets
Event	Arguments	Invokes
paintoverlay		When highlighting a Panel in the "UI Panels" tab
qt.mousepressed		When the Editor receives a mouse event
gameframe.statusbar	StatusBar	When the status bar is being built<br>(Used to add your own Widgets)
tools.headerbar.build	HeadBarEvent	When the header bar is build built<br>(Used to add your own Widgets)
editor.preferences	NavigationView	When the preferences widget is opened<br>(Used to add your own pages)
Tools
Event	Arguments	Invokes
modeldoc.menu.tools	Menu	When launching ModelDoc
hammer.initialized		When hammer is opened
hammer.selection.changed		When hammer selection has changed
hammer.rendermapview	MapView	For each MapView before rendering begins
hammer.rendermapviewhud		When the hammer hud is rendered
hammer.mapview.contextmenu	Menu, MapView	When the MapView is right clicked
actiongraph.saving	ActionGraph, GameResource	Right before an ActionGraph is saved
actiongraph.saved	ActionGraph	When an ActionGraph is saved
actiongraph.inspect	IMessageContext	When inspecting anything in the ActionGraph
actiongraph.findreflectionnodes	FindReflectionNodeTypesEvent	When attempting to get a list of reflection nodes
actiongraph.findtarget	FindGraphTargetEvent	When attempting to find the target
actiongraph.globalnodes	GetGlobalNodeTypesEvent	When attempting to get global nodes
actiongraph.localnodes	GetLocalNodeTypesEvent	When attempting to get local nodes
actiongraph.querynodes	QueryNodeTypesEvent	When filtering through an existing list of nodes
actiongraph.nodemenu	PopulateNodeMenuEvent	When populating the node menu
actiongraph.createsubgraphmenu	PopulateCreateSubGraphMenuEvent	When right clicking to create a sub-graph
actiongraph.outputplugmenu	PopulateOutputPlugMenuEvent	When clicking and dragging out of an output plug
actiongraph.inputplugmenu	PopulateInputPlugMenuEvent	When clicking and dragging out of an input plug
actiongraph.gotoplugsource	GoToPlugSourceEvent	When double clicking on a plug to go to it's source
actiongraph.inputlabel	BuildInputLabelEvent	When building an input label
actiongraph.geteditorproperties	GetEditorPropertiesEvent	Called when launching ActionGraph

---

### Code Location
*Source: https://sbox.game/dev/doc/editor/texture-generators/*

In the editor Textures can be generated in a number of ways. They can be created from an image (which is pretty normal), or a color, or a gradient, or text, or SVG source. These are called texture generators.

You can create your own texture generators in code, and they will then be selectable.

[Title( "Cicle" )]
[Icon( "palette" )]
[ClassName( "circlegenerator" )]
public class CircleTextureGenerator : Sandbox.Resources.TextureGenerator
{
	[KeyProperty]
	public Color Color { get; set; } = Color.Magenta;

	public Color BackgroundColor { get; set; } = Color.White;

	[Hide, JsonIgnore]
	public override bool CacheToDisk => true;

	protected override ValueTask<Texture> CreateTexture( Options options, CancellationToken ct )
	{
		var bitmap = new Bitmap( 128, 128 );

		bitmap.SetFill( Color );

		bitmap.Clear( BackgroundColor );
		bitmap.DrawCircle( 64, 64, 40 );

		return ValueTask.FromResult( bitmap.ToTexture() );
	}
}

The code above creates a circle texture generator. It draws a circle in the middle of a 128x128 texture.

You can select the color of the circle and the background in the UI.

Properties

Any properties on your generator are automatically saved and restored, and are editable in the UI. To hide a property and stop them serializing, use [Hide, JsonIgnore].

The properties can use all the same attributes as Component properties, like [Range] an [Header] etc.

Cache To Disk

By default Texture Generators are runtime. They generate the texture on load. That isn't always ideal, because the generation might take longer than it would take to read the texture from disk.

If you override CacheToDisk and set it to true, on compile the texture will be generated and saved to disk, and will be loaded from that file instead of generated at runtime.

This is also useful if you're not going to ship the texture generator with your game (or whatever is going to use the texture).

Code Location

If you need your texture generator to run at runtime, because you're not caching to disk, then you should put it in your game code.

If you only need it to run in the editor - then you can put it in your editor project (or library).

---

### Scope Based Undo
*Source: https://sbox.game/dev/doc/editor/undo-system/*

Scope Based Undo

The scope based system works by creating a snapshot of a change set when the scope is entered and another one when the scope is disposed of. The system will automatically take care of restoring the state on undo/redo.

A basic blank scope can be created as follows:

// In Game & Editor Code
var undoScope = Scene.Editor?.UndoScope( "You Action Name");

// In Editor Code
var undoScope = SceneEditorSession.Active.UndoScope( "Your Action Name" );

// Push() will turn the scope into an disposable
using ( SceneEditorSession.Active.UndoScope( "You Action Name" ).Push() )
{
  // Actions that modify the scene
}

var undoScope = SceneEditorSession.Active.UndoScope( "You Action Name" );
using ( undoScope.Push() )
{
  // Actions that modify the scene
}

{
  using var undoScope = SceneEditorSession.Active.UndoScope( "You Action Name" ).Push();
  // Actions that modify the scene
}

However, these scopes will not capture anything yet.

You will have to tell the scope what objects you are about to modify.
To ensure the best performance you should keep the set of captured objects as small as possible.

GameObjects

To capture GameObject changes use undoScope.WithGameObjectChanges().
You also have to specify what part of the GameObject(s) you you would like to capture.

GameObjectUndoFlags.Properties
Is always enabled and captures basic properties of the object (Parent, Transform, Name…)

GameObjectUndoFlags.Components
Captures the components list of the object

GameObjectUndoFlags.Children
Captures all children of the object, this can become very expensive for complex scene hierarchies, so use it only if you have to.

GameObjectUndoFlags.All
Shortcut to capture everything

\

using var undoScope = SceneEditorSession.Active.UndoScope( "You Action Name" )
  .WithGameObjectChanges( gameObject, GameObjectUndoFlags.Properties | GameObjectUndoFlags.Components)
  .Push();

To capture GameObject creation you can use GameObjectCreations().

using ( SceneEditorSession.Active.UndoScope( "Create Empty" ).WithGameObjectCreations().Push() )
{
	var go = new GameObject( true, "Object" );
}

Similarly you can capture objects that are about to be destroyed.

using ( SceneEditorSession.Active.UndoScope( "Delete Object(s)" ).WithGameObjectDestructions( selectedGos ).Push() )
{
	foreach ( var go in selectedGos )
	{
		if ( !go.IsDeletable() )
			return;

		go.Destroy();
	}
}

Components

Components offer similar functionality. Components are always captured as a whole so there are no flags that need to be specified.

using ( SceneEditorSession.Active.UndoScope( "Drop Material" ).WithComponentChanges( c as Component ).Push() )
{
	c.SetMaterial( material, trace.Triangle );
}

Capture Creation
using ( SceneEditorSession.Active.UndoScope( "Add Component(s)" ).WithComponentCreations().Push() )
{
    var component = go.Components.Create( componentType );
    createdComponents.Add( component );
}

Capture Destruction
using ( SceneEditorSession.Active.UndoScope( $"Cut Component" ).WithComponentDestructions( component ).Push() )
{
	component.CopyToClipboard();
	component.Destroy();
}

Selections

Selections are always captured and restored on undo/redo

Chaining

As you may have already noticed you can chain the different functions together to capture a variety of changes and events.

var undoScope = SceneEditorSession.Active.UndoScope( "Extract Faces" )
                  .WithComponentChanges( components )
                  .WithGameObjectDestructions( gameObjects )
                  .WithGameObjectCreations();

using ( undoScope.Push() )

More Examples
Actions that span multiple frames

If you have an action that spans multiple frames (e.g. dragging something around) you can use the following pattern to create an undo.

public class BoxColliderTool : EditorTool<BoxCollider>
{
	private IDisposable _componentUndoScope;

	public override void OnUpdate()
	{
		var boxCollider = GetSelectedComponent<BoxCollider>();
		if ( boxCollider == null )
			return;

		var currentBox = BBox.FromPositionAndSize( boxCollider.Center, boxCollider.Scale );

		using ( Gizmo.Scope( "Box Collider Editor", boxCollider.WorldTransform ) )
		{
			if ( Gizmo.Control.BoundingBox( "Bounds", currentBox, out var newBox ) )
			{
				if ( _componentUndoScope == null )
				{
                   // Create scope if it not exists
					_componentUndoScope = SceneEditorSession.Active.UndoScope( "Resize Box Collider" )
                                            .WithComponentChanges( boxCollider )
                                            .Push();
				}
				boxCollider.Center = newBox.Center;
				boxCollider.Scale = newBox.Size;
			}

            // Dispose scope when mouse is release
			if ( Gizmo.WasLeftMouseReleased )
			{
				_componentUndoScope?.Dispose();
				_componentUndoScope = null;
			}
		}
	}
}

Group GameObjects Action
var undoScope = SceneEditorSession.Active.UndoScope( "Group Objects" )
                  .WithGameObjectChanges( selection, GameObjectUndoFlags.Properties )
                  .WithGameObjectCreations();

using ( undoScope.Push() )
{
	var go = new GameObject();
	go.WorldTransform = first.WorldTransform;
	go.MakeNameUnique();

	first.AddSibling( go, false );

	for ( var i = 0; i < selection.Length; i++ )
	{
		selection[i].SetParent( go, true );
	}

	EditorScene.Selection.Clear();
	EditorScene.Selection.Add( go );
}

---


## 20. Code Basics

*Cheat sheet, math types, console vars, API whitelist*

### Cheat Sheet
*Source: https://sbox.game/dev/doc/code/basics/cheat-sheet/*

Sometimes you know what you're looking for, but you don't know where it is.

Debugging
Name	Code
Logging to console	Log.Info( $"Hello {username}" );
Drawing to screen	DebugOverlay.ScreenText( new Vector2( 50, 50 ), "Hello" );
Asserting	Assert.NotNull( obj, "Object was null!" )
Transforms
Name	Code
Get GameObject Position	var p = go.WorldPosition;
Set GameObject Position	go.WorldPosition = new Vector3( 10, 0, 0 );
Get Local Position	var p = go.LocalPosition;
GameObjects
Name	Code
Find by name	Scene.Directory.FindByName( "Cube" ).First();
Find by Guid	Scene.Directory.FindByGuid( guid );
Creating	var go = new GameObject();
Deleting	go.Destroy()
Disabling	go.Enabled = false;
Duplicating	var newGo = go.Clone();
Adding a Tag	go.Tags.Add( "player" );
Iterate Children	foreach( var child in go.Children )
Deleted Check	if ( go.IsValid() )
Components
Name	Code
Add component	var c = go.AddComponent<ModelRenderer>();
Remove component	c.Destroy()
Disabling	c.Enabled = false;
Get GameObject	var go = c.GameObject;
Get Component	var c = go.GetComponent<ModelRenderer>();
Get or Add	var c = go.GetOrAddComponent<ModelRenderer>();
Iterate	foreach ( var c in go.Components.GetAll() )
Deleted check	if ( c.IsValid() )
Get all active	foreach ( var c in Scene.GetAll<CameraComponent>() )

---

### Math Types
*Source: https://sbox.game/dev/doc/code/basics/math-types/*

Vectors

A Vector is just a set of numbers that describe a position or a direction in space.

Vector2 – for flat 2D stuff (like a screen): x and y.
Vector3 – for 3D stuff: x, y, z.
Vector4 – same as Vector3 but with a bonus W value. Used commonly for shaders, or margins, or borders, or corners.
Vector2Int + Vector3Int – like normal, but with whole numbers only.
// Make a new vector
var a = new Vector3(1, 2, 3);
var b = new Vector3(4, 5, 6);

// Get or set the values
float x = a.x;
a.z = 10;

// You can add and subtract them
var sum = a + b;
var difference = b - a;

// You can scale them (make them bigger or smaller)
var twiceAsBig = a * 2;
var halfAsBig = a / 2;

// Get how long the vector is (like the length of the arrow)
float length = a.Length;

// Make it point in the same direction but only 1 unit long
Vector3 normal = a.Normal;

// How far apart two vectors are
float distance = a.Distance(b);

// Dot product tells how similar two directions are
float dot = Vector3.Dot(a, b);

// Cross product gives you a new direction that's 90° to both
Vector3 cross = Vector3.Cross(a, b);

// Lerp moves smoothly from a to b (0.5 is halfway)
Vector3 halfway = Vector3.Lerp(a, b, 0.5f);

Rotation & Angles

A Rotation tells you which way something is facing in 3D. It's technically a Quaternion.

There's also Angles, which uses pitch, yaw, and roll. It's easier to write by hand, but it can suffer from gimbal lock. That's why we mostly use Rotation in code - it avoids those problems and is more reliable.

// Make some angles (easy to understand)
Angles ang = new Angles(30, 90, 0);  // pitch, yaw, roll

// Convert to Rotation
Rotation rot = ang.ToRotation();

// Convert back to Angles
Angles ang2 = rot.ToAngles();

// Get direction vectors from a Rotation
Vector3 fwd = rot.Forward;
Vector3 up = rot.Up;
Vector3 right = rot.Right;

// Make a Rotation that looks in a direction
Rotation look = Rotation.LookAt(new Vector3(1, 0, 0), Vector3.Up);

// Create basic axis rotations
Rotation ry = Rotation.FromYaw(90);    // turn right 90°
Rotation rx = Rotation.FromPitch(45);  // look up 45°
Rotation combined = ry * rx;           // do both

// Smoothly rotate between two rotations
Rotation start = Rotation.FromYaw(0);
Rotation end = Rotation.FromYaw(180);
Rotation halfway = Rotation.Slerp(start, end, 0.5f);

// Rotate a direction vector
Vector3 forward = Vector3.Forward;
Vector3 rotated = rot * forward;

// Invert a rotation
Rotation inverse = rot.Inverse;

Transform

A Transform holds three things:

a Position (Vector3),
a Rotation (Rotation),
and a Scale (Vector3)

In this way, it can fully represent the "transform" of a GameObject. On a GameObject it's available in two forms..

GameObject.WorldTransform - position in the world
GameObject.LocalTransform - position relative to its parent
// Create a basic Transform at origin, facing forward
Transform t = Transform.Zero;

// Or with custom position and rotation
Vector3 pos = new Vector3(10, 0, 0);
Rotation rot = Rotation.FromYaw(90);
Transform custom = new Transform(pos, rot, 1f);  // scale is 1

// Access individual parts
Vector3 p = custom.Position;
Rotation r = custom.Rotation;
float scale = custom.Scale;

// Get direction vectors from the transform
Vector3 forward = custom.Forward;
Vector3 up = custom.Up;
Vector3 right = custom.Right;

// Move a point from local space to world space
Vector3 local = new Vector3(1, 0, 0);
Vector3 world = custom.PointToWorld(local);

// Move a point from world space to local space
Vector3 backToLocal = custom.PointToLocal(world);

// You can also turn the Transform into a Matrix
Matrix mat = custom.ToMatrix();

// Or create a Transform from a Matrix
Transform fromMat = Transform.FromMatrix(mat);

---

### Commands
*Source: https://sbox.game/dev/doc/code/basics/console-variables/*

You can create variables and commands that you can run from the console.

Commands

Console commands are just static methods with an attribute. Here running hello in the console will cause it to print Hello There!

    [ConCmd("hello")]
    static void HelloCommand()
    {
        Log.Info( "Hello there!" );
    }

Commands can also have arguments. Here running hello dave will print out Hello there dave!.

    [ConCmd("hello")]
    static void HelloCommand( string name )
    {
        Log.Info( $"Hello there {name}!" );
    }

The backend will try its best to convert the arguments from strings to the type you specify.

Server Commands

You might want to have a command that is always run on the server. You can do this by adding the ConVarFlags.Server flag to the ConCmd attribute.

If you make the first parameter of the method have the Connection type, you'll be able to tell who actually ran the command.

	[ConCmd( "test", ConVarFlags.Server )]
	public static void TestCmd( Connection caller )
	{
		Log.Info( "The caller is: " + caller.DisplayName  );
	}

Variables

It can useful to have variables that you can tweak to configure certain elements of your game.

Here's an example:

	[ConVar]
	public static bool debug_bullets{ get; set; } = false;

ConVars can have flags. These can be combined.

//
// Is saved to disk and restored, so it persists between sessions
//
[ConVar( "bullet_count", ConVarFlags.Saved )]
public static int BulletCount { get; set; } = 6;

//
// Only the host can change it, and its value is synced to all clients
//
[ConVar( "friendly_fire", ConVarFlags.Replicated )]
public static bool FriendlyFire { get; set; } = false;

//
// Is sent to the host in UserInfo, which is available via the client's Connection
//
[ConVar( "view_mode", ConVarFlags.UserInfo )]
public static string ViewMode{ get; set; } = "firstperson";

//
// Hide in find and autocomplete. This works on ConCmd too.
//
[ConVar( "secret", ConVarFlags.Hidden )]
public static int SecretVariableMode { get; set; } = 3;

Game Settings

You can use ConVarFlags.GameSetting to expose a setting to the game's creation screen, useful for configuring a game.

//
// Shows up in the game creation screen, as a slider.
//
[ConVar( "player_speed", ConVarFlags.GameSetting ), Range( 50f, 1024f, 1 )]
public static float PlayerSpeed { get; set; } = 250f;

---

### Api Whitelist
*Source: https://sbox.game/dev/doc/code/basics/api-whitelist/*

We prevent access to classes and functions that could be used maliciously by whitelisting what can be used.

When playing s&box, any code that doesn't pass these checks will not be loaded. In the editor, the compiler will generate a SB1000 Whitelist Error: ``*'x' is not allowed when whitelist is enabled*.

Editor code, including libraries, doesn't have to follow these restrictions. If you are developing a standalone game, you can opt out, but you won't be able to publish to the platform while the whitelist is disabled.

Reporting a False Positive

If there's an API you need access to that isn't in the whitelist and you believe it's harmless, please report it on our issue tracker with your reasoning, and we'll review it. Please include the symbol as it appears in the error.

Reporting a Vulnerability

As bugs in this system represent serious security issues, please report discoveries as described here.
Do not report them publicly.

Cheat Sheet

For many standard .NET APIs that are blocked, we provide similar functionality via a safe sandboxed API of our own.

Here are some of the common ones to help new devs. Check out our full API reference here.

❌ Not allowed	✅ Allowed
Console.Log	Use Log.Info as a drop-in replacement.
System.IO*	Most standard .NET IO isn't allowed, but you can use our Filesystem API for storage of user data.

---


## 21. Advanced Topics

*Hotloading, unit tests, code generation, libraries*

### How it Works
*Source: https://sbox.game/dev/doc/code/advanced/hotloading/*

Whenever you save a code file in your project (.cs or .razor files), we recompile and attempt to live-reload the changed assembly. This lets you quickly iterate and see your changes without needing to restart the editor. Ideally we want to support this for 99% of code changes without you needing to think about what happens under the hood, but this document will help you investigate cases where things go wrong.

How it Works

If you change any type definitions, we need to explore the heap to find and upgrade any instances of those types. We do a full walk starting at static fields, recursing into instance fields that we think could contain stuff to upgrade.

IL Hotload (Fast Hotload)

If you've only changed the bodies of methods, we can usually just patch in the new instructions without walking the heap. This is almost instant, and won't cause any of the pitfalls mentioned later on in this guide. It's enabled by default, but can be disabled in Editor Settings in the General tab.

If disabling fast hotload fixes your issue, please let us know!

Optimization

Walking the heap can be very slow! Here's some tricks to help speed things up. These won't apply to IL hotloads.

Diagnosis

Enter hotload_log 2 in the console to get verbose timing information next time you hotload. This will generate a table describing how long it took to process instances of each type, and the total number of those instances. It's sorted by descending processing time, so the biggest culprit should be at the top.

If the number of instances increases drastically each hotload, you probably have a leak somewhere. For example, you might have a static list that you keep adding to and never clear. hotload_log 2 lists the static fields through which instances were discovered, to help you find the source of leaks.

Arrays

We have a fast path for arrays and lists containing value types, as long as they have no reference typed fields.

public record struct UserStruct( Vector3 Foo, int Bar );

public int[] UserStructArray;        // Fast
public string[] StringArray;         // Slow
public object[] ObjectArray;         // Slow
public Vector3[] VectorArray;        // Fast
public UserStruct[] UserStructArray; // Fast

Skipping

Internal: this mostly only applies to engine development.

We skip processing instances of a type if it can't possibly contain references to instances of user-defined types in its fields. Hotload can attempt to figure this out automatically, but you can force it to skip a field or entire type using the [SkipHotload] attribute.

You should be very careful with manual skipping. It can cause instances that should have been processed to leak into the post-hotload application state, causing lots of weird errors.

Sealing classes will let hotload be certain that a user type can't inherit from it, so more types can be automatically skipped.

Pitfalls

Here's some cases that might have unexpected behaviour. Again, these don't apply to IL hotloads.

Removing / Renaming Types

If you remove or rename a type, any references to instances of that type will be replaced with null. We try to account for this in the engine, for example removing components automatically if their types are removed, but you might have to handle this yourself too. You can always just restart the editor if you start getting lots of errors after removing a type.

We currently don't have a good way of detecting if a type is renamed, so at the moment this will behave like the old type was just removed completely.

Changing Default Field Values

Since we copy the runtime value of fields into the newly loaded assembly (where possible), changing the default values of fields won't have an effect until you restart the editor. This doesn't apply to const fields or properties with get method bodies. You can also bypass this behaviour using the [SkipHotload] attribute.

// Code before Hotload
public static Example1 = "Hello";
public static Example2 { get; } = "Hello";
public static Example3 => "Hello";
public const Example4 = "Hello";
[SkipHotload] public static Example5 = "Hello";

// Code after hotload                           // Actual runtime value
public static Example1 = "World";               // "Hello" ❌
public static Example2 { get; } = "World";      // "Hello" ❌
public static Example3 => "World";              // "World" ✔️
public const Example4 = "World";                // "World" ✔️
[SkipHotload] public static Example5 = "World"; // "World" ✔️

Dictionary / HashSet

Extra care is needed for collections that call Equals() on your types. It's possible to make a code change that makes two previously non-equal instances start being equivalent, which can put dictionaries and sets into an invalid state.

We emit a warning if we detect that happening, and an editor restart might be required.

Static Fields in Generic Types

We currently can't process static fields in any generic types during hotload. We emit a compile-time warning for any such fields to make sure you know that the values in those fields will be forgotten during hotloads. This warning can be suppressed using [SkipHotload].

Delegates / Lambda Methods

Hotload will do its best to let Delegate instances survive hotloads, but it might not always be possible for delegates implemented by lambda methods. This will usually happen if a method has multiple lambda methods inside it that get reordered, or if the definition changes drastically. If hotload isn't sure about how to process a delegate, it'll replace it with a delegate that logs a warning when invoked.

If you get warnings about delegates that fail to process even when changing unrelated code, please let is know!

Reflection Caching

If you're caching the results of expensive reflection operations, be mindful that a hotload might mean cached values might become incorrect. You should always repopulate such caches after hotload, and mark the field containing the cache with [SkipHotload].

Internal: we have a ReflectionCache<TKey, TValue> helper dictionary type to do this for you.

Internal: we also need to consider reflection caches in third-party libraries, like System.Text.Json. These caches should also be cleared during hotloads if there's any possibility that they can contain user-defined types.

Concurrency

During hotload, we need to suspend all other managed threads that could possibly touch anything it processes. Hotload will politely wait for any active worker thread tasks (GameTask.RunInThreadAsync etc) to yield before it starts, so ideally you should write such tasks in an async way that yields often.

Internal: we need to be careful about threads in engine code, they can't run during hotload unless they never touch anything hotloadable.

---

### Unit Tests
*Source: https://sbox.game/dev/doc/code/advanced/unit-tests/*

If you add a UnitTests directory to your project folder, we will automatically generate a Unit Test project for you.
In order for the project to be generated you need to restart your editor if you have it open.

Your First Test

Add a new File to the unit test project call it MyFirstTest.cs

using Microsoft.VisualStudio.TestTools.UnitTesting;

[TestClass]
public class MyFirstTest
{
	[TestMethod]
	public void Simple()
	{
		Assert.AreEqual( 4, 2 + 2 );
	}
}

You can run the tests using the dotnet test command via CLI. If your using Visual Studio you can use the Test Explorer to run your tests.

Game Tests

If your test relies on engine functionality, for example if you want to test a component. You can initialize the engine using the following code:

global using Microsoft.VisualStudio.TestTools.UnitTesting;

[TestClass]
public class TestInit
{
	public static Sandbox.AppSystem TestAppSystem;

	[AssemblyInitialize]
	public static void AssemblyInitialize( TestContext context )
	{
		TestAppSystem = new TestAppSystem();
		TestAppSystem.Init();
	}

	[AssemblyCleanup]
	public static void AssemblyCleanup()
	{
		TestAppSystem.Shutdown();
	}
}

Just drop it into any file in the unit test project e.g. InitTests.cs

Example Component Test
using Sandbox;

[TestClass]
public class Camera
{
	[TestMethod]
	public void MainCamera()
	{
		var scene = new Scene();
		using var sceneScope = scene.Push();

		Assert.IsNull( scene.Camera );

		var go = scene.CreateObject();
		var cam = go.Components.Create<CameraComponent>();

		Assert.IsNotNull( scene.Camera );

		go.Destroy();
		scene.ProcessDeletes();

		Assert.IsNull( scene.Camera );
	}
}

---

### Capabilities
*Source: https://sbox.game/dev/doc/code/advanced/code-generation/*

Capabilities

s&box has a [CodeGenerator] attribute that you can use to decorate another attribute specifically for use with methods and properties. It lets you wrap methods and properties to perform some other action when the method is called or the property is set or to return a different value when the property is read.

Examples

[The Scene System] uses CodeGen for Broadcast RPCs, it could also be used for creating networked variables.

RPCs

The following is an example of how you could use CodeGenerator to create an RPC system.

[CodeGenerator( CodeGeneratorFlags.WrapMethod | CodeGeneratorFlags.Instance, "OnRPCInvoked" )]
public class RPC : Attribute {}

public class MyObject
{
  [RPC]
  public void SendMessage( string message )
  {
    Log.Info( message );
  }

  internal void OnRPCInvoked( WrappedMethod m, params object[] args )
  {
    if ( IsServer )
    {
      // Send a networked message with the specified args and method name to all clients.
    }

    // Call the original method.
    m.Resume();
  }
}

Networked Vars

This is an example of how you could use CodeGenerator to create a system for networked variables.

[CodeGenerator( CodeGeneratorFlags.WrapPropertySet | CodeGeneratorFlags.Instance, "OnNetVarSet" )]
[CodeGenerator( CodeGeneratorFlags.WrapPropertyGet | CodeGeneratorFlags.Instance, "OnNetVarGet" )]
public class NetVar : Attribute {}

public class MyObject
{
  [NetVar] public string Name { get; set; }

  internal T OnWrapGet<T>( WrappedPropertyGet<T> p)
  {
    // Return the actual value from the network.
    if ( MyNetVarTable.TryGetValue( p.PropertyName, out var netValue ) )
    {
      return (T)netValue;
    }

    return val;
  }

  internal void OnWrapSet<T>( WrappedPropertySet<T> p )
  {
    if ( IsServer )
    {
      MyNetVarTable[propertyName] = p.Value;
      // Send a networked message setting the property to this value for all clients.
    }

    p.Setter( p.Value);
  }
}

Code Generator Flags

The CodeGenerator attribute needs to have CodeGeneratorFlags set which determine what it will wrap and whether the wrapping applies to static methods and properties or instance methods and properties or both.
nAn attribute class can be decorated with more than one CodeGenerator attribute to handle many different scenarios.

Wrapping Methods

To simply wrap a method call you can create an attribute that is decorated with CodeGenerator and the CodeGeneratorFlags.WrapMethod flag. To create one that wraps both instance and static methods you can use something like this:

[AttributeUsage( AttributeTargets.Method )]
[CodeGenerator( CodeGeneratorFlags.WrapMethod | CodeGeneratorFlags.Instance, "OnMethodInvoked" )]
[CodeGenerator( CodeGeneratorFlags.WrapMethod | CodeGeneratorFlags.Static,
	"MyObject.OnMethodInvokedStatic" )]
public class WrapCall : Attribute {}

The passed callbackName to the CodeGenerator attribute can either be a static method (determined by whether there's a . in the string) or an instance method. You can only use an instance method if the wrapped method itself is an instance method.

You can then setup those target methods on your object or static class like this:

public class MyObject
{
	internal static void OnMethodInvokedStatic( WrappedMethod m, params object[] args ) {}
	internal void OnMethodInvoked( WrappedMethod m, params object[] args ) {}
}

methodName on a static callback will be the fully qualified name. For example if [WrapCall] was added to a method called DoSomething on MyClass then the method name would be MyClass.DoSomething.

Different Parameter Types

You can handle different parameter types instead of having a single generic callback signature. The correct method will be called based on the original parameters of the wrapped method. You can even use generics here.

public class MyObject
{
	internal static void OnMethodInvokedStatic<T1, T2>( WrappedMethod m, T1 arg1, T2 arg2 ) {}

	internal void OnMethodInvoked( WrappedMethod m, bool enabled ) {}
	internal void OnMethodInvoked<T1, T2, T3>( WrappedMethod m, T1 arg1, T2 arg3, T3 arg3 ) {}
}

Different Return Types

If you want to handle specific return types you can also do that. The crucial part is that instead of the first parameter of the callback method being an Action it would be a Func<T> instead.

public class MyObject
{
	internal T OnMethodInvoked( WrappedMethod<T> m )
	{
		return m.Resume();
	}
}

Wrapping Properties

Wrapping properties is similar to wrapping a method, but your attribute class should use CodeGeneratorFlags.WrapPropertySet and/or CodeGeneratorFlags.WrapPropertyGet.

[AttributeUsage( AttributeTargets.Property )]
[CodeGenerator( CodeGeneratorFlags.WrapPropertySet | CodeGeneratorFlags.Instance, "OnWrapSet" )]
[CodeGenerator( CodeGeneratorFlags.WrapPropertyGet | CodeGeneratorFlags.Instance, "OnWrapGet" )]
public class WrapGetSet : Attribute {}

Similarly to wrapping methods, the callback method can handle any generic property or specific property types.

When wrapping the setter of properties the callback method should have 3 parameters. The first is the property name, the second is the value that the property wants to be set to, and the third is an Action that will call the original setter function.

public void OnWrapSet<T>( WrappedPropertySet<T> p )
{
	p.Setter( p.Value );
}

When wrapping the getter of properties, the callback method should have a return type and 2 parameters. The first being the property name and the second being the value that the getter would have returned usually.

public T OnWrapGet<T>( WrappedPropertyGet<T> p )
{
	return p.Value;
}

Wrapping Everything

To demonstrate how you can mix CodeGeneratorFlags to handle multiple use cases, here is an example of an attribute that could wrap anything and everything.

Because we specify CodeGeneratorFlags.Static in this attribute, the callbackName must refer to a static method, too.

[CodeGenerator(
	CodeGeneratorFlags.WrapPropertySet | CodeGeneratorFlags.WrapPropertyGet | CodeGeneratorFlags.WrapMethod |
	CodeGeneratorFlags.Static | CodeGeneratorFlags.Instance, "MyStaticClass.OnWrapAnything" )]
public class WrapAnything : Attribute {}

public class MyObject
{
  [WrapAnything] public string MyString { get; set; }
  [WrapAnything] public static string MyStaticString { get; set; }

  [WrapAnything]
  public void MyMethod()
  {
  }

  [WrapAnything]
  public static void MyStaticMethod()
  {
  }
}

public static class MyStaticClass
{
  internal static void OnWrapAnything<T>( WrappedPropertySet<T> p )
  {
  }

  internal static T OnWrapAnything<T>( WrappedPropertyGet<T> p )
  {
    return value;
  }

  internal static void OnWrapAnything( WrappedMethod m, params object[] args )
  {
    m.Resume();
  }

  internal static T OnWrapAnything( WrappedMethod<T> p, params object[] args )
  {
    m.Resume();
  }
}

---


## 22. VR

*Virtual Reality development*

### Enabling VR
*Source: https://sbox.game/dev/doc/systems/vr/*

Enabling VR

Editor: Launch the editor with a headset connected. You'll see a VR button at the top right of your editor. It should be on by default.
Game: If you have a VR headset plugged in and it's active, you'll launch in VR.

Components
VR Anchor: locks the player's playspace to a GameObject's transform
VR Tracked Object: moves a GameObject's transform to a tracked device, e.g. a controller or headset
VR Hand: matches the animgraph for a hand model with SteamVR's skeletal inputs
Example Setup

An example setup is available on sbox-scenestaging under the "test.vr" scene.

Here's a basic explanation of the different parts you'll come across:

Anchor

The anchor represents the center of the player's playspace.

If you want your player to be able to move around, add the "VR Anchor" component to the same GameObject that you're using for your player. This will move the virtual playspace along with it.

Camera
Rendering

The Target Eye represents the eye(s) that the camera can render to. A basic VR setup should have both Left and Right eye targets in order to display properly inside the headset.

Tracking

A basic VR setup should also have a VR Tracked Object component on the camera, set up with the Head pose source, in order for the camera to follow the player's head movements.

Input
Tracking

On any object you want to track the player's controllers, add a VR Tracked Component for the object you want to track.

For example, this component will make the GameObject it's attached to follow the player's left controller:

API

Here's a basic overview of the VR C# API:

Input

Get controller values

// Get left hand controller grip
var grip = Input.VR.LeftHand.Grip.Value;

// Check if the player is pulling the trigger a bit
if ( Input.VR.LeftHand.Trigger.Value > 0.5f )
{
  Log.Trace( "Shoot!" );

  // Vibrate the controller
  Input.VR.LeftHand.TriggerHapticVibration( duration: 0.5f, frequency: 10, amplitude: 1.0f );
}

Check if the player is running in VR
if ( Game.IsRunningInVR )
{
   Log.Info( "I am running the game in VR!" );
}

---


## 23. Movie Maker

*Cinematic recording and playback*

### Movie Editor
*Source: https://sbox.game/dev/doc/systems/movie-maker/getting-started/*

In this article you'll find out:

How to open the Movie Maker editor
What is a Movie Player
What is a Movie Resource
What is a Track
Movie Editor
Opening the Movie Editor

If Movie Maker isn't already visible, you can toggle it in the View menu.

It should fit nicely as a tab in the lower panel if you dock it there.

Movie Player

To edit and preview movies, you'll need a Movie Player component somewhere in your scene. Movie Players decide which objects in the scene should be animated by a particular movie, and control the current playback position.

Creating a Movie Player

If you don't have any Movie Players in the current scene, you should see a big button to create one.

Otherwise, you can switch between players or create new ones by using the drop-down in Movie Maker's menu bar. This will only list players in the currently active scene.

You can also add one to a GameObject in the Inspector like any other Component.

Movie Resources

Movies can either be embedded inside a Movie Player, or saved as a .movie asset.

Saving & Loading

New movies will be embedded in the current Movie Player by default. You can save them as reusable .movie files, or embed a copy of the currently edited .movie, with the File menu.

Sequences

You can also reference segments of movies from each other using sequence blocks. This can be done by selecting Import Movie in either the file menu or when right-clicking in the timeline.

Tracks

Movies describe how properties in a scene should change over time, and this information is stored in tracks.

There's a few types of track you'll need to know about.

Reference Track

This track references a GameObject or Component in the scene that should be controlled. It's up to the Movie Player to decide which particular object to bind each track to, so you can re-use the same .movie to control different actors.

Property Track

This represents a property somewhere in the scene, and describes how it should animate. This could be the position of the camera, the colour of a light, or text in a speech bubble.

Property tracks are always nested inside other tracks, either reference tracks or other properties. This is how the track knows what to control in the scene: it checks what the parent track is bound to, and looks up a property inside it with the track's name.

Sequence Track

This track references blocks of time from another movie, helping you organize and edit bigger movie projects with multiple shots.

Creating Tracks

Reference and property tracks can be created by dragging from the hierarchy or inspector into the track list.

You can also create sub-tracks by right-clicking an existing track, and selecting the tracks you want from the context menu.

Sequence tracks are created automatically when you import a movie: either by right-clicking in the timeline, or through the file menu.

Next Steps
Editor Map - find your way around the movie editor
Keyframe Editing - a great place to start making simple animations
Motion Editing - make more detailed animations and edit recordings
Recording - manually puppet characters or record gameplay
Sequences - cleanly structure big projects with nested movies
[API] - control movies in more complex ways with C#

---

### Recording in Play Mode
*Source: https://sbox.game/dev/doc/systems/movie-maker/recording/*

Capture gameplay or hand-puppet properties to quickly animate parts of your scene, then tweak the recordings to polish everything up.

Recording in Play Mode

This lets you act out the roles of characters by controlling them directly in-game, be a camera operator for a scene you've already animated, or capture gameplay for a trailer.

First create tracks for any properties you want to record. Common tracks will be added for you when dragging certain objects into the track list, like Player Controllers.

After that, enter play mode and press the Toggle Record button. Any changes to properties bound to tracks will get recorded directly to your movie. When you're done, hit the Toggle Record button again.

Your recording will still be in your movie when you exit play mode, letting you edit it further and save it.

Recording in Editor

You can record even when the game isn't in play mode, letting you puppet properties as a starting point for polishing later. As before, make sure you've created tracks for any properties you want to record, then use the Toggle Record button to start and stop recording.

Editing

After making your recording, you can then polish it up in the Motion Editor or Keyframe Editor. Here's some extra recording specific tips:

Cutting

Your best bet here is to save your recording to a separate .movie, then reference it as a sequence block in your main project. This lets you take clips from the recording, tweaking the start and end times as needed.

Smoothing

Recordings can be shaky, especially when hand puppeting or manually operating a camera. Use the Smoothen option in the Motion Editor context menu to round out any rough edges in a selected time range. You can control how big the smoothing window is, with larger values leading to smoother tracks.

---

### Recording Basics
*Source: https://sbox.game/dev/doc/systems/movie-maker/recording-api/*

You can record gameplay into a MovieClip, which can be played back or imported into Movie Maker for editing. This could be useful for killcams in shooters, leaderboard replays in racing games, or for capturing gameplay to edit into a trailer.

This feature is provided by the MovieRecorder class. You can use its default behaviour to capture all display-related components in the scene, or configure it to capture only certain objects, components and properties.

Recording Basics

By default, a MovieRecorder will capture all Renderers, Cameras, SoundPoints and particle-related components. Just create a recorder, and call Start.

// Create a recorder using the default capturing options
var recorder = new MovieRecorder( Scene );

// Start capturing
recorder.Start();

This starts capturing the scene every fixed update. When you want to stop recording, call Stop.

// Stop capturing
recorder.Stop();

You can then compile the recording into a MovieClip for playback in a MoviePlayer, or to save into a MovieResource.

// Convert the recording to a MovieClip
var clip = recorder.ToClip();

// Play the clip!
GetComponent<MoviePlayer>().Play( clip );

// Save it to a .movie
FileSystem.Data.WriteJson( $"movies/example.movie", clip.ToResource() );

Manual Capture

In some cases you'll need more control over when captures are taken, like when recording in-editor. Instead of Start and Stop, call Advance to move the recording along by an amount of time, and Capture to record a frame.

while ( IsRecording )
{
    SimulateScene();

    // Move the recording time along by 10ms
    recorder.Advance( 0.01 );

    // Capture a frame
    recorder.Capture();
}

Configuring Recorders

If you want to be more selective about what gets recorded, create a MovieRecorderOptions instance and pass it in to the MovieRecorder constructor. These options let you specify the sample rate, filter which GameObjects are allowed to be captured, and add actions to run during each capture.

A new options instance won't capture anything, letting you fully configure it.

var options = new MovieRecorderOptions( SampleRate: 60 )
    .WithCaptureAll<Player>()
    .WithComponentCapturer<PlayerCapturer>()
    .WithCaptureAction( x => x
        .GetTrackRecorder( scoreManager )
        .Property( "Score" )
        .Capture() );

var recorder = new MovieRecorder( Scene, options );

Default Options

If you want to base your configuration off the default options (which captures all renderers, cameras, particles etc), you can access it through MovieRecorderOptions.Default.

var options = MovieRecorderOptions.Default with { SampleRate = 60 }
    .WithFilter( x => !x.Tags.Has( "effect" ) );

You can customize the default options by creating one or more static methods with a [DefaultMovieRecorderOptions] attribute, like this:

[DefaultMovieRecorderOptions]
public static MovieRecorderOptions BuildDefaultOptions( MovieRecorderOptions options )
{
	return options
		.WithFilter( x => !x.Tags.Has( "viewmodel" ) )
		.WithFilter( x => x.PrefabInstanceSource?.StartsWith( "prefabs/surface/" ) is not true );
}

The default options will be used when pressing the Record button in the Movie Maker editor, and the movie console command in-game.

Sample Rate

By default, property tracks in the generated recording are resampled at 30 FPS to keep file sizes small. You can choose a different rate when creating a MovieRecorderOptions, or use the with syntax on an existing instance.

var options = new MovieRecorderOptions( SampleRate: 60 );

var options = MovieRecorderOptions.Default with { SampleRate = 60 };

Filters

Adding filters lets you control which GameObjects are allowed to be captured. Components won't be recorded if their containing GameObject, or any of its ancestors, are filtered out. The first time a capture is attempted on an object, it is checked against all provided filters. If any return false, that object and any of its descendants will be ignored during recording.

var options = MovieRecorderOptions.Default
    .WithFilter( x => !x.Tags.Has( "effect" ) )
    .WithFilter( x => !x.Name.StartsWith( "decal_" ) );

Capture Actions

Every time Capture is called on a MovieRecorder, it goes through all the capture actions in its MovieRecorderOptions. These actions will then access track recorders for GameObjects, Components, and properties, then call Capture on them.

// Capture "ExampleProperty" from inside exampleComponent
var options = new MovieRecorderOptions()
    .WithCaptureAction( x => x
      .GetTrackRecorder( exampleComponent )
      .Property( "ExampleProperty" )
      .Capture() );

Component Capturers

Most of the time you'll want to capture specific properties from all instances of a given component type. To simplify this, you can implement ComponentCapturer<T>. Whenever Capture is called on a component's track recorder, all component capturer instances that match that component's type will be invoked.

class ModelRendererCapturer : ComponentCapturer<ModelRenderer>
{
	protected override void OnCapture( IMovieTrackRecorder recorder, ModelRenderer component )
	{
		recorder.Property( nameof( ModelRenderer.Model ) ).Capture();
		recorder.Property( nameof( ModelRenderer.Tint ) ).Capture();
		recorder.Property( nameof( ModelRenderer.MaterialOverride ) ).Capture();
		recorder.Property( nameof( ModelRenderer.RenderType ) ).Capture();

		if ( component.HasBodyGroups )
		{
			recorder.Property( nameof( ModelRenderer.BodyGroups ) ).Capture();
		}
	}
}

All capturers with public parameterless constructors will be included in MovieRecorderOptions.Default, or you can manually add capturers to an options instance. These capturers are invoked when Capture is called on a matching component's track recorder.

var options = new MovieRecorderOptions()
    .WithComponentCapturer( new ModelRendererCapturer() )
    .WithCaptureAction( recorder =>
    {
        foreach ( var renderer in recorder.Scene.GetAllComponents<ModelRenderer>() )
        {
            // This will look for matching ComponentCapturers
            recorder.GetTrackRecorder( renderer )?.Capture();
        }
    } );

For convenience, WithCaptureAll does the same job as the above example capture action.

var options = new MovieRecorderOptions()
    .WithComponentCapturer( new ModelRendererCapturer() )
    .WithCaptureAll<ModelRenderer>();

---

### Tracks
*Source: https://sbox.game/dev/doc/systems/movie-maker/playback-api/*

You can define movies and play them back using C#.

Tracks

Tracks can represent a GameObject reference, a Component reference, or a property that should be animated.

Create a reference track that, when played, binds to an object named "Camera".

using Sandbox.MovieMaker;

var objectTrack = MovieClip.RootGameObject( "Camera" );

Create a property track that binds to a property named "LocalPosition" inside of whatever objectTrack is bound to.

Give it a constant value between 0s and 2s, then a different constant value from 2s to 5s. After 5s it has no value.

var positionTrack = objectTrack
    .Property<Vector3>( "LocalPosition" )
    .WithConstant( timeRange: (0.0, 2.0), new Vector3( 100, 200, 300 ) )
    .WithConstant( timeRange: (2.0, 5.0), new Vector3( 200, 100, -800 ) );

Create a property track that binds to the "FieldOfView" property of a CameraComponent attached to whatever objectTrack is bound to. Give it an array of samples between 1s-3s.

var fovTrack = objectTrack
    .Component<CameraComponent>()
    .Property<float>( "FieldOfView" )
    .WithSamples( timeRange: (1f, 3f), sampleRate: 2, [60f, 75f, 65f, 90f, 50f] );

Animate the scene with each track. They will try to bind to root objects in the scene with the matching names and component types, and silently fail if they don't exist.

positionTrack.Update( time );
fovTrack.Update( time );

MovieClip

Group tracks into a clip so we can animate them all together.

var clip = MovieClip.FromTracks( positionTrack, fovTrack );

clip.Update( time );

You can search for tracks by path.

var camTrack = clip.GetReference( "Camera" );
var posTrack = clip.GetProperty<Vector3>( "Camera", "LocalPosition" );

Clips can be serialized to and from JSON.

Log.Info( Json.Serialize( clip ) );

TrackBinder

What does objectTrack bind to in the current scene?

var target = TrackBinder.Default.Get( objectTrack );

Log.Info( target.IsBound );
Log.Info( target.Value );

12:59:08 Generic  True
12:59:08 Generic  GameObject:Camera

Bind the track to something else.

target.Bind( Game.ActiveScene.Camera.GameObject );

Binders can be serialized too.

Log.Info( Json.Serialize( Binder.Default ) );

We can have multiple Binder instances, so the same clip can control different objects.

var binder = new TrackBinder( Game.ActiveScene );

binder.Get( cameraTrack ).Bind( Game.ActiveScene.Camera );

// Using Binder.Default

cameraTrack.Update( time );

// Using our own Binder instance

cameraTrack.Update( time, binder );

Target Creation
By default, the player will create any GameObjects or Components referenced by the recording that aren't already in the scene. These targets will be flagged as *NotSaved	NotNetworked	Hidden*. You can turn this off with the CreateTargets property.
moviePlayer.CreateTargets = false;
moviePlayer.Play( clip );

You can also manually create targets through the player's TrackBinder.

// Create targets for every track in the given clip
moviePlayer.Binder.CreateTargets( clip );

// Create targets for a specific set of tracks
var track1 = clip.GetTrack( "Example", "ModelRenderer" );
var track2 = clip.GetTrack( "Player", "PlayerController" );

moviePlayer.Binder.CreateTargets( [track1, track2] );

MoviePlayer

The MoviePlayer component has a clip, a binder, and a time position.

var moviePlayer = GameObject.AddComponent<MoviePlayer>();

moviePlayer.Clip = clip;
moviePlayer.Binder.Get( clip.GetReference( "Camera" ) ).Bind( Game.ActiveScene.Camera );

// Time in seconds

moviePlayer.Position = 0.75;

The clip can also be from a resource.

moviePlayer.Resource = ResourceLibrary.Get<MovieResource>( "example.movie" );

---


### Keyframe Editing
*Source: https://sbox.game/dev/doc/systems/movie-maker/keyframe-editing/*

This is the simplest way to describe how stuff in your scene should animate. Create keyframes in property tracks that are snapshots for what values that track should have at specific times, then control how the track should blend between those values.

It will be active by default, but you can toggle it with the key-shaped button in the toolbar.

Creating Keyframes

There's three ways to create a keyframe for a property track:

Change the property that the track is bound to, a keyframe is created at the current playhead time

Right-click on the track in the timeline, and select Create Keyframe in the context menu

Toggle Create Keyframe on Click either by holding Shift or pressing the button in the toolbar, then clicking in the timeline

\

Copy the scene view's perspective into a selected camera with Ctrl+Shift+F

Interpolation Mode

Each keyframe can choose between three different interpolation modes:

Linear - change at a constant velocity
Quadratic - ease in / out, with 0 velocity at the keyframe
Cubic - move along a smooth spline connecting keyframes

You can combine different modes for neighbouring keyframes to make animations ease in or out.

Automatic Track Creation

For convenience, you can enable this mode to create tracks whenever you touch something in the scene. This helps the most when you're changing lots of properties on many objects, so you don't have to manually created dozens of tracks.

---

### Time Range Selection
*Source: https://sbox.game/dev/doc/systems/movie-maker/motion-editing/*

This editor mode gives you finer control over track data. Instead of keyframes, you sculpt the raw track data itself at any resolution you want.

To motion edit, you select a time range of one or more tracks, then tweak the properties that those tracks represent to manipulate track data. You can also cut / copy / paste ranges of time, or save the selection as a separate .movie file.

Time Range Selection

A time range selection describes how much a modification affects each moment in time. It's broken up into three parts:

Fade In - modifications gradually ramp up within this range
Peak Range - times in this range are fully affected by modifications
Fade Out - modifications ramp down within this range

To make and update a selection:

Click and drag in the timeline to select a time range
Hold Shift and scroll to increase / decrease the fade in / out time
Drag any of the parts of a time range to move or resize them
Use Ctrl+A to select the whole movie's duration
Press Esc to clear the selection

You can change the easing type of the fade in / out sections with the corresponding button in the toolbar, or pressing number keys when mousing over those sections. The shape of the top / bottom of the time selection UI shows what the current easing function looks like.

Modifications

After selecting a time range, you can manipulate the properties of your movie's tracks. Only the selected time range will be affected, with the changes ramping up and down inside the fade in / out sections of your selection.

The selection will turn yellow to show a modification is in progress. At this point you can freely tweak the time selection, and when you're happy hit Enter or click the green tick to commit the change.

Context Menu

The context menu for time selections has a ton of extra editing actions.

Time Selection Actions
Insert (Tab) - add empty time inside the selected range, moving existing track data to the right
Remove (Backspace) - delete the selected range, moving everything afterwards to the left
Clear (Delete) - delete the selected range, leaving empty time
Save As Sequence.. - save the selected range to a .movie, and reference it in the current project
Clipboard Actions
Cut (Ctrl+X) - copy the selected range to the clipboard, then clear the range
Copy (Ctrl+C) - copy the selected range to the clipboard
Paste (Ctrl+V) - paste previously copied track data as a modification
Additive Editing

When pasting a time selection, you can additively layer track data over the existing animation. This is enabled by default, can can be disabled by clicking the Additive button in the toolbar while modifications are active.

---

### Exporting Video
*Source: https://sbox.game/dev/doc/systems/movie-maker/exporting-video/*

The currently open movie can be exported to a video file using the Export Video.. option in the file menu. This will open a window that lets you tweak the output path, resolution, format, and quality of the exported video.

Camera Control

The video will be rendered from the perspective of any active camera in the scene. You should therefore control camera motion using tracks in your movie project. You can also use tracks to toggle which cameras are enabled in the scene, if necessary.

Motion Settings

When exporting, you can optionally enable motion smoothing at various strengths and quality levels. When enabled, each final exported frame will be made out of many sub-frames with a very small time step between them. This simulates having the camera's photosensor be exposed for a non-zero amount of time for each frame.

Exposure

This controls what fraction time for each frame the sensor is exposed for. Very Fast will expose the sensor for 1/12th of a frame, Medium is ½, and Very Slow makes the sensor always exposed for very blurry motion. Instant will disable motion smoothing.

Motion Quality

Use this to control how many sub-frames are rendered for every final output frame. This will greatly affect how long it takes to render your project, so you can set this to Low when previewing an export to save a lot of time.

Image Sequence / Atlas

You can also export individual images for each frame of your video, or a single atlas with a tile for each frame. You can find these options in the Encoding tab.

Exported images can be JPEG or PNG, with PNG supporting transparent backgrounds.

The background will be transparent if you have no skybox, and the camera's clear colour has 0 alpha.

---

### Toolbar
*Source: https://sbox.game/dev/doc/systems/movie-maker/editor-map/*

Toolbar

Project
File Menu

Create, open, or import movies.

Movie Player Selection

Select a or create a movie player in the current scene.

History

Toggle a panel to show modification history, so you can undo / redo changes.

Playback
Record

Toggle recording changes from the scene into the timeline.

Play / Pause

Toggle playback of your movie.

Repeat

Toggle looping for when playback reaches the end of the movie.

Playback Speed

Controls both playback and recording rate.

Sync Movie Players

Toggle synchronizing all movie players in the scene when previewing playback.

Edit Modes
Keyframe Editor

Switch to the keyframe editor mode, for simpler animations. Find out more here.

Motion Editor

Switch to the motion editor mode, for finer control than keyframe editing. Find out more here.

Snapping
Object Snap

Snap to objects in the timeline when dragging.

Frame Snap

Snap to the current frame rate when dragging.

Frame Rate

Resolution to use when frame snapping.

Track List

Project Navigation
Parent Movie

Name of the outer movie, when editing a sequence.

Current Movie

Name of the currently opened movie resource.

Sequence Start / End

Block of time referenced by an outer movie, when editing a sequence.

Track Types
Game Object Track

Gets bound to an object in the scene to be controlled during playback. Contains component and property tracks.

Component Track

Gets bound to a component in the scene, defaults to a matching component of the parent game object track. Contains property tracks.

Property Track

Named property within the parent track, describes how that property should change over time. Can contain nested property tracks.

Sequence Track

Contains time blocks from another movie, making it easier to organize and edit complex projects.

Track Controls
Expand / Collapse

Show or hide nested tracks.

Lock / Unlock

Toggle lock state of a track.

Selected Track

Selecting a track will show relevant gizmos in the scene view.

Locked Track

Disables modifications of this track in the movie.

Timeline

Shift - smoothly preview the time under the mouse

Scroll - vertically scroll through track list

Shift+Scroll or Middle-Click+Drag - pan horizontally

Ctrl+Scroll - zoom in / out

Alt+Scroll - scrub forwards / backwards by a frame

\

Regions
Scrub Bar

Displays time labels. Click and drag to move the playhead.

Tracks

Describe how properties change over time. Edited in either the keyframe or motion editor.

Sequence Block

References a time block from another movie, nested in this one.

Sequence Start / End

Start and end time of the currently edited sequence block, nested in another movie.

Markers
Playhead Marker

Currently selected time for editing.

Preview Marker

Current time being previewed by holding shift and mousing over the timeline.

Frame Tick

Based on the current frame rate, frame snapping will snap to these.

Loop Start / End

Time range to loop when previewing playback. Set with alt+click+drag on a scrub bar.

---


## 24. Game Exporting

*Standalone game export*

### Game Exporting
*Source: https://sbox.game/dev/doc/systems/game-exporting/*

You can export your game, but you shouldn't distribute your exported game just yet.

You can choose to export your games as executables, so that you can put them on other storefronts directly - e.g. Steam.

These games don't have the typical restrictions that platform games have - there's no whitelist for code, and you can use standalone-exclusive APIs.

How to Export your Game

Exporting your game is done through the Export Wizard.

To access the wizard:

Click on the "Project" menu
Click "Export…"
Add an icon and a splash screen if you want, if you're exporting to Steam then insert your App ID
Click 'Next', wait for the project to export
Your project's executable will be in the folder you selected in Step 3. You can click "Open Folder" to open the folder containing your game, and double click the exe to play it.
Restrictions

The following restrictions apply to all games that are exported from s&box:

Your game must be put on Steam (it can be on other storefronts too, but Steam is a hard requirement)

---


## 25. Clutter

*Vegetation and detail object scattering*

### Clutter
*Source: https://sbox.game/dev/doc/systems/clutter/*

The clutter system allows you to procedurally place prefabs/models in your scene using a brush, volumes or infinitely.

---


## 26. Post Processing

*Visual effects and post-processing*

---

## 27. API Deep Reference (from Source Code & Community Docs)

*Compiled from GitHub source (Facepunch/sbox-public), Steam Developer Guide, superdocs.sbox.cool, and community resources.*

### Class Hierarchy

```
System.Object
  ├── Resource (abstract) : IValid
  │     └── GameResource (abstract)
  │
  ├── GameObject : IJsonConvert
  │     └── Scene : GameObject  (Scene IS a GameObject!)
  │
  └── Component (abstract) : IJsonConvert, IValid
        ├── Renderer (abstract)
        │     ├── ModelRenderer
        │     ├── SkinnedModelRenderer
        │     ├── SpriteRenderer
        │     ├── DecalRenderer
        │     ├── LineRenderer
        │     ├── TextRenderer
        │     └── TrailRenderer
        ├── Collider (abstract)
        │     ├── BoxCollider
        │     ├── SphereCollider
        │     ├── CapsuleCollider
        │     ├── HullCollider
        │     ├── ModelCollider
        │     └── PlaneCollider
        ├── Rigidbody
        ├── CharacterController
        ├── PanelComponent (UI, Razor-based)
        ├── ScreenPanel / WorldPanel
        ├── CameraComponent
        ├── Light components (DirectionalLight, PointLight, SpotLight)
        ├── Joint components (BallJoint, HingeJoint, FixedJoint, SpringJoint, SliderJoint)
        ├── NavMeshAgent
        ├── ParticleEffect
        └── Game components (Prop, SpawnPoint, Hitbox, etc.)

GameObjectSystem (abstract) : IDisposable
  └── GameObjectSystem<T> (generic, static Current accessor)
```

### Complete Component Lifecycle

```
GameObject Created
  → OnAwake()          // Called once when component is first created
  → OnEnabled()        // Called when component becomes active
  → OnStart()          // Called once before first Update, after OnAwake
  → [Frame Loop]:
      → OnUpdate()         // Every frame
      → OnPreRender()      // Every frame (rendering phase)
  → [Physics Loop]:
      → OnFixedUpdate()    // Fixed timestep (physics tick)
  → OnDisabled()       // When component becomes inactive
  → OnDestroy()        // When component or GameObject is destroyed
```

**Additional callbacks:**
- `OnLoad()` — async, called during scene load
- `OnValidate()` — after deserialization or property changes in editor
- `OnRefresh()` — after network snapshot updates
- `OnTagsChanged()` — when GameObject tags change
- `OnParentChanged()` — when parent GameObject changes
- `OnParentDestroy()` — when parent is destroyed

### All Component Interfaces

```csharp
// Execute lifecycle methods in editor (not just play mode)
Component.ExecuteInEditor

// Physics collision events
Component.ICollisionListener  // or implement ISceneCollisionEvents
{
    void OnCollisionStart( Collision collision );
    void OnCollisionUpdate( Collision collision );
    void OnCollisionStop( CollisionStop collision );
}

// Trigger volume events
Component.ITriggerListener
{
    void OnTriggerEnter( Collider other );
    void OnTriggerExit( Collider other );
}

// Damage system
Component.IDamageable
{
    void OnDamage( in DamageInfo damage );
}

// Network connection events
Component.INetworkListener
{
    void OnActive( Connection connection );  // Player connected
}

// Network spawn events
Component.INetworkSpawn

// Scene startup (for GameObjectSystems)
ISceneStartup
{
    void OnHostPreInitialize( SceneFile scene );  // Before scene load, host only
    void OnHostInitialize();                       // After scene load, host only
    void OnClientInitialize();                     // After client loads scene
}

// Scene physics events
IScenePhysicsEvents
{
    void PrePhysicsStep();
    void PostPhysicsStep();
    void OnOutOfBounds( Rigidbody body );
    void OnFellAsleep( Rigidbody body );
}

// Scene loading events
ISceneLoadingEvents
{
    void BeforeLoad( Scene scene, SceneLoadOptions options );
    Task OnLoad( Scene scene, SceneLoadOptions options );
    void AfterLoad( Scene scene );
}

// Network ownership changes
IGameObjectNetworkEvents
{
    void NetworkOwnerChanged( Connection newOwner, Connection previousOwner );
    void StartControl();    // Became controller (no longer proxy)
    void StopControl();     // Became proxy (controlled by someone else)
}
```

### Complete Attribute Reference

**Component & Property Attributes:**

| Attribute | Target | Description |
|-----------|--------|-------------|
| `[Property]` | Property | Expose in inspector, serialized |
| `[Range(min, max)]` | Property | Slider with min/max |
| `[Title("name")]` | Class/Property | Custom display name |
| `[Category("name")]` | Class | Add Component menu category |
| `[Group("name")]` | Property | Collapsible section in inspector |
| `[Icon("icon")]` | Class | Component icon |
| `[Description("text")]` | Class | Tooltip description |
| `[RequireComponent]` | Property | Auto-get or create component |
| `[Step(amount)]` | Property | Value increment step |
| `[Feature]` | Property | Feature toggle grouping |
| `[Hide]` | Property | Hide from inspector |
| `[TextArea]` | Property | Multi-line text input |

**Networking Attributes:**

| Attribute | Target | Description |
|-----------|--------|-------------|
| `[Sync]` | Property | Auto-sync from owner to all clients |
| `[Sync(SyncFlags.Interpolate)]` | Property | Sync with interpolation |
| `[Sync(SyncFlags.FromHost)]` | Property | Host owns the value |
| `[Sync(SyncFlags.Query)]` | Property | Poll-based change detection |
| `[Change("callback")]` | Property | Call method on value change |
| `[Rpc.Broadcast]` | Method | Execute on ALL clients |
| `[Rpc.Owner]` | Method | Execute on owning client only |
| `[Rpc.Host]` | Method | Execute on host/server only |

**Other Attributes:**

| Attribute | Description |
|-----------|-------------|
| `[ConCmd("name")]` | Console command |
| `[ConVar]` | Console variable |
| `[GameResource("name", "ext", "desc")]` | Custom asset type |
| `[Library]` | Type registration |
| `[Expose]` | Expose to reflection/ActionGraph |
| `[CodeGenerator]` | Code generation wrapper |
| `[SkipHotload]` | Exclude from hotload state preservation |
| `[Event.Tick]` | Subscribe to tick events |
| `[Event.Hotload]` | Called after hot-reload |

### DamageInfo Class

```csharp
public class DamageInfo
{
    public GameObject Attacker { get; set; }
    public GameObject Weapon { get; set; }
    public Hitbox Hitbox { get; set; }
    public float Damage { get; set; }
    public Vector3 Origin { get; set; }
    public Vector3 Position { get; set; }
    public PhysicsShape Shape { get; set; }
    public TagSet Tags { get; set; } = new();
}

// Usage:
public class Health : Component, Component.IDamageable
{
    [Property, Sync] public float HP { get; set; } = 100f;

    public void OnDamage( in DamageInfo damage )
    {
        HP -= damage.Damage;
        if ( HP <= 0 ) GameObject.Destroy();
    }
}

// Applying damage via trace:
var tr = Scene.Trace.Ray( start, end ).Run();
var damageable = tr.GameObject.Components.Get<Component.IDamageable>();
damageable?.OnDamage( new DamageInfo { Damage = 25f, Attacker = GameObject } );
```

### ComponentFlags Enum

```csharp
[Flags]
public enum ComponentFlags
{
    None = 0,
    Hidden = 1,            // Hide in inspector
    NotSaved = 2,          // Don't save to disk
    Error = 4,
    Loading = 8,
    Deserializing = 16,
    NotEditable = 32,
    NotNetworked = 64,     // Don't network this component
    NotCloned = 256,       // Don't serialize when cloning
    ShowAdvancedProperties = 512
}
```

### SyncFlags & NetworkFlags

```csharp
[Flags]
public enum SyncFlags : uint
{
    FromHost = 1,      // Host owns the value
    Query = 2,         // Poll for changes each network update
    Interpolate = 4    // Interpolate between ticks (float, Vector3, Rotation, etc.)
}

[Flags]
public enum NetworkFlags
{
    None = 0,
    NoInterpolation = 1,
    NoPositionSync = 2,
    NoRotationSync = 4,
    NoScaleSync = 8,
    NoTransformSync = NoPositionSync | NoRotationSync | NoScaleSync
}
```

### RPC NetFlags

```csharp
// Usage: [Rpc.Broadcast( NetFlags.Unreliable | NetFlags.OwnerOnly )]
NetFlags.Unreliable      // May not arrive, may be out of order. Fast + cheap.
NetFlags.Reliable         // Default. Multiple attempts until received.
NetFlags.SendImmediate    // Don't batch with other messages.
NetFlags.DiscardOnDelay   // Drop if can't send quickly (unreliable only).
NetFlags.HostOnly         // Only callable from host.
NetFlags.OwnerOnly        // Only callable from object owner.
```

### RPC Helpers

```csharp
Rpc.Caller           // Connection that invoked the RPC
Rpc.Caller.IsHost    // Was it the host?
Rpc.Caller.SteamId   // Caller's Steam ID
Rpc.Calling          // bool, true if being called remotely

// Filter recipients
using ( Rpc.FilterExclude( c => c.DisplayName == "Harry" ) )
    MyBroadcastRpc();

using ( Rpc.FilterInclude( c => c.DisplayName == "Garry" ) )
    MyBroadcastRpc();
```

### IsProxy Pattern

```csharp
// IsProxy = true when current client does NOT own/control this object
protected override void OnUpdate()
{
    if ( IsProxy ) return; // Not our object, skip input

    if ( Input.Pressed( "attack1" ) )
        Fire();
}
```

### Network Object Lifecycle

```csharp
// Spawn networked
var go = PlayerPrefab.Clone( spawnPos );
go.NetworkSpawn( connection );  // Assign to connection

// Ownership
go.Network.Owner = connection;       // Assign
go.Network.Owner = Connection.Local;  // Take ownership
go.Network.Owner = null;              // Drop

// Force refresh all sync properties (expensive, use sparingly)
Network.Refresh();
```

### GameObjectSystem Stages

```csharp
public class MySystem : GameObjectSystem<MySystem>
{
    public MySystem( Scene scene ) : base( scene )
    {
        Listen( Stage.PhysicsStep, 0, DoPhysics, "Physics" );
        Listen( Stage.StartUpdate, -1, BeforeUpdate, "BeforeUpdate" );
    }
}

// Available stages:
Stage.StartUpdate
Stage.UpdateBones
Stage.PhysicsStep
Stage.Interpolation
Stage.FinishUpdate
Stage.StartFixedUpdate
Stage.FinishFixedUpdate
Stage.SceneLoaded
```

### Trace API (Complete)

```csharp
// Ray trace
var tr = Scene.Trace.Ray( start, end )
    .WithoutTags( "player", "trigger" )
    .IgnoreGameObject( myObj )
    .IgnoreGameObjectHierarchy( root )
    .Size( 10 )           // AABB size
    .Radius( 5 )          // Sphere/capsule radius
    .UseHitboxes()         // Use hitbox colliders
    .HitTriggers()         // Include triggers
    .Run();                // First hit

// Box trace
var tr = Scene.Trace.Box( bbox, start, end ).Run();

// Sphere trace
var tr = Scene.Trace.Sphere( radius, start, end ).Run();

// Body trace
var tr = Scene.Trace.Body( physicsBody, start, end ).Run();

// Multi-hit
SceneTraceResult[] hits = Scene.Trace.Ray( start, end ).RunAll();

// Result properties:
tr.Hit           // bool
tr.GameObject    // what was hit
tr.HitPosition   // Vector3
tr.Normal         // surface normal
tr.Distance       // distance to hit
```

### Sound System

```csharp
Sound.Play( "sound.name" );                            // 2D
Sound.FromWorld( "fx.explosion", worldPos );            // 3D positioned
Sound.FromScreen( "ui.click" );                         // UI sound

// With SoundEvent property
[Property] public SoundEvent HitSound { get; set; }
Sound.Play( HitSound, WorldPosition );

// Control handle
SoundHandle h = Sound.FromWorld( "loop", pos );
h.Volume = 0.5f;
h.Pitch = 1.2f;
h.SetPosition( newPos );
h.Stop();
```

### Particle System

```csharp
// ParticleEffect component properties
MaxParticles    // int, default 1000
Lifetime        // float, seconds
TimeScale       // float, multiplier
PreWarm         // float, seconds

// Emitter types: ParticleBoxEmitter, ParticleConeEmitter, ParticleSphereEmitter
// Renderers: ParticleSpriteRenderer, ParticleModelRenderer

// BeamEffect
var beam = go.Components.Create<BeamEffect>();
beam.TargetPosition = targetPos;
beam.MaxBeams = 1;
beam.Looped = false;
```

### Rendering Components

```csharp
// ModelRenderer
var mr = go.AddComponent<ModelRenderer>();
mr.Model = Model.Load( "models/dev/box.vmdl" );
mr.Tint = Color.Red;
mr.MaterialOverride = Material.Load( "materials/custom.vmat" );
mr.SetBodyGroup( "head", 1 );

// SkinnedModelRenderer (animated)
var sk = go.AddComponent<SkinnedModelRenderer>();
sk.Model = Model.Load( "models/citizen.vmdl" );
sk.Set( "move_speed", 100f );     // Animation parameter
sk.Set( "grounded", true );
sk.BoneMergeTarget = otherSkinned; // Clothing bone merge

// TextRenderer
var tr = go.AddComponent<TextRenderer>();
tr.Text = "Hello";
tr.FontSize = 32f;

// LineRenderer
var lr = go.AddComponent<LineRenderer>();
lr.VectorPoints = new List<Vector3>();

// TrailRenderer
var trail = go.AddComponent<TrailRenderer>();
trail.LifeTime = 2f;
trail.Emitting = true;
```

### HudPainter (Immediate-Mode 2D Drawing)

```csharp
protected override void OnUpdate()
{
    if ( Scene.Camera is null ) return;
    var hud = Scene.Camera.Hud;

    hud.DrawRect( new Rect( 300, 300, 10, 10 ), Color.White );
    hud.DrawLine( new Vector2( 100, 100 ), new Vector2( 200, 200 ), 10, Color.White );
    hud.DrawText( new TextRendering.Scope( "Hello!", Color.Red, 32 ), Screen.Width * 0.5f );
}
```

### UI Razor Deep Reference

```razor
@using Sandbox;
@using Sandbox.UI;
@inherits PanelComponent

<root>
    <div class="hud">
        <div class="health">@Health</div>
        @if ( IsAlive )
        {
            <div class="alive">Alive!</div>
        }
        @foreach ( var item in Items )
        {
            <div class="item">@item.Name</div>
        }
    </div>
</root>

@code
{
    [Property] public float Health { get; set; } = 100f;
    public bool IsAlive => Health > 0;
    public List<Item> Items { get; set; } = new();

    // Panel only rebuilds when hash changes
    protected override int BuildHash() => System.HashCode.Combine( Health, IsAlive, Items.Count );
}
```

**Key Razor patterns:**
- `Value:bind=@Property` — two-way binding
- `@ref="PanelRef"` — store Panel reference
- `<ChildPanel Health=@(30) />` — pass parameters
- `RenderFragment` — slot/content injection
- `ScreenPanel` component required on same GameObject for HUD
- `WorldPanel` component for 3D-space UI

**Styling:** `.razor.scss` files, everything is flexbox by default, supports `filter`, `backdrop-filter`, transitions, animations, pseudo-classes `:intro`/`:outro` for enter/exit animations.

### Project File Structure

```
project_root/
  myproject.sbproj              # Project config (JSON) — source of truth
  myproject.slnx                # Solution file (auto-generated)
  Assets/
    scenes/
      minimal.scene             # Scene files (JSON)
  Code/
    Assembly.cs                 # global using Sandbox; etc.
    MyComponent.cs              # Game components
  Editor/
    Assembly.cs                 # global using Editor; etc.
    MyEditorTool.cs             # Editor-only code
  Libraries/
    org.package_name/           # Downloaded libraries
  ProjectSettings/
    Collision.config            # Collision layer rules (JSON)
    Input.config                # Input action bindings (JSON)
```

### .sbproj Schema

```json
{
  "Title": "my_game",
  "Type": "game",                    // "game", "addon", "library"
  "Org": "local",
  "Ident": "my_game",
  "Schema": 1,
  "Metadata": {
    "MaxPlayers": 64,
    "MinPlayers": 1,
    "TickRate": 50,
    "GameNetworkType": "Multiplayer",
    "StartupScene": "scenes/minimal.scene"
  }
}
```

### Scene File Format (.scene)

```json
{
  "__guid": "325a4042-0696-43dd-a79d-dcc314299ba3",
  "GameObjects": [
    {
      "__guid": "bfc59c12-1ed2-4f91-8956-a95a315eac3c",
      "Name": "Sun",
      "Position": "0,0,0",
      "Rotation": "-0.0729,0.4822,0.1305,0.8631",
      "Scale": "1,1,1",
      "Tags": "light_directional,light",
      "Enabled": true,
      "Components": [
        {
          "__type": "Sandbox.DirectionalLight",
          "__guid": "d3659344-a90d-48fa-927a-095f70fe041f",
          "LightColor": "0.944,0.977,1,1",
          "Shadows": true
        }
      ],
      "Children": []
    }
  ]
}
```

### Key API Reference URLs

| Topic | URL |
|-------|-----|
| Full API Reference | https://sbox.game/api/all |
| Component API | https://sbox.game/api/all/Sandbox.Component/ |
| Community API Docs | https://superdocs.sbox.cool/ |
| Engine Source (MIT) | https://github.com/Facepunch/sbox-public |
| Scene Staging Examples | https://github.com/Facepunch/sbox-scenestaging |
| Release Notes | https://sbox.game/release-notes |

### API Scale

~3,157 types, ~6,867 methods, ~6,260 properties across 67 namespaces including `Sandbox`, `Sandbox.Physics`, `Sandbox.Audio`, `Sandbox.UI`, `Sandbox.Network`, `Sandbox.Rendering`, `Sandbox.ActionGraphs`.

