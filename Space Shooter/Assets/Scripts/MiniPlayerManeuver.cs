using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MiniPlayerManeuver : MonoBehaviour {
	//public Boundary boundary;
	//public float tilt;
	//public float dodge;
	//public float smoothing;
	//public Vector2 startWait;
	//public Vector2 maneuverTime;
	//public Vector2 maneuverWait;
	
	public float currentSpeed;
	public Vector3 target;
	//Vector3 target = new Vector3(-1.5f,0.0f,0.0f);
	private Vector3 targetPosition;

	// Use this for initialization
	void Start () {
		//transform.position = Vector3.MoveTowards(transform.position, transform.position + target, 1.5f);
		//currentSpeed = rigidbody.velocity.z;
		//currentSpeed = 1.7f;

		//targetManeuver = dodge * -Mathf.Sign (transform.position.x);
		//StartCoroutine(Evade());
	}


	void Update() {

		targetPosition = transform.parent.position + target;
		//Debug.Log (targetPosition);
		float step = currentSpeed * Time.deltaTime;
		transform.position = Vector3.MoveTowards(transform.position, targetPosition, step);
		targetPosition = new Vector3 (0.0f, 0.0f, 0.0f);
	}

	
	/*IEnumerator Evade ()
	{
		yield return new WaitForSeconds (Random.Range (startWait.x, startWait.y));
		while (true) //infinite loop
		{
			targetManeuver = Random.Range (1, dodge) * -Mathf.Sign (transform.position.x); //randomly choose a value between 1 and dodge to make targetManeuver
			yield return new WaitForSeconds (Random.Range (maneuverTime.x, maneuverTime.y));
			targetManeuver = 0;
			yield return new WaitForSeconds (Random.Range (maneuverWait.x, maneuverWait.y));
		}
	}*/
	
	
	/*void FixedUpdate ()
	{
		float newManeuver = Mathf.MoveTowards (rigidbody.velocity.x, targetManeuver, smoothing * Time.deltaTime); //create position enemy must move towards, store in newManeuver
		rigidbody.velocity = new Vector3 (newManeuver, 0.0f, currentSpeed);
		rigidbody.position = new Vector3
			(
				Mathf.Clamp(rigidbody.position.x, boundary.xMin, boundary.xMax), 
				0.0f, 
				Mathf.Clamp(rigidbody.position.z, boundary.zMin, boundary.zMax)
				);
		
		rigidbody.rotation = Quaternion.Euler (0, 0, rigidbody.velocity.x * -tilt);
	}*/
}
