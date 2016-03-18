using UnityEngine;
using System.Collections;

public class RainTrigger : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnParticleCollision(GameObject other)
    {
        Debug.Log("Splat!");
       
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Splash!");
        Destroy(other.gameObject);
    }

}
