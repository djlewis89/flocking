using System.Linq;
using System.Collections.Generic;

using UnityEngine;

public class Boid : MonoBehaviour 
{
	private List<GameObject> neighbours;

	private new Rigidbody rigidbody;
	private Flock flock;

	void Start () 
	{
		neighbours = new List<GameObject> ();
		rigidbody = GetComponent<Rigidbody> ();
		flock = transform.parent.GetComponent<Flock> ();

		rigidbody.velocity = new Vector3 (Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f)).normalized;
	}
	
	void FixedUpdate () 
	{
		Vector3 alignment = Align () * flock.AlignmentWeight;
		Vector3 cohesion = Cohere () * flock.CohesionWeight;
		Vector3 separation = Separate () * flock.SeparationWeight;
		Vector3 bounds = Bounds () * flock.BoundsWeight;

		Vector3 v = (rigidbody.velocity + alignment + cohesion + separation + bounds).normalized;

		rigidbody.velocity = v;
	}

	private Vector3 Align()
	{
		if (neighbours.Count == 0)
			return Vector3.zero;

		Vector3 v = new Vector3 ();
		foreach (GameObject obj in neighbours) {
			Rigidbody r = obj.GetComponent<Rigidbody> ();	
			v += r.velocity;
		}

		v /= neighbours.Count;

		return v.normalized;
	}

	private Vector3 Cohere()
	{
		if (neighbours.Count == 0)
			return Vector3.zero;

		Vector3 v = new Vector3 ();
		foreach (GameObject obj in neighbours)
			v += obj.transform.position;

		v /= neighbours.Count;

		return v.normalized;
	}

	private Vector3 Separate()
	{
		if (neighbours.Count == 0)
			return Vector3.zero;

		Vector3 v = new Vector3 ();
		foreach (GameObject obj in neighbours)
			v += obj.transform.position - transform.position;

		v *= -1;
		v /= neighbours.Count;

		return v.normalized;
	}

	private Vector3 Bounds()
	{
		float distance = Vector3.Distance (transform.position, flock.transform.position);

		return (-transform.position * (distance / flock.Radius)).normalized;
	}

	void OnTriggerEnter(Collider o)
	{
		//Debug.Log ("Trigger enter");

		if (o.isTrigger && !neighbours.Contains(o.gameObject) && o.gameObject.GetComponent<Boid> () != null)
			neighbours.Add (o.gameObject);
	}

	void OnTriggerStay(Collider o)
	{
		//Debug.Log ("Trigger stay");

		if (o.isTrigger && !neighbours.Contains(o.gameObject) && !neighbours.Contains (o.gameObject))
			neighbours.Add (o.gameObject);
	}

	void OnTriggerExit(Collider o)
	{
		//Debug.Log ("Trigger exit");

		if (neighbours.Contains (o.gameObject))
			neighbours.Remove (o.gameObject);
	}
}
