using System.Collections.Generic;
using SimVik;
using UnityEngine;

public class Movement : MonoBehaviour
{
	Vector3 moveDirection;

	[Header("Movement")]
	public float maxMoveSpeed = 5;
	public float acceleration = 10;
	public float rotateSpeed = 720;
	Vector3 velocity;
	Rigidbody rb;

	void Awake()
	{
		rb = GetComponent<Rigidbody>();

	}


	void Update()
	{
		// movement
		velocity = Vector3.MoveTowards( velocity, moveDirection * maxMoveSpeed, acceleration * Time.deltaTime);
		velocity = Vector3.ClampMagnitude(velocity, maxMoveSpeed);
		rb.velocity = velocity;
		rb.angularVelocity = Vector3.zero;
		// rotate to face direction of movement
		if (moveDirection != Vector3.zero)
		{
			transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(moveDirection), rotateSpeed * Time.deltaTime);
		}
	}

	public void Move( Vector2 direction)
	{
		moveDirection = new Vector3(direction.x, 0, direction.y);
	}


	// on trigger enter add tool to available tools list
 }