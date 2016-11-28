using UnityEngine;
using System.Collections;

public class BorderScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter2D(Collider2D c){
		if (c.tag == "Border")
			return;
		if (c.tag == "Enemy")
			c.gameObject.GetComponent<EnemyClass> ().takeDamage (9999999);
		else
			DestroyObject (c);
	}
}
