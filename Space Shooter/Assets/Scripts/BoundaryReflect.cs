using UnityEngine;
using System.Collections;

public class BoundaryReflect : MonoBehaviour {

    void OnCollisionEnter(Collision collision)
    {
        ContactPoint contact = collision.contacts[0];
        
      /*  Quaternion rot = Quaternion.FromToRotation(Vector3.up, contact.normal);
        Vector3 pos = contact.point; */
        if (contact.otherCollider.tag == "Laser")
        {
            Debug.Log("if reached collision");
            Vector3 reflectedLaser = Vector3.Reflect(contact.otherCollider.transform.position, contact.normal);
            contact.otherCollider.transform.position = reflectedLaser;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Laser")
        {
            RaycastHit hit;
            Vector3 reflectedLaser = other.transform.position;

            if (Physics.Raycast(transform.position, transform.right, out hit))
            {
                Debug.Log("If reached");
                reflectedLaser = Vector3.Reflect(other.transform.position, hit.normal);
            }

            
            //reflectedLaser.x = reflectedLaser.x * -1;
            other.transform.position = reflectedLaser;
        }
    } 
}
