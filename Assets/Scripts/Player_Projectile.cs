using UnityEngine;
using System.Collections;

public class Player_Projectile : MonoBehaviour {
	public float speed = 1.0f;
	public float wpnDmg = 20f;
	public int punchthrough = 0; //how many times arrow can pierce through enemy. 1 = can pass through once 
	public GameObject arrowProp;
	// Use this for initialization
	void Start () {
		Destroy (gameObject, 5f);
	}
	
	// Update is called once per frame
	void Update () {
		transform.Translate (speed * Vector3.up * Time.deltaTime);
	}

}
