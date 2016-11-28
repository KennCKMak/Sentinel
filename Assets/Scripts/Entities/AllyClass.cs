using UnityEngine;
using System.Collections;

public class AllyClass : MonoBehaviour {

	protected GameMaster gameMaster;

	public float health;
	public float maxHealth;
	public float armour = 0;

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


	public bool walking;
	public float speed;
	protected GameObject target;
	private float searchTime; //how long before it checks again for a target
	private float elapsedTime; //how long since it last attacked
	public Quaternion defaultRot = new Quaternion (0, 0, 0, 1);

	public enum unitTypes{
		ARCHER,
		SPEARMAN
	};
	public unitTypes myClass;

	void Start(){
		gameMaster = GameObject.Find ("GameMaster").GetComponent<GameMaster> ();
		alive = true;
		health = maxHealth;
		createHealthBar ();
		updateHP ();
	}


	//_______HEALTH BAR FUNCTIONS_______//
	void createHealthBar(){
		HealthBarLocation = transform.FindChild ("HealthBar").transform;
		HealthBar = Instantiate (HealthBarPrefab, HealthBarLocation.position, Quaternion.identity) as GameObject;
		HealthBar.GetComponent<healthBarTarget> ().target = HealthBarLocation;
		HPImg = HealthBar.transform.FindChild ("HPImg").transform;
	}

	public void updateHP(){
		float percentage = health / maxHealth;
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
	public void Die(){ //if dead
		if (health <= 0 && alive) {
			alive = !alive;
			GetComponent<CircleCollider2D> ().enabled = false;
			HPImg.GetComponent<SpriteRenderer> ().enabled = false;
			transform.tag = "Untagged";
			gameMaster.refreshAllyList ();
			gameMaster.allyCount--;
			Die2 ();
			Destroy (gameObject, 1f);
		}
	}
	public void Die2(){ //checing for unit class...
		if (myClass == unitTypes.SPEARMAN){
			gameMaster.spearmanCount--;
			gameMaster.Spears [indexLoc] = null; //setting its position in the allyarray to null
		} else if (myClass == unitTypes.ARCHER){
			gameMaster.archerCount--;
			gameMaster.Archers [indexLoc] = null; //setting its position in the allyarray to null
		} 
	}

	//_______TARGET FUNCTIONS________//
	public void setTarget(GameObject newTarget){
		target = newTarget;
	}

	public GameObject returnTarget(){
		return target;
	}
	public Transform returnTargetTransform(){
		return target.transform;
	}
	public void nullTarget(){
		target = null;
	}
	public Vector3 returnTargetPosition(){
		return target.transform.position;
	}


	public void GetTarget(){
		if (searchTime > 1) {   //check for new target every 1 seconds
			for (int i = 0; i < gameMaster.EnemyList.Count; i++) {
				if (target == null)
					target = gameMaster.EnemyList [i].gameObject;
				else if (target != null) {
					//looking for lowest distance
					if (Vector3.Distance (transform.position, gameMaster.EnemyList [i].transform.position)
						< Vector3.Distance (transform.position, returnTargetPosition())) {
						target = gameMaster.EnemyList [i].gameObject;
					}
				}
			}
			searchTime = 0;
		}
		searchTime += Time.deltaTime;
	}

	public bool inTargetDist(){
		Vector3 Distance = returnTargetPosition () - transform.position;
		if (Distance.magnitude < range) 
			//attacking
			inCombat = true;
		else 
			//walking
			inCombat = false;

		return inCombat;
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


	//_______ACCESS FUNCTIONS_______//
	public float returnSearchTime(){
		return searchTime;
	}
	public float returnElapsedTime(){
		return elapsedTime;
	}
	public float incSearchTime(){
		return searchTime += Time.deltaTime;
	}
	public float incElapsedTime(){
		return elapsedTime += Time.deltaTime;
	}
	public void setSearchTime(float num){
		searchTime = num;
	}
	public void setElapsedTime(float num){
		elapsedTime = num;
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
	//_______More access functions_______//


	public void setIndexLoc(int n){
		indexLoc = n;
	}
	public int retIndexLoc(){
		return indexLoc;
	}
	//arrow upgrades
	public int retScatterLevel(){
		return gameMaster.scatterLevel;
	}
	public int retPunchthroughLevel(){
		return gameMaster.punchthroughLevel;
	}
	public int retDamageLevel(){
		return gameMaster.damageLevel;
	}
	public int retArmourLevel(){
		return gameMaster.armourLevel;
	}
}
