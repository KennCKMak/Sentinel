using UnityEngine;
using System.Collections;

public class NumberMaster : MonoBehaviour {


	//Player things
	public int punchthroughLevel = 0; //increases punchthrough amount of arrows
	public int scatterLevel = 0; //increase arrow fired by player
	public int damageLevel = 0; //increases damage by 50% / level
	public int armourLevel = 0; //decrease damage taken by 10% per level


	public int gold;
	public int score;
	public int enemiesKilled;
	public float timer;

	public int min;
	public int sec;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
