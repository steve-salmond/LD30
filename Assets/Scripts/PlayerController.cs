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

	/** Jumping sound. */
	public AudioClip JumpSound;

	/** Jump audio source. */
	public AudioSource JumpSource;

	/** Number of gems player needs to win. */
	public int GemsToWin = 7;


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

	/** How many beans player has. */
	public int BeanCounter
	{ get; private set; }

	/** How many gems player has. */
	public int GemCounter
	{ get; private set; }

	/** How much to smooth the player's up vector. */
	public float UpSmoothingFactor = 0.5f;


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

	/** Initialization. */
	void Start()
	{
		// CameraController.Instance.FadeIn();
		TitleController.Instance.FadeIn();

		SpeechManager.Instance.Say("OnceUponATime", 1);
		SpeechManager.Instance.Say("WhereAmI", 3);
		SpeechManager.Instance.Say("IsThisADream", 5);
		SpeechManager.Instance.Say("LittleDidHeKnow", 6);
		SpeechManager.Instance.Say("Adventure", 12);
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

	/** Player has picker up a bean. */
	public void PickUpBean()
	{
		// Count the beans... :)
		BeanCounter++;

		// Schedule speech.
		SpeechManager.Instance.Say("AGiantBean", 2);
		SpeechManager.Instance.Say("HmmIWonder", 4);
		SpeechManager.Instance.Say("MaybeICouldPlantIt", 6);
		SpeechManager.Instance.Say("PlantPrompt01", 8);
		SpeechManager.Instance.Say("PlantPrompt02", 12);
	}

	/** Player has sown a bean. */
	public void SowBean()
	{
		// Count the beans... :)
		BeanCounter--;

		// Key off speech lines
		SpeechManager.Instance.Say("Aah", 0);
		SpeechManager.Instance.Say("ABeanstalk", 0);
		SpeechManager.Instance.Say("ItsHuge", 1);
		SpeechManager.Instance.Say("MaybeICanClimbIt", 4);
		SpeechManager.Instance.Say("MoreBeans", 10);

		// Cancel speech that's no longer relevant.
		SpeechManager.Instance.Cancel("AGiantBean");
		SpeechManager.Instance.Cancel("HmmIWonder");
		SpeechManager.Instance.Cancel("MaybeICouldPlantIt");
		SpeechManager.Instance.Cancel("PlantPrompt01");
		SpeechManager.Instance.Cancel("PlantPrompt02");
	}

	/** Player has picked up a gem. */
	public void PickUpGem()
	{
		// Count the gems.
		GemCounter++;

		// Key off speech lines
		SpeechManager.Instance.Say("AGemStone", 2);
		SpeechManager.Instance.Say("MoreOfThem", 4);
		SpeechManager.Instance.Say("CollectThemAll", 8);
		SpeechManager.Instance.Say("ThreeShouldBeEnough", 9);
		SpeechManager.Instance.Say("MightyQuest", 10);
		SpeechManager.Instance.Say("WhyNoIdea02", 15);

		// Win condition.
		if (GemCounter >= GemsToWin)
			Win();
	}


	// Private Methods
	// -----------------------------------------------------

	/** Called when player wins the game. */
	public void Win()
	{
		EndController.Instance.FadeIn();
		SpeechManager.Instance.Say("TheEnd");
		StartCoroutine(WaitForKeypress());
	}

	private IEnumerator WaitForKeypress()
	{
		while (!Input.GetButton("Fire1"))
			yield return new WaitForEndOfFrame();

		EndController.Instance.FadeOut();
		yield return new WaitForSeconds(3);

		Application.LoadLevel(Application.loadedLevel);
	}

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
		up = Vector3.SmoothDamp(up, target, ref upVelocity, UpSmoothingFactor);
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
		{
			rigidbody.AddForce(up * JumpSpeed * dt);
			JumpSource.PlayOneShot(JumpSound, 0.15f);
		}
	}
}
