using UnityEngine;
using System.Collections.Generic;

/**
 * GravityManager can give you a gravity force vector at any point in the scene.
 */

public class GravityManager : Singleton<GravityManager> 
{

	// Properties
	// -----------------------------------------------------

	/** Maximum distance from ground at which gravity applies. */
	public float GroundMaxDistance = 100;

	/** Layer mask for determining what constitutes solid ground. */
	public LayerMask GroundLayers;

	/** Strength of the gravitational field. */
	public float Strength;

	/** Number of samples to take when estimating gravity. */
	public int Samples = 128;


	// Members
	// -----------------------------------------------------

	/** Raycast hit info. */
	private RaycastHit hit =  new RaycastHit();


	// Public Methods
	// -----------------------------------------------------

	/** Return a gravity force vector, given a point in world space. */
	public Vector3 ForceAt(Vector3 point)
	{
		// Scatter rays out randomly, looking for solid ground.
		// When we hit it, accumulate the resulting surface normal.
		Vector3 gravity = Vector3.zero;
		for (int i = 0; i < Samples; i++)
		{
			Vector3 direction = Random.onUnitSphere;
			if (Physics.Raycast(point, direction, out hit, GroundMaxDistance, GroundLayers))
				gravity -= (hit.normal * 1 / (hit.distance * hit.distance));

			/*
			{
				// Recover the gravity source.
				GravitySource source = hit.collider.gameObject.GetComponent<GravitySource>();
				if (source)
					gravity += hit.normal * source.Strength;
			}
			*/
		}

		// Return the overall gravity direction.
		return gravity.normalized * Strength;
	}

}
