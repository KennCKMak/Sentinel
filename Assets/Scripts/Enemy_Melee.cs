using UnityEngine;
using System.Collections;

public class Enemy_Melee : MonoBehaviour {

	private GameMaster gameMaster;

	private GameObject target;
	public float speed;
	public float health;
	public float maxHealth;

	public GameObject HealthBarPrefab; //the health bar prefab
	private GameObject HealthBar; //access to the health bar
	private Transform HealthBarLocation; //location of the health bar
	private Transform HPImg;
	private float leftMost = -0.475f;


	private bool walking;
	private bool alive;


	public float range;
	public float wpnDmg;
	public float wpnSpeed;
	public bool inCombat = false;

	private float searchTime; //how long before it checks again for a target
	private float elapsedTime; //how long since it last attacked

	private Vector3 startLoc;
	private Vector3 endLoc;

	private GameObject idleImg;
	private GameObject fireImg;
	private GameObject bulletSpawn;



	private Rigidbody2D rb;

	// Use this for initialization
	void Start () {
		gameMaster = GameObject.Find ("GameMaster").GetComponent<GameMaster> ();
		target = GameObject.Find ("MainChar");


		HealthBarLocation = transform.FindChild ("HealthBar").transform;
		HealthBar = Instantiate (HealthBarPrefab, HealthBarLocation.position, Quaternion.identity) as GameObject;
		HealthBar.GetComponent<healthBarTarget> ().target = HealthBarLocation;
		HPImg = HealthBar.transform.FindChild ("HPImg").transform;

		idleImg = transform.FindChild ("IdleImg").gameObject;
		fireImg = transform.FindChild ("FireImg").gameObject;
		endLoc = new Vector3 (gameMaster.xEnd, transform.position.y, 0);
		walking = true;
		alive = true;

		health = maxHealth;
		updateHP ();
		rb = transform.GetComponent<Rigidbody2D> ();
		rb.constraints = RigidbodyConstraints2D.FreezeRotation;

	}

	// Update is called once per frame
	void Update () {
		if (alive){
			GetTarget ();
			if (walking) {
				Walk ();
			} else { //not walking, in range
				Vector3 dir = target.transform.position - transform.position;
				float angle = Mathf.Atan2 (dir.y, dir.x) * Mathf.Rad2Deg;
				transform.rotation = Quaternion.AngleAxis (angle, Vector3.forward);
				rb.velocity = new Vector2 (0, 0);
			}
		} else {
			rb.velocity = new Vector2 (0, 0);
		}
	}

	void GetTarget(){
		if (endLoc.x - transform.position.x < range) {
			walking = false;
			rb.velocity = new Vector2(0, 0);
			idleImg.SetActive(false);
			fireImg.SetActive (true);
		} else {
			walking = true;
			transform.rotation = Quaternion.Euler (Vector3.zero);
			idleImg.SetActive(true);
			fireImg.SetActive (false);
		}
	}

	void Walk (){
		rb.velocity = (Vector2.right * speed * Time.deltaTime);
	}

	void OnTriggerEnter2D(Collider2D col){
		if (col.tag == "Player_Projectile") {
			health = health - col.GetComponent<Player_Projectile> ().wpnDmg;
			if (col.GetComponent<Player_Projectile> ().punchthrough >= 1) {
				col.GetComponent<Player_Projectile> ().punchthrough--;
			} else if (col.GetComponent<Player_Projectile> ().punchthrough < 1) {
				GameObject newProp = Instantiate (col.GetComponent<Player_Projectile> ().arrowProp,
					col.transform.position, col.transform.rotation) as GameObject;
				newProp.transform.parent = transform;
				col.GetComponent<Player_Projectile> ().punchthrough--;
				Destroy (col.gameObject);
			}
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
			walking = false;
			speed = 0;
			GetComponent<CircleCollider2D> ().enabled = false;
			HPImg.GetComponent<SpriteRenderer> ().enabled = false;
			transform.tag = "Untagged";
			Destroy (gameObject, 1f);
			gameMaster.enemyCount--;
			gameMaster.refreshEnemyList ();
		}
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

}
