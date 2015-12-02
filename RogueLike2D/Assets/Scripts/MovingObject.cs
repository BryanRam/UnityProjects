using UnityEngine;
using System.Collections;

/*class MovingObject determines what happens when objects move around in the scene*/
public abstract class MovingObject : MonoBehaviour {
	public float moveTime = .1f; //the time it will take our object to move in seconds
	public LayerMask blockingLayer; //the layer on which we'll check collision to determine if a space can be moved into
	private BoxCollider2D boxCollider;
	private Rigidbody2D rb2D;
	private float inverseMoveTime; //to make movement calculations more effective

	// Use this for initialization
	protected virtual void Start () { //protected virtual methods can be overridden by inheritants
		boxCollider = GetComponent<BoxCollider2D> ();
		rb2D = GetComponent<Rigidbody2D> ();
		inverseMoveTime = 1f / moveTime;
	}

	protected bool Move (int xDir, int yDir, out RaycastHit2D hit) //out keyword causes arguments to be passed by reference
	{
		Vector2 start = transform.position;//store current transform position,  discarding z axis data
		Vector2 end = start + new Vector2 (xDir, yDir); //calculate end position based on the direction parameters of the starting point

		boxCollider.enabled = false; //make sure when you're casting your ray, you don't hit your own box collider
		hit = Physics2D.Linecast (start, end, blockingLayer); //cast a line from startpoint to endpoint, checking collision on blockingLayer which is stored in hit
		boxCollider.enabled = true;

		if (hit.transform == null) { //check if anything was stored in hit, if null aka space is available to move into
			StartCoroutine (SmoothMovement (end)); 
			return true;
		}
		return false; //else move was unsuccessful

	}
	



	protected IEnumerator SmoothMovement (Vector3 end) //use to move units from one space to the next
	{
		float sqrRemainingDistance = (transform.position - end).sqrMagnitude;

		while (sqrRemainingDistance>float.Epsilon) { //Epsilon is used for very small numbers, almost zero
			Vector3 newPosition = Vector3.MoveTowards (rb2D.position, end, inverseMoveTime * Time.deltaTime);
			rb2D.MovePosition (newPosition);
			sqrRemainingDistance = (transform.position - end).sqrMagnitude;
			yield return null; //wait for a frame before reevaluating the condition of the loop
		}
	}

	protected virtual void AttemptMove <T>(int xDir, int yDir) //generic parameter T is used to determine the type of component we expect our unit to interact with if blocked
		where T:Component //for player: this would be walls, enemies: the player
	{
		RaycastHit2D hit;
		bool canMove = Move (xDir, yDir, out hit);

		if (hit.transform == null)
			return;

		T hitComponent = hit.transform.GetComponent<T> (); //if something was hit, get component reference to the component of type T attached to the object that was hit

		if (!canMove && hitComponent != null)
			OnCantMove (hitComponent);

	}

	protected abstract void OnCantMove<T> (T component) 
		where T : Component;

}
