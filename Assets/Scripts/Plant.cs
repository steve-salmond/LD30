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
	public int MaxDepth = 10;


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

		/** Duration of growth phase. */
		public float MinDuration = 1, MaxDuration = 1;

		/** Offset of branch from its parent. */
		public Vector3 MinTranslate, MaxTranslate;

		/** Rotation of branch relative to its parent. */
		public Vector3 MinRotate, MaxRotate;

		/** Scale of branch relative to its parent. */
		public float MinScale = 1, MaxScale = 1;

		/** Depth increment. */
		public int MinDeepen = 1, MaxDeepen = 1;
	}


	// Unity Methods
	// -----------------------------------------------------

	/** Initialization. */
	void Start() 
	{
		// Start growing the plant!
		Grow();
	}


	// Growth
	// -----------------------------------------------------

	/** Grows the plant from the supplied parent piece. */
	private void Grow(GameObject from = null, GameObject parent = null, int depth = 0)
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

	/** Generates a branch, and kicks off subsequent growth. */
	private IEnumerator Generate(Branch b, GameObject parent, int depth)
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
		Vector3 translate = RandomVectorInRange(b.MinTranslate, b.MaxTranslate);
		Vector3 rotate = RandomVectorInRange(b.MinRotate, b.MaxRotate);
		float scale = Random.Range(b.MinScale, b.MaxScale);
		int deepen = Random.Range(b.MinDeepen, b.MaxDeepen);

		// Move branch into its desired orientation.
		piece.transform.localPosition = translate;
		piece.transform.localRotation = Quaternion.Euler(rotate);
		piece.transform.localScale = Vector3.zero;

		// Scale up the branch over time.
		float duration = Random.Range(b.MinDuration, b.MaxDuration);
		float start = Time.time, end = start + duration;
		while (Time.time < end)
		{
			float f = (Time.time - start) / (end - start);
			float s = scale * f;
			piece.transform.localScale = new Vector3(s, s, s);
			yield return new WaitForEndOfFrame();
		}

		// Set branch to final scaling.
		piece.transform.localScale = new Vector3(scale, scale, scale);

		// Kick off further growth from this branch.
		Grow(b.Piece, piece, depth + deepen);

		// Done.
		yield return null;
	}


	private Vector3 RandomVectorInRange(Vector3 a, Vector3 b)
	{
		float x = Random.Range(a.x, b.x);
		float y = Random.Range(a.y, b.y);
		float z = Random.Range(a.z, b.z);
		return new Vector3(x, y, z);
	}

}
