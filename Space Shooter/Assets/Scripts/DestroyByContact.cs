using UnityEngine;
using System.Collections;

public class DestroyByContact : MonoBehaviour {
	public GameObject explosion;
	public GameObject playerExplosion;
	private GameController gameController;
	public int scoreValue;
	public PlayerController player;

	void Start()
	{
		GameObject gameControllerObject = GameObject.FindWithTag ("GameController");
		if (gameControllerObject != null) {
			gameController = gameControllerObject.GetComponent<GameController> ();
		}

		if (gameController == null) 
		{
			Debug.Log ("Cannot find 'GameController' script");
		}
		player = GameObject.Find ("Player").GetComponent<PlayerController>();
	}

	void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Boundary" || other.tag == "Enemy")//|| other.tag == "Laser" 
		{
			return;
		}

		if (gameObject.tag == "Enemy" && gameObject.name == "EnemyBolt(Clone)" && other.tag == "Laser") //enemy laser hits player laser
		{//Do nothing, thus enemy laser phases through player laser
			return;
		}

		if (explosion != null) 
		{
			Instantiate (explosion, transform.position, transform.rotation);
		}

		if (other.tag == "Player") 
		{
			/*if (player.lives > 0)
			 * Instantiate (playerExplosion, other.transform.position, other.transform.rotation);
			 * player.lives--;
			 *else
			 *{
			 */
			Instantiate (playerExplosion, other.transform.position, other.transform.rotation);
			gameController.GameOver();

			/*}
			 */
		}

		if (other.tag == "MiniPlayer") 
		{

			if(other.name=="MiniPlayer")
			{
				player.boundary.xMin = -6;
				Debug.Log ("M1 destroyed!");
				Debug.Log ("DBC boundary xMin: " + player.boundary.xMin);
				//boundary.xMax = 4.5;
				//boundary.zMin = -3.7;

			}

			if(other.name=="MiniPlayer 2")
			{
				//boundary.xMin = -6;
				player.boundary.xMax = 6;
				Debug.Log ("M2 destroyed!");
				Debug.Log ("DBC boundary xMax: " + player.boundary.xMax);
				//boundary.zMin = -3.7;
			}
			Instantiate (explosion, transform.position, transform.rotation);
			other.transform.position = new Vector3(other.transform.parent.position.x, other.transform.parent.position.y, other.transform.parent.position.z);
			other.gameObject.SetActive (false);
			return;
		}
			Destroy (other.gameObject);
			Destroy (gameObject);
		gameController.AddScore (scoreValue);
	}
}
