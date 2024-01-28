using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

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
	public GameObject fartParticles;
	public GameObject poopPrefab;
	public GameObject poopStain;

	[Header("Approval")] 
	public float approval = 2;
	public float disapproval = -2;

	public TextMeshProUGUI fartText;
	public int fartAmmo = 0;
	
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
		if (velocity.magnitude > maxMoveSpeed)
		{
			velocity *= 0.97f;
		}
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
		
		Invoke(nameof(GetUp), 3);

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

	public bool CanFart() => fartCooldownLeft <= 0 && fartAmmo > 0;

	public void Fart()
	{
		if(!CanFart())return;

		fartCooldownLeft = fartCooldown;
		OnFart.Invoke();
		fartAmmo--;

		velocity += transform.forward * 30;
		var obj = Instantiate( fartParticles, butt.position, Quaternion.identity);
		obj.transform.forward = -transform.forward;
		obj.transform.name = transform.name + " fart";
		Destroy( obj.GetComponent<Collider>(),0.5f);

		var poopChance = Random.Range(0, 100);
		if( poopChance < 50)
		{
			var poop = Instantiate(poopPrefab, butt.position, butt.rotation);
			poop.GetComponent<Rigidbody>().AddForce(-transform.forward * 2, ForceMode.Impulse);
		}
		var stainChance = Random.Range(0, 100);
		if( stainChance < 50)
		{
			var pos = transform.position;
			pos.y = -0.47f;
			var rot = Quaternion.Euler(90, Random.Range(0, 360), 0);
			var stain = Instantiate(poopStain, pos, rot);
			stain.transform.localScale = Vector3.one * Random.Range(0.75f, 1.25f);
		}

		fartText.text = fartAmmo.ToString();
		fartText.transform.DOLocalRotate( Vector3.forward * 360, 0.5f, RotateMode.FastBeyond360);
	}
	
	// on trigger enter add tool to available tools list

	private void OnCollisionEnter(Collision other)
	{
		if (other.transform.CompareTag("Food"))
		{
			fartAmmo += 3;
			fartText.text = fartAmmo.ToString();
			Destroy(other.gameObject);
		}
	}

	void OnTriggerEnter(Collider other)
	{
		if(other.CompareTag("Fart") && other.transform.name != transform.name + " fart")
		{
			// get approval from audience
			AudienceManager.Instance.SetApproval(transform, approval);

			// get dazed
			GetDazed();

			// destroy trigger
			Destroy(other.gameObject.GetComponent(typeof(Collider)));
		}
	}
}