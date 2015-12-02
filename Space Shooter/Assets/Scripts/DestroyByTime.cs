using UnityEngine;
using System.Collections;

public class DestroyByTime : MonoBehaviour {
	public float lifeTime;

	// Use this for initialization
	void Start () {
		if (gameObject.tag == "Enemy") {
				}
		else
		Destroy (gameObject, lifeTime);
	}

	void FixedUpdate ()
	{
		if (gameObject.tag == "Enemy" && gameObject.transform.position.z < 3) {
			Destroy (gameObject, lifeTime);
		}

	}

}
