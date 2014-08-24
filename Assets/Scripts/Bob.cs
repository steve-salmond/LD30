using UnityEngine;
using System.Collections;

public class Bob : MonoBehaviour {

	public Vector3 Offset;

	public float Speed;

	private Vector3 initial;

	void Start()
	{
		initial = transform.localPosition;
	}

	void Update() 
	{
		transform.localPosition = initial + Offset * Mathf.Sin(Time.time * Speed);
	}
}
