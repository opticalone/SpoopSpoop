using UnityEngine;
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(ConfigurableJoint))]
[RequireComponent(typeof(Rigidbody))]


public class PlayerController : MonoBehaviour

{

	[SerializeField]
	private float speed = 15f;
	[SerializeField]
	private float lookSense = 3f;

	[SerializeField]
	private float thrusterForce = 1000f;
	[SerializeField]
	private float thrusterBurnSpeed = 3f;
	[SerializeField]
	private float thrusterFuelRegionSpeed = 2f;
	private float thrusterFuelAmmount = 1f;

	public float GetThrusterFuelAmmount()
	{
		return thrusterFuelAmmount;
	}

	[Header(" J O I N T -- O P T I O N S ")]

	[SerializeField]
	private float jointSpring = 40;
	[SerializeField]
	private float jointMaxForce = 80;

	[SerializeField]
	private LayerMask enviormentmask;

	//component cache
	private PlayerMotor motor;
	private ConfigurableJoint joint;
	private Animator animator;


	void Start()
	{
		motor = GetComponent<PlayerMotor> ();
		joint = GetComponent<ConfigurableJoint> ();
		SetJointSettings (jointSpring);
		animator = GetComponent<Animator> ();
	}

	void Update()
	{

//		RaycastHit _hit;
//
//		if (Physics.Raycast (transform.position, Vector3.down, out _hit, 100f, enviormentmask)) {
//			joint.targetPosition = new Vector3 (0f, -_hit.point.y, 0f);
//		
//		} 
//		else 
//		{
//		
//			joint.targetPosition = new Vector3 (0f, 0f, 0f);
//		}



		//Calc movement

		float _xMove = Input.GetAxis("Horizontal");
		float _zMove = Input.GetAxis("Vertical");

		Vector3 _moveHoriz = transform.right * _xMove;
		Vector3 _moveVert = transform.forward * _zMove;

		//final movment vectors
		Vector3 _velocity = (_moveHoriz + _moveVert) * speed;

		//animate movement

		animator.SetFloat ("ForwardVelocity", _zMove);

		//apply movement :)

		motor.Move (_velocity);

		//calculate rotation as 3D vector
		float _yRot = Input.GetAxisRaw ("Mouse X");

		Vector3 _rot = new Vector3 (0, _yRot, 0) * lookSense;

		//aply rot
		motor.Rot(_rot);




		//calculate CAMERA rotation as 3D vector
		float _xRot = Input.GetAxisRaw ("Mouse Y");

		float _camRotX = _xRot * lookSense;

		//aply rot
		motor.camRot(-_camRotX);


		//calculate thruster force

		Vector3 _thrusterForce = Vector3.zero;
		if (Input.GetButton ("Jump")&& thrusterFuelAmmount>0) 
		{
		 	
			thrusterFuelAmmount -= thrusterBurnSpeed * Time.deltaTime;
			if (thrusterFuelAmmount > 0.1f) {
				_thrusterForce = Vector3.up * thrusterForce;
				SetJointSettings (0f);
			}
		} 
		else 
		{
			thrusterFuelAmmount +=  thrusterFuelRegionSpeed* Time.deltaTime;
			SetJointSettings (jointSpring);
		}

		thrusterFuelAmmount = Mathf.Clamp (thrusterFuelAmmount, 0, 6f);

		//apply thruster

		motor.applyThruster (_thrusterForce);

	}

	private void SetJointSettings(float _jointSpring)
	{

		joint.yDrive = new JointDrive
		{
		 
			positionSpring = _jointSpring, 
			maximumForce = jointMaxForce 
		};
	}



}
