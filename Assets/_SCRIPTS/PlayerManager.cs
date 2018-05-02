
using UnityEngine;
using UnityEngine.Networking;
using System.Collections;


[RequireComponent(typeof(PlayerSetup))]
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
	public Light flashlight;
	[SyncVar]
	private int currentHealth;

	[SerializeField]
	private Behaviour[] disableOnDeath;
	private bool[] wasEnabled;

	[SerializeField]
	private GameObject deathEffect;

	[SerializeField]
	private GameObject spawnEffect;

	[SerializeField] 
	private GameObject[] disableGameObjectsOnDeath;


	private bool firstSetup = true;



	// --- setup --- setup --- setup --- setup --- setup --- setup --- setup --- setup --- setup --- setup --- setup ---
	//setup --- setup --- setup --- setup --- setup --- setup --- setup --- setup --- setup --- setup --- setup ---
	public void Setup()
	{
		if (isLocalPlayer) 
		{
			//cam switch

			GameManager.instance.SetSceneCameraActive (true);
			GetComponent<PlayerSetup> ().playerUIinstance.SetActive (false);
		}
		CmdBrodcastNewPlayerSetup ();

	}
	[Command]
	private void CmdBrodcastNewPlayerSetup()
	{
		RpcSetupPlayerOnAllClients ();
	}

	[ClientRpc]
	private void RpcSetupPlayerOnAllClients()
	{
		if (firstSetup)
		{
			wasEnabled = new bool[disableOnDeath.Length];
			for (int i = 0; i < wasEnabled.Length; i++) {
				wasEnabled [i] = disableOnDeath [i].enabled;
			}
			firstSetup = false;
		}
		SetDefaults ();
	}
	//setup --- setup --- setup --- setup --- setup --- setup --- setup --- setup --- setup --- setup --- setup ---
	// --- setup --- setup --- setup --- setup --- setup --- setup --- setup --- setup --- setup --- setup --- setup ---







	void Update()
	{
		if (!isLocalPlayer) 
		{
			return;
		}
		if (Input.GetKey(KeyCode.F)) {
			flashlight.intensity = 7;

		} else {
			flashlight.intensity = 0;
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

		StartCoroutine (Respawn ());

	}







	private	IEnumerator Respawn()
	{
		Debug.Log (GameManager.instance.matchSettings.respawnTime);
		yield return new WaitForSeconds(GameManager.instance.matchSettings.respawnTime);


		Transform _spawnPoint = NetworkManager.singleton.GetStartPosition();
		transform.position = _spawnPoint.position;
		transform.rotation = _spawnPoint.rotation;
		//cam switch
		yield return new WaitForSeconds(.1f);
		GameManager.instance.SetSceneCameraActive (false);
		GetComponent<PlayerSetup> ().playerUIinstance.SetActive (true);


		Setup ();
		Debug.Log (transform.name + " respawnerd");
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
			GetComponent<PlayerSetup> ().playerUIinstance.SetActive (true);
		}
		//create spawn effect
		GameObject _gfxInst = (GameObject)Instantiate(spawnEffect, transform.position,Quaternion.identity);
		Destroy (_gfxInst, 10f);


	}
}
