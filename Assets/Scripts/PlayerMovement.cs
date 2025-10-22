using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
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
	}

	void OnDrawGizmosSelected()
	{
		if (groundCheck == null) return;
		Gizmos.color = Color.yellow;
		Gizmos.DrawWireSphere(groundCheck.position, groundRadius);
	}

	public void ChangeGravityDirection(int direction)
	{
		switch (direction)
		{
			case 0:
				Physics2D.gravity = new Vector2(3 * 9.8f, 0);
				characterCollider.size = new Vector2(1.25f, .9f);
				groundCheck.localPosition = new Vector3(.63f, 0, 0);
				sprite.rotation = Quaternion.Euler(0, 0, 90);
				gravityDirection = 0;
				break;
			case 1:
				Physics2D.gravity = new Vector2(3 * -9.8f, 0);
				characterCollider.size = new Vector2(1.25f, .9f);
				groundCheck.localPosition = new Vector3(-.63f, 0, 0);
				sprite.rotation = Quaternion.Euler(0, 0, -90);
				gravityDirection = 1;
				break;
			case 2:
				Physics2D.gravity = new Vector2(0, 3 * 9.8f);
				characterCollider.size = new Vector2(.9f, 1.25f);
				groundCheck.localPosition = new Vector3(0, .63f, 0);
				sprite.rotation = Quaternion.Euler(0, 0, 180);
				gravityDirection = 2;
				break;
			case 3:
				Physics2D.gravity = new Vector2(0, 3 * -9.8f);
				characterCollider.size = new Vector2(.9f, 1.25f);
				groundCheck.localPosition = new Vector3(0, -.63f, 0);
				sprite.rotation = Quaternion.Euler(0, 0, 0);
				gravityDirection = 3;
				break;

		}
	}
}