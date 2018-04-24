using UnityEngine;

public class PlayerUI : MonoBehaviour {

	[SerializeField]
	RectTransform ThrusterFuelFill;
	private PlayerController contr;
	public void SetController(PlayerController _contr)
	{
		contr = _contr;
	}

	void Update()
	{
		SetFuelAmmount(contr.GetThrusterFuelAmmount ());
	}

	void SetFuelAmmount(float amount)
	{
		ThrusterFuelFill.localScale = new Vector3 (1f, amount, 1f);
	}
}
