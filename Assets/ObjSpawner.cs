using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjSpawner : MonoBehaviour {

	ObjectPooling objPool;
	private void Start()
	{
		objPool = ObjectPooling.Instance;
	}
	// Update is called once per frame
	void FixedUpdate () 
	{
		objPool.SpawnFromPool ("ball", transform.position, Quaternion.identity);
	}
}
