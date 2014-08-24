using UnityEngine;
using System.Collections;

public class EndController : Singleton<EndController> {

	void Start()
	{ renderer.sharedMaterial.color = new Color(1, 1, 1, 0); }

	public void FadeIn()
	{ 
		StopAllCoroutines();
		StartCoroutine(FadeInRoutine()); 
	}
	
	public void FadeOut()
	{ 
		StopAllCoroutines();
		StartCoroutine(FadeOutRoutine()); 
	}
	

	/** Fade in from black. */
	private IEnumerator FadeInRoutine()
	{
		float start = Time.time;
		float end = start + 3;
		while (Time.time < end)
		{
			float f = (Time.time - start) / (end - start);
			renderer.sharedMaterial.color = new Color(1, 1, 1, f);
			yield return new WaitForEndOfFrame();
		}
	}
	
	/** Fade out to black. */
	private IEnumerator FadeOutRoutine()
	{
		float start = Time.time;
		float end = start + 3;
		while (Time.time < end)
		{
			float f = (Time.time - start) / (end - start);
			renderer.sharedMaterial.color = new Color(1, 1, 1, 1 -f);
			yield return new WaitForEndOfFrame();
		}
	}
}
