using UnityEngine;
using System.Collections;

public class BG_Scroller : MonoBehaviour {
	public float scrollSpeed;
	public float tileSizeZ;
	private float newPosition;

	private Vector3 startPosition;

	// Use this for initialization
	void Start () {
		startPosition = transform.position; //x:0 y:-0.9999998 z:9.536741e-07
	}
	
	// Update is called once per frame
	void Update () {
		/*loops the background at a rate of scrollSpeed
		 * Making sure not to exceed tileSizeZ and returning a number greater
		 * than zero when the loop resets
		 */
		newPosition = Mathf.Repeat (Time.time * scrollSpeed, tileSizeZ); 
		transform.position = startPosition + Vector3.forward * newPosition; 

	}
}
