using UnityEngine;
using System.Collections;

public class Mover : MonoBehaviour
{
    public float speed;
    public int points;
    public float maxSpeed;//Replace with your max speed
    private bool collision = false;

    // Use this for initialization
    void Start()
    {
        //	GetComponent<Rigidbody2D>().velocity = transform.up * -speed;
    }


    void FixedUpdate()
    {
        if (GetComponent<Rigidbody2D>().velocity.magnitude > maxSpeed && collision)
        {
            GetComponent<Rigidbody2D>().velocity = GetComponent<Rigidbody2D>().velocity.normalized * maxSpeed;
        }
    }

    void OnCollisionEnter2D()
    {
       // Debug.Log("Collider entered");
        collision = true;
    }
}