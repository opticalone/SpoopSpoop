using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandAddForce : MonoBehaviour , IPooledObject
{


	public float upForce = 1f;
	public float sideForce = .25f;

	public void OnObjectSpawn()
	{
		float xForce = Random.Range (-sideForce, sideForce);
		float yForce = Random.Range (-upForce, upForce);
		float zForce = Random.Range (-sideForce, sideForce);

		Vector3 force = new Vector3 (xForce, yForce, zForce);

		GetComponent<Rigidbody> ().velocity = force;	
	}
}
