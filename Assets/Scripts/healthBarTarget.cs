using UnityEngine;
using System.Collections;

public class healthBarTarget : MonoBehaviour {
	public Transform target;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (target != null)
			transform.position = target.transform.position;
		else
			Destroy (gameObject);
			
	}
}
