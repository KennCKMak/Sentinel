﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameMaster : MonoBehaviour {

	public bool debugMode = true;

	public bool paused;
	public float timeScale;
	public NumberMaster numberMaster;
	public CanvasScript canvasScript;


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

	//enemy shotgunner counter
	public GameObject[] Shotgunners;
	public GameObject shotgunnerPrefab;
	public int shotgunnerCount = 0;
	public int maxShotgunner = 2;


	//
	public int enemyToBoss = 50;
	//enemy warlord counter
	public GameObject[] Warlords;
	public GameObject warlordPrefab;
	public int warlordCount = 0;
	public int maxWarlord = 1;

	public int enemyCount;
	public List<GameObject> EnemyList = new List<GameObject> ();

	//ALLY VARIABLES
	//allied spearman counter
	public GameObject[] Spears;
	public GameObject spearmanPrefab;
	private float spearmanX = 6.9511f;
	public int spearmanCount = 0;
	public int maxSpearman = 7;
	private List<float> spearmanY = new List<float> (); //0.13f;

	//allied archer counter
	public GameObject[] Archers;
	public GameObject archerPrefab;
	private float archerX = 8.73f;
	public int archerCount = 0;
	public int maxArcher = 4;
	private List<float> archerY = new List<float>();
	public int allyCount;
	public List<GameObject> AllyList = new List<GameObject>();


	void Start () {
		if(debugMode)
			Time.timeScale = timeScale;
		numberMaster = transform.GetComponent<NumberMaster> ();
		canvasScript = GameObject.Find ("Canvas").GetComponent<CanvasScript> ();
		paused = false;
		float num = (5.53f - (-4.06f)) / 9.0f; //change the 8 to how many grids
		for (int i = 0; i < 9; i++) { //setting up the  grids
			yRange.Add(-4.06f + i * num); //setting up grid yaxis
		}

		//setting up the order in which allies will spawn
		/*spearmanY.Add (0.51f); spearmanY.Add (2.51f) ;spearmanY.Add (-1.49f);
		spearmanY.Add (1.51f);spearmanY.Add (-0.49f);spearmanY.Add (3.51f);spearmanY.Add (-2.49f);*/
		spearmanY.Add (0.740f);spearmanY.Add (2.728f);
		spearmanY.Add (-1.248f);spearmanY.Add (4.716f);
		spearmanY.Add (-3.236f);spearmanY.Add (1.734f);
		spearmanY.Add (-0.254f);spearmanY.Add (3.722f);
		spearmanY.Add (-2.242f);spearmanY.Add (-4.230f);
		spearmanY.Add (5.710f);

		archerY.Add (1.88f); archerY.Add (-1.13f); archerY.Add (3.38f); archerY.Add (-2.62f);archerY.Add (4.88f); archerY.Add (-4.12f);
		//middle top, middle bottom, top, and bottom position
		Warriors = new GameObject[maxWarrior];
		Rifles = new GameObject[maxRifle];
		Shotgunners = new GameObject[maxShotgunner];
		Warlords = new GameObject[maxWarlord];
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


		if (numberMaster.enemiesKilled >= enemyToBoss) {
			enemyToBoss += enemyToBoss;
			SpawnBossWave ();
		}

		if (Input.GetKeyDown (KeyCode.Escape)) {
			if (paused) {
				paused = false;
				Time.timeScale = 1.0f;
				canvasScript.transform.FindChild ("PauseScreen").gameObject.SetActive (false);
			} else {
				paused = true;
				Time.timeScale = 0.0f;
				canvasScript.transform.FindChild ("PauseScreen").gameObject.SetActive (true);
			}
		}

		if (debugMode) { //these are hotkeys for debug mode.
			if (Input.GetKeyDown (KeyCode.Alpha5)) {
				SpawnSpearman ();
				Debug.Log ("spawning spears");
			}
			if (Input.GetKeyDown (KeyCode.Alpha6)) {
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

	public void SpawnShotgunner(){
		if (shotgunnerCount >= maxShotgunner)
			return;
		for (int i = 0; i < maxShotgunner; i++) {
			if (Shotgunners [i] == null) {		
				enemyCount++;
				GameObject newEnemy = Instantiate (shotgunnerPrefab,
					new Vector2 (xStart, yRange [Random.Range (0, yRange.Count)]), transform.rotation) as GameObject;
				newEnemy.GetComponent<Enemy_Shotgun> ().enemy.setIndexLoc (i);
				shotgunnerCount++;
				Shotgunners [i] = newEnemy;
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

	public void SpawnShotgunner(int num){
		if (shotgunnerCount >= maxShotgunner)
			return;
		for (int i = 0; i < maxShotgunner; i++) {
			if (Shotgunners [i] == null) {		
				enemyCount++;
				GameObject newEnemy = Instantiate (shotgunnerPrefab,
					new Vector2 (xStart, yRange [num]), transform.rotation) as GameObject;
				newEnemy.GetComponent<Enemy_Shotgun> ().enemy.setIndexLoc (i);
				shotgunnerCount++;
				Shotgunners [i] = newEnemy;
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
			spawning = true;
			StartCoroutine (spawnTroops ());
			StartCoroutine (spawnShotgun ());
	}

	public void StopSpawnEnemies(){
			spawning = false;
			StopCoroutine (spawnTroops ());
			StartCoroutine (spawnShotgun ());
	}

	IEnumerator spawnTroops()
	{
		int num;
		while (spawning == true) {
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
			yield return new WaitForSeconds (2f);
		}
	}

	IEnumerator spawnShotgun(){
		while (spawning == true) {
			yield return new WaitForSeconds (25f);
			SpawnShotgunner ();
			Debug.Log ("spawned one shotgunner");
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
		SpawnWarrior (7);
		SpawnWarrior (2);
		SpawnWarrior (3);
		SpawnWarrior (4);
		SpawnWarrior (5);
		SpawnWarrior (6);
		yield return new WaitForSeconds (1.5f);
		SpawnShotgunner (2);
		SpawnShotgunner (6);
		SpawnWarlord (4);
		yield return new WaitForSeconds (1.9f);
		SpawnRifle (3);
		SpawnRifle (5);
		SpawnRifle (1);
		SpawnRifle (7);
		yield return new WaitForSeconds (30f);
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
		if (GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().isAlive())
			AllyList.Add (GameObject.FindGameObjectWithTag ("Player"));
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
