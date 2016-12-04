using UnityEngine;
using System.Collections;

public class NumberMaster : MonoBehaviour {


	private GameMaster gameMaster;
	private CanvasScript canvasScript;
	//Player things
	public int punchthroughLevel = 0; //increases punchthrough amount of arrows
	public int scatterLevel = 0; //increase arrow fired by player
	public int damageLevel = 0; //increases damage by 50% / level
	public int armourLevel = 0; //decrease damage taken by 10% per level

	public int spearmanCost = 80;
	public int archerCost = 120;

	public int baseUpgradeCost = 100;
	public int scaleUpgradeLevel = 50;
	//
	public int gold;
	public int score;
	public int enemiesKilled;


	// Timer
	public float timer;
	public int min;
	public int sec;

	void Start () {
		gameMaster = transform.GetComponent<GameMaster> ();
		canvasScript = GameObject.Find ("Canvas").GetComponent<CanvasScript> ();
		timer = 0;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public int returnGold(){
		return gold;
	}

	public void addGold(int num){
		gold += num;
		canvasScript.updateAll ();

	}

	public void subGold(int num){
		gold -= num;
		canvasScript.updateAll ();
	}

	public void addScore(int num){
		score += num;
		canvasScript.updateAll ();
	}

	public int returnScore(){
		return score;
	}

	public void incEnemyKilled(int num){
		enemiesKilled += num;;
		canvasScript.updateAll ();
	}

	public int returnEnemiesKilled(){
		return enemiesKilled;
	}


	//functions checking if you can buy unit
	public bool canBuySpearman(){
		if (gameMaster.spearmanCount >= gameMaster.maxSpearman)
			return false;
		if (gold >= spearmanCost)
			return true;
		else
			return false;
	}
	public bool canBuyArcher(){
		if (gameMaster.archerCount >= gameMaster.maxArcher)
			return false;
		if (gold >= archerCost)
			return true;
		else
			return false;
	}
	public void BuySpearman(){
		if (canBuySpearman ()) {
			subGold (spearmanCost);
			gameMaster.SpawnSpearman ();
		}
	}

	public void BuyArcher(){
		if (canBuyArcher ()) {
			subGold (archerCost);
			gameMaster.SpawnArcher ();
		}

	}





	//functions checking for can upgrade
	public bool canUpgradeScatter(){
		if (scatterLevel == 5)
			return false;
		if (gold >= baseUpgradeCost + (scaleUpgradeLevel * scatterLevel))
			return true;
		else
			return false;
	}
	public bool canUpgradePunchthrough(){
		if (punchthroughLevel == 5)
			return false;
		if (gold >= baseUpgradeCost + (scaleUpgradeLevel * punchthroughLevel))
			return true;
		else
			return false;
	}
	public bool canUpgradeDamage(){
		if (damageLevel == 5)
			return false;
		if (gold >= baseUpgradeCost + (scaleUpgradeLevel * damageLevel))
			return true;
		else
			return false;
	}

	public bool canUpgradeArmour(){
		if (armourLevel == 5)
			return false;
		if (gold >= baseUpgradeCost + (scaleUpgradeLevel * armourLevel))
			return true;
		else
			return false;
	}


	//functions for upgrading
	public void UpgradeScatter(){
		if (canUpgradeScatter ()) {
			subGold (baseUpgradeCost + (scaleUpgradeLevel * scatterLevel));
			scatterLevel++;
			canvasScript.updateAll ();
		}
	}
	public void UpgradePunchthrough(){
		if (canUpgradePunchthrough ()) {
			subGold (baseUpgradeCost + (scaleUpgradeLevel * punchthroughLevel));
			punchthroughLevel++;
			canvasScript.updateAll ();
		}

	}
	public void UpgradeDamage(){
		if (canUpgradeDamage ()) {
			subGold (baseUpgradeCost + (scaleUpgradeLevel * damageLevel));
			damageLevel++;
			canvasScript.updateAll ();
		}

	}
	public void UpgradeArmour(){
		if (canUpgradeArmour ()) {
			subGold (baseUpgradeCost + (scaleUpgradeLevel * armourLevel));
			armourLevel++;
			canvasScript.updateAll ();
		}

	}

}
