using UnityEngine;
using System.Collections;

public class DestroyByBoundary : MonoBehaviour {

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Laser" && other.transform.position.x < -7.4)
        {
            other.transform.position = Vector3.Reflect(other.transform.position, this.transform.forward);
        }
    }


	void OnTriggerExit(Collider other )
	{
		Destroy (other.gameObject);
	}
	

}
