using Sandbox;

/// <summary>
/// A projectile that moves forward in the direction it's facing and self-destructs after a lifetime.
/// Attach to a GameObject with a ModelRenderer (the visible bullet) and a Collider set to Trigger mode.
/// </summary>
public sealed class Projectile : Component
{
	/// <summary>How fast the projectile moves (units per second).</summary>
	[Property] public float Speed { get; set; } = 1000f;

	/// <summary>How long (seconds) before the projectile destroys itself.</summary>
	[Property] public float Lifetime { get; set; } = 2f;

	/// <summary>
	/// TimeSince is an s&box struct that works like a stopwatch.
	/// Set it to 0 and it automatically counts up each frame.
	/// So "timeSinceSpawn > 2f" means "has 2 seconds passed?"
	/// </summary>
	private TimeSince timeSinceSpawn;

	protected override void OnStart()
	{
		// Start the stopwatch when this component first runs
		timeSinceSpawn = 0;
	}

	protected override void OnUpdate()
	{
		// Move forward along the GameObject's facing direction every frame.
		// WorldRotation.Forward gives us the direction the object is pointing.
		// Multiply by Speed and Time.Delta to get frame-independent movement.
		WorldPosition += WorldRotation.Forward * Speed * Time.Delta;

		// Self-destruct after lifetime expires
		if ( timeSinceSpawn >= Lifetime )
		{
			GameObject.Destroy();
		}
	}
}
