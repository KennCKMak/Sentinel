using UnityEngine;
using System.Collections;

public class Enemy_Projectile : MonoBehaviour {
	public float speed = 10;
	public float wpnDmg = 5;
	public Vector2 dir;
	public GameObject bulletProp;

	private Rigidbody2D rb;
	// Use this for initialization
	void Start () {
		Destroy (gameObject, 3f);
		/*dir.Normalize();
		rb = transform.GetComponent<Rigidbody2D> ();
		rb.velocity = dir * speed*100 * Time.deltaTime;
		Destroy (gameObject, 5f);*/
	}
	
	// Update is called once per frame
	void Update () {
		transform.Translate (speed * Vector3.up * Time.deltaTime);
	}
}
