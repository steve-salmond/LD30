using UnityEngine;
using System.Collections.Generic;

public class OrbitalManager : Singleton<OrbitalManager> 
{

	/** The set of known orbital bodies. */
	public List<OrbitalBody> Bodies;

	/** The orbital body that defines the physics frame of reference. */
	public OrbitalBody Origin;


	/** Preinitialization. */
	void Awake()
	{
		Bodies = new List<OrbitalBody>(FindObjectsOfType<OrbitalBody>());
	}

	/** Set an orbital body as the physics origin. */
	public void SetOrigin(OrbitalBody body, Collision collision)
	{
		if (body == Origin)
			return;

		Origin = body;
	}

}
