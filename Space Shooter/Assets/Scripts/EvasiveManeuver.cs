using UnityEngine;
using System.Collections;

public class EvasiveManeuver : MonoBehaviour {
	public Boundary boundary;
	public float tilt;
	public float dodge;
	public float smoothing;
	public Vector2 startWait;
	public Vector2 maneuverTime;
	public Vector2 maneuverWait;

	private float currentSpeed;
	private float targetManeuver;


	// Use this for initialization
	void Start () {
		currentSpeed = rigidbody.velocity.z;
		StartCoroutine(Evade());
	}


	IEnumerator Evade ()
	{
		yield return new WaitForSeconds (Random.Range (startWait.x, startWait.y));
		while (true) //infinite loop
		{
			targetManeuver = Random.Range (1, dodge) * -Mathf.Sign (transform.position.x); //randomly choose a value between 1 and dodge to make targetManeuver
			yield return new WaitForSeconds (Random.Range (maneuverTime.x, maneuverTime.y));
			targetManeuver = 0;
			yield return new WaitForSeconds (Random.Range (maneuverWait.x, maneuverWait.y));
		}
	}


	void FixedUpdate ()
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
	}
}
