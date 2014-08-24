using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[ExecuteInEditMode]
public class PlanetSpawner : MonoBehaviour 
{

	/** Object to spawn. */
	public GameObject Prefab;

	/** Radius of sphere to spawn on. */
	public float Radius = 50;

	/** Number of items to spawn. */
	public int MinSpawn, MaxSpawn;

	/** Scale ranges. */
	public float MinScale = 1, MaxScale = 1;


	/** Spawn a bunch of items. */
	public void Generate()
	{
		// Clean up old spawned objects.
		List<GameObject> old = new List<GameObject>();
		foreach (Transform child in transform) 
			old.Add(child.gameObject);
		old.ForEach(child => DestroyImmediate(child));

		// Spawn a bunch of new ones.
		int n = Random.Range(MinSpawn, MaxSpawn);
		for (int i = 0; i < n; i++)
			Spawn();
	}

	/** Spawn an object and randomize it. */
	private void Spawn()
	{
		Vector3 p = Random.onUnitSphere * Radius;
		Vector3 up = p.normalized;
		Vector3 side = Vector3.Cross(up, Random.onUnitSphere);
		Vector3 forward = Vector3.Cross(side, up);
		Quaternion q = Quaternion.LookRotation(forward, up);

		GameObject go = Instantiate(Prefab) as GameObject;
		go.transform.parent = transform;
		go.transform.localPosition = p;
		go.transform.localRotation = q;
		go.transform.localScale = Vector3.one * Random.Range(MinScale, MaxScale);
	}
}

