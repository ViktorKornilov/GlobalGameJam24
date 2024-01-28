using System;
using System.Collections.Generic;
using DG.Tweening;
using SimVik;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class Movement : MonoBehaviour
{

	public static UnityEvent OnFart = new();
	public static UnityEvent onDaze = new();
	
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
	public AudioSource moveSource;
	
	[Header("Hitting")]
	public float hitCooldown = 1f;
	public float hitCooldownLeft;
	public float fartCooldown = 1f;
	public float fartCooldownLeft;
	public GameObject hitCollider;
	public AudioClip swooshSound;
	public Transform butt;
	public Color fartColor;

	[Header("Approval")] 
	public float approval = 2;
	public float disapproval = -2;

	public TextMeshProUGUI fartText;
	private int fartStorage = 0;
	
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
			moveSource.volume = 1;
			transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(moveDirection), rotateSpeed * Time.deltaTime);
		}
		else
		{
			moveSource.volume = 0;
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
		onDaze.Invoke();
		canMove = false;
		
		if(animator != null)
			animator.state = AnimationState.Dazed;
		
		Invoke(nameof(GetUp), 5);

		// dotween make mat blink to white
		GetComponentInChildren<Renderer>().material.DOColor(fartColor, 0.07f).SetLoops(6, LoopType.Yoyo);

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


		
		if (Vector3.Distance(transform.position, enemy.position) < hitDistance)
		{
			enemy.GetComponent<Movement>().GetDazed();
		}
	}

	public void Fart()
	{
		if(fartCooldownLeft > 0) return;
		
		fartCooldownLeft = fartCooldown;
		OnFart.Invoke();

        /*var ray = new Ray(transform.position, transform.forward);
		if (Physics.Raycast(ray, out var hit, 2))
		{
			transform.DOLookAt(-hit.point, 0.1f).OnComplete(() => animator.Fart());
			//ransform.DORotate(Vector3.up * 180, 0.1f).OnComplete(() => animator.Fart());
			
			AudienceManager.Instance.SetApproval(transform, approval);
			AudienceManager.Instance.Laugh();
		}
		else
		{
			animator.Fart();
			AudienceManager.Instance.SetApproval(transform, disapproval);
		}*/

        if (Vector3.Distance(transform.position, enemy.position) < hitDistance && fartStorage > 0)
        {
	        Sequence mySequence = DOTween.Sequence();

	        mySequence.Append(Camera.main.DOFieldOfView(10, 0.5f).OnComplete(()=>
                              	        {
                              		        Time.timeScale = 0.5f;
                              		        Time.fixedDeltaTime = 0.02f * Time.timeScale; 
                              	        }));
            
            mySequence.Append(transform.DOLookAt(-enemy.position, 0.1f).OnComplete(() =>
            {
	            animator.Fart();
	            enemy.GetComponent<Movement>().GetDazed();
            }));

            mySequence.Append(Camera.main.DOFieldOfView(25, 0.5f).OnComplete(()=>
            {
	            Time.timeScale = 1f;
	            Time.fixedDeltaTime = 0.02f * Time.timeScale; 
            })) ;

            mySequence.Play().OnComplete(() =>
            {
	            AudienceManager.Instance.SetApproval(transform, approval);
	            AudienceManager.Instance.Laugh();
            });

            fartText.text = (--fartStorage).ToString();

        }
		else if(fartStorage > 0)
		{
            animator.Fart();
            AudienceManager.Instance.SetApproval(transform, disapproval);
			AudienceManager.Instance.Hate();
			fartText.text = (--fartStorage).ToString();
        }
    }
	
	// on trigger enter add tool to available tools list

	private void OnCollisionEnter(Collision other)
	{
		if (other.transform.CompareTag("Food"))
		{
			fartText.text = (++fartStorage).ToString();
			Destroy(other.gameObject);
		}
	}
}