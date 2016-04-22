using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

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
   // private float totalTime;

    public Text goalText;
    public int goalScore;

    public bool gameOver;
    public bool levelComplete;
    private GameObject bin;

    public GameObject pauseButton, gameOverText, levelCompleteText, nextLevelButton, mainMenuButton;
    public UIManager uimanager;

    public Text penalty;

    //public static GameController instance = null;
    public LevelController levelParams;
    private List<float> defaultGrav = new List<float>();
   

    //When the game has started
   

        // Use this for initialization
        void Awake () {
        levelParams.LevelParams();
        StartCoroutine(SpawnWaves());
        /*if (PlayerPrefs.HasKey("Goal Score"))
            goalScore = PlayerPrefs.GetInt("Goal Score");
        else
            PlayerPrefs.SetInt("Goal Score", goalScore); */

        if (PlayerPrefs.HasKey("Time"))
            timeLeft = PlayerPrefs.GetFloat("Time");
        else
            PlayerPrefs.SetFloat("Time", timeLeft);

        //   totalTime = timeLeft;

        //goalText.text = "Goal: " + goalScore;
        goalText.text = "Goal: " + LevelController.goalScore[LevelController.level];
        bin = GameObject.Find("bin");
        gameOver = false;
        levelComplete = false;
       
        spawnWait -= LevelController.cooldown;
        //Debug.Log("SpawnWait:" + spawnWait + "Reduce cooldown by: " + LevelController.cooldown);
        /*
        if (PlayerPrefs.HasKey("Default Gravity"))
        {
            defaultGrav.AddRange(PlayerPrefsX.GetFloatArray("Default Gravity"));
            
        }

        else
        {
            foreach (GameObject h in hazards)
            {
                defaultGrav.Add(h.GetComponent<Rigidbody2D>().gravityScale);
            }
            PlayerPrefsX.SetFloatArray("Default Gravity", defaultGrav.ToArray());
        }
        */

        if (LevelController.level == 1)
        {
            foreach (GameObject h in hazards)
            {
                defaultGrav.Add(h.GetComponent<Rigidbody2D>().gravityScale);
            }
            PlayerPrefsX.SetFloatArray("Default Gravity", defaultGrav.ToArray());
        }

        else
        {
            
            foreach (GameObject h in hazards)
            {
                h.GetComponent<Rigidbody2D>().gravityScale += levelParams.gravScaleToAdd;
                //Debug.Log("Gravity Scale: " + h.GetComponent<Rigidbody2D>().gravityScale + " GravAdd: " + levelParams.gravScaleToAdd);
            }
        }
        //goalScore = 3000;
        //foreach (GameObject h in hazards)
        //{
        //    h.GetComponent<Rigidbody2D>().gravityScale += 0.5f;
        //}
        //spawnWait = .7f; 




    }

    void OnDestroy()
    {
        //int i = 1;
        //Debug.Log("hazards: " + hazards.Length + " defaultgrav: " + defaultGrav.Count);
        defaultGrav.Clear();
        defaultGrav.AddRange(PlayerPrefsX.GetFloatArray("Default Gravity"));
        for (int i=0; i< hazards.Length; i++)
        {
            hazards[i].GetComponent<Rigidbody2D>().gravityScale = defaultGrav[i];
            //i++;
        }
    }
    
	
	// Update is called once per frame
	void Update () {
        //Debug.Log(gameOver);
        
        if (LevelController.isTimed)
        {
        
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

        
        }

        else
        {
            time.text = "";
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
            timeLeft = PlayerPrefs.GetFloat("Time");
            
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
            timeLeft = PlayerPrefs.GetFloat("Time");
        }
    }

    void HideElements()
    {
        bin.gameObject.SetActive(false);
        pauseButton.SetActive(false);
        penalty.text = "";
    }

    IEnumerator SpawnWaves()
    {
        yield return new WaitForSeconds(startWait);
        while (!gameOver && !levelComplete) //loop until trash level is at max or the level is complete
        {
            for (int i = 0; i < hazardCount; i++) //spawn a number of hazards determined by hazardCount
            {
                if (slider.value >= 100 || timeLeft <= 0)
                {
                    gameOver = true;
                    break;
                }

                //   GameObject powerup = powerups[Random.Range(0, powerups.Length)]; //choose a random powerup from the ones specified in the inspector
                //if (LevelController.level == 2)
                //{
                  
                //    Debug.Log("Score in Spawn: " + LevelController.goalScore[LevelController.level]);
                //    Debug.Log("Score in Spawn: " + LevelController.goalScore[2]);
                //}
                
                if (bin.GetComponent<BinController>().score >= LevelController.goalScore[LevelController.level])
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
        timeLeft = 90;
        slider.value = 0;
        bin.GetComponent<BinController>().score = 0;
    }

    public void NextLevel()
    {
        //PlayerPrefs.SetInt("Goal Score", goalScore + 500);
        if((LevelController.level + 1) % 2 == 0)
            PlayerPrefs.SetFloat("Time", timeLeft - 20);
        levelParams.NextLevel();
        SceneManager.LoadScene("Demo");
        //foreach (GameObject h in hazards)
        //{
        //    h.GetComponent<Rigidbody2D>().gravityScale += .25f;
        //}

        
        
        
        

        /*
        Figure out how to feature the sudden death round. Determine point of round.
        How to increase difficulty: 
	  
	   i. Goal Score increases (use a fibonacci sequence for score increases)
       ii. Reduce time left (bottom limit should be err, 15 seconds?)
       iii. make items fall faster (upper limit 1.5 gravity scale? .25 increments)
       iv. reduce cooldown time between spawns  (bottom limit should be around 0.8-0.5 seconds. .1 decrements)
       v. start levels with dirtiness level raised, (upper limit 90% dirtiness in non-sudden death)

        So a sample set of rounds:
        EASY MODE
        1.
        i.  Goal Score: 500
        ii. Untimed
        iii.Grav Scale: Standard
        iv. Cooldown time: Standard
        v.  Dirtiness level: 0

        2.
        Goal Score: 1000                fibo
        Time: 150 seconds               even numbered levels
        Grav Scale: +0.25               +.25
        Cooldown time: Standard         +0
        Dirtiness level: 0              +0
        
        3.
        Goal Score: 1500                fibo
        Untimed                         odd numbered levels
        Grav Scale: +0.5                +.25
        Cooldown time: -.1              -.1
        Dirtiness level: 20%            +20%

        
        4.
        Goal Score: 2500                fibo
        Time: 200 seconds               even
        Grav Scale: +0.75               +.25
        Cooldown time: -.2              -.1
        Dirtiness level: 30%            +10%
        
        5.
        Goal Score: 4000                fibo
        Untimed                         odd
        Grav Scale: +0.5                +.25
        Cooldown time: -.3              -.1
        Dirtiness level: 40%            +10%

         
        Then we do it again with Medium and Hard
        Medium

        5.
        Goal Score: 3000
        Untimed
        Grav Scale: +0.5
        Cooldown time: -.3
        Dirtiness level: 50%

        */
    }
}
