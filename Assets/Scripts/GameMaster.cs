using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameMaster : MonoBehaviour {

	public bool debugMode = true;
	public float timeScale;



	//enemies...
	public bool spawning;
	public float xStart = -10.76f; //offscreen spawns for enemy
	public List<float> yRange = new List<float>(); //y value for grids spawn
	//enemy rifle counter
	public GameObject[] Rifles;
	public GameObject riflePrefab;
	public int rifleCount = 0;
	public int maxRifle = 3;

	//enemy warrior counter
	public GameObject[] Warriors;
	public GameObject warriorPrefab;
	public int warriorCount = 0;
	public int maxWarrior = 7;

	//enemy warlord counter
	public GameObject[] Warlords;
	public GameObject warlordPrefab;
	public int warlordCount = 0;
	public int maxWarlord = 7;

	public int enemyCount;
	public List<GameObject> EnemyList = new List<GameObject> ();

	//ALLY VARIABLES
	//allied spearman counter
	public GameObject[] Spears;
	public GameObject spearmanPrefab;
	private float spearmanX = 6.9511f;
	public int spearmanCount = 0;
	private int maxSpearman = 7;
	private List<float> spearmanY = new List<float> (); //0.13f;

	//allied archer counter
	public GameObject[] Archers;
	public GameObject archerPrefab;
	private float archerX = 8.73f;
	public int archerCount = 0;
	private int maxArcher = 4;
	private List<float> archerY = new List<float>();
	public int allyCount;
	public List<GameObject> AllyList = new List<GameObject>();


	void Start () {
		if(debugMode)
			Time.timeScale = timeScale;

		for (int i = 0; i < 7; i++) { //setting up the 7 grids
			yRange.Add(-2.62f + i * 1); //setting up grid yaxis
		}

		//setting up the order in which allies will spawn
		spearmanY.Add (0.51f); spearmanY.Add (2.51f) ;spearmanY.Add (-1.49f);
		spearmanY.Add (1.51f);spearmanY.Add (-0.49f);spearmanY.Add (3.51f);spearmanY.Add (-2.49f);
		archerY.Add (1.88f); archerY.Add (-1.13f); archerY.Add (3.38f); archerY.Add (-2.62f);
		//middle top, middle bottom, top, and bottom position
		Warriors = new GameObject[maxWarrior];
		Rifles = new GameObject[maxRifle];
		Spears = new GameObject[maxSpearman];
		Archers = new GameObject[maxArcher];


		if (EnemyList == null)
			EnemyList.AddRange(GameObject.FindGameObjectsWithTag ("Enemy"));
		if (AllyList == null) {
			AllyList.AddRange (GameObject.FindGameObjectsWithTag ("Player"));
			AllyList.AddRange (GameObject.FindGameObjectsWithTag ("Ally"));
		}
		StartSpawnEnemies ();
//		SpawnBossWave ();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.Alpha5)) {
			SpawnSpearman ();
			Debug.Log ("spawning spears");
		}
		if (Input.GetKeyDown (KeyCode.Alpha6)){
			SpawnArcher ();
			Debug.Log ("spawning archers");
		}

		if (Input.GetKeyDown (KeyCode.Alpha4)) {
			if (spawning) {
				StopSpawnEnemies ();
				Debug.Log ("stopping");
			} else {
				StartSpawnEnemies ();
				Debug.Log ("starting");
			}
		}
		if (Input.GetKeyDown (KeyCode.R)) {
			killAllEnemies ();
		}
		if (Input.GetKeyDown (KeyCode.T)) {
			SpawnBossWave ();
		}
	}	
	//ENEMY SPAWN FUNCTIONS
	public void refreshEnemyList(){ //used by allies to find available enemies
		EnemyList.Clear ();
		EnemyList.AddRange(GameObject.FindGameObjectsWithTag ("Enemy"));
		for (int i = 0; i < EnemyList.Count; i++) {
			if (EnemyList [i] == null)
				EnemyList.RemoveAt (i);
		}
	}


	public void SpawnWarrior(){
		if (warriorCount >= maxWarrior)
			return;
		for (int i = 0; i < maxWarrior; i++) {
			if (Warriors [i] == null) {		
				enemyCount++;
				GameObject newEnemy = Instantiate (warriorPrefab,
					new Vector2 (xStart, yRange [Random.Range (0, yRange.Count)]), transform.rotation) as GameObject;
				newEnemy.GetComponent<Enemy_Melee> ().enemy.setIndexLoc (i);
				warriorCount++;
				Warriors [i] = newEnemy;
				refreshEnemyList ();
				return;
			}
		}
	}

	public void SpawnRifle(){
		if (rifleCount >= maxRifle)
			return;
		for (int i = 0; i < maxRifle; i++) {
			if (Rifles [i] == null) {		
				enemyCount++;
				GameObject newEnemy = Instantiate (riflePrefab,
					new Vector2 (xStart, yRange [Random.Range (0, yRange.Count)]), transform.rotation) as GameObject;
				newEnemy.GetComponent<Enemy_Ranged> ().enemy.setIndexLoc (i);
				rifleCount++;
				Rifles [i] = newEnemy;
				refreshEnemyList ();
				return;
			}
		}
	}
	public void SpawnWarrior(int num){
		if (warriorCount >= maxWarrior)
			return;
		for (int i = 0; i < maxWarrior; i++) {
			if (Warriors [i] == null) {		
				enemyCount++;
				GameObject newEnemy = Instantiate (warriorPrefab,
					new Vector2 (xStart, yRange[num]), transform.rotation) as GameObject;
				newEnemy.GetComponent<Enemy_Melee> ().enemy.setIndexLoc (i);
				warriorCount++;
				Warriors [i] = newEnemy;
				refreshEnemyList ();
				return;
			}
		}
	}

	public void SpawnRifle(int num){
		if (rifleCount >= maxRifle)
			return;
		for (int i = 0; i < maxRifle; i++) {
			if (Rifles [i] == null) {		
				enemyCount++;
				GameObject newEnemy = Instantiate (riflePrefab,
					new Vector2 (xStart, yRange[num]), transform.rotation) as GameObject;
				newEnemy.GetComponent<Enemy_Ranged> ().enemy.setIndexLoc (i);
				rifleCount++;
				Rifles [i] = newEnemy;
				refreshEnemyList ();
				return;
			}
		}
	}

	public void SpawnWarlord(int num){
		if (warlordCount >= maxWarlord)
			return;
		for (int i = 0; i < maxWarlord; i++) {
			if (Warlords [i] == null) {		
				enemyCount++;
				GameObject newEnemy = Instantiate (warlordPrefab,
					new Vector2 (xStart, yRange[num]), transform.rotation) as GameObject;
				newEnemy.GetComponent<Enemy_Melee> ().enemy.setIndexLoc (i);
				warlordCount++;
				Warlords [i] = newEnemy;
				refreshEnemyList ();
				return;
			}
		}
	}

	public void killAllEnemies(){
		GameObject[] tempArray = new GameObject[enemyCount];
		tempArray = GameObject.FindGameObjectsWithTag ("Enemy");
		for (int i = 0; i < tempArray.Length; i++) {
			tempArray [i].GetComponent<EnemyClass> ().takeDamage (99999f);
		}
	}


	/*TIMER FOR SPAWNING ENEMEIS*/
	public void StartSpawnEnemies(){
		if(!spawning){
			spawning = true;
			StartCoroutine (spawnObject ());
		}
	}

	public void StopSpawnEnemies(){
		if(spawning){
			spawning = false;
			StopCoroutine (spawnObject ());
		}
	}

	IEnumerator spawnObject()
	{
		int num;
		while (spawning == true) {
			Debug.Log ("spawning");
			num = Random.Range (0, 2);
			switch (num) {
			case(0):
				SpawnWarrior ();
				break;
			case(1):
				SpawnRifle ();
				break;
			default:
				break;
			}
			yield return new WaitForSeconds (1f);
		}
	}
	public void SpawnBossWave(){
		StopSpawnEnemies ();
		StartCoroutine(SpawnBossPlatoon());
	}

	/*TIMER FOR SPAWNING BOSS WAVE*/
	IEnumerator SpawnBossPlatoon(){
		
		StopSpawnEnemies ();
		killAllEnemies ();
		yield return new WaitForSeconds (1f);
		SpawnWarrior (1);
		SpawnWarrior (2);
		SpawnWarrior (3);
		SpawnWarrior (4);
		SpawnWarrior (5);
		yield return new WaitForSeconds (1.5f);
		SpawnWarrior (1);
		SpawnWarrior (5);
		SpawnWarlord (3);
		yield return new WaitForSeconds (1.9f);
		SpawnRifle (2);
		SpawnRifle (4);
		SpawnRifle (0);
		SpawnRifle (6);
		yield return new WaitForSeconds (1f);
		StartSpawnEnemies ();

	}

	//ALLIED SPAWN FUNCTIONS
	public void refreshAllyList(){ //used by enemies to find available targets to shoot at
		AllyList.Clear ();
		AllyList.AddRange(GameObject.FindGameObjectsWithTag ("Ally"));
		for (int i = 0; i < AllyList.Count; i++) {
			if (AllyList [i] == null)
				AllyList.RemoveAt (i);
		}
	}

	public void SpawnArcher(){
		if (archerCount >= maxArcher)
			return;
		for (int i = 0; i < maxArcher; i++) {
			if (Archers [i] == null) {
				GameObject newAlly = Instantiate(archerPrefab, 
					new Vector2(archerX, archerY[i]), Quaternion.identity) as GameObject;
				newAlly.GetComponent<Ally_Ranged> ().ally.setIndexLoc (i);
				archerCount++;
				Archers [i] = newAlly;
				allyCount++;
				refreshAllyList ();
				return;
			}
		}
	}

	public void SpawnSpearman(){
		if (spearmanCount >= maxSpearman)
			return;
		for (int i = 0; i < maxSpearman; i++) {
			if (Spears [i] == null) {
				GameObject newAlly = Instantiate(spearmanPrefab, 
					new Vector2(spearmanX, spearmanY[i]), Quaternion.identity) as GameObject;
				newAlly.GetComponent<Ally_Melee> ().ally.setIndexLoc (i);
				spearmanCount++;
				Spears [i] = newAlly;
				allyCount++;
				refreshAllyList ();
				return;
			}
		}
	}


}
