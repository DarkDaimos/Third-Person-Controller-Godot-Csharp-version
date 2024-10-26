using Godot;
using System;

public partial class AnimationController : Node
{
	public AnimationTree animationTree;
	public CharacterBody3D player;
	public Tween tween;

	public float onFloorBlend = 1.0f;
	public float onFloorBlendTarget = 1.0f;

    public override void _Ready()
    {
        player = GetNode<CharacterBody3D>("..");
		animationTree = GetNode<AnimationTree>("../MeshRoot/AnimationTree");
    }

    public override void _PhysicsProcess(double delta)
    {
        onFloorBlendTarget = player.IsOnFloor() ? 1 : 0;
		onFloorBlend = Mathf.Lerp(onFloorBlend, onFloorBlendTarget, 10 * (float)delta);
		animationTree.Set("parameters/onFloorBlend/blend_amount", onFloorBlend);
    }

	public void _PressedJump(JumpState jumpState)
	{
		animationTree.Set("parameters/" + jumpState.AnimationName + "/request", (int)AnimationNodeOneShot.OneShotRequest.Fire);
	}

	public void _SetMovementState(MovementState movementState)
	{
		if(tween != null)
    {
        tween.Kill();
    }

    tween = CreateTween();
    tween.TweenProperty(animationTree, "parameters/movement_blend/blend_position", movementState.Id, 0.25);
    tween.Parallel().TweenProperty(animationTree, "parameters/movement_anim_speed/scale", movementState.AnimationSpeed, 0.7);
	}
}
