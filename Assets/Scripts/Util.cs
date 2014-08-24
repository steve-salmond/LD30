using UnityEngine;
using System.Collections;

public class Util  
{

	/** Utility method for generating a random vector within a value range. */
	public static Vector3 RandomInRange(Vector3 a, Vector3 b)
	{
		float x = Random.Range(a.x, b.x);
		float y = Random.Range(a.y, b.y);
		float z = Random.Range(a.z, b.z);
		return new Vector3(x, y, z);
	}

}
