using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Ally_Melee : MonoBehaviour {

	//private GameObject idleImg;
	public AllyClass ally;
	void Start(){
		ally.myClass = AllyClass.unitTypes.SPEARMAN;
	}

	// Update is called once per frame
	void Update () {
		if (ally.isAlive()){
			ally.GetTarget ();
			if (ally.returnTarget() != null) {
				if (ally.inTargetDist ()) { //if you are within range
					ally.rotateToTarget (ally.returnTargetPosition (), transform.position);
					ally.inCombat = true;
					Attack ();
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

	public void Attack(){
		if (ally.returnElapsedTime() > ally.wpnSpeed && ally.inCombat){
			if (ally.returnTarget().GetComponent<Enemy_Melee> () != null) {
				if (ally.returnTarget().GetComponent<Enemy_Melee> ().enemy.isAlive ()) {
					ally.returnTarget().GetComponent<Enemy_Melee> ().enemy.takeDamage (ally.wpnDmg);
				} else {
					ally.nullTarget();
					ally.GetTarget ();
				}
			} else if (ally.returnTarget().GetComponent<Enemy_Ranged> () != null) {
				if (ally.returnTarget().GetComponent<Enemy_Melee> ().enemy.isAlive ()) {
					ally.returnTarget().GetComponent<Enemy_Melee> ().enemy.takeDamage (ally.wpnDmg);
				} else {
					ally.nullTarget();
					ally.GetTarget ();
				}
			}
			ally.setElapsedTime(0);
		}
		ally.incElapsedTime();
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
