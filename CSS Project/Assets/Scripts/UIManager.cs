using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using System.Collections;

public class UIManager : MonoBehaviour {

    public GameObject[] pauseObjects;
    private GameController gamecontroller;

    // Use this for initialization
    void Start()
    {
      //  DontDestroyOnLoad(gameObject);
        Time.timeScale = 1;
        pauseObjects = GameObject.FindGameObjectsWithTag("Paused");
    //    gamecontroller = GameObject.Find("GameController").GetComponent<GameController>();
        hidePaused();
    }

    // Update is called once per frame
    void Update()
    {

        //uses the p button to pause and unpause the game
        if (Input.GetKeyDown(KeyCode.P))
        {
            TogglePauseMenu();
        }
    }

    public void TogglePauseMenu()
    {
        
        if (Time.timeScale == 1)
        {
            Time.timeScale = 0;
            showPaused();
        }
        else if (Time.timeScale == 0)
        {
           // Debug.Log("high");
            Time.timeScale = 1;
            hidePaused();
        }
    }

    //Reloads the Level
    public void Reload()
    {
        //Application.LoadLevel(Application.loadedLevel);
        SceneManager.LoadScene(SceneManager.GetSceneAt(0).buildIndex);
        if (LevelController.level > 3)
        {
            LevelController.addToTrashLevel -= 10f;
        }
       // gamecontroller.ReloadScene();
    }

    //controls the pausing of the scene
    public void pauseControl()
    {
        if (Time.timeScale == 1)
        {
            Time.timeScale = 0;
            showPaused();
        }
        else if (Time.timeScale == 0)
        {
            Time.timeScale = 1;
            hidePaused();
        }
    }

    //shows objects with ShowOnPause tag
    public void showPaused()
    {
        foreach (GameObject g in pauseObjects)
        {
            g.SetActive(true);
        }
    }

    //hides objects with ShowOnPause tag
    public void hidePaused()
    {
        foreach (GameObject g in pauseObjects)
        {
            g.SetActive(false);
        }
    }

    //loads inputted level
    public void LoadLevel(string level)
    {
        //Application.LoadLevel(level);
        SceneManager.LoadScene(level);
        if (level.Equals("MainMenu"))
        {
            PlayerPrefs.SetInt("Goal Score", 500);
            PlayerPrefs.SetInt("Player Score", 0);
            PlayerPrefs.SetFloat("Time", 110);
            LevelController.goalScore.Clear();
            LevelController.level = 1;
            LevelController.addToTrashLevel = 0f;
        }
    }


}
