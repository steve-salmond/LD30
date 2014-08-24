using UnityEngine;
using System.Collections;

public class Bean : MonoBehaviour {

	public AudioSource Source; 

	private bool PickedUp = false;

	void OnTriggerEnter(Collider other) 
	{
		if (PickedUp)
			return;

		// Tell player they've picked up a bean.
		PlayerController.Instance.PickUpBean();
		PickedUp = true;

		// Play pickup sound.
		if (Source)
			Source.Play();

		// Disable this bean.
		gameObject.SetActive(false);
	}

}
