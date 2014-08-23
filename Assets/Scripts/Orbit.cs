﻿using UnityEngine;

/**
 * Rotates a game object about the given local axis with a set speed.
 */

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


	// Unity Methods
	// -----------------------------------------------------

	/** Initialization. */
	void Start() 
	{ t = transform; }
	
	/** Physics update. */
	void FixedUpdate() 
	{ t.Rotate (Axis, Speed * Time.deltaTime, Space.Self); }

}
