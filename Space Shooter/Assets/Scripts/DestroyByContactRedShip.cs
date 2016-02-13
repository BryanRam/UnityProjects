using UnityEngine;
using System.Collections;

public class DestroyByContactRedShip : MonoBehaviour {
    public GameObject explosion;
    public GameObject playerExplosion;
    private GameController gameController;
    public int scoreValue;
    public GameObject playerObj;
    public PlayerController player;
    private Dodge dodge;

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
        playerObj = GameObject.Find("Player");
        player = playerObj == null ? null : playerObj.GetComponent<PlayerController>();
        dodge = gameObject.transform.GetChild(3).GetComponent<Dodge>();
       
    }

    void OnTriggerEnter(Collider other)
    {
        if (GetComponent<Collider>().GetType() == typeof(CapsuleCollider))
        {
            if (other.tag == "Boundary" || other.tag == "Enemy")
            {
                return;
            }

            if (gameObject.tag == "Laser" && other.tag == "Laser")
            {
                Debug.Log("tagged");
                return;
            }

            if (gameObject.tag == "Enemy" && gameObject.name == "EnemyBolt(Clone)" && other.tag == "Laser") //enemy laser hits player laser
            {//Do nothing, thus enemy laser phases through player laser
                return;
            }

            if (gameObject.tag == "Enemy" && other.tag == "Enemy")
            {
                return;
            }

            if (other.tag == "Player")
            {
                /*if (player.lives > 0)
			     * Instantiate (playerExplosion, other.transform.position, other.transform.rotation);
			     * player.lives--;
			     *else
			     *{
			     */
                Instantiate(playerExplosion, other.transform.position, other.transform.rotation);
                gameController.GameOver();
                Destroy(other.gameObject);
                Destroy(gameObject);
                return;
                /*}
			     */
            }

            if (other.tag == "MiniPlayer")
            {

                if (other.name == "MiniPlayer")
                {
                    player.boundary.xMin = -6;
                    //Debug.Log ("M1 destroyed!");
                    //Debug.Log ("DBC boundary xMin: " + player.boundary.xMin);
                    //boundary.xMax = 4.5;
                    //boundary.zMin = -3.7;

                }

                if (other.name == "MiniPlayer 2")
                {
                    //boundary.xMin = -6;
                    player.boundary.xMax = 6;
                    //Debug.Log ("M2 destroyed!");
                    //Debug.Log ("DBC boundary xMax: " + player.boundary.xMax);
                    //boundary.zMin = -3.7;
                }
                Instantiate(explosion, transform.position, transform.rotation);
                other.transform.position = new Vector3(other.transform.parent.position.x, other.transform.parent.position.y, other.transform.parent.position.z);
                other.gameObject.SetActive(false);
                return;
            }


            if (dodge != null)
            {
                Debug.Log("count: " + dodge.count);
                if (dodge.count > 0)
                {
                    Instantiate(explosion, transform.position, transform.rotation);
                    Destroy(other.gameObject);
                    Destroy(gameObject);
                    gameController.AddScore(scoreValue);
                    return;
                }
                else
                    return;
            }

            if (explosion != null)
            {
                Instantiate(explosion, transform.position, transform.rotation);
            }

            if (other.tag == "Beam")
            {

                Instantiate(explosion, transform.position, transform.rotation);
                gameController.AddScore(scoreValue);
                return;

            }






            Destroy(other.gameObject);
            Destroy(gameObject);
            gameController.AddScore(scoreValue);
        }

    }
}
