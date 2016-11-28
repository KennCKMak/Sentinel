using UnityEngine;
using System.Collections;

public class Enemy_Ranged : MonoBehaviour {

	private GameObject idleImg;
	private GameObject fireImg;
	private GameObject bulletSpawn;
	public GameObject bulletPrefab;

	public EnemyClass enemy;

	private Rigidbody2D rb;

	// Use this for initialization
	void Start () {
		enemy.setTarget (GameObject.FindGameObjectWithTag ("Player"));
		enemy.myClass = EnemyClass.unitTypes.RIFLE;

		idleImg = transform.FindChild ("IdleImg").gameObject;
		fireImg = transform.FindChild ("FireImg").gameObject;
		bulletSpawn = transform.FindChild ("BulletSpawn").gameObject;

		rb = transform.GetComponent<Rigidbody2D> ();
		rb.constraints = RigidbodyConstraints2D.FreezeRotation;

	}

	// Update is called once per frame
	void Update () {
		if (enemy.isAlive ()) {
			enemy.GetTarget ();
			if (enemy.returnTarget () != null) {
				if (enemy.inTargetDist ()) { //not walking, in range
					rb.velocity = new Vector2 (0, 0);
					enemy.rotateToTarget (enemy.returnTargetPosition (), transform.position);
					enemy.inCombat = true;
					idleImg.SetActive (false);
					fireImg.SetActive (true);
					Attack ();
				} else {
					Walk ();
					enemy.inCombat = false;
					idleImg.SetActive (true);
					fireImg.SetActive (false);
				}
				if (enemy.inCombat && enemy.returnTarget () != null)
					Attack ();
				else
					enemy.setElapsedTime (0);
			} else { //if no targets
				enemy.inCombat = false;
				idleImg.SetActive (true);
				fireImg.SetActive (false);
				Walk ();
				enemy.rotateToTarget (new Quaternion(0, 0, 0, 1), transform.rotation);
			} 
		} else { //if not alive
			rb.velocity = new Vector2 (0, 0);
		}

	}

	public void Attack() { //since this is a ranged attack, we need to override the melee attack function
		if (enemy.returnElapsedTime() > enemy.wpnSpeed + Random.value*0.5f){
			/*GameObject newArrow = Instantiate (arrowPrefab, arrowSpawn.transform.position, arrowSpawn.transform.rotation) as GameObject;
			newArrow.transform.eulerAngles = new Vector3(0, 0, transform.eulerAngles.z+90.9f);*/
			GameObject newBullet = Instantiate (bulletPrefab, bulletSpawn.transform.position, bulletSpawn.transform.rotation) as GameObject;
			//newBullet.GetComponent<Enemy_Projectile> ().dir = enemy.returnTargetPosition () - bulletSpawn.transform.position;
			newBullet.GetComponent<Enemy_Projectile> ().wpnDmg = enemy.wpnDmg;
			newBullet.tag = "Enemy_Projectile";
			enemy.setElapsedTime (0);
		}
		enemy.incElapsedTime ();

	}

	void Walk (){
		rb.velocity = (transform.right * enemy.speed * Time.deltaTime);
	}

	void OnTriggerEnter2D(Collider2D col){
		if (col.tag == "Player_Projectile") {
			enemy.takeDamage (col.GetComponent<Player_Projectile> ().wpnDmg);
			if (col.GetComponent<Player_Projectile> ().punchthrough >= 1) {
				col.GetComponent<Player_Projectile> ().punchthrough--;
			} else if (col.GetComponent<Player_Projectile> ().punchthrough < 1) {
				//making new arrow, adding it to this
				GameObject newProp = Instantiate (col.GetComponent<Player_Projectile> ().arrowProp,
					col.transform.position, col.transform.rotation) as GameObject;
				newProp.transform.parent = transform;
				col.GetComponent<Player_Projectile> ().punchthrough--;
				Destroy (col.gameObject);
			}
			enemy.updateHP ();
		}
	}
}