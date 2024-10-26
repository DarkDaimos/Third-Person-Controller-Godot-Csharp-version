using Godot;
using System;

[GlobalClass]
public partial class JumpState : Resource
{
	[Export] public string AnimationName;
	[Export] public float JumpHeight = 4.0f;
	[Export] public float ApexDuraction = 0.5f;
}
