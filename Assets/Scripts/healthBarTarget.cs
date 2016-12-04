using UnityEngine;
using System.Collections;

public class healthBarTarget : MonoBehaviour {
	public Transform target;
	public Vector3 newTarget;
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		if (target != null) {
			newTarget = target.transform.parent.transform.position + target.transform.localPosition;
			transform.position = newTarget;
		}else
			Destroy (gameObject);
			
	}
}
