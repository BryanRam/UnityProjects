using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameController : MonoBehaviour {
    public GameObject[] hazards;
    public GameObject[] powerups;
    public Vector3 spawnValues;
    public int hazardCount;
    public float spawnWait;
    public float startWait;
    public float waveWait;
    //public Animator mainCamera;

    public Slider slider;

    public Text time;
    public float timeLeft;

    public Text goalText;
    public int goalScore;

    public bool gameOver;
    public bool levelComplete;
    private GameObject bin;

    public GameObject pauseButton, gameOverText, levelCompleteText, nextLevelButton, mainMenuButton;
    public UIManager uimanager;

    public static GameController instance = null;


    //When the game has started
    void Awake()
    {
        /*Only one instance of the GameController script can exist at runtime,
		 *so if another instance is found, destroy it
		 */
    /*    if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);*/
    }

        // Use this for initialization
        void Start () {
        StartCoroutine(SpawnWaves());
        if (PlayerPrefs.HasKey("Goal Score"))
            goalScore = PlayerPrefs.GetInt("Goal Score");
        else
            PlayerPrefs.SetInt("Goal Score", goalScore);
        goalText.text = "Goal: " + goalScore;
        bin = GameObject.Find("bin");
        gameOver = false;
        levelComplete = false;
    }
	
	// Update is called once per frame
	void Update () {
        Debug.Log(gameOver);
        if (timeLeft <= 0f)
        {
            gameOver = true;
            time.text = "Time: " + 0;
        }

        else if(levelComplete || gameOver)
        {
            time.text = "Time: " + (int)timeLeft;
        }

        else
        {
            time.text = "Time: " + (int)timeLeft;
            timeLeft = timeLeft - Time.deltaTime;
        }

        CheckIfGameOver();

	}

    void CheckIfGameOver()
    {
        if(gameOver)
        {
            HideElements();
            uimanager.showPaused();
            uimanager.pauseObjects[0].SetActive(false);
            GameObject.Find("Paused").SetActive(false);
            gameOverText.SetActive(true);
            PlayerPrefs.SetInt("Player Score", bin.GetComponent<BinController>().score);
        }
        else
        {
            //Debug.Log(bin.GetComponent<BinController>().score);
            CheckIfLevelComplete();
        }
    }

    void CheckIfLevelComplete()
    {
       // Debug.Log(levelComplete);
        if (levelComplete)
        {
            HideElements();
            levelCompleteText.SetActive(true);
            nextLevelButton.SetActive(true);
            mainMenuButton.SetActive(true);
            //Debug.Log(levelCompleteText);
            PlayerPrefs.SetInt("Player Score", bin.GetComponent<BinController>().score);
        }
    }

    void HideElements()
    {
        bin.gameObject.SetActive(false);
        pauseButton.SetActive(false);
    }

    IEnumerator SpawnWaves()
    {
        yield return new WaitForSeconds(startWait);
        while (!gameOver && bin.GetComponent<BinController>().score <= goalScore) //loop until trash level is at max
        {
            for (int i = 0; i < hazardCount; i++) //spawn a number of hazards determined by hazardCount
            {
                if (slider.value >= 100 || timeLeft <= 0)
                {
                    gameOver = true;
                    break;
                }

                //   GameObject powerup = powerups[Random.Range(0, powerups.Length)]; //choose a random powerup from the ones specified in the inspector
                if (bin.GetComponent<BinController>().score >= goalScore)
                {
                    levelComplete = true;
                    break;
                }


                

                GameObject hazard = hazards[Random.Range(0, hazards.Length)];  //choose a random hazard from the ones specified in the inspector
                Vector3 spawnPosition = new Vector3(Random.Range(-spawnValues.x, spawnValues.x), spawnValues.y, spawnValues.z); //create a random spawning position between spawnValues
                Quaternion spawnRotation = Quaternion.identity;
               
                Instantiate(hazard, spawnPosition, spawnRotation); //spawn hazard a location determined by spawnPosition, with a default rotation
               

                

                yield return new WaitForSeconds(spawnWait); //wait spawnWait seconds before calling another hazard
            }

            //  Vector3 powerupPosition = new Vector3(Random.Range(-spawnValues.x, spawnValues.x), spawnValues.y, spawnValues.z);
            if (gameOver || levelComplete)
            {
                break;
            }



            hazardCount = hazardCount + 2; //increase number of hazards 
            yield return new WaitForSeconds(waveWait); //wait waveWait seconds before calling another wave (calling SpawnWaves again)



        }
    }

    public void ReloadScene()
    {
        timeLeft = 60;
        slider.value = 0;
        bin.GetComponent<BinController>().score = 0;
    }

    public void NextLevel()
    {
        PlayerPrefs.SetInt("Goal Score", goalScore + 500);
        SceneManager.LoadScene("Demo");
    }
}
