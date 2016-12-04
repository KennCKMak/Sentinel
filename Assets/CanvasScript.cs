using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CanvasScript : MonoBehaviour {


	private NumberMaster numberMaster;
	// Use this for initialization
	private Vector3 defaultSize;
	private Vector3 enlargedSize;


	private GameObject Weapon1;
	private GameObject Weapon2;
	private GameObject Weapon3;
	private GameObject Ability4;
	private GameObject BuySpearman;
	private GameObject BuyArcher;

	private GameObject UpgradeScatter;
	private GameObject UpgradePunchthrough;
	private GameObject UpgradeDamage;
	private GameObject UpgradeArmor;

	private int AbilitiesSize = 9;
	//private GameObject Ability7;

	private GameObject[] Abilities;

	void Start () {
		numberMaster = GameObject.Find ("GameMaster").GetComponent<NumberMaster> ();
		defaultSize = new Vector3 (1.0f, 1.0f, 1.0f);
		enlargedSize = new Vector3 (1.2f, 1.2f, 1.0f);

		Weapon1 = transform.FindChild("LowerBorder").transform.FindChild ("Weapon1").gameObject; //bow
		Weapon2 = transform.FindChild("LowerBorder").transform.FindChild ("Weapon2").gameObject; //scatter
		Weapon3 = transform.FindChild("LowerBorder").transform.FindChild ("Weapon3").gameObject; //ballista

		BuySpearman = transform.FindChild("LowerBorder").transform.FindChild ("BuySpearman").gameObject; //spawn spear
		BuyArcher = transform.FindChild("LowerBorder").transform.FindChild ("BuyArcher").gameObject; //spawn archer

		UpgradeScatter = transform.FindChild("LowerBorder").transform.FindChild ("UpgradeScatter").gameObject;
		UpgradePunchthrough = transform.FindChild("LowerBorder").transform.FindChild ("UpgradePunchthrough").gameObject;
		UpgradeDamage = transform.FindChild("LowerBorder").transform.FindChild ("UpgradeDamage").gameObject;
		UpgradeArmor = transform.FindChild("LowerBorder").transform.FindChild ("UpgradeArmor").gameObject;


		Abilities = new GameObject[AbilitiesSize];
		Abilities [0] = Weapon1;
		Abilities [1] = Weapon2;
		Abilities [2] = Weapon3;
		Abilities [3] = BuySpearman;
		Abilities [4] = BuyArcher;
		Abilities [5] = UpgradeScatter;
		Abilities [6] = UpgradePunchthrough;
		Abilities [7] = UpgradeDamage;
		Abilities [8] = UpgradeArmor;

		updateAbilities ();
	}
	
	// Update is called once per frame
	void Update () {
		
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
		//ability.transform.FindChild ("RawImage").localScale = defaultSize;
	}

	//this greys out if you can't use it
	public void updateAbilities(){
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
		
		if (numberMaster.canUpgradePunchthrough ())
			Enable (Abilities [6]);
		else
			Disable (Abilities [6]);
		
		if (numberMaster.canUpgradeDamage ())
			Enable (Abilities [7]);
		else
			Disable (Abilities [7]);
		
		if (numberMaster.canUpgradeArmour ())
			Enable (Abilities [8]);
		else
			Disable (Abilities [8]);
	}
}
