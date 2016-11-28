using UnityEngine;
using UnityEngine.UI;
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

	protected GameObject HealthBar; //access to the health bar
	protected GameObject HPImg;
	protected float leftMost = -109.7f;


	// Use this for initialization
	void Start () {	
		gameMaster = GameObject.Find ("GameMaster").GetComponent<GameMaster> ();

		health = maxHealth;
		alive = true;
		HealthBar = GameObject.Find ("Canvas").transform.FindChild ("UpperBorder").transform.FindChild ("HealthBar").gameObject;
		HPImg = HealthBar.transform.FindChild ("HPImg").gameObject;
		updateHP ();
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
		updateHP ();
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
			takeDamage(col.GetComponent<Enemy_Projectile> ().wpnDmg);
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
	//_______Health Functions_______//
	public void healDamage(float num){
		health = health + num;
		updateHP ();
	}
	public void takeDamage(float  num){
		health = health - num * (1 - retArmourLevel() * 0.1f);
		//armouru takes away 10% of the damage, up to cap of lvl5
		updateHP ();
	}
	public float returnHP(){
		return health;
	}
	//_______Alive Check Functions_______//
	public bool isAlive(){
		return alive;
	}
	public void setAlive(bool b){
		setAlive (b);
	}	//_______HEALTH BAR FUNCTIONS_______//

	public void updateHP(){
		if (alive) {
			float percentage = health / maxHealth;
			Vector3 newScale = new Vector3 (0.98f * percentage + 0.0000001f, 0.80f, 1);
			HPImg.GetComponent<RectTransform> ().localScale = newScale; //scaling it.
			Vector3 newLoc = new Vector3 (leftMost + -leftMost * percentage, 0, 0);
			//leftMost + -leftMost*percentage
			HPImg.GetComponent<RectTransform> ().localPosition = newLoc;


			if (percentage > 0.50f)
				HPImg.GetComponent<RawImage> ().color = Color.Lerp (Color.green, Color.yellow, (maxHealth - health) / (maxHealth / 2));
		//i.e. @ 75hp, 100 - 75 = 25, divided by 50 gives you 0.5
		else if (percentage <= 0.50f)
				HPImg.GetComponent<RawImage> ().color = Color.Lerp (Color.yellow, Color.red, (maxHealth - health) / (maxHealth / 2));
			//i.e. @ 25hp, 50 - 25 = 25, divided by 50 gives you 0.5 again
			Die ();
		}
	}
	public void Die(){ //if dead
		if (health <= 0 && alive) {
			alive = !alive;
			GetComponent<CircleCollider2D> ().enabled = false;
			Destroy (gameObject, 1f);
		}
	}
}
