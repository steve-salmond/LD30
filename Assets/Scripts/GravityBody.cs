using UnityEngine;


 /** 
  * A body that is affected by gravity.
  * Uses GravityManager to compute gravity force vectors.
  */

public class GravityBody : MonoBehaviour 
{
	// Members
	// -----------------------------------------------------
	
	/** Cached rigid body. */
	private Rigidbody r;

	// Unity Methods
	// -----------------------------------------------------

	/** Preinitialization. */
	void Awake()
	{ r = rigidbody; }

	/** Physics update. */
	void FixedUpdate() 
	{ 
		// Ask gravity manager for the force at our current location.
		Vector3 f = GravityManager.Instance.ForceAt(r.position);

		// Accumulate gravity in with other forces.
		r.AddForce(f);
	}
}
