using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic; //allows us to use lists
using Random = UnityEngine.Random; //specify this because there's two random functions
//in the system and unity namespaces

public class BoardManager : MonoBehaviour {
	[Serializable] //allows us to modify how variables appear in the Inspector and Editor
	//and allows us to hide and show them as appropriate
	public class Count
	{
		public int minimum;
		public int maximum;

		public Count (int min, int max)//constructor
		{
			minimum = min;
			maximum = max;
		}
	}
//delineate the dimensions of our game board
//This means we have an 8 x 8 game board
	public int columns = 8; 
	public int rows = 8;
	
	/*These are the different objects we want to generate on the
	 *board
	 */
	public Count wallCount = new Count (5,9); //minimum of 5, maximum of 9 random walls per level
	public Count foodCount = new Count (1,5); //minimum of 1, maximum of 5 random food items
	public GameObject exit; //tile to go to next level
	
	public GameObject[] floorTiles; //array of floor tiles
	public GameObject[] wallTiles; //array of wall tiles, the ones that the player can chop through
	public GameObject[] foodTiles; //array of collectable food tiles
	public GameObject[] enemyTiles; //array of enemy tiles
	public GameObject[] outerWallTiles; //array of outer wall tiles

	private Transform boardHolder; //child gameobjects will go in here
	private List <Vector3> gridPositions = new List<Vector3>(); 
	//track all the different possible positions on the gameboard, 
	//track whether an object has been spawned on that position or not
	
	/*Used to set up the spawnable areas of the game board
	*/
	void InitialiseList()
	{
		gridPositions.Clear ();

		for (int x=1; x < columns - 1; x++) { /*one space is left to create a border of accessible floor tiles within outer walls
		so levels are not completely impassable*/
			for (int y=1; y<rows-1; y++) {
				gridPositions.Add (new Vector3 (x, y, 0f)); //Fill list with each position on gameboard
			}
		}
	}

	void BoardSetup() //used to set up outer wall and the floor of the game board
	{
		boardHolder = new GameObject ("Board").transform;

		for (int x = -1; x<columns + 1; x++) {//laying out the floor and outer wall
			for (int y=-1; y<rows + 1; y++) {
				GameObject toInstantiate = floorTiles [Random.Range (0, floorTiles.Length)];
				if (x == -1 || x == columns || y == -1 || y == rows) //if in outer wall position, choose outer wall sprite
					toInstantiate = outerWallTiles [Random.Range (0, outerWallTiles.Length)];
				GameObject instance = Instantiate (toInstantiate, new Vector3 (x, y, 0f), Quaternion.identity) as GameObject;
				instance.transform.SetParent (boardHolder); //child the gameObject under boardHolder
			}
		}
	}

	Vector3 RandomPosition() //creates a random position for enemies, food and the player
	{
		int randomIndex = Random.Range (0, gridPositions.Count);
		Vector3 randomPosition = gridPositions [randomIndex];
		gridPositions.RemoveAt (randomIndex); //remove that grid position so that 2 objects are not spawned in the same position
		return randomPosition;
	}

	void LayoutObjectAtRandom(GameObject[] tileArray, int minimum, int maximum)
	{
		int objectCount = Random.Range (minimum, maximum + 1); //controls how many of a given object to spawn

		for (int i=0; i<objectCount; i++) {
			Vector3 randomPosition = RandomPosition ();
			GameObject tileChoice = tileArray [Random.Range (0, tileArray.Length)];
			Instantiate (tileChoice, randomPosition, Quaternion.identity);
		}
	}

	// Use this for initialization
	public void SetupScene (int level) {
		BoardSetup ();
		InitialiseList ();
		LayoutObjectAtRandom (wallTiles, wallCount.minimum, wallCount.maximum);
		LayoutObjectAtRandom (foodTiles, foodCount.minimum, foodCount.maximum);
		int enemyCount = (int)Mathf.Log (level,2f);
		LayoutObjectAtRandom (enemyTiles, enemyCount, enemyCount);
		Instantiate (exit, new Vector3 (columns - 1, rows - 1, 0f), Quaternion.identity);

	}
	

}
