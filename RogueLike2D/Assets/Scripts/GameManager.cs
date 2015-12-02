using UnityEngine;
using System.Collections;
using System.Collections.Generic; //Allows use of lists
using UnityEngine.UI;

/*class GameManager determines the current game state*/
public class GameManager : MonoBehaviour {
	public float levelStartDelay = 2f; // time to wait before starting level
	public float turnDelay = .1f; //time to wait before the player can move
	public static GameManager instance = null;
	private BoardManager boardScript; //reference to the BoardManager script
	public int playerFoodPoints = 100; //the Player's health
	[HideInInspector] public bool playersTurn = true;

	private Text levelText;
	private GameObject levelImage;
	private int level = 1024; //Used to keep track of the current level
	private List<Enemy> enemies; //Used to keep track of enemies and send orders to move
	private bool enemiesMoving;
	private bool doingSetup;

	//When the game has started
	void Awake () {
		/*Only one instance of the GameManager script can exist at runtime,
		 *so if another instance is found, destroy it
		 */
		if (instance == null) 
			instance = this;
		else
			Destroy (gameObject);
		
		/*When a new scene is loaded, all of the objects in the current scene are usually destroyed,
		*since GameManager is keeping track of the levels and player's Food, we need it present at 
		*all times.
		*/
		DontDestroyOnLoad (gameObject);
		enemies = new List<Enemy>(); //Declare a list of enemies
		boardScript = GetComponent<BoardManager> ();
		InitGame ();
	}

	private void OnLevelWasLoaded (int index)//Unity API that's called everytime level is loaded
	{
		level++;

		InitGame ();
	}

	void InitGame()
	{
		doingSetup = true;

		levelImage = GameObject.Find ("LevelImage");
		levelText = GameObject.Find ("LevelText").GetComponent<Text> ();
		levelText.text = "Day " + level;
		levelImage.SetActive (true);

		Invoke ("HideLevelImage", levelStartDelay); //wait two seconds before hiding title card 

		enemies.Clear (); //The gameManager will not be reset once the game starts, so enemies from previous levels must be removed
		boardScript.SetupScene (level);
	}

	private void HideLevelImage()
	{
		levelImage.SetActive (false);
		doingSetup = false;
	}

	public void GameOver()
	{
		levelText.text = "After " + level + " days,\n you starved\n to death";
		levelImage.SetActive (true);
		enabled = false;
	}

	// Update is called once per frame
	void Update () {
	if (playersTurn || enemiesMoving || doingSetup)
			return;

		StartCoroutine (MoveEnemies ());
	}

	public void AddEnemyToList(Enemy script)
	{
		enemies.Add (script);
	}

	IEnumerator MoveEnemies() //use this to move enemies one at a time in sequence
	{
		enemiesMoving = true;
		yield return new WaitForSeconds (turnDelay);
		if (enemies.Count == 0) 
		{
			yield return new WaitForSeconds(turnDelay);
		}

		for (int i=0; i<enemies.Count; i++) {
			enemies [i].MoveEnemy ();
			yield return new WaitForSeconds (enemies [i].moveTime);
		}

		playersTurn = true;
		enemiesMoving = false;
	}
}
