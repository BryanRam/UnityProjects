using UnityEngine;
using System.Collections;

public class BoltController : MonoBehaviour {
    public Dodge dodge;

	// Use this for initialization
	void Start () {
	
	}

    void OnTriggerEnter(Collider other)
    {
       

        if (other.name == "BoxCollider")
        {

            dodge.Evade();
        }

    }
}
