using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


public class WeaponManager : NetworkBehaviour 
{
	[SerializeField]
	private string weaponLayerName = "weapon";

	[SerializeField]
	private Transform weaponHolder;

	[SerializeField]
	private PLayerWeapon primaryWeapon;
	private PLayerWeapon currentWeapon;
	private WeaponGraphics currentGraphics;


	void Start ()
	{
		EquipWeapon (primaryWeapon);
	}

	public PLayerWeapon GetCurrentWeapon()
	{
		return currentWeapon;
	}
	public WeaponGraphics GetCurrentGraphics()
	{
		return currentGraphics;
	}


	void EquipWeapon (PLayerWeapon _weapon)
	{
		currentWeapon = _weapon;

		GameObject _weaponIns = (GameObject)Instantiate (_weapon.graphics, weaponHolder.position, weaponHolder.rotation);
		_weaponIns.transform.SetParent (weaponHolder);

		currentGraphics = _weaponIns.GetComponent<WeaponGraphics> ();
		if (currentGraphics == null) {
			Debug.LogError ("no graphics " + _weapon.name);
		}


		if (isLocalPlayer)
		{
			Util.SetLayerRecursively (_weaponIns, LayerMask.NameToLayer (weaponLayerName));
		}
	}



}
