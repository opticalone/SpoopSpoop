
using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class PlayerManager : NetworkBehaviour 
{


	[SyncVar]
	private bool _isDead = false;

	public bool isDead 
	{
		get{ return _isDead;}
		protected set {_isDead = value; }
	}



	[SerializeField]
	private int maxHealth = 100;

	[SyncVar]
	private int currentHealth;

	[SerializeField]
	private Behaviour[] disableOnDeath;
	private bool[] wasEnabled;

	[SerializeField]
	private GameObject deathEffect;

	[SerializeField]
	private GameObject[] disableGameObjectsOnDeath;


	public void Setup()
	{

		wasEnabled = new bool[disableOnDeath.Length];
		for (int i = 0; i < wasEnabled.Length; i++) 
		{
			wasEnabled [i] = disableOnDeath [i].enabled;
		}

		SetDefaults ();

	}


	void Update()
	{
		if (!isLocalPlayer) 
		{
			return;
		}
		if (Input.GetKeyDown (KeyCode.K)) 
		{
		
			RpctakeDamage (100);
		}
	}


	[ClientRpc]
	public void RpctakeDamage(int _amount)
	{
		if (isDead)
			return;
		
		currentHealth -= _amount;	



		Debug.Log(transform.name +"Now has "+ currentHealth +" health");

		if (currentHealth <= 0) 
		{
			
			Die ();
		}

	}

	private void Die()
	{
		isDead = true;

		//DISABLE COMPONENTS
		for (int i = 0; i <disableOnDeath.Length; i++)
		{
			disableOnDeath [i].enabled = false;
		}

		//DISABLE GAMEOBJEXT COMPONENTS
		for (int i = 0; i <disableGameObjectsOnDeath.Length; i++)
		{
			disableGameObjectsOnDeath [i].SetActive(false);
		}
		//DISABLE COLLIDER
		Collider _col = GetComponent<Collider> ();
		if (_col != null)
		{
			_col.enabled = true;
		}
		//SPAWN DEATH EFFECRT
		GameObject _gfxInst = (GameObject)Instantiate(deathEffect, transform.position,Quaternion.identity);
		Destroy (_gfxInst, 10);
		//Debug.Log (transform.name + " IS DEAD");

		//cam switch
		if (isLocalPlayer) 
		{
			GameManager.instance.SetSceneCameraActive (true);
		}


		StartCoroutine (Respawn ());

	}

	IEnumerator Respawn()
	{
		Debug.Log (GameManager.instance.matchSettings.respawnTime);
		yield return new WaitForSeconds(GameManager.instance.matchSettings.respawnTime);

		SetDefaults();
		Transform _spawnPoint = NetworkManager.singleton.GetStartPosition();
		transform.position = _spawnPoint.position;
		transform.rotation = _spawnPoint.rotation;
	}


	public void SetDefaults()
	{
		isDead = false;

		currentHealth = maxHealth;

		for (int i = 0; i <disableOnDeath.Length; i++)
		{
			disableOnDeath [i].enabled = wasEnabled [i];	
		}
		for (int i = 0; i <disableGameObjectsOnDeath.Length; i++)
		{
			disableGameObjectsOnDeath [i].SetActive(true);	
		}

		if (isLocalPlayer) 
		{
			GameManager.instance.SetSceneCameraActive (false);
		}

		//enable collider

		Collider _col = GetComponent<Collider> ();
		if (_col != null)
		{
			_col.enabled = true;
		}

	}
}
