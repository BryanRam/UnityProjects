using UnityEngine;
using System.Collections;

public class KeepUIObjectsAlive : MonoBehaviour {
    public GameObject[] uiobjects;
	// Use this for initialization
	void Start () {
        DontDestroyOnLoad(gameObject);
        foreach (GameObject g in uiobjects)
        {
            DontDestroyOnLoad(g);
        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
