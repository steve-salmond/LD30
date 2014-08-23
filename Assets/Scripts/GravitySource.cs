using UnityEngine;

/**
 * Imbues a game object with gravitational attraction.
 * Registers itself with the GravityManager, which handles the 
 * gravity force computations.
 */

public class GravitySource : MonoBehaviour 
{

	// Properties
	// -----------------------------------------------------

	/** Strength of the gravitational field. */
	public float Strength;


	// Unity Methods
	// -----------------------------------------------------
	
	/** Preinitialization. */
	void Awake() 
	{ GravityManager.Instance.Register(this); }

}
