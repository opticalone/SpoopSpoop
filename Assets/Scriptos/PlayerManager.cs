
using UnityEngine;
using UnityEngine.Networking;
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

	public void Setup()
	{

		wasEnabled = new bool[disableOnDeath.Length];
		for (int i = 0; i < wasEnabled.Length; i++) 
		{
			wasEnabled [i] = disableOnDeath [i].enabled;
		}

		SetDefaults ();

	}

	[ClientRpc]
	public void RpctakeDamage(int _amount)
	{
		if (isDead)
			return;
		
		currentHealth -= _amount;	



		Debug.Log(transform.name +"Now has "+ currentHealth +" health");

		if (currentHealth <= 0) {
		
			Die ();
		}

	}

	private void Die()
	{
		isDead = true;
		//disable components on player object

		Debug.Log (transform.name + " IS DEAD");

		//call respawn method

	}

	public void SetDefaults()
	{
		isDead = false;

		currentHealth = maxHealth;

		for (int i = 0; i <disableOnDeath.Length; i++)
		{
			disableOnDeath [i].enabled = wasEnabled [i];	
		}
	}
}
