using Godot;
using System;
using System.Numerics;

public partial class CameraController : Node
{
	[Signal] public delegate void SetCamRotationEventHandler(float _camRotation);
	private Camera3D camera3D;
	private Node3D YawNode;
	private Node3D PitchNode;

	private float YawCamera;
	private float PitchCamera;
	[Export] public float CameraSensitivity = 0.3f;
	private float CameraAcceleration = 15.0f;
	public Tween tween;

	public override void _Ready()
	{
		Input.MouseMode = Input.MouseModeEnum.Captured;

		YawNode = GetNode<Node3D>("CamYaw");
		PitchNode = GetNode<Node3D>("CamYaw/CamPitch");
		camera3D = GetNode<Camera3D>("CamYaw/CamPitch/SpringArm3D/Camera3D");
	}

	public override void _Input(InputEvent @event)
	{
		if (@event is InputEventMouseMotion mouseMotion)
		{
			YawCamera += -mouseMotion.Relative.X * CameraSensitivity;
			PitchCamera += mouseMotion.Relative.Y * CameraSensitivity;
		}
	}

	public override void _PhysicsProcess(double delta)
	{
		CameraControllProcess((float)delta);
	}

	public void CameraControllProcess(float delta)
	{
		float PitchCameraMax = 75.0f;
		float PitchCameraMin = -55.0f;

		PitchCamera = Mathf.Clamp(PitchCamera, PitchCameraMin, PitchCameraMax);

		YawCamera = Mathf.Lerp(YawNode.RotationDegrees.Y, YawCamera, CameraAcceleration * (float)delta);
		PitchCamera = Mathf.Lerp(PitchNode.RotationDegrees.X, PitchCamera, CameraAcceleration * (float)delta);

		YawNode.RotationDegrees = new Godot.Vector3(0, YawCamera, 0);
		PitchNode.RotationDegrees = new Godot.Vector3(PitchCamera, 0, 0);

		EmitSignal(nameof(SetCamRotation), YawNode.Rotation.Y);
	}

	public void _SetMovementState(MovementState movementState)
	{
		if (tween != null)
    {
        tween.Kill();
    }

    tween = CreateTween();
    tween.TweenProperty(camera3D, "fov", movementState.CameraFov, 0.5).SetTrans(Tween.TransitionType.Sine).SetEase(Tween.EaseType.Out);
	}

}

