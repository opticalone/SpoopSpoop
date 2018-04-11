using UnityEngine;
using UnityEngine.Networking;
[RequireComponent(typeof(PlayerManager))]
public class PlayerSetup : NetworkBehaviour 
{ 

	[SerializeField]
	Behaviour[] componentToDisable;

	Camera sceneCamera;

	[SerializeField]
	string remoteLayerName = "RemotePLayer";

	void Start()
	{
		
		if (!isLocalPlayer) 
		{
			DisableComponenets ();
			AssignRemoteLayer ();
		} 
		else 
		{
			sceneCamera = Camera.main;

			if (sceneCamera != null)
			{
				sceneCamera.gameObject.SetActive (false);
			}

		}

	}

	public override void OnStartClient()
	{
		base.OnStartClient ();

		string _netId = GetComponent<NetworkIdentity> ().netId.ToString();
		PlayerManager _player = GetComponent<PlayerManager> ();
		GameManager.RegisterPLayer (_netId,_player);
	}


	void registerPlayer()
	{
		string _ID = "Player"+ GetComponent<NetworkIdentity> ().netId;
		transform.name = _ID;
	}

	void AssignRemoteLayer()
	{
		gameObject.layer = LayerMask.NameToLayer (remoteLayerName);
	}
		

	void DisableComponenets()
	{
		for (int i = 0; i < componentToDisable.Length; i++)
		{

			componentToDisable [i].enabled = false;

		}
	}

	void OnDisable()
	{
		if (sceneCamera != null) {
			sceneCamera.gameObject.SetActive (true);
		}

		GameManager.UnRegisterPlayer (transform.name);
	}


}
