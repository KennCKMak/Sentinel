using UnityEngine;
using System.Collections;

public class Ally_Ranged : MonoBehaviour {

	public GameObject arrowPrefab;
	private GameObject idleImg;
	private GameObject fireImg;
	private GameObject arrowSpawn;

	public AllyClass ally;


	// Use this for initialization
	void Start () {
		ally.myClass = AllyClass.unitTypes.ARCHER;
		idleImg = transform.FindChild ("IdleImg").gameObject;
		arrowSpawn = idleImg.transform.FindChild ("ArrowSpawn").gameObject;
	}

	// Update is called once per frame
	void Update () {
		if (ally.isAlive()){
			ally.GetTarget ();
			if (ally.returnTarget() != null) {
				if (ally.inTargetDist ()) { //if you are within range
					ally.rotateToTarget (ally.returnTargetPosition (), transform.position);
					ally.inCombat = true;
					Attack (); //using the overriden attack
				} else {
					ally.inCombat = false;
					ally.rotateToTarget (ally.defaultRot, transform.rotation);
				}
				if (ally.inCombat && ally.returnTarget() != null)
					Attack ();
				else
					ally.setElapsedTime (0);
			} else {
				ally.GetTarget ();
			}
		}
	}



	void Attack() {

		if (ally.returnElapsedTime() > ally.wpnSpeed + Random.value*0.5f){ //timer done, can fire
			GameObject newArrow = Instantiate (arrowPrefab, arrowSpawn.transform.position, arrowSpawn.transform.rotation) as GameObject;
			newArrow.transform.eulerAngles = new Vector3(0, 0, transform.eulerAngles.z+90.9f);
			newArrow.tag = "Player_Projectile";
			newArrow.AddComponent<Player_Projectile> ();
			newArrow.GetComponent<Player_Projectile> ().speed = 10f;
			newArrow.GetComponent<Player_Projectile> ().wpnDmg = ally.wpnDmg;
			ally.setElapsedTime (0); //elapsedTime = 0;
		}
		ally.incElapsedTime (); //elapsedTime+=Time.deltaTime
	}



	void OnTriggerEnter2D(Collider2D col){
		if (col.tag == "Enemy_Projectile") {
			GameObject newProp = Instantiate (col.GetComponent<Enemy_Projectile>().bulletProp,
				col.transform.position, col.transform.rotation) as GameObject;
			newProp.transform.parent = transform;
			ally.takeDamage (col.GetComponent<Enemy_Projectile> ().wpnDmg);
			Destroy (col.gameObject);
			Destroy (newProp.gameObject, 10f);
			ally.updateHP ();
		}
	}
}