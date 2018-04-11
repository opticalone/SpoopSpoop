
using UnityEngine;
[RequireComponent(typeof(PlayerMotor))]

public class PlayerMotor : MonoBehaviour 
{
	[SerializeField]
	private Camera CAMMMMMM;


	private Vector3 vel = Vector3.zero;
	private Vector3 rotation = Vector3.zero;
	private float camRotaionX = 0f;
	private float currentCamRotX = 0f;

	private Vector3 thrusterForce = Vector3.zero;

	[SerializeField]
	private float camRotLimit = 89f ;

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
	//gets force vector for thrust vector

	public void applyThruster (Vector3 _thrusterForce)
	{
		thrusterForce = _thrusterForce;
	}

	public void camRot(float _camRotationX)
	{
		camRotaionX = _camRotationX; 
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

		if (thrusterForce != Vector3.zero) {
		
			rb.AddForce (thrusterForce * Time.fixedDeltaTime, ForceMode.Impulse);
		}
	}

	void preformRotato ()
	{
		rb.MoveRotation(rb.rotation * Quaternion.Euler(rotation));
		if (CAMMMMMM != null) 
		{
			//rotational calc with clamps
			currentCamRotX += camRotaionX;
			currentCamRotX = Mathf.Clamp (currentCamRotX, -camRotLimit, camRotLimit);

			CAMMMMMM.transform.localEulerAngles = new Vector3 (currentCamRotX,0f,0f);
		}


	}


}