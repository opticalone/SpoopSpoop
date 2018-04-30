using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeBody : MonoBehaviour {


	public bool isRewinding = false;
	Rigidbody rb;
	List<PointInTime> positions;

	void Start () 
	{
		positions = new List<PointInTime> ();
		rb = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (Input.GetKeyDown (KeyCode.R))
			StartRewind ();
		if (Input.GetKeyUp (KeyCode.R))
			StopRewind ();
	}
	void FixedUpdate()
	{
		if (isRewinding)
			Rewind ();
		else
			Record();
	}

	void Rewind ()
	{
		if (positions.Count > 0) 
		{
			PointInTime pointintime = positions [0];
			transform.position = pointintime.pos;
			transform.rotation = pointintime.rot;
			positions.RemoveAt(0);
		}
		else
		StopRewind ();
		}



	void Record()
	{
		if (positions.Count > Mathf.Round (5f / Time.fixedDeltaTime)) 
		{
			positions.RemoveAt (positions.Count - 1);
		}
		positions.Insert (0,new PointInTime(transform.position, transform.rotation));
	}

	public void StartRewind ()
	{
		isRewinding = true;
		rb.isKinematic = true;
	}

	public void StopRewind ()
	{
		isRewinding = false;
		rb.isKinematic = false;
	}

}
