using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UIGems : MonoBehaviour 
{
	public Camera UICamera;
	public Vector3 Offset = new Vector3(0, 0, 0);

	public List<GameObject> Gems;

	void Start() 
	{
		foreach (GameObject gem in Gems)
			gem.SetActive(false);
	}
	
	void Update() 
	{
		transform.position = UICamera.ViewportToWorldPoint(Offset);
		int active = PlayerController.Instance.GemCounter;
		int n = Gems.Count;
		for (int i = 0; i < n; i++)
			Gems[i].SetActive(i < active);
	}
}
