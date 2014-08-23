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
	protected Transform t;
	
	
	// Unity Methods
	// -----------------------------------------------------
	
	/** Preinitialization. */
	void Awake()
	{ t = transform; }
	
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
		
		camRotY = Mathf.Clamp(camRotY, -85, 85);
		t.localEulerAngles = new Vector3(camRotY, camRotX, 0);
	}
	
}
