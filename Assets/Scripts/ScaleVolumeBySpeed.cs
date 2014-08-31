using UnityEngine;
using System.Collections;

public class ScaleVolumeBySpeed : MonoBehaviour {

	public float MinVolume = 0;
	public float MaxVolume = 1;

	public float MinSpeed = 0;
	public float MaxSpeed = 0;

	public float SmoothTime = 1;

	public Rigidbody Body;


	private float velocity;

	// Update is called once per frame
	void Update () 
	{
		float s = Body.velocity.magnitude;
		float r = MaxSpeed - MinSpeed;
		float f = Mathf.Clamp01((s - MinSpeed) / r);
		float target = Mathf.Lerp(MinVolume, MaxVolume, f);
		audio.volume = Mathf.SmoothDamp(audio.volume, target, ref velocity, SmoothTime);
	}
}
