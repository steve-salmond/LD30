using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlantSpawner : MonoBehaviour 
{

	// Properties
	// -----------------------------------------------------

	/** Input button for spawning plants. */
	public string ButtonId = "Fire1";

	/** Type of plant to spawn. */
	public Plant Plant;

	/** Limit on how many plants can be active (0 means infinite). */
	public int Limit;

	/** How often player can spawn a plant. */
	public float Timeout = 1;

	/** Maximum planting distance. */
	public float MaxDistance = 50;

	/** Layers that can be planted on. */
	public LayerMask PlantableLayers;



	// Members
	// -----------------------------------------------------
	
	/** The set of spawned plants. */
	private Queue<Plant> spawned = new Queue<Plant>();

	/** Physics raycasting info. */
	private RaycastHit hit = new RaycastHit();

	/** Time at which a plant can next be spawned. */
	private float nextSpawnTime = 0;


	// Unity Methods
	// -----------------------------------------------------

	/** Update. */
	void Update() 
	{
		// Attempt to sow plants when player fires.
		if (Input.GetButtonDown(ButtonId))
			Sow();
	}


	// Private Methods
	// -----------------------------------------------------

	/** Attempts to sow a plant. */
	private void Sow()
	{
		// Check if enough time has elapsed.
		if (Time.time < nextSpawnTime)
			return;

		// Does player have a bean to sow?
		if (PlayerController.Instance.BeanCounter <= 0)
			return;

		// Get the main camera transform.
		Transform t = Camera.main.transform;

		// Have we hit a plantable surface?
		if (Physics.Raycast(t.position, t.forward, out hit, MaxDistance, PlantableLayers))
		{
			// Check if spawn limit has been reached.
			// If so, kill off an old plant.
			if (Limit > 0 && spawned.Count >= Limit)
			{
				Plant old = spawned.Dequeue();
				old.Die();
			}

			// Determine desired orientation of plant.
			Vector3 up = hit.normal;
			Vector3 side = Vector3.Cross(up, Vector3.up);
			Vector3 forward = Vector3.Cross(up, side);
			Quaternion q = Quaternion.LookRotation(forward, up);

			// Spawn a new plant at the desired location.
			Plant plant = Instantiate(Plant, hit.point, q) as Plant;

			// Parent plant to the surface it was planted on.
			plant.transform.parent = hit.collider.transform;

			// Store plant in spawned queue.
			spawned.Enqueue(plant);

			// Schedule next spawn time.
			nextSpawnTime = Time.time + Timeout;

			// Tell player we're sowing a bean.
			PlayerController.Instance.SowBean();
		}
	}

}
