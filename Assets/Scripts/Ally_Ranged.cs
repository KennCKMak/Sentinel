﻿using UnityEngine;
using System.Collections;

public class Ally_Ranged : MonoBehaviour {

	private GameMaster gameMaster;

	private float health;
	public float maxHealth;

	public GameObject HealthBarPrefab; //the health bar prefab
	private GameObject HealthBar; //access to the health bar
	private Transform HealthBarLocation; //location of the health bar
	private Transform HPImg;
	private float leftMost = -0.475f;


	private bool alive;
	private int indexLoc; //its position in the gameMaster array 

	public GameObject target;
	public float range;
	public float wpnDmg;
	public float wpnSpeed;
	public bool inCombat = false;

	public GameObject arrowPrefab;
	private GameObject idleImg;
	private GameObject fireImg;
	private GameObject arrowSpawn;
	private Quaternion defaultRot = new Quaternion (0, 0, 0, 1);

	private float searchTime; //how long before it checks again for a target
	private float elapsedTime; //how long since it last attacked


	// Use this for initialization
	void Start () {
		gameMaster = GameObject.Find ("GameMaster").GetComponent<GameMaster> ();

		HealthBarLocation = transform.FindChild ("HealthBar").transform;
		HealthBar = Instantiate (HealthBarPrefab, HealthBarLocation.position, Quaternion.identity) as GameObject;
		HealthBar.GetComponent<healthBarTarget> ().target = HealthBarLocation;
		HPImg = HealthBar.transform.FindChild ("HPImg").transform;

		idleImg = transform.FindChild ("IdleImg").gameObject;
		arrowSpawn = idleImg.transform.FindChild ("ArrowSpawn").gameObject;
		alive = true;

		health = maxHealth;
		updateHP ();

	}

	// Update is called once per frame
	void Update () {
		if (alive) {

			GetTarget ();

			if (target != null) { //if I have a target...
				Vector3 dir = target.transform.position - transform.position;
				if (dir.magnitude < range) { //if within range
					float angle = Mathf.Atan2 (dir.y, dir.x) * Mathf.Rad2Deg + 180;
					Quaternion newRotation = Quaternion.AngleAxis (angle, Vector3.forward);
					transform.rotation = Quaternion.Lerp (transform.rotation, newRotation, 15 * Time.deltaTime);
					inCombat = true;
				} else { //not in range
					inCombat = false;
					transform.rotation = Quaternion.Lerp (transform.rotation, defaultRot, Time.deltaTime);
				}
				if (inCombat && target != null)
					Fire ();
				else
					elapsedTime = 0;
			} else {
				GetTarget ();
			}
		}
		
	}

	void GetTarget(){
		if (searchTime > 1) {   //check for new target every 1 seconds
			for (int i = 0; i < gameMaster.EnemyList.Count; i++) {
				if (target == null)
					target = gameMaster.EnemyList [i].gameObject;
				else if (target != null) {
					//looking for lowest distance
					if (Vector3.Distance (transform.position, gameMaster.EnemyList [i].transform.position)
						< Vector3.Distance (transform.position, target.transform.position)) {
						target = gameMaster.EnemyList [i].gameObject;
					}
				}
			}
			searchTime = 0;
		}
		searchTime += Time.deltaTime;
	}


	void Fire() {

		if (elapsedTime > wpnSpeed + Random.value*0.5f){
			GameObject newArrow = Instantiate (arrowPrefab, arrowSpawn.transform.position, arrowSpawn.transform.rotation) as GameObject;
			newArrow.transform.eulerAngles = new Vector3(0, 0, transform.eulerAngles.z+90.9f);
			newArrow.tag = "Player_Projectile";
			newArrow.AddComponent<Player_Projectile> ();
			newArrow.GetComponent<Player_Projectile> ().speed = 10f;
			newArrow.GetComponent<Player_Projectile> ().wpnDmg = wpnDmg;
			elapsedTime = 0;
		}
		elapsedTime += Time.deltaTime;
	}


	void OnTriggerEnter2D(Collider2D col){
		if (col.tag == "Enemy_Projectile") {
			health = health - col.GetComponent<Enemy_Projectile> ().wpnDmg;
			GameObject newProp = Instantiate (col.GetComponent<Enemy_Projectile> ().bulletProp,
				col.transform.position, col.transform.rotation) as GameObject;
			newProp.transform.parent = transform;
			Destroy (col.gameObject);
			updateHP ();
			//if ded


		}
	}

	void updateHP(){
		float percentage = health / maxHealth;
		Vector3 newScale = new Vector3 (0.95f*percentage, 0.60f, transform.localScale.z);
		HPImg.localScale = newScale; //scaling it.
		Vector3 newPos = new Vector3 (leftMost + -leftMost*percentage, 0.0f, 0.0f); //moving hp loc to left
		//-0.475 is the leftmost, add the percentage to return it back to the middle
		HPImg.localPosition = newPos;
		if (percentage > 0.50f)
			HPImg.GetComponent<SpriteRenderer> ().color = Color.Lerp (Color.green, Color.yellow, (maxHealth - health) / (maxHealth / 2));
		//i.e. @ 75hp, 100 - 75 = 25, divided by 50 gives you 0.5
		else if (percentage <= 0.25f)
			HPImg.GetComponent<SpriteRenderer> ().color = Color.Lerp (Color.yellow, Color.red, (maxHealth/ - health) / (maxHealth / 2));
		//i.e. @ 25hp, 50 - 25 = 25, divided by 50 gives you 0.5 again

		if (health <= 0 && alive) {
			alive = !alive;
			GetComponent<CircleCollider2D> ().enabled = false;
			HPImg.GetComponent<SpriteRenderer> ().enabled = false;
			transform.tag = "Untagged";
			Destroy (gameObject, 1f);
			gameMaster.archerCount--;
			gameMaster.Archers [indexLoc] = null;
			gameMaster.refreshAllyList ();
		}
	}


	public void takeDamage(float  num){
		health = health - num;
		updateHP ();
	}
	public bool isAlive(){
		return alive;
	}
	public void setIndexLoc(int n){
		indexLoc = n;
	}
}