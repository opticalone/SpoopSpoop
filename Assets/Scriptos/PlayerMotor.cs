
using UnityEngine;
[RequireComponent(typeof(PlayerMotor))]

public class PlayerMotor : MonoBehaviour 
{
	[SerializeField]
	private Camera CAMMMMMM;


	private Vector3 vel = Vector3.zero;
	private Vector3 rotation = Vector3.zero;
	private Vector3 camRotaion = Vector3.zero;



	private Rigidbody rb;

	void Start()
	{
		rb = GetComponent<Rigidbody> ();

	}


	//gets movement vector
	public void Move(Vector3 _velocity)
	{
		vel = _velocity;
	}

	//gets Rotation vector
	public void Rot(Vector3 _rotato)
	{
		rotation = _rotato; 
	}



	public void camRot(Vector3 _camRotation)
	{
		camRotaion = _camRotation; 
	}



	//run every physics itteraation
	void FixedUpdate()
	{
		preformMovement ();
		preformRotato ();
	}


	//preform movement based on velocity

	void preformMovement()
	{
		if (vel != Vector3.zero) {
		
			rb.MovePosition(rb.position+ vel * Time.fixedDeltaTime);
		
		}
	}

	void preformRotato ()
	{
		rb.MoveRotation(rb.rotation * Quaternion.Euler(rotation));
		if (CAMMMMMM != null) {
			CAMMMMMM.transform.Rotate (camRotaion);
		}
	}


}