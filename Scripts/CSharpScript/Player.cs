using Godot;
using Godot.Collections;
using System;

public partial class Player : CharacterBody3D
{
	[Signal] public delegate void SetMovementStateEventHandler(MovementState movementState);
	[Signal] public delegate void SetMovementDirectionEventHandler(Vector3 movementDirection);
	[Signal] public delegate void PressedJumpEventHandler(JumpState jumpState);

	[Export] public Dictionary MovementStates;
	[Export] public Dictionary JumpStates;

	public Vector2 inputDir;
	public Vector3 _movementDirection;
	public string JumpName;

	public int AirJumpCounter = 0;
	public int MaxAirJump = 1;

	public enum PlayerState
	{
		idle, walk, run, sprint, ground_jump, air_jump
	}

	public PlayerState CurrentState = PlayerState.idle;

	public override void _PhysicsProcess(double delta)
	{
		if (IsMoving())
		{
			EmitSignal(nameof(SetMovementDirection), _movementDirection);
		}
		JumpState();

		switch (CurrentState)
		{
			case PlayerState.idle:
				EmitSignal(nameof(SetMovementState), MovementStates["idle"]);
				break;
			case PlayerState.walk:
				EmitSignal(nameof(SetMovementState), MovementStates["walk"]);
				break;
			case PlayerState.run:
				EmitSignal(nameof(SetMovementState), MovementStates["run"]);
				break;
			case PlayerState.sprint:
				EmitSignal(nameof(SetMovementState), MovementStates["sprint"]);
				break;
			case PlayerState.ground_jump:
				EmitSignal(nameof(PressedJump), JumpStates[JumpName]);
				break;
			case PlayerState.air_jump:
				EmitSignal(nameof(PressedJump), JumpStates[JumpName]);
				break;
		}

	}

	public override void _Process(double delta)
	{
		_movementDirection.X = Input.GetActionStrength("MoveLeft") - Input.GetActionStrength("MoveRight");
		_movementDirection.Z = Input.GetActionStrength("MoveForward") - Input.GetActionStrength("MoveBack");

		if (Input.IsActionPressed("movement"))
		{
			_movementDirection = new Vector3(_movementDirection.X, 0, _movementDirection.Z).Normalized();
			CurrentState = PlayerState.run;

			if (IsMoving())
			{
				if (Input.IsActionPressed("sprint"))
				{
					CurrentState = PlayerState.sprint;
				}
				else if (Input.IsActionPressed("walk"))
				{
					CurrentState = PlayerState.walk;
				}
			}
		}

		else if (_movementDirection.X == 0 && _movementDirection.Z == 0)
		{
			CurrentState = PlayerState.idle;
		}
	}

	public bool IsMoving()
	{
		return Mathf.Abs(_movementDirection.X) > 0 || Mathf.Abs(_movementDirection.Z) > 0;
	}

	public void JumpState()
	{
		if (IsOnFloor())
		{
			AirJumpCounter = 0;
		}
		else if (AirJumpCounter == 0)
		{
			AirJumpCounter = 1;
		}
		if (AirJumpCounter <= MaxAirJump)
		{
			if (Input.IsActionJustPressed("jump"))
			{
				JumpName = "ground_jump";
				CurrentState = PlayerState.ground_jump;

				if (AirJumpCounter > 0)
				{
					JumpName = "air_jump";
					CurrentState = PlayerState.air_jump;
				}
				AirJumpCounter += 1;

			}

		}

	}

}
