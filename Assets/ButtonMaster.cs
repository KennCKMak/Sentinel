using UnityEngine;
using System.Collections;

public class ButtonMaster : MonoBehaviour {

	private NumberMaster numberMaster;
	private Player player;
	// Use this for initialization
	void Start () {
		numberMaster = GameObject.Find ("GameMaster").GetComponent<NumberMaster> ();
		player = GameObject.FindGameObjectWithTag ("Player").GetComponent<Player> ();
	}

	public void BuySpearman(){
		if (player.isAlive ())
			numberMaster.BuySpearman ();
	}

	public void BuyArcher(){
		if (player.isAlive ())
			numberMaster.BuyArcher ();
	}

	public void UpgradeScatter () {
		if (player.isAlive ())
			numberMaster.UpgradeScatter ();
	}

	public void UpgradePunchthrough () {
		if (player.isAlive ())
			numberMaster.UpgradePunchthrough ();
	}

	public void UpgradeDamage () {
		if (player.isAlive ())
			numberMaster.UpgradeDamage ();

	}

	public void UpgradeArmour () {
		if (player.isAlive ())
			numberMaster.UpgradeArmour ();
	}
}
