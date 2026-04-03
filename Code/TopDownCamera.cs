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

	private int warmupFrames;
	private bool cameraReady;

	protected override void OnUpdate()
	{
		if ( Target is null ) return;
		if ( !cameraReady ) warmupFrames++;

		Mouse.Visibility = MouseVisibility.Visible;

		FollowTarget();

		if ( !cameraReady )
		{
			// Camera needs a few rendered frames before its projection matrix is valid
			if ( warmupFrames < 3 ) return;
			cameraReady = true;
		}

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
	/// Approach: project the target's world position to screen pixels, compute a 2D delta
	/// to the mouse, then convert that delta to a 3D ground direction using the camera's
	/// orientation as a basis (change of basis from screen space to world space).
	///
	///   Screen (pixels)          World (ground plane)
	///   ┌──────────────┐              ↑ forward
	///   │         ✕    │              │    ◆ aim
	///   │       ↗      │  ────────►   ●──↗
	///   │     ●        │            target
	///   └──────────────┘
	///   delta = mouse - target     direction = left·Δx - forward·Δy
	/// </summary>
	private void AimTargetAtMouse()
	{
		var camera = Scene.Camera;
		if ( camera is null ) return;

		// Both PointToScreenPixels and Mouse.Position use window-pixel coordinates,
		// so subtracting them cancels out any editor viewport offset
		var playerScreenPos = camera.PointToScreenPixels( Target.WorldPosition );
		var playerToCursor = Mouse.Position - playerScreenPos;

		// Camera basis vectors projected flat onto the ground (Z=0, then re-normalized).
		// These map screen axes → world axes regardless of camera tilt.
		//   Screen +X (right)  → worldLeft  (s&box screen X is mirrored vs world Y)
		//   Screen +Y (down)   → -worldForward (screen Y is inverted vs world forward)
		var worldLeft = camera.WorldRotation.Left.WithZ( 0 ).Normal;
		var worldForward = camera.WorldRotation.Forward.WithZ( 0 ).Normal;

		// Change of basis: 2D pixel delta → 3D ground direction
		var aimDirection = (worldLeft * playerToCursor.x - worldForward * playerToCursor.y).Normal;

		if ( aimDirection.Length > 0.1f )
		{
			Target.WorldRotation = Rotation.LookAt( aimDirection );
		}
	}
}
