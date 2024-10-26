using Godot;
using System;

[GlobalClass]
public partial class MovementState : Resource
{
    [Export]
    public int Id;
    [Export]
    public float MovementSpeed;
    [Export]
    public float Acceleration = 6.0f;
    [Export]
    public float CameraFov = 75.0f;
    [Export]
    public float AnimationSpeed = 1.0f;
}
