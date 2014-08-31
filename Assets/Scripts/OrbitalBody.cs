using UnityEngine;
using System.Collections;

/** Drives another game object using basic orbital mechanics. */

public class OrbitalBody : MonoBehaviour 
{

	/** Axis of orbit. */
	public Vector3 OrbitalAxis;

	/** Orbital radius. */
	public float OrbitalRadius;

	/** Orbital speed, in degrees / second. */
	public float OrbitalSpeed;

	/** Axis of spin. */
	public Vector3 SpinAxis;

	/** Speed of spin, in degrees / second. */
	public float SpinSpeed;

	/** The transform of this body. */
	private Transform t;



	/** Preinitialization. */
	void Awake()
	{
		t = transform;
	}

	/** Update this body's orbital motion. */
	void FixedUpdate()
	{
		t.localPosition = Quaternion.AngleAxis(OrbitalSpeed * Time.fixedTime, OrbitalAxis) * (Vector3.forward * OrbitalRadius);
		// t.localRotation = Quaternion.AngleAxis(SpinSpeed * Time.fixedTime, SpinAxis);
	}

}
