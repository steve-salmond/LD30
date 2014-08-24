using UnityEngine;
using System.Collections;

public class UIScale : MonoBehaviour {

	public float FixedWidth = 1024;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		float s = Screen.width / FixedWidth;
		transform.localScale = Vector3.one * s;
	}
}
