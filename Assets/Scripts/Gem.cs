using UnityEngine;
using System.Collections;

public class Gem : MonoBehaviour {
	
	public AudioSource Source; 
	
	private bool PickedUp = false;
	
	void OnTriggerEnter(Collider other) 
	{
		if (PickedUp)
			return;
		
		// Tell player they've picked up a gem.
		PlayerController.Instance.PickUpGem();
		PickedUp = true;
		
		// Play pickup sound.
		if (Source)
			Source.Play();
		
		// Disable this pickup.
		gameObject.SetActive(false);
	}
	
}
