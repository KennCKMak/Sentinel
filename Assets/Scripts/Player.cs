using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

	private GameMaster gameMaster;

	public GameObject arrowTip;
	public GameObject arrowBase;

	private bool alive;

	public float health;
	public float maxHealth;
	public float reloadTime;
	public float wpnDmg;

	private int weapon = 1;
	//this is the angle for firing...
	private float angleEven = 2f;
	private float angleOdd = 2f;
	public GameObject arrow;



	// Use this for initialization
	void Start () {	
		gameMaster = GameObject.Find ("GameMaster").GetComponent<GameMaster> ();
		health = maxHealth;
	}
	
	// Update is called once per frame
	void Update() {
		//rotating to mouse location
		Vector3 dir = Input.mousePosition - Camera.main.WorldToScreenPoint(transform.position);
		float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg; 
		transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);



		//switching weapons
		if (Input.GetKeyDown(KeyCode.Alpha1))
			weapon = 1;
		if (Input.GetKeyDown (KeyCode.Alpha2))
			weapon = 2;
		if (Input.GetKeyDown (KeyCode.Alpha3))
			weapon = 3;

		//shooting
		if(Input.GetMouseButtonDown(0)) {
			switch (weapon) {
			case(1):
				FireArrow ();
				break;
			case(2):
				FireScatterArrow (retScatterLevel()+1);
				break;
			case(3):
				Debug.Log ("Firing Ballista");
				FireBallista ();
				break;
			default:
				break;
			}
		}
	}

	void FireArrow(){
		Vector3 mousePosition = Camera.main.ScreenToWorldPoint (Input.mousePosition); 
		mousePosition.z = 0;

		Quaternion rot = Quaternion.LookRotation(arrowBase.transform.position - mousePosition, Vector3.forward);
		GameObject newArrow = Instantiate (arrow, arrowBase.transform.position, rot) as GameObject;
		newArrow.transform.eulerAngles = new Vector3(0, 0, transform.eulerAngles.z-89.1f);

		newArrow.tag = "Player_Projectile";
		newArrow.GetComponent<Player_Projectile> ();
		newArrow.GetComponent<Player_Projectile> ().speed = 10f;
		newArrow.GetComponent<Player_Projectile> ().punchthrough = retPunchthroughLevel ();
		newArrow.GetComponent<Player_Projectile> ().wpnDmg = wpnDmg + wpnDmg * retDamageLevel() * 0.5f;
	}

	void FireArrow(float angle){ //this is used to offset things
		Vector3 mousePosition = Camera.main.ScreenToWorldPoint (Input.mousePosition); 
		mousePosition.z = 0;

		Quaternion rot = Quaternion.LookRotation(arrowBase.transform.position - mousePosition, Vector3.forward);
		GameObject newArrow = Instantiate (arrow, arrowBase.transform.position, rot) as GameObject;
		newArrow.transform.eulerAngles = new Vector3(0, 0, transform.eulerAngles.z-89.1f+angle);

		newArrow.tag = "Player_Projectile";
		newArrow.GetComponent<Player_Projectile> ();
		newArrow.GetComponent<Player_Projectile> ().speed = 10f;
		newArrow.GetComponent<Player_Projectile> ().punchthrough = retPunchthroughLevel ();
		newArrow.GetComponent<Player_Projectile> ().wpnDmg = wpnDmg + wpnDmg * retDamageLevel() * 0.5f;
	}

	void FireScatterArrow(int num){
		if (num == 0 || num == 1)
			FireArrow ();
		else {
			if (num % 2 == 0) {//even number of arrows 
				for (int i = 0; i < num; i++) {
					i++;
					FireArrow (angleEven + i * angleEven);
					FireArrow (-angleEven - i * angleEven);
				}
			} else { //odd number of arrows
				FireArrow(); //fires a straight arrow
				num--;// we fired one
				for (int i = 0; i < num; i++) {
					i++; //we fired twice, once in for and once here
					FireArrow (angleOdd + i * angleOdd); //fires one arrow angled up
					FireArrow (-angleOdd - i * angleOdd); //fires one arrow angled down
				}
			}
		}
	}
	void FireBallista(){ //massive punchthrough, increased size, damage x10
		Vector3 mousePosition = Camera.main.ScreenToWorldPoint (Input.mousePosition); 
		mousePosition.z = 0;
		Quaternion rot = Quaternion.LookRotation(arrowBase.transform.position - mousePosition, Vector3.forward);
		GameObject newArrow = Instantiate (arrow, arrowBase.transform.position, rot) as GameObject;
		newArrow.transform.eulerAngles = new Vector3(0, 0, transform.eulerAngles.z-89.1f);
		newArrow.tag = "Player_Projectile";

		newArrow.GetComponent<Player_Projectile> ().speed = 7f;
		newArrow.GetComponent<Player_Projectile> ().punchthrough = retPunchthroughLevel () + 10;
		newArrow.GetComponent<Player_Projectile> ().wpnDmg = (wpnDmg + wpnDmg * retDamageLevel() * 0.5f)*10;
		Vector2 newSize = new Vector2 (newArrow.transform.localScale.x, newArrow.transform.localScale.y);
		newSize *= 3;
		newArrow.transform.localScale = newSize;
	}


	void OnTriggerEnter2D(Collider2D col){
		if (col.tag == "Enemy_Projectile") {
			health = health - col.GetComponent<Enemy_Projectile> ().wpnDmg;
			Destroy (col.gameObject);
			//if ded
		}

	}

	//getting upgrades from main
	int retScatterLevel(){
		return gameMaster.scatterLevel;
	}
	int retPunchthroughLevel(){
		return gameMaster.punchthroughLevel;
	}
	int retDamageLevel(){
		return gameMaster.damageLevel;
	}
	int retArmourLevel(){
		return gameMaster.armourLevel;
	}

}
