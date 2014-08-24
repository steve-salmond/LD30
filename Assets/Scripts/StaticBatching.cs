using UnityEngine;
using System.Collections.Generic;

/** Combine everything under this object into static batches. */
public class StaticBatching : MonoBehaviour {

	void Awake() 
	{ StaticBatchingUtility.Combine(gameObject); }
}
