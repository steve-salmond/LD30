using UnityEngine;
using System.Collections;

public class DisableOnAwake : MonoBehaviour {

	// Use this for initialization
	void Awake () {
		gameObject.SetActive(false);	
	}
}
