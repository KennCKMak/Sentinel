using UnityEngine;
using System.Collections;

public class debugDetect : MonoBehaviour {

	private GameMaster gameMaster;

	// Use this for initialization
	void Start () {
		gameMaster = GameObject.Find ("GameMaster").GetComponent<GameMaster> ();
		if (!gameMaster.debugMode)
			this.gameObject.SetActive (false);
		else
			this.gameObject.SetActive (true);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
