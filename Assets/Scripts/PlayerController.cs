using UnityEngine;
using System.Collections;

public class PlayerController : Singleton<PlayerController> {

	// Properties
	// -----------------------------------------------------

	/** Speed at which player can move when grounded. */
	public float GroundedSpeed;

	/** How fast player can move when airborne. */
	public float AirborneSpeed;

	/** Speed at which player can jump. */
	public float JumpSpeed;

	/** Layer mask for determining what constitutes solid ground. */
	public LayerMask GroundMask;

	/** How close player has to be to a solid surface to be considered 'grounded'. */
	public float GroundedThreshold;

	/** 
	 * Player's upright transform. 
	 * This stays oriented perpendicular to the current gravity vector.
	 */
	public Transform Upright;


	// Accessors
	// -----------------------------------------------------

	/** Whether player is currently grounded. */
	public bool Grounded
	{ get; private set; }

	/** Player's current health. */
	public float Health
	{ get; private set; }

	/** Player's health fraction [0..1] */
	public float HealthFactor
	{ get { return Mathf.Clamp01(Health / 100); } } 

	/** Whether the player is alive. */
	public bool Alive
	{ get { return Health > 0; } }

	/** Whether the player is dead. */
	public bool Dead
	{ get { return Health <= 0; } }


	// Members
	// -----------------------------------------------------
	
	/** Cached transform. */
	private Transform t;

	/** Reference up vector. */
	private Vector3 up = Vector3.up;

	/** Reference up vector velocity. */
	private Vector3 upVelocity = Vector3.zero;


	// Unity Methods
	// -----------------------------------------------------

	/** Initialization. */
	void Awake()
	{
		Application.targetFrameRate = 60;
		t = transform;
		Health = 100;
	}

	/** Update. */
	void Update()
	{
		// Go fullscreen on enter.
		if (Input.GetKeyDown(KeyCode.Return))
			Screen.fullScreen = !Screen.fullScreen;
	}

	/** Physics update. */
	void FixedUpdate() 
	{
		// Update up vector.
		UpdateUp();

		// Update player's control inputs.
		if (Alive)
			UpdateInput();

		// Update player's orientation.
		Vector3 lookat = Upright.forward;
		Vector3 right = Vector3.Cross(up, lookat);
		Vector3 forward = Vector3.Cross(right, up);
		Upright.rotation = Quaternion.LookRotation(forward, up);

		// Update player's grounded status.
		Ray ray = new Ray(t.position, -up);
		Grounded = Physics.Raycast(ray, GroundedThreshold, GroundMask);
	}



	// Private Methods
	// -----------------------------------------------------

	/** Computes a instantaneous target reference up vector. */
	private Vector3 GetTargetUp()
	{
		// Get a gravity vector at player's current location.
		Vector3 g = GravityManager.Instance.ForceAt(t.position);
		Debug.DrawRay(t.position, g, Color.cyan);

		// Check for zero gravity.
		if (g.sqrMagnitude == 0)
			return Vector3.up;
		
		// Convert that to a target up vector.
		Vector3 target = -g.normalized;
		return target;
	}

	/** Updates the player's reference up vector. */
	private void UpdateUp()
	{
		// Update the smoothed reference up vector.
		Vector3 target = GetTargetUp();
		up = Vector3.SmoothDamp(up, target, ref upVelocity, 0.5f);
		up = up.normalized;
	}
	
	/** Updates player inputs. */
	private void UpdateInput()
	{
		// Framerate correction factor.
		float dt = Mathf.Clamp(Time.deltaTime * Application.targetFrameRate, 0.8f, 1.2f);
		
		// Get movement axes.
		Transform tCam = Camera.main.transform;
		Vector3 strafe = Vector3.Cross(up, tCam.forward);
		Vector3 move = tCam.forward;
		
		// Apply movement inputs.
		float speed = Grounded ? GroundedSpeed : AirborneSpeed;
		float dx = Input.GetAxis("Horizontal");
		float dy = Input.GetAxis("Vertical");
		Vector3 f = (move * dy + strafe * dx).normalized;
		rigidbody.AddForce(f * speed * dt);
		Debug.DrawRay(rigidbody.position, f, Color.yellow);
		
		// Apply jump.
		if (Input.GetButton("Jump") && Grounded)
			rigidbody.AddForce(up * JumpSpeed * dt);
	}
}
