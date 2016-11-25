using UnityEngine;
using System.Collections;

public class Ally_Projectile : MonoBehaviour {
	public float speed = 10;
	public float wpnDmg = 5;
	public GameObject arrowProp;

	// Use this for initialization
	void Start () {
		//transform.rotation = Quaternion.Euler (dir);

		Destroy (gameObject, 5f);
	}

	// Update is called once per frame
	void Update () {
		transform.Translate (speed * Vector3.up * Time.deltaTime);
	}
}
