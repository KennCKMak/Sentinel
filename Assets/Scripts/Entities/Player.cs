using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class Player : MonoBehaviour {

	private NumberMaster numberMaster;
	private GameMaster gameMaster;

	public GameObject arrowTip;
	public GameObject arrowBase;

	private bool alive;

	public float health;
	public float maxHealth;
	public float wpnDmg;

	public float bowCounter;
	public float bowReload = 0.3f;
	public float scatterCounter;
	public float scatterReload = 1.0f;
	public float ballistaCounter;
	public float ballistaReload = 5.0f;


	private int weapon = 1;
	//this is the angle for firing...
	private float angle = 2f;
	public GameObject arrow;

	protected GameObject HealthBar; //access to the health bar
	protected GameObject HPImg;
	protected float leftMost;

	private GameObject canvas;
	private CanvasScript canvasScript;
	// Use this for initialization
	void Start () {	
		gameMaster = GameObject.Find ("GameMaster").GetComponent<GameMaster> ();
		numberMaster = GameObject.Find ("GameMaster").GetComponent<NumberMaster> ();

		health = maxHealth;
		alive = true;
		canvas = GameObject.Find ("Canvas").gameObject;
		canvasScript = canvas.GetComponent<CanvasScript> ();
		HealthBar = canvas.transform.FindChild ("UpperBorder").transform.FindChild ("HealthBar").gameObject;
		leftMost = HealthBar.transform.FindChild ("HPImgLoc").GetComponent<RectTransform> ().localPosition.x;

		HPImg = HealthBar.transform.FindChild ("HPImg").gameObject;
		updateHP ();

		StartCoroutine (LateStart (0.1f));
		bowCounter = bowReload;
		scatterCounter = scatterReload;
		ballistaCounter = ballistaReload;
	}

	IEnumerator LateStart(float num){
		yield return new WaitForSeconds (num);
		canvasScript.SwitchWeapon (1);
	}


	// Update is called once per frame
	void Update() {
		if (bowCounter < bowReload)
			bowCounter += Time.deltaTime;
		if (scatterCounter < scatterReload)
			scatterCounter += Time.deltaTime;
		if (ballistaCounter < ballistaReload)
			ballistaCounter += Time.deltaTime;
		//rotating to mouse location
		Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		if (mousePos.y > -3.215f && mousePos.y < 4.19f) {
			Vector3 dir = Input.mousePosition - Camera.main.WorldToScreenPoint (transform.position);
			float angle = Mathf.Atan2 (dir.y, dir.x) * Mathf.Rad2Deg; 
			transform.rotation = Quaternion.AngleAxis (angle, Vector3.forward);
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
					FireBallista ();
					break;
				default:
					break;
				}
			}
		} else {
			transform.rotation = Quaternion.Lerp (transform.rotation, new Quaternion(0, 0, 180, 1), 0.1f*Time.deltaTime);
		}



		//switching weapons
		if (Input.GetKeyDown(KeyCode.Alpha1))
			WeaponSelect (1);
		if (Input.GetKeyDown (KeyCode.Alpha2))
			WeaponSelect (2);
		if (Input.GetKeyDown (KeyCode.Alpha3))
			WeaponSelect (3);


		updateHP ();
	}

	void FireArrow(){
		if (bowCounter >= bowReload) {
			bowCounter = 0;
			Vector3 mousePosition = Camera.main.ScreenToWorldPoint (Input.mousePosition); 
			mousePosition.z = 0;

			Quaternion rot = Quaternion.LookRotation (arrowBase.transform.position - mousePosition, Vector3.forward);
			GameObject newArrow = Instantiate (arrow, arrowBase.transform.position, rot) as GameObject;
			newArrow.transform.eulerAngles = new Vector3 (0, 0, transform.eulerAngles.z - 89.1f);

			newArrow.tag = "Player_Projectile";
			newArrow.GetComponent<Player_Projectile> ();
			newArrow.GetComponent<Player_Projectile> ().speed = 10f;
			newArrow.GetComponent<Player_Projectile> ().punchthrough = retPunchthroughLevel ();
			newArrow.GetComponent<Player_Projectile> ().wpnDmg = wpnDmg + wpnDmg * retDamageLevel () * 0.5f;
		}
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
		if (scatterCounter >= scatterReload) {
			scatterCounter = 0;
			float tempAngle = angle - (0.15f * (retScatterLevel () + 1));
			if (num == 0)
				FireArrow ();
			else {
				if (num % 2 == 0) {//even number of arrows 
					for (int i = 0; i < num; i++) {
						i++;
						FireArrow (tempAngle / 2 + i * tempAngle);
						FireArrow (-tempAngle / 2 - i * tempAngle);
						//FireArrow (tempAngle + i * tempAngle - tempAngle/1.5f);
						//FireArrow (-tempAngle - i * tempAngle + tempAngle/1.5f);
					}
				} else { //odd number of arrows
					FireArrow (0); //fires a straight arrow
					num--;// we fired one
					for (int i = 0; i < num; i++) {
						i++; //we fired twice, once in for and once here
						FireArrow (tempAngle + i * tempAngle); //fires one arrow angled up
						FireArrow (-tempAngle - i * tempAngle); //fires one arrow angled down
					}
				}
			}
		}
	}

	void FireBallista(){ //massive punchthrough, increased size, damage x10
		if (ballistaCounter >= ballistaReload) {
			ballistaCounter = 0;
			Vector3 mousePosition = Camera.main.ScreenToWorldPoint (Input.mousePosition); 
			mousePosition.z = 0;
			Quaternion rot = Quaternion.LookRotation (arrowBase.transform.position - mousePosition, Vector3.forward);
			GameObject newArrow = Instantiate (arrow, arrowBase.transform.position, rot) as GameObject;
			newArrow.transform.eulerAngles = new Vector3 (0, 0, transform.eulerAngles.z - 89.1f);
			newArrow.tag = "Player_Projectile";

			newArrow.GetComponent<Player_Projectile> ().speed = 7f;
			newArrow.GetComponent<Player_Projectile> ().punchthrough = retPunchthroughLevel () + 10;
			newArrow.GetComponent<Player_Projectile> ().wpnDmg = (wpnDmg + wpnDmg * retDamageLevel () * 0.5f) * 10;
			Vector2 newSize = new Vector2 (newArrow.transform.localScale.x, newArrow.transform.localScale.y);
			newSize *= 3;
			newArrow.transform.localScale = newSize;
		}
	}

	void OnTriggerEnter2D(Collider2D col){
		if (col.tag == "Enemy_Projectile") {
			takeDamage(col.GetComponent<Enemy_Projectile> ().wpnDmg);
			Destroy (col.gameObject);
			//if ded
		}
	}

	//_______HEALTH BAR FUNCTIONS_______//
	public void updateHP(){
		if (alive) {
			float percentage = health / maxHealth;
			Vector3 newScale = new Vector3 (0.98f * percentage + 0.0000001f, 0.80f, 1);
			HPImg.GetComponent<RectTransform> ().localScale = newScale; //scaling it.
			Vector3 newLoc = new Vector3 (leftMost + -leftMost * percentage, 0, 0);
			//leftMost + -leftMost*percentage //456.45
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
			gameMaster.refreshAllyList ();
			transform.tag = "Untagged";
			StartCoroutine (LoseGame ());
		}
	}

	IEnumerator LoseGame(){
		yield return new WaitForSeconds (2f);
		GameObject.Find ("Canvas").transform.FindChild ("DefeatScreen").gameObject.SetActive (true);
		yield return new WaitForSeconds (6f);
		SceneManager.LoadScene (1);
	}
	//getting upgrades from main
	int retScatterLevel(){
		return numberMaster.scatterLevel+1;
	}
	int retPunchthroughLevel(){
		return numberMaster.punchthroughLevel;
	}
	int retDamageLevel(){
		return numberMaster.damageLevel;
	}
	int retArmourLevel(){
		return numberMaster.armourLevel;
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

	}

	//selecting weapon
	public void WeaponSelect(int num){
		weapon = num;
		switch (num) {
		case(1):
			canvasScript.SwitchWeapon (1);
			break;
		case(2):
			canvasScript.SwitchWeapon (2);
			break;
		case(3):
			canvasScript.SwitchWeapon (3);
			break;
		default:
			break;
		}
	}
}
