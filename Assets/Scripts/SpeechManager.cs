using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpeechManager : Singleton<SpeechManager> 
{

	// Properties
	// -----------------------------------------------------

	/** Audio source to use. */
	public AudioSource Source;

	/** The lines of speech. */
	public List<Line> Lines;

	/** Acceptable interval range between lines. */
	public float MinInterval = 1, MaxInterval = 1;


	// Inner Classes
	// -----------------------------------------------------

	/** A line of speech. */
	[System.Serializable]
	public class Line
	{
		/** The associated sound asset. */
		public AudioClip Clip;

		/** Number of times line can be played. */
		public int Limit = 1;

		/** Number of times line has been played. */
		[System.NonSerialized]
		public int PlayCount = 0;

	}

	// Members
	// -----------------------------------------------------

	/** Look up map for lines. */
	private Dictionary<string, Line> lookup = new Dictionary<string, Line>();

	/** Queue of lines to play. */
	private Queue<Line> playlist = new Queue<Line>();

	/** Whether source was known to be playing last frame. */
	private bool wasPlaying = false;

	/** Next available play time. */
	private float nextPlayTime = 0;


	// Unity Methods
	// -----------------------------------------------------

	/** Preinitialization. */
	void Awake()
	{
		foreach (Line line in Lines)
			lookup[line.Clip.name] = line;

		if (!Source)
			Source = audio;
	}

	/** Initialization. */
	void Start()
	{
		Say("WhereAmI", 6);
		Say("HaveToFindMyWayHome", 15);
	}

	/** Play speech as needed. */
	void Update()
	{
		// Can't say two things at once!
		if (Source.isPlaying)
		{
			wasPlaying = true;
			return;
		}

		// Do we have anything to say right now?
		if (playlist.Count > 0)
		{
			if (wasPlaying)
			{
				// Wait until the right moment to say it.
				nextPlayTime = Time.time + Random.Range(MinInterval, MaxInterval);
			}
			else if (Time.time >= nextPlayTime)
			{
				// Let's say the line now!
				Line line = playlist.Dequeue();
				Source.PlayOneShot(line.Clip);
			}
		}

		// Remember whether audio was playing this frame.
		wasPlaying = false;
	}


	// Public Methods
	// -----------------------------------------------------

	/** Schedule a line of speech. */
	public void Say(string id, float delay = 0)
	{
		if (!lookup.ContainsKey(id))
			return;

		Line line = lookup[id];
		if (line.PlayCount > 0)
			if (line.PlayCount >= line.Limit)
				return;

		if (delay > 0)
			StartCoroutine(Schedule(id, delay));
		else
		{
			line.PlayCount++;
			playlist.Enqueue(line);
		}
	}


	// Private Methods
	// -----------------------------------------------------

	/** Schedule a line of speech. */
	private IEnumerator Schedule(string id, float delay)
	{
		// Wait for a bit.
		yield return new WaitForSeconds(delay);

		// Say the line immediately.
		Say(id);
	}

}
