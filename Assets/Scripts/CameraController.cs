using UnityEngine;
using System.Collections;

public class CameraController : Singleton<CameraController>
{

	// Properties
	// -----------------------------------------------------
	
	/** Mouse sensitivity. */
	public Vector2 Sensitivity;
	
	
	// Members
	// -----------------------------------------------------
	
	/** Cached transform. */
	private Transform t;

	/** Shake amount. */
	private float shake = 0;

	
	// Unity Methods
	// -----------------------------------------------------
	
	/** Preinitialization. */
	void Awake()
	{ 
		t = transform; 
	}
	
	/** Update the camera's orientation. */
	void LateUpdate() 
	{
		// Framerate correction factor.
		float dt = Mathf.Clamp(Time.deltaTime * Application.targetFrameRate, 0.8f, 1.2f);
		
		// Lock cursor when playing.
		bool alive = PlayerController.Instance.Alive;
		Screen.lockCursor = alive;
		if (!alive)
			return;
		
		// Look up and down.
		Vector3 camAngles = t.localEulerAngles;
		if (camAngles.x > 180)
			camAngles.x -= 360;
		float camRotY = camAngles.x - Input.GetAxis("Mouse Y") * Sensitivity.y * dt;
		float camRotX = camAngles.y + Input.GetAxis("Mouse X") * Sensitivity.x * dt;

		camRotX += Random.Range(-shake, shake);
		camRotY += Random.Range(-shake, shake);
		
		camRotY = Mathf.Clamp(camRotY, -85, 85);
		t.localEulerAngles = new Vector3(camRotY, camRotX, 0) ;
	}

	// Public Methods
	// -----------------------------------------------------

	/** Shake the camera. */
	public void Shake(Vector3 p, float strength, float duration)
	{
		StartCoroutine(Shaker(p, strength, duration));
	}

	/** Shakes the camera. */
	private IEnumerator Shaker(Vector3 p, float strength, float duration)
	{
		float start = Time.time;
		float end = start + duration;
		while (Time.time < end)
		{
			float f = (Time.time - start) / (end - start);
			float d = Vector3.Distance(p, t.position);
			shake = strength * (1 - f) * (1 / (d + 1));
			yield return new WaitForEndOfFrame();
		}
	}

}
