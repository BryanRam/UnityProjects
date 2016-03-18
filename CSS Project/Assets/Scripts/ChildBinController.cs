using UnityEngine;
using System.Collections;

public class ChildBinController : MonoBehaviour {
    public Sprite[] bin;
    private SpriteRenderer sprite;

    // Use this for initialization
    void Start () {
        sprite = GetComponent<SpriteRenderer>();
    }
	
	

    public void SwapBinBottom(int index)
    {
        sprite.sprite = bin[index];
    }
}
