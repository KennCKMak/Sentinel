using UnityEngine;
using System.Collections;

public class ButtonMaster : MonoBehaviour {

	private GameMaster gameMaster;
	private NumberMaster numberMaster;
	private CanvasScript canvasScript;
	// Use this for initialization
	void Start () {
		gameMaster = GameObject.Find ("GameMaster").GetComponent<GameMaster> ();
		numberMaster = GameObject.Find ("GameMaster").GetComponent<NumberMaster> ();
		canvasScript = GameObject.Find ("Canvas").GetComponent<CanvasScript> ();
	
	}


	void UpgradeScatter () {
		numberMaster.UpgradeScatter ();
	}

	void UpgradePunchthrough () {
		numberMaster.UpgradePunchthrough ();
	}

	void UpgradeDamage () {
		numberMaster.UpgradeDamage ();

	}

	void UpgradeArmour () {
		numberMaster.UpgradeArmour ();
	}
}
