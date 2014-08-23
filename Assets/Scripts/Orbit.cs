using UnityEngine;
using System.Collections;

public class Orbit : MonoBehaviour 
{

	// Properties
	// -----------------------------------------------------

	/** Axis of rotation. */
	public Vector3 Axis;

	/** Rotation speed (degrees per second). */
	public float Speed;


	// Members
	// -----------------------------------------------------

	/** Cached transform. */
	private Transform t;

	// Use this for initialization
	void Start() 
	{
		t = transform;
	}
	
	// Update is called once per frame
	void FixedUpdate() 
	{
		t.Rotate (Axis, Speed * Time.deltaTime, Space.Self);
	}
}
