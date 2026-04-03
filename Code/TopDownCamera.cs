using Sandbox;

/// <summary>
/// Top-down camera that follows a target and rotates it to face the mouse cursor.
/// Attach to a GameObject with a CameraComponent.
/// </summary>
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

	/// <summary>
	/// Positions the camera above and behind the target, looking down at it.
	///
	///   Side view:        Camera
	///                    /
	///        Height     /  (camera looks along this line)
	///          |       /
	///          |      /
	///          |     /
	///          Target──────
	///            BackwardOffset
	/// </summary>
	private void FollowTarget()
	{
		var targetPos = Target.WorldPosition;

		// Offset = (-BackwardOffset, 0, +Height) relative to target
		var offset = Vector3.Backward * BackwardOffset + Vector3.Up * Height;
		WorldPosition = targetPos + offset;

		// Point the camera from its position toward the target
		WorldRotation = Rotation.LookAt( targetPos - WorldPosition );
	}

	/// <summary>
	/// Rotates the target to face the mouse cursor on the ground plane.
	///
	/// The camera is tilted, so "right on screen" and "up on screen" don't map directly
	/// to world X/Y. We ask the camera what its Right and Forward directions are, flatten
	/// them onto the ground, and use those as a basis to convert screen deltas to world directions.
	///
	///   Screen (pixels)                     World (top-down view of ground)
	///   ┌──────────────┐                         screenUp
	///   │    screen up  ↑                           ↑
	///   │               │                           │
	///   │  screen  ←────●────→ screen               │
	///   │  left       center    right     ←─────────●─────────→
	///   │               │             screenLeft  player  screenRight
	///   │    screen down ↓
	///   └──────────────┘
	/// </summary>
	private void AimTargetAtMouse()
	{
		var camera = Scene.Camera;
		if ( camera is null ) return;

		// Camera always looks at the target, so target ≈ screen center
		var screenCenter = Screen.Size / 2f;
		var playerToCursor = Mouse.Position - screenCenter;

		// What does "right on screen" mean on the ground?
		// What does "up on screen" mean on the ground?
		// The camera knows — we just flatten its axes onto the ground plane (Z=0).
		//
		// We use Left instead of Right because s&box's screen X is mirrored vs world Y.
		// We negate screenUp because screen Y goes downward but "up" means forward in world.
		var screenRight = camera.WorldRotation.Left.WithZ( 0 ).Normal;
		var screenUp = camera.WorldRotation.Forward.WithZ( 0 ).Normal;

		// Change of basis: screen pixels → world ground direction
		// "go (Δx pixels) in the screenRight direction, then (Δy pixels) in the screenUp direction"
		var aimDirection = (screenRight * playerToCursor.x - screenUp * playerToCursor.y).Normal;

		if ( aimDirection.Length > 0.1f )
		{
			Target.WorldRotation = Rotation.LookAt( aimDirection );
		}

		if ( ShowDebugVectors )
		{
			DrawDebugVectors( screenRight, screenUp, aimDirection );
		}
	}

	/// <summary>
	/// Draws colored arrows from the player showing the basis vectors and aim direction.
	///   Red   = screenRight (what "right on screen" means on the ground)
	///   Blue  = screenUp    (what "up on screen" means on the ground)
	///   Green = aimDirection (the final combined direction toward the mouse)
	/// </summary>
	private void DrawDebugVectors( Vector3 screenRight, Vector3 screenUp, Vector3 aimDirection )
	{
		// Reset gizmo transform to world space (otherwise it draws relative to the camera)
		Gizmo.Transform = global::Transform.Zero;

		var origin = Target.WorldPosition + Vector3.Up * 5f;
		var length = 80f;

		// Negate for display — the math vectors point opposite to their visual screen direction
		// because screen X maps to world Left, and screen Y is inverted vs world Forward

		// Red arrow: "screen right" (negated — screen X maps to world Left)
		Gizmo.Draw.Color = Color.Red;
		Gizmo.Draw.Arrow( origin, origin + -screenRight * length, 4f, 2f );

		// Blue arrow: "screen up" as seen from above
		Gizmo.Draw.Color = Color.Blue;
		Gizmo.Draw.Arrow( origin, origin + screenUp * length, 4f, 2f );

		// Green arrow: where the player is actually facing
		// Gizmo has X axis flipped vs world, so we negate it for display
		var facing = Target.WorldRotation.Forward;
		var displayFacing = new Vector3( -facing.x, facing.y, facing.z );
		Gizmo.Draw.Color = Color.Green;
		Gizmo.Draw.Arrow( origin, origin + -displayFacing * length * 1.5f, 4f, 2f );

		// Screen text labels
		var camera = Scene.Camera;
		if ( camera is null ) return;
		var hud = camera.Hud;

		var labelOffset = 20f;
		hud.DrawText(
			new TextRendering.Scope( "RED = screenRight (screen → on ground)", Color.Red, 14 ),
			new Vector2( labelOffset, Screen.Height - 80f )
		);
		hud.DrawText(
			new TextRendering.Scope( "BLUE = screenUp (screen ↑ on ground)", Color.Blue, 14 ),
			new Vector2( labelOffset, Screen.Height - 60f )
		);
		hud.DrawText(
			new TextRendering.Scope( "GREEN = aimDirection (where player faces)", Color.Green, 14 ),
			new Vector2( labelOffset, Screen.Height - 40f )
		);
	}
}
