
using UnityEngine.Networking;
using UnityEngine;

[RequireComponent(typeof(WeaponManager))]
public class PlayerShoot : NetworkBehaviour 
{
	
	private const string PLAYERTAG = "Player";



	[SerializeField]
	private Camera cam;
	//private CameraShake camShake;
	//public float camShakeTime = .5f;
	//public float camShakeMag = 1f;
	[SerializeField]
	private LayerMask mask;


	private PLayerWeapon currentWeapon;

	private WeaponManager weaponManager;

	void Start()
	{
		if (cam == null) {
			Debug.Log("PLAYERSHOOT: no camera ref");
			this.enabled = false;
		}
		weaponManager = GetComponent<WeaponManager>();
	
	}

	void Update()
	{
		currentWeapon = weaponManager.GetCurrentWeapon ();

		if (currentWeapon.fireRate <= 0f)
		{
			if (Input.GetButtonDown ("Fire1"))
			{
				Shoot ();
			}
		}
		else 
		{
			if (Input.GetButtonDown ("Fire1")) 
			{
				InvokeRepeating ("Shoot", 0f, 1f / currentWeapon.fireRate);
			} 
			else if (Input.GetButtonUp ("Fire1"))
			{
				CancelInvoke("Shoot");
			}
		}
	}
	//is called on server when player shoots
	[Command]
	void CmdOnShoot()
	{
		RpcDoShootEffect();
	}


	//is called on all clients when need to do shoot effect
	[ClientRpc]
	void RpcDoShootEffect()
	{
		weaponManager.GetCurrentGraphics().mFlash.Play();
	}
	//is called on server when hit
	//takes in hit location and normal of the surface
	[Command]
	void CmdOnHit(Vector3 _pos, Vector3 _normal)
	{
		RpcDoHitEffect (_pos, _normal);
	}
	//is called on all clients/ spawn in effect
	[ClientRpc]
	void RpcDoHitEffect(Vector3 _pos, Vector3 _normal)
	{
		
		GameObject _hitEffect = (GameObject)Instantiate (weaponManager.GetCurrentGraphics ().hitEffectPrefab, _pos, Quaternion.LookRotation (_normal));
		Destroy (_hitEffect, 6f);
	}




	[Client]
	void Shoot()
	{
		if (!isLocalPlayer)
		{
			return;
		}

		//StartCoroutine(camShake.Shake (camShakeTime,camShakeMag));
		//we are shooting, call on shoot method on server
		CmdOnShoot ();
		RaycastHit _hit;
		if (Physics.Raycast(cam.transform.position, cam.transform.forward, out _hit, currentWeapon.range, mask))
		{
			if (_hit.collider.gameObject.tag == PLAYERTAG) 
			{
				CmdPlayerShot (_hit.collider.name, currentWeapon.damage);

			}

			//hit effect

			CmdOnHit (_hit.point, _hit.normal);

			Debug.DrawRay (cam.transform.position,cam.transform.forward * 200, Color.green );
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
