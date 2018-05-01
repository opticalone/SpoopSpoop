using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Ai_rough : MonoBehaviour 
{
	public Transform target;
	private NavMeshAgent agent;
	Vector3 dir;
	float viewRange;


	void Start()
	{
		agent = GetComponent<NavMeshAgent> ();
		dir = transform.TransformDirection (Vector3.forward);
	}


	void Update()
	{

	NavMeshHit hit;



		if (!agent.Raycast (target.position, out hit)) 
		{
			
		}
			
		

	}


}
