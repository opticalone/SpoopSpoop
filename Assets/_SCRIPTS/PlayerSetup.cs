using UnityEngine;
using UnityEngine.Networking;
[RequireComponent(typeof(PlayerManager))]
[RequireComponent(typeof(PlayerController))]

public class PlayerSetup : NetworkBehaviour 
{ 

	[SerializeField]
	Behaviour[] componentToDisable;



	[SerializeField]
	string remoteLayerName = "RemotePLayer";

	[SerializeField]
	string dontDrawLayerName= "DontDraw";

	[SerializeField]
	GameObject playerGraphics;

	[SerializeField]
	GameObject playerUIprefab;

	[HideInInspector]
	public GameObject playerUIinstance;



	void Start()
	{
		
		
		if (!isLocalPlayer) 
		{
			//sets components dor active player

			DisableComponenets ();
			AssignRemoteLayer ();

		} 

		else 
		{

			//disable player graphics for local
			SetLayerRecursively(playerGraphics, LayerMask.NameToLayer(dontDrawLayerName));
			//create ui

			playerUIinstance = Instantiate (playerUIprefab);
			playerUIinstance.name = playerUIprefab.name;
			PlayerUI ui = playerUIinstance.GetComponent<PlayerUI> ();
			if(ui == null)
				Debug.LogError("NO PLAYERUI PREFAB");
			ui.SetController (GetComponent<PlayerController> ());
			GetComponent<PlayerManager>().Setup ();
		}


	}








	void SetLayerRecursively(GameObject obj, int newLayer)
	{
		obj.layer = newLayer;

		foreach (Transform child in obj.transform)
		{
		
			SetLayerRecursively (child.gameObject, newLayer);
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
		
		Destroy (playerUIinstance);
		if (!isLocalPlayer) 
		{
			//reenable cam
			GameManager.instance.SetSceneCameraActive (true);
			GameManager.UnRegisterPlayer (transform.name);
		}
	}


}
