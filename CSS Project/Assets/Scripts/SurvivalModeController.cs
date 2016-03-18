using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class SurvivalModeController : MonoBehaviour {
    public GameObject[] hazards;
    public GameObject[] powerups;
    public Vector3 spawnValues;
    public int hazardCount;
    public float spawnWait;
    public float startWait;
    public float waveWait;
    //public Animator mainCamera;

    public Slider slider;

    public Text highText;
    public int highScore;

    public bool gameOver;
    private GameObject bin;

    public GameObject pauseButton, gameOverText, restartButton, mainMenuButton;
    public UIManager uimanager;

    public static GameController instance = null;
    private List<float> gravScales = new List<float>();

    //When the game has started
    void Awake()
    {
       
    }

    // Use this for initialization
    void Start()
    {
        StartCoroutine(SpawnWaves());
       // Debug.Log(hazards.Length);
        if (PlayerPrefs.HasKey("High Score"))
            highScore = PlayerPrefs.GetInt("High Score");
        else
            highScore = 0;
        highText.text = "High Score: " + highScore;
        bin = GameObject.Find("bin");
        gameOver = false;
        //PlayerPrefs.DeleteKey("Default Gravity Scale");

     //   if (PlayerPrefsX.GetFloatArray("Default Gravity Scale").Length == 0)
     //   {
            foreach (GameObject h in hazards)
            {
                gravScales.Add(h.GetComponent<Rigidbody2D>().gravityScale);
            }
            //Debug.Log();
     //       PlayerPrefsX.SetFloatArray("Default Gravity Scale", gravScales.ToArray());
     //   }
    }

    // Update is called once per frame
    void Update()
    {
        
        CheckIfGameOver();

    }

    void CheckIfGameOver()
    {
        if (gameOver)
        {
            HideElements();
            uimanager.showPaused();
            uimanager.pauseObjects[0].SetActive(false);
            GameObject.Find("Paused").SetActive(false);
            gameOverText.SetActive(true);
            PlayerPrefs.SetInt("Player Score", bin.GetComponent<BinController>().score);
            PlayerPrefs.SetInt("High Score", bin.GetComponent<BinController>().score);
            highText.text = "High Score: " + highScore;

            for(int i = 0; i<hazards.Length; i++)
            {
                hazards[i].GetComponent<Rigidbody2D>().gravityScale = gravScales[i];
            }
        }
       
    }

    void OnDestroy()
    {
        for (int i = 0; i < hazards.Length; i++)
        {
            hazards[i].GetComponent<Rigidbody2D>().gravityScale = gravScales[i];
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
        while (slider.value < 100 && !gameOver) //loop until trash level is at max
        {
            for (int i = 0; i < hazardCount; i++) //spawn a number of hazards determined by hazardCount
            {
                if (slider.value >= 100)
                {
                    gameOver = true;
                    break;
                }

               
                GameObject hazard = hazards[Random.Range(0, hazards.Length)];  //choose a random hazard from the ones specified in the inspector
                Vector3 spawnPosition = new Vector3(Random.Range(-spawnValues.x, spawnValues.x), spawnValues.y, spawnValues.z); //create a random spawning position between spawnValues
                Quaternion spawnRotation = Quaternion.identity;

                Instantiate(hazard, spawnPosition, spawnRotation); //spawn hazard a location determined by spawnPosition, with a default rotation
                



                yield return new WaitForSeconds(spawnWait); //wait spawnWait seconds before calling another hazard
            }

            //  Vector3 powerupPosition = new Vector3(Random.Range(-spawnValues.x, spawnValues.x), spawnValues.y, spawnValues.z);
            if (gameOver)
            {
                break;
            }



            hazardCount = hazardCount + 2; //increase number of hazards 
            if (hazardCount % 4 == 0)
            {//increase speed of each hazard when number of hazards becomes a number divisible by 4
                foreach (GameObject h in hazards)
                {
                    h.GetComponent<Rigidbody2D>().gravityScale += 0.05f;
                }
            }
            yield return new WaitForSeconds(waveWait); //wait waveWait seconds before calling another wave (calling SpawnWaves again)



        }
    }

    public void ReloadScene()
    {
       
        slider.value = 0;
        bin.GetComponent<BinController>().score = 0;
    }

    
}
