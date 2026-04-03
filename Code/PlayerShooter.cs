using Sandbox;

/// <summary>
/// Attach to the Player GameObject. Spawns a projectile when the player clicks mouse1.
/// </summary>
public sealed class PlayerShooter : Component
{
	/// <summary>Offset in front of the player where the projectile spawns (so it doesn't spawn inside the player).</summary>
	[Property] public float SpawnOffset { get; set; } = 50f;

	protected override void OnUpdate()
	{
		// Input.Pressed returns true on the FRAME the button is first pressed (not held).
		// "attack1" is mapped to mouse1 in Input.config.
		if ( Input.Pressed( "attack1" ) )
		{
			ShootProjectile();
		}
	}

	private void ShootProjectile()
	{
		// Create a brand new empty GameObject in the scene
		var go = new GameObject();
		go.Name = "Projectile";

		// Position it slightly in front of the player so it doesn't collide with us
		// WorldRotation.Forward = the direction the player is facing
		go.WorldPosition = WorldPosition + WorldRotation.Forward * SpawnOffset;
		go.WorldRotation = WorldRotation;

		// Add a visible sphere model so we can see the projectile
		var renderer = go.AddComponent<ModelRenderer>();
		renderer.Model = Model.Load( "models/dev/sphere.vmdl" );
		renderer.Tint = Color.Yellow;
		go.WorldScale = Vector3.One * 0.3f;  // shrink it down — default sphere is big

		// Add our Projectile component to make it fly and self-destruct
		go.AddComponent<Projectile>();

		// Add a collider as a Trigger (for future damage detection).
		// Trigger = things pass through it but we get OnTriggerEnter events.
		var collider = go.AddComponent<SphereCollider>();
		collider.Radius = 10f;
		collider.IsTrigger = true;

		// Tag it so we can identify projectiles later
		go.Tags.Add( "projectile" );
	}
}