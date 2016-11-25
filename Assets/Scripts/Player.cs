using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

	public GameObject arrowTip;
	public GameObject arrowBase;

	private bool alive;

	public float health;
	public float reloadTime;
	public float wpnDmg;

	private int weapon = 1;
	private int ScatterUpgrade = 3;
	private float angleEven = 2f;
	private float angleOdd = 2f;
	public GameObject arrow;



	// Use this for initialization
	void Start () {	
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

		//shooting
		if(Input.GetMouseButton(0)) {
			switch (weapon) {
			case(1):
				FireArrow ();
				break;
			case(2):
				FireScatterArrow (ScatterUpgrade);
				break;
			case(3):
				FireBoulder ();
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
		GameObject testArr = Instantiate (arrow, arrowBase.transform.position, rot) as GameObject;
		testArr.transform.eulerAngles = new Vector3(0, 0, transform.eulerAngles.z-89.1f);
		testArr.GetComponent<Player_Projectile> ().wpnDmg = wpnDmg;
	}

	void FireArrow(float angle){ //this is used to offset things
		Vector3 mousePosition = Camera.main.ScreenToWorldPoint (Input.mousePosition); 
		mousePosition.z = 0;
		Quaternion rot = Quaternion.LookRotation(arrowBase.transform.position - mousePosition, Vector3.forward);
		GameObject testArr = Instantiate (arrow, arrowBase.transform.position, rot) as GameObject;
		testArr.transform.eulerAngles = new Vector3(0, 0, transform.eulerAngles.z-89.1f+angle);
		testArr.GetComponent<Player_Projectile> ().wpnDmg = wpnDmg;
	}

	void FireScatterArrow(int num){
		if (num == 0 || num == 1)
			return;
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
	void FireBoulder(){

	}


	void OnTriggerEnter2D(Collider2D col){
		if (col.tag == "Enemy_Projectile") {
			health = health - col.GetComponent<Enemy_Projectile> ().wpnDmg;
			Destroy (col.gameObject);
			//if ded
		}

	}

}
