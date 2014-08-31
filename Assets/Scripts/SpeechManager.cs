using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpeechManager : Singleton<SpeechManager> 
{

	// Properties
	// -----------------------------------------------------

	/** Audio source to use for the boy. */
	public AudioSource Boy;

	/** Audio source to use for the narrator. */
	public AudioSource Narrator;

	/** The lines of speech. */
	public List<Line> Lines;

	/** Acceptable interval range between lines. */
	public float MinInterval = 1, MaxInterval = 1;

	/** Characters. */
	public enum Character { Boy, Narrator };


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

		/** Desired pitch. */
		public Character Character = Character.Boy;

		/** Number of times line has been played. */
		[System.NonSerialized]
		public int PlayCount = 0;

		/** Whether line has been cancelled. */
		[System.NonSerialized]
		public bool Cancelled = false;

	}

	/** Speaking of a line at with an optional delay. */
	public class Speak
	{
		/** The line to read. */
		public Line Line;

		/** Delay before reading the line. */
		public float Delay = 0;

		/** Constructor. */
		public Speak(Line line, float delay)
		{ Line = line; Delay = delay; }
	}


	// Members
	// -----------------------------------------------------

	/** Look up map for lines. */
	private Dictionary<string, Line> lookup = new Dictionary<string, Line>();

	/** Queue of speech items to play. */
	private Queue<Speak> playlist = new Queue<Speak>();


	// Unity Methods
	// -----------------------------------------------------

	/** Preinitialization. */
	void Awake()
	{
		// Populate lookup for lines of speech.
		foreach (Line line in Lines)
			lookup[line.Clip.name] = line;

		// Kick off the speech update routine.
		StartCoroutine(SpeechRoutine());
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

		// Schedule the speech.
		playlist.Enqueue(new Speak(line, delay));
	}

	/** Cancel a line of speech. */
	public void Cancel(string id)
	{
		if (lookup.ContainsKey(id))
			lookup[id].Cancelled = true;
	}


	// Private Methods
	// -----------------------------------------------------

	/** Say lines of speech in the correct order. */
	private IEnumerator SpeechRoutine()
	{
		// Keep trying to say stuff for as long as the game runs.
		while (true)
		{
			// Wait for a line of speech to be available.
			while (playlist.Count == 0)
				yield return new WaitForEndOfFrame();

			// Get the line of speech.
			Speak speak = playlist.Dequeue();
			
			// Check if the line was cancelled.
			Line line = speak.Line;
			if (line.Cancelled)
				continue;

			// Wait for the specified speech delay.
			yield return new WaitForSeconds(speak.Delay);

			// Play the line.
			if (line.Character == Character.Boy)
				Boy.PlayOneShot(line.Clip);
			else if (line.Character == Character.Narrator)
				Narrator.PlayOneShot(line.Clip);

			// Wait until the line is finished.
			yield return new WaitForSeconds(line.Clip.length);

			// Wait for a random interval.
			float interval = Random.Range(MinInterval, MaxInterval);
			yield return new WaitForSeconds(interval);
		}
	}

}
