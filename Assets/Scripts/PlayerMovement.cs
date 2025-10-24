using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
	private PlayerAnimator playerAnimator;  
    private bool wasGrounded;                
	[Header("movement")] [SerializeField] float moveSpeed = 8f;
	[SerializeField] float jumpForce = 12f;

	[Header("ground check")] [SerializeField]
	Transform groundCheck;

	[SerializeField] float groundRadius = 0.15f;
	[SerializeField] LayerMask groundLayer;

	[Header("jump feel")] [SerializeField] float jumpCutMultiplier = 0.5f;

	Rigidbody2D rb;
	float moveInput;
	private float moveInputUp;
	bool isGrounded;

	[Header("References")]
	[SerializeField] BoxCollider2D characterCollider;
	[SerializeField] private Transform sprite;
	private int gravityDirection = 3;

	void Awake()
	{
		rb = GetComponent<Rigidbody2D>();
		playerAnimator = GetComponent<PlayerAnimator>();
		
	}

	void Update()
	{
		moveInput = Input.GetAxisRaw("Horizontal");
		moveInputUp = Input.GetAxisRaw("Vertical");

		if (Input.GetButtonDown("Jump") && isGrounded)
		{
			switch (gravityDirection)
			{
				case 0:
					rb.linearVelocity = new Vector2(-jumpForce, rb.linearVelocity.y);
					break;
				case 1:
					rb.linearVelocity = new Vector2(jumpForce, rb.linearVelocity.y);
					break;
				case 2:
					rb.linearVelocity = new Vector2(rb.linearVelocity.x, -jumpForce);
					break;
				case 3:
					rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
					break;
			}
			if (playerAnimator) playerAnimator.startedJumping = true;
			AudioManager.instance.PlayJumpSound();
		}

		if (Input.GetButtonUp("Jump") && rb.linearVelocity.y > 0)
		{
			rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * jumpCutMultiplier);
		}

		if (moveInput != 0)
		{
			Vector3 s = transform.localScale;
			s.x = Mathf.Abs(s.x) * Mathf.Sign(moveInput);
			transform.localScale = s;
		}
	}

	void FixedUpdate()
	{
		switch (gravityDirection)
		{
			case 0:
				rb.linearVelocity = new Vector2(rb.linearVelocity.x, moveInputUp * moveSpeed);
				break;
			case 1:
				rb.linearVelocity = new Vector2(rb.linearVelocity.x, moveInputUp * moveSpeed);
				break;
			default:
				rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);
				break;
		}

		isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundRadius, groundLayer);
		if (isGrounded && !wasGrounded && playerAnimator) playerAnimator.justLanded = true;
		wasGrounded = isGrounded;
	}

	void OnDrawGizmosSelected()
	{
		if (groundCheck == null) return;
		Gizmos.color = Color.yellow;
		Gizmos.DrawWireSphere(groundCheck.position, groundRadius);
	}

	public void ChangeGravityDirection(int direction)
	{
		AudioManager.instance.PlayGravityChangeSound();
		switch (direction)
		{
			case 0:
				Debug.Log("Gravity Changing to Right");
				Physics2D.gravity = new Vector2(3 * 9.8f, 0);
				characterCollider.size = new Vector2(1.25f, .9f);
				groundCheck.localPosition = new Vector3(.63f, 0, 0);
				sprite.rotation = Quaternion.Euler(0, 0, 90);
				gravityDirection = 0;
				break;
			case 1:
				Debug.Log("Gravity Changing to Left");
				Physics2D.gravity = new Vector2(3 * -9.8f, 0);
				characterCollider.size = new Vector2(1.25f, .9f);
				groundCheck.localPosition = new Vector3(-.63f, 0, 0);
				sprite.rotation = Quaternion.Euler(0, 0, -90);
				gravityDirection = 1;
				break;
			case 2:
				Debug.Log("Gravity Changing to Up");
				Physics2D.gravity = new Vector2(0, 3 * 9.8f);
				characterCollider.size = new Vector2(.9f, 1.25f);
				groundCheck.localPosition = new Vector3(0, .63f, 0);
				sprite.rotation = Quaternion.Euler(0, 0, 180);
				gravityDirection = 2;
				break;
			case 3:
				Debug.Log("Gravity Changing to Down");
				Physics2D.gravity = new Vector2(0, 3 * -9.8f);
				characterCollider.size = new Vector2(.9f, 1.25f);
				groundCheck.localPosition = new Vector3(0, -.63f, 0);
				sprite.rotation = Quaternion.Euler(0, 0, 0);
				gravityDirection = 3;
				break;

		}
	}

	public void RandomGravitySwitch(bool swap)
	{
		if (swap)
		{
			StartCoroutine(RandomGravityCR());
		}
		else
		{
			Debug.Log("Random Gravity End Early");
			StopCoroutine("RandomGravityCR");
		} 
			
	}

	IEnumerator RandomGravityCR()
	{
		while (true)
		{
			yield return new WaitForSeconds(3f);
			ChangeGravityDirection(Random.Range(0, 4));
		}
	}
}