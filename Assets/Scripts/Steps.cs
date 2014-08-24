using UnityEngine;
using System.Collections;

public class Steps : MonoBehaviour 
{
	
	public AudioClip[] StepSounds;
	
	public float MinSpeed = 0.1f;

	public float MaxSpeed = 10f;

	public float MinDistance = 1;

	public float MaxDistance = 3;
	
	private Vector3 lastStepPos;
	
	/** Initialization. */
	void Start() 
	{
		PlayerController player = PlayerController.Instance;
		lastStepPos = player.transform.position;
	}
	
	/** Update. */
	void Update()
	{
		// Can only make steps on ground.
		PlayerController player = PlayerController.Instance;
		if (!player.Grounded)
			return;
		
		// Determine distance between steps based on speed.
		float speed = player.rigidbody.velocity.magnitude;
		float t = Mathf.Clamp((speed - MinSpeed) / (MaxSpeed - MinSpeed), 0, 1);
		float d = Mathf.Lerp(MinDistance, MaxDistance, t);
		
		// Check if distance threshold has been exceeded.
		if (Vector3.Distance(player.transform.position, lastStepPos) > d)
		{
			Step();
			lastStepPos = player.transform.position;
		}
		
	}
	
	/** Play a footstep sound. */
	private void Step()
	{
		int n = StepSounds.Length;
		int i = Random.Range(0, n);
		AudioClip sound = StepSounds[i];
		if (sound != null && gameObject.activeSelf)
			audio.PlayOneShot(sound);
	}
}
