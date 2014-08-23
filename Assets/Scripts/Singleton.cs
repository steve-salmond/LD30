using UnityEngine;
using System.Collections;

/** 
 * Convenience class for defining singleton components. 
 */ 

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{

	// Public Static Methods
	// -----------------------------------------------------

	/** Returns the instance of this singleton. */
	public static T Instance
	{
		get
		{
			if (!instance)
				instance = (T) FindObjectOfType(typeof(T));

			return instance;
		}
	}


	// Static Members
	// -----------------------------------------------------
	
	/** The singleton instance. */
	private static T instance;


}
