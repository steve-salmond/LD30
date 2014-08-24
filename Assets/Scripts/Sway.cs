using UnityEngine;
using System.Collections;

public class Sway : MonoBehaviour 
{

	public Vector3 MinRotate;
	public Vector3 MaxRotate;
	public float MinInterval = 1;
	public float MaxInterval = 2;
	public float Speed = 1;


	private Quaternion initial;
	private Quaternion target;
	private Transform t;
	private float nextTargetUpdate = 0;

	// Use this for initialization
	void Start() 
	{
		t = transform;
		initial = t.localRotation;
		target = initial;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (Time.time > nextTargetUpdate)
		{
			Vector3 angles = Util.RandomInRange(MinRotate, MaxRotate);
			target = initial * Quaternion.Euler(angles);
			float interval = Random.Range(MinInterval, MaxInterval);
			nextTargetUpdate = Time.time + interval;
		}
	
		float step = Speed * Time.deltaTime;
		t.localRotation = Quaternion.RotateTowards(t.localRotation, target, step);
	}
}
