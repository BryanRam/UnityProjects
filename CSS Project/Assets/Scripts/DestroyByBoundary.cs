using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class DestroyByBoundary : MonoBehaviour {
    public GameController level;
    public Slider slider;
    private int maxTrash;
    public Image Fill;  // assign in the editor the "Fill"
    public Color MaxTrashColor = Color.red;
    public Color MinTrashColor = Color.green;
    public CollectionBoost collectBoost;
    // Use this for initialization
    void Start () {
        //   slider.onValueChanged.AddListener(IncreaseDirtiness);
        maxTrash = (int)slider.maxValue;
        slider.value += LevelController.addToTrashLevel;
        Fill.color = Color.Lerp(MinTrashColor, MaxTrashColor, (float)slider.value / maxTrash);
        Debug.Log("Level: " + LevelController.level + " Trash Level: " + LevelController.addToTrashLevel);
    }
	
	// Update is called once per frame
	void Update () {
	}

    void OnTriggerEnter2D(Collider2D other)
    {
        Destroy(other.gameObject);
        IncreaseTrash(5f);

        if(SceneManager.GetActiveScene().name.Equals("SurvivalMode"))
        {
            collectBoost.hasBoost = false;
            collectBoost.boostCounter = 0;
            collectBoost.bonusCounter = 0;
            collectBoost.collectionScore.text = "";
        }
    }

    void IncreaseTrash(float value)
    {
        if (!level.levelComplete)
        {
            // Debug.Log(slider.value);
            slider.value += value;
            Fill.color = Color.Lerp(MinTrashColor, MaxTrashColor, (float)slider.value / maxTrash);
            // Debug.Log(slider.value);
        }
        else
        {
            if (slider.value >= slider.maxValue)
            {
                level.gameOver = true;
            }
        }
    }

    void HitFloorSound()
    {
        //insert Audio for trash hitting the floor here
    }
   
}
