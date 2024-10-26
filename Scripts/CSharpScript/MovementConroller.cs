using Godot;
using Godot.Collections;
using System;

public partial class MovementConroller : Node
{
	public float Speed;
	public CharacterBody3D player;
	public Node3D meshRoot;
	public Vector3 direction;
	public Vector3 velocity;
	public float CamRotation = 0.0f;
	public float RotationSpeed = 8.0f;
	public float Acceleration;
	[Export] float FallGravity = 45.0f;
	public float JumpGravity;
	public float gravity = ProjectSettings.GetSetting("physics/3d/default_gravity").AsSingle();

	public override void _Ready()
	{
		player = GetNode<CharacterBody3D>("..");
		meshRoot = GetNode<Node3D>("../MeshRoot");
		JumpGravity = FallGravity;
	}

	public override void _PhysicsProcess(double delta)
	{
		MovementProcess(delta);
		player.MoveAndSlide();
	}


	public void MovementProcess(double delta)
	{
		velocity = player.Velocity;

		velocity.X = direction.Normalized().X * Speed;
		velocity.Z = direction.Normalized().Z * Speed;

		if (!player.IsOnFloor())
		{
			if (velocity.Y >= 0)
			{
				velocity.Y -= JumpGravity * (float)delta;

			}
			else velocity.Y -= FallGravity * (float)delta;

		}

		player.Velocity = player.Velocity.Lerp(velocity, Acceleration * (float)delta);
		player.Velocity = velocity;

		float target_rotation = Mathf.Atan2(direction.X, direction.Z) - player.Rotation.Y;
		meshRoot.Rotation = new Vector3(0, Mathf.LerpAngle(meshRoot.Rotation.Y, target_rotation, RotationSpeed * (float)delta), 0);

	}

	public void _SetMovementState(MovementState movementState)
	{
		Speed = movementState.MovementSpeed;
		Acceleration = movementState.Acceleration;
	}

	public void _SetMovementDirection(Vector3 movementDirection)
	{
		direction = movementDirection.Rotated(Vector3.Up, CamRotation);
	}

	public void _SetCamRotation(float _camRotation)
	{
		CamRotation = _camRotation;
	}

	public void _PressedJump(JumpState jumpState)
	{
		velocity = player.Velocity;

		velocity.Y = 2 * jumpState.JumpHeight / jumpState.ApexDuraction;
		JumpGravity = velocity.Y / jumpState.ApexDuraction;
		
		player.Velocity = velocity;
	}
}
