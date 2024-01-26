using System.Collections.Generic;
using SimVik;
using UnityEngine;

public class Character : MonoBehaviour
{
	public Tool currentTool;
	Vector3 moveDirection;

	[Header("Movement")]
	public float maxMoveSpeed = 5;
	public float acceleration = 10;
	public float rotateSpeed = 720;
	Vector3 velocity;

	[Header("Hitting")]
	public float hitCooldown = 0.5f;
	public float hitCooldownLeft;
	public GameObject hitCollider;
	public AudioClip swooshSound;



	void Update()
	{
		// movement
		velocity = Vector3.MoveTowards( velocity, moveDirection * maxMoveSpeed, acceleration * Time.deltaTime);
		velocity = Vector3.ClampMagnitude(velocity, maxMoveSpeed);

		transform.position += velocity * Time.deltaTime;
		// rotate to face direction of movement
		if (moveDirection != Vector3.zero)
		{
			transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(moveDirection), rotateSpeed * Time.deltaTime);
		}

		hitCooldownLeft -= Time.deltaTime;
	}

	public void Move( Vector2 direction)
	{
		moveDirection = new Vector3(direction.x, 0, direction.y);
	}

	public void Use()
	{
		if (currentTool == null)
		{
			 Hit();
		}
		else
		{
			currentTool.Use(this);
		}
	}

	public async void Hit()
	{
		if (hitCooldownLeft > 0) return;
		hitCooldownLeft = hitCooldown;


		hitCollider.SetActive(true);
		await new WaitForSeconds(0.1f);
		hitCollider.SetActive(false);
		swooshSound.Play();
	}

	public void Drop()
	{
		print("trying Dropping");
		if (currentTool == null) return;
		currentTool.Drop(this);
		currentTool = null;
	}

	// on trigger enter add tool to available tools list
 }