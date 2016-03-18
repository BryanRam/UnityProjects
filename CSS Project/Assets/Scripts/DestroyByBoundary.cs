using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DestroyByBoundary : MonoBehaviour {
    public GameController level;
    public Slider slider;
    private int maxTrash;
    public Image Fill;  // assign in the editor the "Fill"
    public Color MaxTrashColor = Color.red;
    public Color MinTrashColor = Color.green;
    // Use this for initialization
    void Start () {
        //   slider.onValueChanged.AddListener(IncreaseDirtiness);
        maxTrash = (int)slider.maxValue;
    }
	
	// Update is called once per frame
	void Update () {
	}

    void OnTriggerEnter2D(Collider2D other)
    {
        Destroy(other.gameObject);
        IncreaseTrash(5f);
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


   
}
