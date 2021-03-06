﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UIBeans : MonoBehaviour 
{
	public Camera UICamera;
	public Vector3 Offset = new Vector3(0, 0, 0);

	public List<GameObject> Beans;

	void Start() 
	{
		foreach (GameObject bean in Beans)
			bean.SetActive(false);
	}
	
	void Update() 
	{
		transform.position = UICamera.ViewportToWorldPoint(Offset);
		int active = PlayerController.Instance.BeanCounter;
		int n = Beans.Count;
		for (int i = 0; i < n; i++)
			Beans[i].SetActive(i < active);
	}
}
