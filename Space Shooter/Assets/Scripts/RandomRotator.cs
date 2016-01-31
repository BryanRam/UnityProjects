using UnityEngine;
using System.Collections;

/*This script takes tumble from editor, then at start we set the new asteroid's angular velocity
 * to a random vector 3 value from insideUnitSphere multiplied by tumble
*/

public class RandomRotator : MonoBehaviour {

	public float tumble;

	void Start()
	{
		rigidbody.angularVelocity = Random.insideUnitSphere * tumble; 
	}
}
