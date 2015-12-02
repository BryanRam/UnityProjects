using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {
	public GameObject[] hazards;
	public GameObject[] powerups;
	public Vector3 spawnValues;
	public int hazardCount;
	public float spawnWait;
	public float startWait;
	public float waveWait;
	public Animator mainCamera;

	public GUIText scoreText;
	public GUIText restartText;
	public GUIText quitText;
	public GUIText gameOverText;
	private bool gameOver, restart;
	private int score;
	private PlayerController player;


	// Use this for initialization
	void Start () {
		gameOver = restart = false;
		restartText.text = gameOverText.text = quitText.text = "";
		score = 0;
		player = GameObject.Find ("Player").GetComponent<PlayerController> ();
		UpdateScore ();
		StartCoroutine (SpawnWaves ());
	}

	void Update() 
	{
		if (restart) 
		{

			if (Input.GetKeyDown (KeyCode.R)) {
				Application.LoadLevel(Application.loadedLevel);
			}

			if (Input.GetKeyDown (KeyCode.Q)) {
				Application.Quit();
			}
		}
	}

	IEnumerator SpawnWaves ()
	{
		yield return new WaitForSeconds(startWait);
		while(true) //infinite loop for game play
		{
			for (int i=0; i<hazardCount; i++) //spawn a number of hazards determined by hazardCount
			{
				GameObject hazard = hazards [Random.Range (0, hazards.Length)];  //choose a random hazard from the ones specified in the inspector
				Vector3 spawnPosition = new Vector3 (Random.Range (-spawnValues.x, spawnValues.x), spawnValues.y, spawnValues.z); //create a random spawning position between spawnValues
				Quaternion spawnRotation = Quaternion.identity;
				Instantiate (hazard, spawnPosition, spawnRotation); //spawn hazard a location determined by spawnPosition, with a default rotation
				yield return new WaitForSeconds(spawnWait); //wait spawnWait seconds before calling another hazard
			}

			Vector3 powerupPosition = new Vector3 (Random.Range (-spawnValues.x, spawnValues.x), spawnValues.y, spawnValues.z);

			GameObject powerup = powerups[Random.Range(0, powerups.Length)]; //choose a random powerup from the ones specified in the inspector
			if(player.hasMiniPlayers) //If the player has a miniPlayers powerup already
			{
				if(powerup.name == "MiniPlayerPowerup")//Refuse to spawn another miniPlayers powerup
				{
					//Do nothing
				}

				else //Otherwise spawn the powerup once it's not the miniPlayers one
				{
					Instantiate(powerup, powerupPosition, Quaternion.identity);
				}
			}
			else //spawn any random powerup if player does not have miniPlayers powerup
			{
				Instantiate(powerup, powerupPosition, Quaternion.identity);
			}

			hazardCount = hazardCount + 2; //increase number of hazards 
			yield return new WaitForSeconds(waveWait); //wait waveWait seconds before calling another wave (calling SpawnWaves again)


			if (gameOver)
			{
				restartText.text = "Press 'R' to restart";
				quitText.text = "Press 'Q' to quit";
				restart = true;
				break;
			}
		}
	}

	public void AddScore(int newScoreValue)
	{
		score += newScoreValue;
		UpdateScore ();
	}

	void UpdateScore ()
	{
		scoreText.text = "Score: " + score;
	}

	// Function GameOver displays Game Over text and triggers camera shake animation when player ship is destroyed
	public void GameOver () {
		gameOverText.text = "Game Over!";
		gameOver = true;
		mainCamera.SetTrigger ("GameOver"); //trigger camera shake animation

	}
}
