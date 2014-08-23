using UnityEngine;
using System.Collections.Generic;

/**
 * GravityManager manages a set of gravitational sources,
 * and can give you a gravity force vector at any point in the scene.
 */

public class GravityManager : Singleton<GravityManager> 
{

	// Members
	// -----------------------------------------------------

	/** The set of gravitational sources in the scene. */
	private List<GravitySource> sources = new List<GravitySource>();


	// Public Methods
	// -----------------------------------------------------

	/** Register a gravitational source. */
	public void Register(GravitySource source)
	{ sources.Add(source); }

	/** Return a gravity force vector, given a point in world space. */
	public Vector3 ForceAt(Vector3 position)
	{
		// TODO: Implement.
		return Vector3.zero;
	}


	
}
