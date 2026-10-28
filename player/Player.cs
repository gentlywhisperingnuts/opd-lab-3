using Godot;
using System;

public partial class Player : CharacterBody2D
{
	[Export]
	private float Gravity = 980f;
	[Export]
	public float JumpVelocity { get; set; } = -800f;
	[Export]
	public float ReloadTime { get; set; } = 0.5f;
	
	private float BulletSpeed = 1000f;
	private PackedScene BulletScene = GD.Load<PackedScene>("res://Bullet/Bullet.tscn");
	[Export]
	public float BulletDamage { get; set; } = 10f;

	public override void _Ready()
	{
	}

	private Vector2 velocity;
	public override void _PhysicsProcess(double delta)
	{
		velocity = Velocity;

		if (!IsOnFloor())
		{
			float multiplier = velocity.Y > 0.0 ? 1.5f : 1.0f;
			velocity += Gravity * (float)delta * Vector2.Down * multiplier;
		}
		if (Input.IsActionJustPressed("jump") && IsOnFloor())
			velocity = Jump(velocity);

		if (Input.IsActionJustReleased("jump") && velocity.Y < 0.0f)
			velocity.Y *= 0.1f;
		
		Velocity = velocity;
		MoveAndSlide();
	}

	public override void _Input(InputEvent @event)
	{
		if (@event.IsActionPressed("shoot"))
		{
			Shoot();
		}
	}
	private Vector2 Jump(Vector2 Velocity)
	{
		//GD.Print("Jumping");
		return new Vector2(0, JumpVelocity);
	}

	private void Shoot()
	{
		//GD.Print("Shooting");
		Bullet bullet = (Bullet)BulletScene.Instantiate();
		AddSibling(bullet);
		bullet.Position = Position + new Vector2(50, 0);
		bullet.Velocity = new Vector2(BulletSpeed, 0);
		bullet.SetDamage(BulletDamage);
	}

	private void _On_Collision(Area2D area)
	{
		if (area is DamagingObject)
		{
			GD.Print("Player hit by damaging object");
			GetTree().Quit();
		}
	}

}
