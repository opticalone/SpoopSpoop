using UnityEngine;


[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour

{

	[SerializeField]
	private float speed = 15f;
	[SerializeField]
	private float lookSense = 3f;

	private PlayerMotor motor;

	void Start()
	{
		motor = GetComponent<PlayerMotor> ();

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

		Vector3 _camRot = new Vector3 (_xRot, 0, 0) * lookSense;

		//aply rot
		motor.camRot(-_camRot);

	}




}
