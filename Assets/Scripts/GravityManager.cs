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
	public Vector3 ForceAt(Vector3 point)
	{
		// Determine closest surface point/normal amongst all 
		// registered sources of gravity.
		GravitySource sClosest = null;
		float dClosest = float.MaxValue;
		int iClosest = -1;

		// Iterate over each source, looking for the closest point.
		foreach (GravitySource source in sources)
		{
			Mesh mesh = source.Mesh;
			Transform t = source.transform;
			Vector3 local = t.InverseTransformPoint(point);
			Vector3[] vertices = mesh.vertices;
			Vector3[] normals = mesh.normals;
			int n = vertices.Length;
			for (int i = 0; i < n; i++)
			{
				float d = Vector3.Distance(local, vertices[i]);
				if (d < dClosest)
				{
					dClosest = d;
					iClosest = i;
					sClosest = source;
				}
			}
		}

		// Compute the current gravity vector.
		if (sClosest != null)
		{
			Mesh mesh = sClosest.Mesh;
			Vector3 nLocal = mesh.normals[iClosest];
			Vector3 n = sClosest.transform.TransformDirection(nLocal);
			return -n * sClosest.Strength;
		}
		else
			return Vector3.zero;
	}

}
