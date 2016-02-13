using UnityEngine;
using System.Collections;

public class Dodge : MonoBehaviour
{

    public Boundary boundary;
    public float tilt;
    public float dodge;
    public float smoothing;
    public Vector2 startWait;
    public Vector2 maneuverTime;
    public Vector2 maneuverWait;

    private float currentSpeed;
    //public float dodgeSpeed;
    private float targetManeuver;
    private bool hasDodged;
    public int count;


    // Use this for initialization
    void Start()
    {
        currentSpeed = gameObject.transform.parent.GetComponent<Rigidbody>().velocity.z;
        hasDodged = false;
        count = 0;
        // StartCoroutine(Evade());
    }


   public IEnumerator Evade()
    {
        // yield return new WaitForSeconds(Random.Range(startWait.x, startWait.y));
        
        while (true) //infinite loop
        {
           
            if (hasDodged == false)
            {
                targetManeuver =  dodge * -Mathf.Sign(transform.position.x); //randomly choose a value between 1 and dodge to make targetManeuver
                yield return new WaitForSeconds(0.3f);
                hasDodged = true;
               
                count++;
            }
            else 
            {
                targetManeuver = 0;
                GetComponent<BoxCollider>().enabled = false;
            }
            
            yield return new WaitForSeconds(0.3f);
            Debug.Log(targetManeuver);
        }
    }


    void FixedUpdate()
    {
        float newManeuver = Mathf.MoveTowards(gameObject.transform.parent.GetComponent<Rigidbody>().velocity.x, targetManeuver, smoothing * Time.deltaTime); //create position enemy must move towards, store in newManeuver
        gameObject.transform.parent.GetComponent<Rigidbody>().velocity = new Vector3(newManeuver, 0.0f, currentSpeed);
        gameObject.transform.parent.GetComponent<Rigidbody>().position = new Vector3
            (
                Mathf.Clamp(gameObject.transform.parent.GetComponent<Rigidbody>().position.x, boundary.xMin, boundary.xMax),
                0.0f,
                Mathf.Clamp(gameObject.transform.parent.GetComponent<Rigidbody>().position.z, boundary.zMin, boundary.zMax)
                );

        gameObject.transform.parent.GetComponent<Rigidbody>().rotation = Quaternion.Euler(0, 0, gameObject.transform.parent.GetComponent<Rigidbody>().velocity.x * -tilt);
       
    }

    void OnTriggerEnter(Collider other)
    {
       // Debug.Log(GetComponent<Collider>().GetType());
        
        if (GetComponent<Collider>().GetType() == typeof(BoxCollider) && other.tag == "Laser")
        {
           
            StartCoroutine(Evade());
        }
        
    }

}