using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerController : MonoBehaviour {
	//private Rigidbody rb;
	public float speed;
	public float jump;
	public Text countText;
	public Text winText;

	private int count;

	// Use this for initialization
	void Start () {
		//rb = GetComponent<Rigidbody> ();
		count = 0;
		SetCountText();
		winText.text = "";
	}
	

	//FixedUpdate is called before performing any physics calculations
	void FixedUpdate()
	{
		float moveHorizontal= Input.GetAxis ("Horizontal");
		float moveVertical = Input.GetAxis ("Vertical");
		Vector3 movement;


		if (Input.GetButton ("Fire1") && transform.position.y <= 0.5) 
			movement = new Vector3 (moveHorizontal, jump, moveVertical);
		else
			movement = new Vector3 (moveHorizontal, 0, moveVertical);

		gameObject.GetComponent<Rigidbody>().AddForce(movement * speed);



	}

	void Update()
	{
		if (transform.position.y <= -20.0) 
		{
			transform.position = new Vector3(0, 10, 0);
		}

	}

	void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.CompareTag("Pickup")) {
			other.gameObject.SetActive (false);
			count++;
			SetCountText();
		}
	}

	void SetCountText()
	{
		countText.text = "Count: " + count;
		if (count >= 11) 
		{
			winText.text = "You Win!";
		}
		}
}
