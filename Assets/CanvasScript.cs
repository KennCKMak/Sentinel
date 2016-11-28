using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CanvasScript : MonoBehaviour {

	// Use this for initialization
	private Vector3 defaultSize;
	private Vector3 enlargedSize;


	private GameObject Ability1;
	private GameObject Ability2;
	private GameObject Ability3;
	private GameObject Ability4;
	private GameObject Ability5;
	private GameObject Ability6;

	private int AbilitiesSize = 6;
	//private GameObject Ability7;

	private GameObject[] Abilities;

	void Start () {
		defaultSize = new Vector3 (1.0f, 1.0f, 1.0f);
		enlargedSize = new Vector3 (1.2f, 1.2f, 1.0f);

		Ability1 = transform.FindChild("LowerBorder").transform.FindChild ("Ability1").gameObject; //bow
		Ability2 = transform.FindChild("LowerBorder").transform.FindChild ("Ability2").gameObject; //scatter
		Ability3 = transform.FindChild("LowerBorder").transform.FindChild ("Ability3").gameObject; //ballista

		Ability4 = transform.FindChild("LowerBorder").transform.FindChild ("Ability4").gameObject;

		Ability5 = transform.FindChild("LowerBorder").transform.FindChild ("Ability5").gameObject; //spawn spear
		Ability6 = transform.FindChild("LowerBorder").transform.FindChild ("Ability6").gameObject; //spawn archer
		//Ability7 = transform.FindChild ("Ability7").gameObject; //placeholder

		Abilities = new GameObject[AbilitiesSize];
		Abilities [0] = Ability1;
		Abilities [1] = Ability2;
		Abilities [2] = Ability3;
		Abilities [3] = Ability4;
		Abilities [4] = Ability5;
		Abilities [5] = Ability6;

	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void SwitchWeapon(int num){
		DisableAbilities ();
		Abilities [num-1].transform.FindChild ("RawImage").GetComponent<RawImage> ().color = Color.white;
		Abilities [num-1].transform.FindChild ("RawImage").localScale = enlargedSize;
	}

	public void DisableAbilities(){ //abilities being disabled
		for (int i = 0; i < 3; i++) {
			Abilities [i].transform.FindChild ("RawImage").GetComponent<RawImage> ().color = Color.grey;
			Abilities [i].transform.FindChild ("RawImage").localScale = defaultSize;
		}
	}
}
