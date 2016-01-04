using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour {
    public Transform target; //for the camera to follow
    public float smoothing = 5f; //camera will follow the player, but we don't want it to be super sharp, want a little lag

    Vector3 offset;

	// Use this for initialization
	void Start ()
    { //store the offset of the camera from the player
      //make sure the distance between position of the camera and the position of the player is stored in offset
        offset = transform.position - target.position;
	}
	
	// FixedUpdate is called once per physics call
    //We used FixedUpdate on the camera because we're following a physics object, and the physics object (the player) is using FixedUpdate to move
	void FixedUpdate ()
    {//We're trying to find a position for the camera to be up above the level
        Vector3 targetCamPos = target.position + offset;
        transform.position = Vector3.Lerp(transform.position, targetCamPos, smoothing * Time.deltaTime); //Lerp smoothly moves between two positions, with smoothing indicating how fast we get there
           
	}
}
