using UnityEngine;
using System.Collections;

public class DestroyByBeam : MonoBehaviour {
    
    private GameController gameController;
    LineRenderer beamLine;
    CapsuleCollider beamCollider;
    Rigidbody player;
    Rigidbody miniplayer1;
    Rigidbody miniplayer2;

    void Start()
    {
        GameObject gameControllerObject = GameObject.FindWithTag("GameController");
        if (gameControllerObject != null)
        {
            gameController = gameControllerObject.GetComponent<GameController>();
        }

        if (gameController == null)
        {
            Debug.Log("Cannot find 'GameController' script");
        }
        beamLine = GetComponent<LineRenderer>();
        beamCollider = GetComponent<CapsuleCollider>();
        player = gameObject.transform.parent.GetComponent<Rigidbody>();
    }

    void OnTriggerEnter(Collider other)
    {

        if (other.tag == "Powerup")
        { //Instantiate(explosion, transform.position, transform.rotation);
            
        }

        else
        Destroy(other.gameObject);
    }

    public void FreezePosition() //stop the player and any helper ships from moving whilst the beam shooting animation plays
    {
        player.drag = Mathf.Infinity;
        miniplayer1 = gameObject.transform.parent.transform.GetChild(2).GetComponent<Rigidbody>();
        miniplayer1.drag = Mathf.Infinity;
        miniplayer2 = gameObject.transform.parent.transform.GetChild(3).GetComponent<Rigidbody>();
        miniplayer2.drag = Mathf.Infinity;

    }

   

    public void DisableBeam() //turn off beam and allow player movement once the shooting animation is over
    {
        beamLine.enabled = false;
        beamCollider.enabled = false;
        player.drag = 0f;
        miniplayer1.drag = 0f;
        miniplayer2.drag = 0f;
    }
}
