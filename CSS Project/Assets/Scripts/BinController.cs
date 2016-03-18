using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

public class BinController : MonoBehaviour
{
    public Camera cam;
    private Rigidbody2D rbody;
    private SpriteRenderer sprite;
    public string[] binname;
    public Sprite[] bin;
    
    
    private int bincount = 0;

    public GameObject[] doNotTrigger;
    //private bool 

    private float maxWidth;
    private float nextFire = 0.0f;
    private float fireRate = 0.05f;
    private ChildBinController cbin;

    public Text scoreText;
    public int score;
    public GameObject penalty;

    public GameController gamecontroller;
    

    // Use this for initialization
    void Start () {
        if (cam == null)
        {
            cam = Camera.main;
        }
        rbody = GetComponent<Rigidbody2D>();
        Vector3 upperCorner = new Vector3(Screen.width, Screen.height, 0.0f);
        Vector3 targetWidth = cam.ScreenToWorldPoint(upperCorner);
        float binWidth = GetComponent<Renderer>().bounds.extents.x;
        maxWidth = targetWidth.x - binWidth;
        sprite = GetComponent<SpriteRenderer>();
        cbin = GetComponentInChildren<ChildBinController>();
        score = 0;
        scoreText.text = "Score: " + score;
        gameObject.tag = "Green";
	}

    /*
     public List<string> _keys;
    public List<Sprite> _values;

    //Unity doesn't know how to serialize a Dictionary
    public Dictionary<string, Sprite> bin = new Dictionary<string, Sprite>
    {
        {binname[0], bincolour[0]},
        {binname[1], bincolour[1]},
        {binname[2], bincolour[2]},
    };
    

    public void OnBeforeSerialize()
    {
        _keys.Clear();
        _values.Clear();
        foreach (var kvp in bin)
        {
            _keys.Add(kvp.Key);
            _values.Add(kvp.Value);
        }
    }

    public void OnAfterDeserialize()
    {
        bin.Clear();
        for (int i = 0; i != Mathf.Min(_keys.Count, _values.Count); i++)
            bin.Add(_keys[i], _values[i]);
    }

    void OnGUI()
    {
        foreach (var kvp in bin)
            GUILayout.Label("Key: " + kvp.Key + " value: " + kvp.Value);
    }

    */


    // Update is called once per frame
    void FixedUpdate () {
        Vector3 rawPosition = cam.ScreenToWorldPoint(Input.mousePosition);
        Vector3 targetPosition = new Vector3(rawPosition.x, -1.84f, 0.0f);
        float targetWidth = Mathf.Clamp(targetPosition.x, -maxWidth, maxWidth);
        targetPosition = new Vector3(targetWidth, targetPosition.y, targetPosition.z);
        rbody.MovePosition(targetPosition);

        SwapBins();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        int points = other.GetComponent<Mover>().points;
        //Add to score if bin is right type, subtract otherwise
        if (gameObject.tag.Equals(other.tag) == false)
        {
            score = score - (points * 2) < 1 ?  0 : score - (points + (points/2));
            scoreText.text = "Score: " + score;
            penalty.SetActive(true);
            penalty.GetComponent<Text>().text = "Sorting Penalty: -" + (points + (points/2));
        }

        else
        {
            penalty.SetActive(false);
            score += other.GetComponent<Mover>().points;
            scoreText.text = "Score: " + score;

            if (SceneManager.GetActiveScene().name.Equals("SurvivalMode") == false)
            {
                if (score >= gamecontroller.goalScore)
                {
                    gamecontroller.levelComplete = true;
                }
            }
        }
        Destroy(other.gameObject);
    }

  /*  void OnMouseUp()
    {
        sprite.sprite = bin;
        Debug.Log("Sprite changed!");
    }*/

    void SwapBins()
    {
        if (Input.GetButtonUp("Fire1") && Time.time > nextFire && !doNotTrigger.Contains(EventSystem.current.currentSelectedGameObject))
        {
            //Debug.Log(EventSystem.current.currentSelectedGameObject);
            nextFire = Time.time + fireRate;
            //sprite.sprite = bin[bincount++ % bin.Length];
            gameObject.tag = binname[bincount];
            sprite.sprite = bin[bincount];
            cbin.SwapBinBottom(bincount);
            bincount++;
            if (bincount == bin.Length)
                bincount = 0;

            //Debug.Log(bincount);
        }
        
    }

  

    public void SwapToYellow()
    {
        gameObject.tag = binname[0];
        sprite.sprite = bin[0];
        cbin.SwapBinBottom(0);
    }

    public void SwapToBlue()
    {
        gameObject.tag = binname[1];
        sprite.sprite = bin[1];
        cbin.SwapBinBottom(1);
    }

    public void SwapToGreen()
    {
        gameObject.tag = binname[1];
        sprite.sprite = bin[1];
        cbin.SwapBinBottom(1);
    }
}
