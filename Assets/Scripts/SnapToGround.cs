using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class SnapToGround : MonoBehaviour 
{
	/** Distance from ground at which snapping takes effect. */
	public float GroundMaxDistance = 5;

	/** Layer mask for determining what constitutes solid ground. */
	public LayerMask GroundLayers;

	void Awake() 
	{
		// Disable during gameplay.
		if (Application.isPlaying)
			enabled = false;
	}
	
	void Update() 
	{
		Vector3 p = transform.position;
		Vector3 d = transform.TransformDirection(Vector3.down);
		RaycastHit hit = new RaycastHit();

		if (Physics.Raycast(p, d, out hit, GroundMaxDistance, GroundLayers))
		{
			Vector3 up = hit.normal;
			Vector3 side = Vector3.Cross(up, transform.forward);
			Vector3 forward = Vector3.Cross(side, up);

			transform.position = hit.point;
			transform.LookAt(hit.point + forward, up);
		}
	}
}
