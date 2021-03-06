﻿using UnityEngine;
using System.Collections;

public class Enemy_Melee : MonoBehaviour {
	
	public EnemyClass enemy;

	private Rigidbody2D rb;

	void Start () {
		enemy.setTarget (GameObject.FindGameObjectWithTag ("Player"));
		enemy.myClass = EnemyClass.unitTypes.WARRIOR;

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
					Attack ();
				} else {
					Walk ();
					enemy.inCombat = false;
				}
				if (enemy.inCombat && enemy.returnTarget () != null)
					Attack ();
				else
					enemy.setElapsedTime (0);
			} else { //if no targets
				enemy.inCombat = false;
				Walk ();
				enemy.rotateToTarget (new Quaternion(0, 0, 0, 1), transform.rotation);
			} 
		} else { //if not alive
			rb.velocity = new Vector2 (0, 0);
		}

	}

	void Walk (){
		rb.velocity = (transform.right * enemy.speed * Time.deltaTime);
		if(transform.position.x > 0 && enemy.returnTarget() != null)
			 enemy.rotateToTarget (enemy.returnTargetPosition (), transform.position);
	}

	public void Attack(){
		if (enemy.returnElapsedTime() > enemy.wpnSpeed && enemy.inCombat){
			if (enemy.returnTarget ().GetComponent<Ally_Melee> () != null) {
				if (enemy.returnTarget ().GetComponent<Ally_Melee> ().ally.isAlive ()) {
					enemy.returnTarget ().GetComponent<Ally_Melee> ().ally.takeDamage (enemy.wpnDmg);
				} else {
					enemy.nullTarget ();
					enemy.GetTarget ();
				}
			} else if (enemy.returnTarget ().GetComponent<Ally_Ranged> () != null) {
				if (enemy.returnTarget ().GetComponent<Ally_Ranged> ().ally.isAlive ()) {
					enemy.returnTarget ().GetComponent<Ally_Ranged> ().ally.takeDamage (enemy.wpnDmg);
				} else {
					enemy.nullTarget ();
					enemy.GetTarget ();
				}
			} else if (enemy.returnTarget ().GetComponent<Player> () != null) {
				if (enemy.returnTarget ().GetComponent<Player> ().isAlive ()) {
					enemy.returnTarget ().GetComponent<Player> ().takeDamage (enemy.wpnDmg);
				} else {
					enemy.nullTarget ();
					enemy.GetTarget ();
				}

			}
			enemy.setElapsedTime(0);
		}
		enemy.incElapsedTime();
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
