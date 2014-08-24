using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Plant : MonoBehaviour 
{

	// Properties
	// -----------------------------------------------------

	/** A list of rules used to grow the plant. */
	public List<Rule> Rules;

	/** Termination depth - determines when branches stop growing. */
	public float MaxDepth = 10;

	/** Duration of the wither effect on each piece. */
	public float MinWitherTime = 0.25f, MaxWitherTime = 0.5f;


	// Inner Classes
	// -----------------------------------------------------
	
	[System.Serializable]
	public class Rule
	{
		/** Weighting factor, governs likelihood of rule being selected. */
		public float Weight = 1;

		/** The parent piece that this rule applies to. */
		public GameObject From;

		/** List of branches that are produced by this rule. */
		public List<Branch> Branches;
	}

	[System.Serializable]
	public class Branch
	{
		/** The piece that represents this branch. */
		public GameObject Piece;

		/** Whether this branch is a leaf. */
		public bool Leaf = false;

		/** Duration of growth phase. */
		public float MinDuration = 1, MaxDuration = 1;

		/** Offset of branch from its parent. */
		public Vector3 MinTranslate, MaxTranslate;

		/** Rotation of branch relative to its parent. */
		public Vector3 MinRotate, MaxRotate;

		/** Scale of branch relative to its parent. */
		public float MinScale = 1, MaxScale = 1;

		/** Depth increment. */
		public float MinDeepen = 1, MaxDeepen = 1;
	}


	// Private members
	// -----------------------------------------------------

	/** The set of plant pieces. */
	private Stack<GameObject> pieces = new Stack<GameObject>();


	// Unity Methods
	// -----------------------------------------------------

	/** Initialization. */
	void Start() 
	{
		// Start growing the plant!
		Grow();
	}


	// Public Methods
	// -----------------------------------------------------

	/** Grows the plant from the supplied parent piece. */
	public void Grow(GameObject from = null, GameObject parent = null, float depth = 0)
	{
		// Check if we should terminate growth.
		if (depth >= MaxDepth)
			return;

		// Figure out which rules apply in this situation.
		List<Rule> possible = new List<Rule>();
		foreach (Rule candidate in Rules)
			if (candidate.From == from)
				possible.Add(candidate);

		// Check if any rules apply.
		if (possible.Count == 0)
			return;

		// Determine total weighting of possible rules.
		float totalWeight = 0;
		foreach (Rule r in possible)
			totalWeight += r.Weight;

		// Perform a weighted random rule selection.
		float w = 0, target = Random.Range(0, totalWeight);
		Rule rule = possible[0];
		foreach (Rule candidate in possible)
		{ 
			rule = candidate; 
			w += candidate.Weight;
			if (w >= target)
				break;
		}

		// Using our selected rule, perform production.
		foreach (Branch branch in rule.Branches)
			StartCoroutine(Generate(branch, parent, depth));
	}

	/** Kill off a plant in reverse order of growth. */
	public void Die()
	{
		// Stop any active growth.
		StopAllCoroutines();

		// Start withering.
		StartCoroutine(Wither());
	}


	// Private Methods
	// -----------------------------------------------------

	/** Generates a branch, and kicks off subsequent growth. */
	private IEnumerator Generate(Branch b, GameObject parent, float depth)
	{
		// If no parent is given, use the plant as parent.
		// This will be the case for roots.
		if (!parent)
			parent = this.gameObject;

		// Create the piece that represents this branch.
		Transform p = parent.transform;
		GameObject piece = Instantiate(b.Piece, p.position, p.rotation) as GameObject;
		piece.transform.parent = p;

		// Randomize parameters.
		Vector3 translate = Util.RandomInRange(b.MinTranslate, b.MaxTranslate);
		Vector3 rotate = Util.RandomInRange(b.MinRotate, b.MaxRotate);
		float scale = Random.Range(b.MinScale, b.MaxScale);
		float deepen = Random.Range(b.MinDeepen, b.MaxDeepen);

		// Move branch into its desired orientation.
		piece.transform.localPosition = translate;
		piece.transform.localRotation = Quaternion.Euler(rotate);
		piece.transform.localScale = new Vector3(scale, 0, scale);

		// Scale up the branch over time.
		float duration = Random.Range(b.MinDuration, b.MaxDuration);
		float start = Time.time, end = start + duration;
		while (Time.time < end)
		{
			float f = (Time.time - start) / (end - start);
			float s = scale * f;
			piece.transform.localScale = new Vector3(scale, s, scale);
			yield return new WaitForEndOfFrame();
		}

		// Set branch to final scaling.
		piece.transform.localScale = new Vector3(scale, scale, scale);

		// Add piece to the stack.
		pieces.Push(piece);

		// Kick off further growth from this branch, unless it's a leaf.
		if (!b.Leaf)
			Grow(b.Piece, piece, depth + deepen);

		// Done.
		yield return null;
	}

	/** Withers the plant over time, then destroys it. */
	private IEnumerator Wither()
	{
		// Keep withering until all pieces are dead.
		while (pieces.Count > 0)
		{
			// Get a piece to wither.
			GameObject piece = pieces.Pop();

			// Scale down the piece over time.
			float duration = Random.Range(MinWitherTime, MaxWitherTime);
			float start = Time.time, end = start + duration;
			float scale = piece.transform.localScale.y;
			while (Time.time < end)
			{
				float f = (Time.time - start) / (end - start);
				float s = scale * (1 - f);
				piece.transform.localScale = new Vector3(scale, s, scale);
				yield return new WaitForEndOfFrame();
			}

			// Kill the piece when it has withered.
			Destroy(piece);
		}

		// Once everything has withered, destroy plant entirely.
		Destroy(gameObject);
	}


}
