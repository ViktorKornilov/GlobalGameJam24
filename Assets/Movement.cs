using System.Collections.Generic;
using DG.Tweening;
using SimVik;
using UnityEngine;

public class Movement : MonoBehaviour
{
	
	public Transform enemy;
	public float hitDistance = 3;
	
	public bool bot = true;
	Vector3 moveDirection;

	public Animator animator;
	
	[Header("Movement")]
	public float maxMoveSpeed = 5;
	public float acceleration = 10;
	public float rotateSpeed = 720;
	Vector3 velocity;
	Rigidbody rb;
	
	[Header("Hitting")]
	public float hitCooldown = 1f;
	public float hitCooldownLeft;
	public float fartCooldown = 1f;
	public float fartCooldownLeft;
	public GameObject hitCollider;
	public AudioClip swooshSound;
	public Transform butt;

	[Header("Approval")] 
	public float approval = 2;
	public float disapproval = -2;
	
	public bool canMove = true;

	void Awake()
	{
		rb = GetComponent<Rigidbody>();

	}


	void Update()
	{
		if (!canMove) return;
		
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
		
		if(hitCooldownLeft > 0) hitCooldownLeft -= Time.deltaTime;
		if(fartCooldownLeft > 0) fartCooldownLeft -= Time.deltaTime;
	}

	public void Move( Vector2 direction)
	{
		moveDirection = new Vector3(direction.x, 0, direction.y);
	}

	public void GetDazed()
	{
		canMove = false;
		
		if(animator != null)
			animator.state = AnimationState.Dazed;
		
		Invoke(nameof(GetUp), 5);
		
		AudienceManager.Instance.SetApproval(transform, disapproval);
	}

	void GetUp()
	{
		if(animator != null)
			animator.state = AnimationState.Active;
		canMove = true;
	}

	public void Hit()
	{
		if (hitCooldownLeft > 0)
		{
			return;
		}
		
		hitCooldownLeft = hitCooldown;


		//hitCollider.SetActive(true);
		//await new WaitForSeconds(0.1f);
		//hitCollider.SetActive(false);
		//swooshSound.Play();
		
		if (Vector3.Distance(transform.position, enemy.position) < hitDistance)
		{
			enemy.GetComponent<Movement>().GetDazed();
		}
	}

	public void Fart()
	{
		if(fartCooldownLeft > 0) return;
		
		fartCooldownLeft = fartCooldown;
		
		var ray = new Ray(transform.position, transform.forward);
		if (Physics.Raycast(ray, out var hit, 2))
		{
			transform.DOLookAt(-hit.point, 0.1f).OnComplete(() => animator.Fart());
			//ransform.DORotate(Vector3.up * 180, 0.1f).OnComplete(() => animator.Fart());
			
			AudienceManager.Instance.SetApproval(transform, approval);
		}
		else
		{
			animator.Fart();
			AudienceManager.Instance.SetApproval(transform, disapproval);
		}
	}
	
	// on trigger enter add tool to available tools list
 }