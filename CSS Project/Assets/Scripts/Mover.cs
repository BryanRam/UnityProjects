using UnityEngine;
using System.Collections;

public class Mover : MonoBehaviour
{
    public float speed;
    public int points, finalPoints;
    public float maxSpeed;//Replace with your max speed
    private bool collision = false;
    private int multiplier;
    LineRenderer fallLine;

    // Use this for initialization
    void Start()
    {
        //	GetComponent<Rigidbody2D>().velocity = transform.up * -speed;
        multiplier = GameObject.Find("bin").GetComponent<CollectionBoost>().multiplier;
        Score();
        fallLine = GetComponent<LineRenderer>();
    }


    void FixedUpdate()
    {
        if (GetComponent<Rigidbody2D>().velocity.magnitude > maxSpeed && collision && gameObject.transform.position.y > -1f)
        {
            GetComponent<Rigidbody2D>().velocity = GetComponent<Rigidbody2D>().velocity.normalized * maxSpeed;
        }

        
            fallLine.SetPosition(0, transform.position);
            fallLine.SetPosition(1, new Vector3(transform.position.x, -10f, 0f));
        
    }

    void OnCollisionEnter2D()
    {
       // Debug.Log("Collider entered");
        collision = true;
    }

    public void Score()
    {
        finalPoints = points * multiplier;
    }
}