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

	/** Vignetting script. */
	private Vignetting vignette;


	
	// Unity Methods
	// -----------------------------------------------------
	
	/** Preinitialization. */
	void Awake()
	{ 
		t = transform; 
		vignette = GetComponent<Vignetting>();
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

		// Try to use XBox controller inputs if possible.
		float lx = Input.GetAxis("Look X");
		float ly = Input.GetAxis("Look Y");
		float lookX = (Mathf.Abs(lx) > 0.05f) ? lx : Input.GetAxis("Mouse X"); 
		float lookY = (Mathf.Abs(ly) > 0.05f) ? ly : Input.GetAxis("Mouse Y");

		// Look up and down.
		Vector3 camAngles = t.localEulerAngles;
		if (camAngles.x > 180)
			camAngles.x -= 360;
		float camRotY = camAngles.x - lookY * Sensitivity.y * dt;
		float camRotX = camAngles.y + lookX * Sensitivity.x * dt;

		camRotX += Random.Range(-shake, shake);
		camRotY += Random.Range(-shake, shake);
		
		camRotY = Mathf.Clamp(camRotY, -85, 85);
		t.localEulerAngles = new Vector3(camRotY, camRotX, 0) ;
	}

	// Public Methods
	// -----------------------------------------------------

	public void FadeIn()
	{ StartCoroutine(FadeInRoutine()); }

	public void FadeOut()
	{ StartCoroutine(FadeOutRoutine()); }

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

	/** Fade in from black. */
	private IEnumerator FadeInRoutine()
	{
		if (!vignette)
			yield return null;

		float start = Time.time;
		float end = start + 3;
		while (Time.time < end)
		{
			float f = (Time.time - start) / (end - start);
			vignette.intensity = Mathf.Lerp(30, 5, f);
			vignette.blur = Mathf.Lerp(5, 0.5f, f);
			vignette.blurSpread = Mathf.Lerp(2, 0.75f, f);
			yield return new WaitForEndOfFrame();
		}
	}

	/** Fade out to black. */
	private IEnumerator FadeOutRoutine()
	{
		if (!vignette)
			yield return null;
		
		float start = Time.time;
		float end = start + 3;
		while (Time.time < end)
		{
			float f = 1 - (Time.time - start) / (end - start);
			vignette.intensity = Mathf.Lerp(30, 5, f);
			vignette.blur = Mathf.Lerp(5, 0.5f, f);
			vignette.blurSpread = Mathf.Lerp(2, 0.75f, f);
			yield return new WaitForEndOfFrame();
		}
	}

}
