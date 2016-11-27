using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameMaster : MonoBehaviour {

	public float xStart = -10.76f;
	public float xOnScreen = -9.11f;
	public float xEnd = 4.16f;

	public bool debugMode = true;


	//enemies...
	//enemy rifle counter
	public List<float> yRange = new List<float>(); //y value for grids spawn
	public GameObject riflePrefab;
	public int rifleCount = 0;
	public int maxRifle = 3;
	public GameObject[] Rifles;
	//enemy warrior counter
	public GameObject warriorPrefab;
	public int warriorCount = 0;
	public int maxWarrior = 7;
	public GameObject[] Warriors;


	public int enemyCount;
	public int maxEnemy;
	public List<GameObject> EnemyList = new List<GameObject> ();

	//allied spearman counter
	public GameObject spearmanPrefab;
	private float spearmanX = 6.9511f;
	public int spearmanCount = 0;
	private int maxSpearman = 7;
	private List<float> spearmanY = new List<float> (); //0.13f;
	public GameObject[] Spears;

	//allied archer counter
	public GameObject archerPrefab;
	private float archerX = 8.73f;
	public int archerCount = 0;
	private int maxArcher = 4;
	private List<float> archerY = new List<float>();
	public GameObject[] Archers;
	public int allyCount;
	public List<GameObject> AllyList = new List<GameObject>();


	void Start () {
		Time.timeScale = 2;

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
		StartCoroutine (spawnObject ());
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
				enemyCount++;
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
				enemyCount++;
				refreshEnemyList ();
				return;
			}
		}
	}

	void SpawnPlatoon(int num){
		enemyCount += 7;
		if (num == 0) {
			for (int i = 0; i < 7; i++) {
				Instantiate (warriorPrefab, new Vector2 (xStart, yRange [i]), transform.rotation);
			}
		}	
		if (num == 1) {
			for (int i = 0; i < 7; i++) {
				Instantiate (riflePrefab, new Vector2 (xStart, yRange [i]), transform.rotation);
			}
		}
		refreshEnemyList ();
	}
		
	/*TIMER FOR SPAWNING ENEMEIS*/
	IEnumerator spawnObject()
	{
		int num;
		while (true) {
			num = Random.Range (0, 4);
			switch (num) {
			case(0):
			case(1):
			//case(2):
				SpawnWarrior ();
				break;
			case(2):
			case(3):
				SpawnRifle ();
				break;
			default:
				break;
			}
			yield return new WaitForSeconds (1f);
		}
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
