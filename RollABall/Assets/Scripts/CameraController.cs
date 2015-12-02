using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {
	public GameObject player;

	private Vector3 offset; //=current transform position of the camera - transform position of the player

	// Use this for initialization
	void Start () {
		offset = transform.position - player.transform.position;
	}
	
	// LateUpdate is called once per frame, but it is guaranteed to run 
	// after all items have been processed in update
	void LateUpdate () {
		transform.position = player.transform.position + offset;
	}

	void SetActive(){
		this.enabled = true;
		player.GetComponent<PlayerController> ().enabled = true;
		}
}
