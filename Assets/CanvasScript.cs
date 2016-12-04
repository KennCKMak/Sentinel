using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CanvasScript : MonoBehaviour {

	private Player playerScript;
	private NumberMaster numberMaster;
	// Use this for initialization
	private Vector3 defaultSize;
	private Vector3 enlargedSize;

	private GameObject GoldText;
	private GameObject ScoreText;
	private GameObject EnemiesKilledText;

	private GameObject Weapon1;
	protected GameObject BowHealthBar; //access to the health bar
	protected GameObject BowHPImg;
	protected float BowLeftMost;


	private GameObject Weapon2;
	protected GameObject ScatterHealthBar; //access to the health bar
	protected GameObject ScatterHPImg;
	protected float ScatterLeftMost;



	private GameObject Weapon3;
	protected GameObject BallistaHealthBar; //access to the health bar
	protected GameObject BallistaHPImg;
	protected float BallistaLeftMost;





	private GameObject Ability4;
	private GameObject BuySpearman;
	private GameObject BuyArcher;

	private GameObject UpgradeScatter;
	private GameObject UpgradePunchthrough;
	private GameObject UpgradeDamage;
	private GameObject UpgradeArmour;

	private int AbilitiesSize = 9;
	//private GameObject Ability7;

	private GameObject[] Abilities;

	void Start () {
		numberMaster = GameObject.Find ("GameMaster").GetComponent<NumberMaster> ();
		playerScript = GameObject.FindGameObjectWithTag ("Player").GetComponent<Player> ();

		GoldText = transform.FindChild ("UpperBorder").transform.FindChild ("GoldText").gameObject;
		ScoreText = transform.FindChild ("UpperBorder").transform.FindChild ("ScoreText").gameObject;
		EnemiesKilledText = transform.FindChild ("UpperBorder").transform.FindChild ("EnemiesKilledText").gameObject;



		defaultSize = new Vector3 (1.0f, 1.0f, 1.0f);
		enlargedSize = new Vector3 (1.2f, 1.2f, 1.0f);

		//BOW
		Weapon1 = transform.FindChild("LowerBorder").transform.FindChild ("Weapon1").gameObject; //bow
		BowHealthBar = Weapon1.transform.FindChild("RawImage").transform.FindChild("HealthBar").gameObject;
		BowLeftMost = BowHealthBar.transform.FindChild ("HPImgLoc").GetComponent<RectTransform> ().localPosition.x;
		BowHPImg = BowHealthBar.transform.FindChild ("HPImg").gameObject;

		//SCATTER SHOT
		Weapon2 = transform.FindChild("LowerBorder").transform.FindChild ("Weapon2").gameObject;
		ScatterHealthBar = Weapon2.transform.FindChild("RawImage").transform.FindChild("HealthBar").gameObject;
		ScatterLeftMost = ScatterHealthBar.transform.FindChild ("HPImgLoc").GetComponent<RectTransform> ().localPosition.x;
		ScatterHPImg = ScatterHealthBar.transform.FindChild ("HPImg").gameObject;

		//BALLISTA
		Weapon3 = transform.FindChild("LowerBorder").transform.FindChild ("Weapon3").gameObject; 
		BallistaHealthBar = Weapon3.transform.FindChild("RawImage").transform.FindChild("HealthBar").gameObject;
		BallistaLeftMost = BallistaHealthBar.transform.FindChild ("HPImgLoc").GetComponent<RectTransform> ().localPosition.x;
		BallistaHPImg = BallistaHealthBar.transform.FindChild ("HPImg").gameObject;

		BuySpearman = transform.FindChild("LowerBorder").transform.FindChild ("BuySpearman").gameObject; //spawn spear
		BuyArcher = transform.FindChild("LowerBorder").transform.FindChild ("BuyArcher").gameObject; //spawn archer

		UpgradeScatter = transform.FindChild("LowerBorder").transform.FindChild ("UpgradeScatter").gameObject;
		UpgradePunchthrough = transform.FindChild("LowerBorder").transform.FindChild ("UpgradePunchthrough").gameObject;
		UpgradeDamage = transform.FindChild("LowerBorder").transform.FindChild ("UpgradeDamage").gameObject;
		UpgradeArmour = transform.FindChild("LowerBorder").transform.FindChild ("UpgradeArmour").gameObject;


		Abilities = new GameObject[AbilitiesSize];
		Abilities [0] = Weapon1;
		Abilities [1] = Weapon2;
		Abilities [2] = Weapon3;
		Abilities [3] = BuySpearman;
		Abilities [4] = BuyArcher;
		Abilities [5] = UpgradeScatter;
		Abilities [6] = UpgradePunchthrough;
		Abilities [7] = UpgradeDamage;
		Abilities [8] = UpgradeArmour;

		updateAll ();
	}
	
	// Update is called once per frame
	void Update () {
		updateWeaponIcons ();
	}

	public void updateWeaponIcons(){
		updateBowReloadBar ();
		updateScatterReloadBar ();
		updateBallistaReloadBar ();

	}

	public void updateBowReloadBar(){
		if (playerScript.bowCounter < playerScript.bowReload) {
			float percentageBow = playerScript.bowCounter / playerScript.bowReload;
			Vector3 newScale = new Vector3 (0.99f * percentageBow + 0.0000001f, 0.80f, 1);
			BowHPImg.GetComponent<RectTransform> ().localScale = newScale; //scaling it.
			Vector3 newLoc = new Vector3 (BowLeftMost + -BowLeftMost * percentageBow, 0, 0);
			BowHPImg.GetComponent<RectTransform> ().localPosition = newLoc;

			if (percentageBow < 0.50f)
				BowHPImg.GetComponent<RawImage> ().color = Color.Lerp (Color.red, Color.yellow, 
					(playerScript.bowReload - playerScript.bowCounter) / (playerScript.bowReload / 2));
			else if (percentageBow >= 0.50f)
				BowHPImg.GetComponent<RawImage> ().color = Color.Lerp (Color.yellow, Color.green, 
					(playerScript.bowReload - playerScript.bowCounter) / (playerScript.bowReload / 2));
		} else {
			playerScript.bowCounter = playerScript.bowReload;
			BowHPImg.GetComponent<RawImage> ().color = Color.green;
		}
	}

	public void updateScatterReloadBar(){
		if (playerScript.scatterCounter < playerScript.scatterReload) {
			float percentageScatter = playerScript.scatterCounter / playerScript.scatterReload;
			Vector3 newScale = new Vector3 (0.99f * percentageScatter + 0.0000001f, 0.80f, 1);
			ScatterHPImg.GetComponent<RectTransform> ().localScale = newScale; //scaling it.
			Vector3 newLoc = new Vector3 (ScatterLeftMost + -ScatterLeftMost * percentageScatter, 0, 0);
			ScatterHPImg.GetComponent<RectTransform> ().localPosition = newLoc;

			if (percentageScatter < 0.50f)
				ScatterHPImg.GetComponent<RawImage> ().color = Color.Lerp (Color.red, Color.yellow, 
					(playerScript.scatterReload - playerScript.scatterCounter) / (playerScript.scatterReload / 2));
			else if (percentageScatter >= 0.50f)
				ScatterHPImg.GetComponent<RawImage> ().color = Color.Lerp (Color.yellow, Color.green, 
					(playerScript.scatterReload - playerScript.scatterCounter) / (playerScript.scatterReload / 2));
		} else {
			playerScript.scatterCounter = playerScript.scatterReload;
			ScatterHPImg.GetComponent<RawImage> ().color = Color.green;
		}
	}
	public void updateBallistaReloadBar(){
		if (playerScript.ballistaCounter < playerScript.ballistaReload) {
			float percentageBallista = playerScript.ballistaCounter / playerScript.ballistaReload;
			Vector3 newScale = new Vector3 (0.99f * percentageBallista + 0.0000001f, 0.80f, 1);
			BallistaHPImg.GetComponent<RectTransform> ().localScale = newScale; //scaling it.
			Vector3 newLoc = new Vector3 (BallistaLeftMost + -BallistaLeftMost * percentageBallista, 0, 0);
			BallistaHPImg.GetComponent<RectTransform> ().localPosition = newLoc;

			if (percentageBallista < 0.50f)
				BallistaHPImg.GetComponent<RawImage> ().color = Color.Lerp (Color.red, Color.yellow, 
					(playerScript.ballistaReload - playerScript.ballistaCounter) / (playerScript.ballistaReload / 2));
			else if (percentageBallista >= 0.50f)
				BallistaHPImg.GetComponent<RawImage> ().color = Color.Lerp (Color.yellow, Color.green, 
					(playerScript.ballistaReload - playerScript.ballistaCounter) / (playerScript.ballistaReload / 2));
		} else {
			playerScript.ballistaCounter = playerScript.ballistaReload;
			BallistaHPImg.GetComponent<RawImage> ().color = Color.green;
		}
	}

	public void SwitchWeapon(int num){
		DisableAbilities ();
		Enable (Abilities [num - 1]);
		Abilities[num-1].transform.FindChild ("RawImage").localScale = enlargedSize;
	}

	public void DisableAbilities(){ //abilities being disabled
		for (int i = 0; i < 3; i++) {
			Disable (Abilities [i]);
		}
	}

	public void Enable(GameObject ability){
		ability.transform.FindChild ("RawImage").GetComponent<RawImage> ().color = Color.white;
	}

	public void Disable(GameObject ability){
		ability.transform.FindChild ("RawImage").GetComponent<RawImage> ().color = Color.grey;
		ability.transform.FindChild ("RawImage").localScale = defaultSize;
	}

	//this greys out if you can't use it
	public void updateAll(){
		GoldText.GetComponent<Text> ().text = "Gold: " + numberMaster.returnGold ().ToString ();
		ScoreText.GetComponent<Text> ().text = "Score: " + numberMaster.returnScore ().ToString ();
		EnemiesKilledText.GetComponent<Text> ().text = "Enemies Killed: " + numberMaster.returnEnemiesKilled ().ToString ();

		if (numberMaster.canBuySpearman ())
			Enable (Abilities [3]);
		else
			Disable (Abilities [3]);

		if (numberMaster.canBuyArcher ())
			Enable (Abilities [4]);
		else
			Disable (Abilities [4]);


		if (numberMaster.canUpgradeScatter ())
			Enable (Abilities [5]);
		else
			Disable (Abilities [5]);
		if (numberMaster.scatterLevel < 5)
			Abilities [5].transform.GetChild (0).transform.FindChild ("GoldText").GetComponent<Text> ().text = 
				"Lvl. " + numberMaster.scatterLevel.ToString () + " - " +
			(numberMaster.baseUpgradeCost + (numberMaster.scaleUpgradeLevel * numberMaster.scatterLevel)).ToString () + "g";
		else
			Abilities [5].transform.GetChild (0).transform.FindChild ("GoldText").GetComponent<Text> ().text = 
				"Lvl. Maxed";
		
		if (numberMaster.canUpgradePunchthrough ())
			Enable (Abilities [6]);
		else
			Disable (Abilities [6]);
		if (numberMaster.punchthroughLevel < 5)
			Abilities [6].transform.GetChild(0).transform.FindChild ("GoldText").GetComponent<Text> ().text = 
				"Lvl. " + numberMaster.punchthroughLevel.ToString() + " - " + 
				(numberMaster.baseUpgradeCost + (numberMaster.scaleUpgradeLevel * numberMaster.punchthroughLevel)).ToString() + "g";
		else
			Abilities [6].transform.GetChild (0).transform.FindChild ("GoldText").GetComponent<Text> ().text = 
				"Lvl. Maxed";
		
		if (numberMaster.canUpgradeDamage ())
			Enable (Abilities [7]);
		else
			Disable (Abilities [7]);
		if (numberMaster.damageLevel < 5)
			Abilities [7].transform.GetChild(0).transform.FindChild ("GoldText").GetComponent<Text> ().text = 
				"Lvl. " + numberMaster.damageLevel.ToString() + " - " + 
				(numberMaster.baseUpgradeCost + (numberMaster.scaleUpgradeLevel * numberMaster.damageLevel)).ToString() + "g";
		else
			Abilities [7].transform.GetChild (0).transform.FindChild ("GoldText").GetComponent<Text> ().text = 
				"Lvl. Maxed";
		
		if (numberMaster.canUpgradeArmour ())
			Enable (Abilities [8]);
		else
			Disable (Abilities [8]);
		if (numberMaster.scatterLevel < 5)
			Abilities [8].transform.GetChild(0).transform.FindChild ("GoldText").GetComponent<Text> ().text = 
				"Lvl. " + numberMaster.armourLevel.ToString() +   " - " + 
				(numberMaster.baseUpgradeCost + (numberMaster.scaleUpgradeLevel * numberMaster.armourLevel)).ToString() + "g";
		else
			Abilities [8].transform.GetChild (0).transform.FindChild ("GoldText").GetComponent<Text> ().text = 
				"Lvl. Maxed";
	}
}
