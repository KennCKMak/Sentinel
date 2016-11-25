using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameMaster : MonoBehaviour {

	public float xStart = -10.76f;
	public float xOnScreen = -9.11f;
	public float xEnd = 4.16f;

	public bool debugMode = true;

	public List<float> yRange = new List<float>(); //y value for grids

	//enemies...
	public GameObject riflePrefab;
	public GameObject warriorPrefab;
	public int enemyCount;
	public List<GameObject> EnemyList = new List<GameObject> ();
	public int maxEnemy;

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
		Time.timeScale = 1;

		for (int i = 0; i < 7; i++) { //setting up the 7 grids
			yRange.Add(-2.62f + i * 1); //setting up grid yaxis
		}

		//setting up the order in which allies will spawn
		spearmanY.Add (0.51f); spearmanY.Add (2.51f) ;spearmanY.Add (-1.49f);
		spearmanY.Add (1.51f);spearmanY.Add (-0.49f);spearmanY.Add (3.51f);spearmanY.Add (-2.49f);
		archerY.Add (1.88f); archerY.Add (-1.13f); archerY.Add (3.38f); archerY.Add (-2.62f);
		//middle top, middle bottom, top, and bottom position
		Spears = new GameObject[maxSpearman];
		Archers = new GameObject[maxArcher];


		if (EnemyList == null)
			EnemyList.AddRange(GameObject.FindGameObjectsWithTag ("Enemy"));
		if (AllyList == null)
			AllyList.AddRange (GameObject.FindGameObjectsWithTag ("Ally"));
		StartCoroutine (spawnObject ());
	}
	
	// Update is called once per frame
	void Update () {
		if (enemyCount < maxEnemy) {
			int num = Random.Range (0, 2); //spawning the warriors
			switch (num) {
			case(0):
				Instantiate (warriorPrefab, new Vector2 (xStart, yRange [Random.Range (0, yRange.Count)]), transform.rotation);
				enemyCount++;
				break;
			case(1):
				Instantiate (riflePrefab, new Vector2 (xStart, yRange [Random.Range (0, yRange.Count)]), transform.rotation);
				enemyCount++;
				break;
			default:
				break;
			}
			refreshEnemyList ();
		}

		if (Input.GetKeyDown (KeyCode.Alpha5)) {
			SpawnSpearman ();
			Debug.Log ("spawning spears");
		}
		if (Input.GetKeyDown (KeyCode.Alpha6)){
			SpawnArcher ();
			Debug.Log ("spawning archers");
		}
	}	

	public void refreshEnemyList(){ //used by allies to find available enemies
		EnemyList.Clear ();
		EnemyList.AddRange(GameObject.FindGameObjectsWithTag ("Enemy"));
		for (int i = 0; i < EnemyList.Count; i++) {
			if (EnemyList [i] == null)
				EnemyList.RemoveAt (i);
		}
	}

	public void refreshAllyList(){ //used by enemies to find available targets to shoot at
		AllyList.Clear ();
		AllyList.AddRange(GameObject.FindGameObjectsWithTag ("Ally"));
		for (int i = 0; i < AllyList.Count; i++) {
			if (AllyList [i] == null)
				AllyList.RemoveAt (i);
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
		

	IEnumerator spawnObject()
	{
		while (true) {
			yield return new WaitForSeconds (30f);
			SpawnPlatoon (0);
			yield return new WaitForSeconds (30f);
			SpawnPlatoon (1);
		}
	}

	public void SpawnArcher(){
		if (archerCount == maxArcher)
			return;
		for (int i = 0; i < maxArcher; i++) {
			if (Archers [i] == null) {
				GameObject newAlly = Instantiate(archerPrefab, 
					new Vector2(archerX, archerY[i]), Quaternion.identity) as GameObject;
				newAlly.GetComponent<Ally_Ranged> ().setIndexLoc (i);
				archerCount++;
				Archers [i] = newAlly;
				allyCount++;
				return;
			}
		}
	}
	public void SpawnSpearman(){
		if (spearmanCount == maxSpearman)
			return;
		for (int i = 0; i < maxSpearman; i++) {
			if (Spears [i] == null) {
				GameObject newAlly = Instantiate(spearmanPrefab, 
					new Vector2(spearmanX, spearmanY[i]), Quaternion.identity) as GameObject;
				newAlly.GetComponent<Ally_Melee> ().setIndexLoc (i);
				spearmanCount++;
				Spears [i] = newAlly;
				allyCount++;
				return;
			}
		}
	}
}
