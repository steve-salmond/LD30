using UnityEngine;
using System.Collections;

public class Planet : MonoBehaviour 
{
	/** The orbital body that drives this planet. */
	public OrbitalBody Body;


	/** Update the physics frame of reference when player hits this planet. */
	void OnCollisionEnter(Collision collision)
	{
		if (collision.collider.tag == "Player")
			OrbitalManager.Instance.SetOrigin(Body, collision);
	}


	/** Update planet position to match the orbital body. */
	void LateUpdate()
	{
		// Get orientations of this body and the origin.
		Transform tBody = Body.transform;
		Transform tOrigin = OrbitalManager.Instance.Origin.transform;

		// Project into origin's reference frame.
		Vector3 p = tOrigin.InverseTransformPoint(tBody.position);
		transform.position = p;
	}

}
