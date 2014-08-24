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

		/** Whether line has been cancelled. */
		[System.NonSerialized]
		public bool Cancelled = false;

	}

	// Members
	// -----------------------------------------------------

	/** Look up map for lines. */
	private Dictionary<string, Line> lookup = new Dictionary<string, Line>();

	/** Queue of lines to play. */
	private Queue<Line> playlist = new Queue<Line>();

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

	/** Play speech as needed. */
	void Update()
	{
		// Do we have anything to say right now?
		if (playlist.Count > 0 && Time.time >= nextPlayTime)
		{
			Line line = playlist.Dequeue();

			// Check if the line was cancelled.
			if (line.Cancelled)
				return;

			// Play the line.
			Source.PlayOneShot(line.Clip);
			nextPlayTime = Time.time + line.Clip.length + Random.Range(MinInterval, MaxInterval);
		}

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

		line.PlayCount++;

		if (delay > 0)
			StartCoroutine(Schedule(line, delay));
		else
			playlist.Enqueue(line);
	}

	/** Cancel a line of speech. */
	public void Cancel(string id)
	{
		if (lookup.ContainsKey(id))
			lookup[id].Cancelled = true;
	}


	// Private Methods
	// -----------------------------------------------------

	/** Schedule a line of speech. */
	private IEnumerator Schedule(Line line, float delay)
	{
		// Wait for a bit.
		yield return new WaitForSeconds(delay);

		// Say the line immediately.
		playlist.Enqueue(line);
	}

}
