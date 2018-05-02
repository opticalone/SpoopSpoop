using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(PlayerManager))]

public class Ai_rough : MonoBehaviour 
{
	public float wanderRadius;
	public float wanderTimer;

	public SphereCollider targetZone;


	[SerializeField]
	public GameObject target;
	private NavMeshAgent agent;
	private float timer;
	private Vector3 dir;
	private float chaseDist;




	void Start()
	{
		
		agent = GetComponent<NavMeshAgent> ();
		timer = wanderTimer;
		dir = transform.TransformDirection (Vector3.forward);
		
	}


	void Update()
	{

		timer += Time.deltaTime;

		if (target != null) 
		{

		}
		else 
		{
			Wander ();
		}


	}


	public void OnTriggerEnter()
	{
		Chase ();

	}

	public void Chase()
	{
		if (targetZone.radius > chaseDist ) 
		{

		//	agent.SetDestination (target);

		} 
		else 
		{
			return;
		}
	}

	public void Wander()
	{
		if (timer >= wanderTimer) 
		{
			Vector3 newPos = RandomNavSphere (transform.position, wanderRadius, -1);
			agent.SetDestination (newPos);
			timer = 0;
		}
	}

	public static Vector3 RandomNavSphere(Vector3 origin, float dist, int layermask)
	{
		Vector3 randDir = Random.insideUnitSphere * dist;
		randDir += origin;

		NavMeshHit hit;

		NavMesh.SamplePosition (randDir, out hit,dist, layermask);
		return hit.position;
	}
	private void OnDrawGizmos()
	{
		Gizmos.color = Color.yellow;
		Gizmos.DrawWireSphere (transform.position, wanderRadius);
	}
}

