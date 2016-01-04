using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
	public float speed = 6f;

	Vector3 movement; //stores the movement we want the Player to have
	Animator anim;
	Rigidbody playerRigidbody;
    //set drag and angular drag to infinity so that the player does not slow down over time
    //Constrain position is used to ensure the player does not fall forward or any other direction (when moving?)
    //Freeze Y position so player does not sink through the floor or end up flying

    int floorMask;//for the floor Quad. To tell the Raycast that we only want to hit that floor is with this variable, a LayerMask. Which is stored as an integer
	float camRayLength = 100f; //The length of the Ray that we cast from the camera

	void Awake()//Similar to Start, but gets called whether the script is enabled or not
	{
		floorMask = LayerMask.GetMask ("Floor");
		anim = GetComponent<Animator> ();
		playerRigidbody = GetComponent<Rigidbody> ();

	}

	void FixedUpdate()
	{
        /*Raw Axes only have values of -1, 0 or 1, rather than the character slowly moving towards his full speed,
         *he's immediately going to go full speed, helping response time
         */
        float h = Input.GetAxisRaw ("Horizontal"); 
		float v = Input.GetAxisRaw ("Vertical");

		Move (h, v);
		Turning ();
		Animating (h, v);
	}

    //Function to store player movement
	void Move(float h, float v)
	{
		movement.Set (h, 0f, v); //since it's Vector3

        /*If you move just in the x axis by 1 unit, or the same in the z axis, then that's ok,
         *however if you move in both, you get a value of 1.4, resulting in an advantage in moving diagonally
         *So we'll normalize that: make sure that the value returned is one no matter the key combination.
         */
		movement = movement.normalized * speed * Time.deltaTime; //deltaTime is the time between each update call
        //player is moving at defined speed for each 50th of a second essentially

        //MovePosition moves a rigidBody to a defined position in World Space
		playerRigidbody.MovePosition (transform.position + movement);
	}

    //Self Explanatory, based on mouse input
	void Turning()
	{
        //this is a Ray from the camera 
		Ray camRay = Camera.main.ScreenPointToRay (Input.mousePosition);
        /*ScreenPointToRay takes a point on the screen (this case the mouse) and cast a ray/line from that point, and into the scene.
        *We need to get information back from this Raycast upon it coming into contact with something, and to do this we define a RaycastHit variable
        */
		RaycastHit floorHit;

        //now to actually cast a Ray from the Ray variable, if it has hit something, it returns true/false
        //We need to give it a few parameters, the Ray itself, the variable to store the hit, length of the raycast, 
        //floor mask for where you want the ray to hit
		if(Physics.Raycast(camRay, out floorHit, camRayLength, floorMask))
		{
			Vector3 playerToMouse = floorHit.point - transform.position; //point where the mouse hit the floor - player position
			playerToMouse.y = 0f; //you want the player to turn, but don't want the player to lean back, so the y component must be 0

            //Quaternion: way to store rotation. 
            //LookRotation makes the target the target vector the forward vector
			Quaternion newRotation = Quaternion.LookRotation(playerToMouse);
			playerRigidbody.MoveRotation(newRotation);
		}
	}

	void Animating(float h, float v)
	{
        //if player is walking or not, set the walking animation appropriately
		bool walking = h != 0f || v != 0f;
		anim.SetBool ("IsWalking", walking);
	}
}
