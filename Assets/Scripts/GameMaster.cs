using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameMaster : MonoBehaviour {

	public float xStart = -10.76f;
	public float xOnScreen = -9.11f;
	public float xEnd = 4.16f;

	public bool debugMode = true;

	public List<float> yRange = new List<float>(); //y value for grids


	public GameObject riflePrefab;
	public GameObject warriorPrefab;
	public int enemyCount;
	public List<GameObject> EnemyList = new List<GameObject> ();
	public int maxEnemy;
	// Use this for initialization

	public GameObject spearmanPrefab;
	public float spearmanX = 7.1511f;
	public float spearmanY = 0.13f;
	public int spearmanCount;
	public int maxSpearman = 7;
	public GameObject archerPrefab;
	public float archerX = 8.73f;
	public List<float> archerY = new List<float>();
	public int archerCount;
	public int maxArcher = 4;
	public List<GameObject> AllyList = new List<GameObject>();


	void Start () {
		Time.timeScale = 1;

		for (int i = 0; i < 7; i++) { //setting up the 7 grids
			yRange.Add(-2.62f + i * 1); //setting up grid yaxis
			Instantiate(spearmanPrefab, new Vector2(spearmanX, yRange[i] + spearmanY), Quaternion.identity);
			spearmanCount++;
		}


		archerY.Add (1.88f); archerY.Add (-1.13f); archerY.Add (3.38f); archerY.Add (-2.62f);
		//middle top, middle bottom, top, and bottom position
		for (int i = 0; i < 4; i++) {
			Instantiate (archerPrefab, new Vector2 (archerX, archerY [i]), Quaternion.identity);
			archerCount++;
		}

		if (EnemyList == null)
			EnemyList.AddRange(GameObject.FindGameObjectsWithTag ("Enemy"));
		StartCoroutine (spawnObject ());
		if (AllyList == null)
			AllyList.AddRange (GameObject.FindGameObjectsWithTag ("Ally"));
	}
	
	// Update is called once per frame
	void Update () {
		if (enemyCount < maxEnemy) {
			int num = Random.Range (0, 2); //spawning the warriors
			switch (num) {
			case(0):
				Instantiate (warriorPrefab, new Vector2 (xStart, yRange[Random.Range(0, yRange.Count)]), transform.rotation);
				enemyCount++;
				break;

			case(1):
				Instantiate (riflePrefab, new Vector2 (xStart, yRange[Random.Range(0, yRange.Count)]), transform.rotation);
				enemyCount++;
				break;
			default:
				break;
			}
			refreshEnemyList ();
		}

	}	

	public void refreshEnemyList(){
		EnemyList.Clear ();
		EnemyList.AddRange(GameObject.FindGameObjectsWithTag ("Enemy"));
		for (int i = 0; i < EnemyList.Count; i++) {
			if (EnemyList [i] == null)
				EnemyList.RemoveAt (i);

		}
	}

	public void refreshAllyList(){
		AllyList.Clear ();
		AllyList.AddRange(GameObject.FindGameObjectsWithTag ("Enemy"));
		for (int i = 0; i < EnemyList.Count; i++) {
			if (EnemyList [i] == null)
				EnemyList.RemoveAt (i);

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
			yield return new WaitForSeconds (15f);
			SpawnPlatoon (0);
			yield return new WaitForSeconds (15f);
			SpawnPlatoon (1);
		}
	}
}
