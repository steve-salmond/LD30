using UnityEngine;
using System.Collections;

public class TitleController : Singleton<TitleController> {

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

			if (Input.GetButton("Fire1"))
				FadeOut();
		}

		yield return new WaitForSeconds(4);
		FadeOut();
	}
	
	/** Fade out to black. */
	private IEnumerator FadeOutRoutine()
	{
		float a = renderer.sharedMaterial.color.a;

		float start = Time.time;
		float end = start + 3;
		while (Time.time < end)
		{
			float f = (Time.time - start) / (end - start);
			renderer.sharedMaterial.color = new Color(1, 1, 1, (1 - f) * a);
			yield return new WaitForEndOfFrame();
		}
	}
}
