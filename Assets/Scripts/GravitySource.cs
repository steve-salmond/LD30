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


	/** The source's mesh. */
	public Mesh Mesh
	{ get; private set; }


	// Unity Methods
	// -----------------------------------------------------
	
	/** Preinitialization. */
	void Awake() 
	{ 
		// Look for a mesh on this source.
		MeshFilter m = GetComponent<MeshFilter>();
		Mesh = m ? m.mesh : null;

		// Only register ourselves as a source if we have a mesh to work with.
		if (Mesh != null)
			GravityManager.Instance.Register(this); 
	}

}
