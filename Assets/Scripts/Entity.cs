using UnityEngine;
using System.Collections;


//this is the base class
//All units will have this, excluding the player
//Health, Damage, Weapon Speed

public class Entity : MonoBehaviour {
	/*protected GameMaster gameMaster;

	public float health;
	public float maxHealth;

	public float range;
	public float wpnDmg;
	public float wpnSpeed;
	public bool inCombat = false;

	public int indexLoc;//used for the gameMaster arrays

	protected bool alive;
	public GameObject HealthBarPrefab; //the health bar prefab
	protected GameObject HealthBar; //access to the health bar
	protected Transform HealthBarLocation; //location of the health bar
	protected Transform HPImg;
	protected float leftMost = -0.475f;




	public Entity(){
		alive = true;
		health = maxHealth;
	}
	void Start(){
		gameMaster = GameObject.Find ("GameMaster").GetComponent<GameMaster> ();
		createHealthBar ();
		updateHP ();
	}

	void createHealthBar(){
		HealthBarLocation = transform.FindChild ("HealthBar").transform;
		HealthBar = Instantiate (HealthBarPrefab, HealthBarLocation.position, Quaternion.identity) as GameObject;
		HealthBar.GetComponent<healthBarTarget> ().target = HealthBarLocation;
		HPImg = HealthBar.transform.FindChild ("HPImg").transform;
	}

	public void updateHP(){
		float percentage = health / maxHealth;
		Debug.Log ("percentage = " + percentage);
		Vector3 newScale = new Vector3 (0.95f*percentage, 0.60f, transform.localScale.z);
		HPImg.localScale = newScale; //scaling it.
		Vector3 newPos = new Vector3 (leftMost + -leftMost*percentage, 0.0f, 0.0f); //moving hp loc to left
		//-0.475 is the leftmost, add the percentage to return it back to the middle
		HPImg.localPosition = newPos;

			
		if (percentage > 0.50f)
			HPImg.GetComponent<SpriteRenderer> ().color = Color.Lerp (Color.green, Color.yellow, (maxHealth - health) / (maxHealth / 2));
		//i.e. @ 75hp, 100 - 75 = 25, divided by 50 gives you 0.5
		else if (percentage <= 0.50f)
			HPImg.GetComponent<SpriteRenderer> ().color = Color.Lerp (Color.yellow, Color.red, (maxHealth/ - health) / (maxHealth / 2));
		//i.e. @ 25hp, 50 - 25 = 25, divided by 50 gives you 0.5 again

		Die ();
	}


	public void rotateToTarget(Vector3 targetpos, Vector3 mypos){
		Vector3 dir = targetpos - mypos;
		float angle = Mathf.Atan2 (dir.y, dir.x) * Mathf.Rad2Deg + 180;
		Quaternion newRotation = Quaternion.AngleAxis (angle, Vector3.forward);
		transform.rotation = Quaternion.Lerp (transform.rotation, newRotation, 15 * Time.deltaTime);
	}

	public void rotateToTarget(Quaternion newRot, Quaternion myRot){
		transform.rotation = Quaternion.Lerp (myRot, newRot, 15 * Time.deltaTime);
	}

	public void healDamage(float num){
		health = health + num;
		updateHP ();
	}

	public void takeDamage(float  num){
		health = health - num;
		updateHP ();
	}
	public float returnHP(){
		return health;
	}

	public bool isAlive(){
		return alive;
	}
	public void setAlive(bool b){
		setAlive (b);
	}

	public GameMaster GameMasterAccess(){
		return gameMaster;
	}

	public void setIndexLoc(int n){
		indexLoc = n;
	}
	public int retIndexLoc(){
		return indexLoc;
	}*/
}
