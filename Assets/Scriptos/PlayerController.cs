using UnityEngine;

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

	[Header(" J O I N T -- O P T I O N S ")]
	[SerializeField]
	private JointDriveMode jointMode;
	[SerializeField]
	private float jointSpring = 40;
	[SerializeField]
	private float jointMaxForce = 80;


	private PlayerMotor motor;
	private ConfigurableJoint joint;

	void Start()
	{
		motor = GetComponent<PlayerMotor> ();
		joint = GetComponent<ConfigurableJoint> ();
		SetJointSettings (jointSpring);
	}

	void Update()
	{
		//Calc movement

		float _xMove = Input.GetAxisRaw ("Horizontal");
		float _zMove = Input.GetAxisRaw ("Vertical");

		Vector3 _moveHoriz = transform.right * _xMove;
		Vector3 _moveVert = transform.forward * _zMove;

		//final movment vectors
		Vector3 _velocity = (_moveHoriz + _moveVert).normalized * speed;

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


		if (Input.GetButton ("Jump")) {
		 	
			_thrusterForce = Vector3.up * thrusterForce;
			SetJointSettings (0f);
		
		} else {
		
			SetJointSettings (jointSpring);
		}

		//apply thruster

		motor.applyThruster (_thrusterForce);

	}

	private void SetJointSettings(float _jointSpring)
	{

		joint.yDrive = new JointDrive
		{
			mode = jointMode, 
			positionSpring = _jointSpring, 
			maximumForce = jointMaxForce 
		};
	}



}
