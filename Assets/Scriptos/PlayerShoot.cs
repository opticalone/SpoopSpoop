
using UnityEngine.Networking;
using UnityEngine;

public class PlayerShoot : NetworkBehaviour 
{
	private const string PLAYERTAG = "Player";

	public PLayerWeapon weapon;


	[SerializeField]
	private Camera cam;

	[SerializeField]
	private LayerMask mask;

	void Start()
	{
		if (cam == null) {
			Debug.Log("PLAYERSHOOT: no camera ref");
			this.enabled = false;
		}
		cam = GetComponentInChildren<Camera> ();

	}
	void Update()
	{
		

		if (Input.GetButtonDown ("Fire1")) 
		{
			Shoot ();
		}
	}

	[Client]
	void Shoot()
	{
		RaycastHit _hit;
		if (Physics.Raycast(cam.transform.position, cam.transform.forward, out _hit, weapon.range, mask))
		{
			if (_hit.collider.gameObject.tag == PLAYERTAG) 
			{
				CmdPlayerShot (_hit.collider.name, weapon.damage);

			}
			Debug.DrawRay (cam.transform.position,cam.transform.forward * 100, Color.green );
			Debug.Log ("we hit" + _hit.collider.name);
			//hit something
		}
	}


	[Command]
	void CmdPlayerShot(string _player_id, int _damage)
	{
		Debug.Log (_player_id + "has been shot");
		PlayerManager _player = GameManager.GetPlayer (_player_id);
		_player.RpctakeDamage (_damage);
	}
}
