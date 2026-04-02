using Sandbox;

public sealed class TopDownCamera : Component
{
	
	[Property] public GameObject Target { get; set; }
	[Property] public float Height { get; set; } = 500f;
	[Property] public float BackwardOffset { get; set; } = 60f;

	// Wait a few frames before aiming so the camera projection matrix is valid
	private int warmupFrames;
	private bool cameraReady;

	protected override void OnUpdate()
	{
		// Don't do anything if we haven't assigned a Target in the inspector
		if ( Target is null ) return;

		if ( !cameraReady )
			warmupFrames++;

		Mouse.Visibility = MouseVisibility.Visible;
		
		// Get the player's current position in the world (x, y, z coordinates)
		var targetPos = Target.WorldPosition;

		// Calculate where the camera should sit relative to the player.
		//
		// In s&box: X = forward/backward, Y = left/right, Z = up/down
		//
		// Vector3.Backward * BackwardOffset:
		//   Vector3.Backward is (-1, 0, 0) — pointing away from "forward"
		//   Multiplying by BackwardOffset (60) gives (-60, 0, 0)
		//   This pushes the camera 60 units "behind" the player
		//   This creates the slight tilt — without it, you'd look straight down
		//
		// Vector3.Up * Height:
		//   Vector3.Up is (0, 0, 1) — pointing straight up
		//   Multiplying by Height (500) gives (0, 0, 500)
		//   This lifts the camera 500 units above the player
		//
		// Combined: offset = (-60, 0, 500)
		//   The camera ends up high above and slightly behind the player
		var offset = Vector3.Backward * BackwardOffset + Vector3.Up * Height;

		// Place the camera at the player's position + the offset we calculated
		// If the player is at (100, 200, 0), the camera ends up at (40, 200, 500)
		// Every frame this updates, so the camera follows the player as they move
		WorldPosition = targetPos + offset;

		// Make the camera look toward the player.
		//
		// targetPos - WorldPosition = the direction vector FROM the camera TO the player
		// Example: (100, 200, 0) - (40, 200, 500) = (60, 0, -500)
		//   That's a vector pointing forward and steeply downward — exactly where the player is
		//
		// Rotation.LookAt() converts that direction vector into a rotation
		//   so the camera's "forward" axis points along that direction
		//   Result: camera aims down at the player from above
		WorldRotation = Rotation.LookAt( targetPos - WorldPosition );
		
		// --- Mouse Aim ---
		// Skip first few frames — camera projection matrix isn't valid until it has rendered
		if ( !cameraReady )
		{
			if ( warmupFrames < 3 ) return;
			cameraReady = true;
		}

		var camera = Scene.Camera;
		if ( camera is null ) return;

		// Project the player's world position onto the screen (same space as Mouse.Position)
		// This way both coordinates use the same pixel space, so any editor offset cancels out
		var playerScreenPos = camera.PointToScreenPixels( Target.WorldPosition );

		// Screen-space direction from player to mouse cursor
		var playerToCursor = Mouse.Position - playerScreenPos;

		// Convert screen direction to world direction using camera's orientation
		// Camera's right vector = "screen X" in world space
		// Camera's forward vector = "screen Y" in world space (inverted because screen Y goes down)
		var worldLeft = camera.WorldRotation.Left.WithZ( 0 ).Normal;
		var worldForward = camera.WorldRotation.Forward.WithZ( 0 ).Normal;

		var worldDirection = (worldLeft * playerToCursor.x - worldForward * playerToCursor.y).Normal;

		if ( worldDirection.Length > 0.1f )
		{
			Target.WorldRotation = Rotation.LookAt( worldDirection );
		}
	}
}
