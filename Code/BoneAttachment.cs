using Sandbox;

/// <summary>
/// Attaches this GameObject to a bone on a SkinnedModelRenderer.
/// The object follows the bone's position and rotation through all animations.
/// Use PositionOffset and RotationOffset to fine-tune placement in the inspector.
/// </summary>
public sealed class BoneAttachment : Component, Component.ExecuteInEditor
{
	[Property] public SkinnedModelRenderer Body { get; set; }
	[Property] public string BoneName { get; set; } = "head";
	[Property] public Vector3 PositionOffset { get; set; }
	[Property] public Angles RotationOffset { get; set; }

	protected override void OnUpdate()
	{
		if ( Body is null ) return;

		var bone = Body.GetBoneObject( BoneName );
		if ( bone is null ) return;

		WorldPosition = bone.WorldPosition + bone.WorldRotation * PositionOffset;
		WorldRotation = bone.WorldRotation * RotationOffset.ToRotation();
	}
}