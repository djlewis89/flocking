using UnityEngine;
using System.Collections.Generic;

public class Flock : MonoBehaviour
{
	public int Size = 25;
	public GameObject Obj;

	public float Radius = 20.0f;

	public float SeparationWeight = .75f;
	public float AlignmentWeight = 1.0f;
	public float CohesionWeight = 1.0f;
	public float BoundsWeight = 0.5f;

	private List<GameObject> flock;

	void Awake () 
	{
		flock = new List<GameObject>();
		for (int i = 0; i < Size; ++i) {
			var newBoid = Instantiate (Obj, transform.position, Random.rotation) as GameObject;
			newBoid.transform.parent = transform;

			flock.Add (newBoid);
		}
	}
	
	void Update () 
	{
		
	}

	void OnDrawGizmosSelected()
	{
		Gizmos.DrawWireSphere (transform.position, Radius);
	}
}
