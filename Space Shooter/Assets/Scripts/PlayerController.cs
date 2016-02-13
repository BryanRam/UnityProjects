using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class Boundary
{

	public float xMin, xMax, zMin, zMax;
	/*boundary.xMin = -6;
	 *boundary.xMax = 6;
	 *boundary.zMin = -4;
	 *boundary.zMax = 8;
	 */
}

public class PlayerController : MonoBehaviour {

	public float speed;
	public float tilt;
	public Boundary boundary;

	public GameObject shot, shot2, shot3;
	public Transform shotSpawn, shotSpawnLeft, shotSpawnRight;
	public float fireRate;

	public bool hasMiniPlayers = false;
	private GameObject miniplayer1; //For the miniship powerup
	private GameObject miniplayer2; //For the miniship powerup
	private Transform mini1;
	private Transform mini2;

	private float nextFire = 0.0f;
	public GUIText machineGunText; //For the machinegun powerup
	private bool hasMachineGun; //For the machinegun powerup
	private int machineGunRounds = 0; //For the machinegun powerup

    public bool hasBeam;
    LineRenderer beamLine;
    CapsuleCollider beamCollider;
    Animator beamAnim;
    private int beamRounds = 0;

    

    void Start()
	{
		hasMachineGun = hasBeam = false;
		machineGunText.text = "";
		mini1 = gameObject.transform.GetChild (2);
		mini2 = gameObject.transform.GetChild (3);
        beamLine = gameObject.transform.GetChild(6).GetComponent<LineRenderer>();
        beamCollider = gameObject.transform.GetChild(6).GetComponent<CapsuleCollider>();
        beamAnim = gameObject.transform.GetChild(6).GetComponent<Animator>();
        
    }
 
	void Update()
	{
		MachineGun ();

		if (Input.GetButton ("Fire1") && Time.time > nextFire) 
		{
			//Debug.Log ("PlayerController boundary xMin: " + boundary.xMin + " xMax: " + boundary.xMax);			
						nextFire = Time.time + fireRate;
            DetermineShot();
            //Instantiate (shot, shotSpawn.position, shotSpawn.rotation);
            //Instantiate (shot2, shotSpawnLeft.position, shotSpawnLeft.rotation);
            //Instantiate (shot3, shotSpawnRight.position, shotSpawnRight.rotation);
            //beamLine.enabled = true; //turn on line renderer element
            //beamLine.SetPosition(0, transform.position); //start position of the line
            //beamLine.SetPosition(1, transform.position + new Vector3(0f, 0f, 35f));
            //beamCollider.enabled = true;
            //beamCollider.height = 35f;
            //beamAnim.SetTrigger("FireLaser");
            GetComponent<AudioSource>().Play();
			if(hasMachineGun)
			{
				machineGunRounds--;
				machineGunText.text = "Volley Shots: " + machineGunRounds;
				if (machineGunRounds<=0)
				hasMachineGun = false;
			}
		}



	}

	void FixedUpdate()
	{
      //  if (canMove)
     //   {
            float moveHorizontal = Input.GetAxis("Horizontal");
            float moveVertical = Input.GetAxis("Vertical");



            /*The Rigidbody Component is how we address the physics engine.
		     So if we want to change velocity, address the rigidbody component
             vector3 = x, y, z axes
		     */
            Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);
            GetComponent<Rigidbody>().velocity = movement * speed;  //uses a vector 3 value, this tells us the direction we're going and how fast as a vector and its magnitude
            GetComponent<Rigidbody>().position = new Vector3
                (
                    Mathf.Clamp(GetComponent<Rigidbody>().position.x, boundary.xMin, boundary.xMax),
                    0.0f,
                    Mathf.Clamp(GetComponent<Rigidbody>().position.z, boundary.zMin, boundary.zMax)
                );

            GetComponent<Rigidbody>().rotation = Quaternion.Euler(0.0f, 0.0f, GetComponent<Rigidbody>().velocity.x * -tilt);
            //	Debug.Log("M1: " + mini1.transform.parent.name + "  M2: " + mini2.transform.parent.name);
       // }
	}

	void OnTriggerEnter(Collider other)
	{
        

        if (other.tag == "Powerup") 
		{
			Destroy(other.gameObject);
			if(other.name == "MachineGunPowerup(Clone)")
			{
                
                MachineGunObtained();

			}

			if(other.name == "MiniPlayerPowerup(Clone)")
			{
				MiniPlayersObtained();
				/*Instantiate (miniplayer1, new Vector3(0.0f, 0.0f, 0.0f), Quaternion.identity);
				mini1 = miniplayer1.transform;
				Instantiate (miniplayer2, new Vector3(0.0f, 0.0f, 0.0f), Quaternion.identity);
				mini2 = miniplayer2.transform;
				Debug.Log (mini1.transform + "   " + mini2.transform + " " + gameObject.transform);
				mini1.transform.parent = gameObject.transform;
				mini2.transform.parent = gameObject.transform;
				Debug.Log("M1: " + mini1.transform.parent.name + "  M2: " + mini2.transform.parent.name); */
			
			  	
			}

            if(other.name == "BeamPowerup(Clone)")
            {
                BeamObtained();
               
            }
		}
	}



	void MachineGun()
	{
		/*if (Input.GetKeyDown (KeyCode.M)) 
		{
			hasMachineGun = !hasMachineGun;
		}*/

		if (hasMachineGun) 
		{
			fireRate = 0.05f;
			machineGunText.text = "Volley Shots: " + machineGunRounds;
			//Debug.Log("Volley Shots: " + machineGunRounds);
		}

		if (!hasMachineGun && !hasBeam) {
           
			fireRate = 0.25f;
			machineGunText.text = "";
		}
	}

    void DetermineShot()
    {
        if(hasBeam && beamRounds > 0)
        {
            ShootBeam();
        }

        else
        {
            DisableBeam();
            Instantiate (shot, shotSpawn.position, shotSpawn.rotation);
            //Instantiate (shot2, shotSpawnLeft.position, shotSpawnLeft.rotation);
            //Instantiate (shot3, shotSpawnRight.position, shotSpawnRight.rotation);
        }


        GetComponent<AudioSource>().Play();
    }

	public void MachineGunObtained()
	{
        hasBeam = false;
        beamRounds = 0;
        hasMachineGun = true;
		machineGunRounds = machineGunRounds >= 100 ? 100 : machineGunRounds + 30;
	}

	public void MiniPlayersObtained()
	{
		mini1.gameObject.SetActive(true);
		mini2.gameObject.SetActive(true);
		hasMiniPlayers = true;
		boundary.xMin = -4.5f;
		boundary.xMax = 4.5f;
		boundary.zMin = -3.7f;
		boundary.zMax = 8f;

	}

    public void BeamObtained()
    {
        hasBeam = true;
        hasMachineGun = false;
        machineGunRounds = 0;
        beamRounds = beamRounds < 16 ? beamRounds + 8 : 16;
        machineGunText.text = "Beam Shots: " + beamRounds;
        fireRate = 1.05f; //give the Beam a higher cooldown time for balance
    }

    public void ShootBeam()
    {
       
        beamLine.enabled = true; //turn on line renderer element
        beamLine.SetPosition(0, transform.position); //start position of the line
        beamLine.SetPosition(1, transform.position + new Vector3(0f, 0f, 35f));
        beamCollider.enabled = true;
        beamCollider.height = 35f;
        beamAnim.SetTrigger("FireLaser");
        beamRounds--;
        machineGunText.text = "Beam Shots: " + beamRounds;
        if (beamRounds == 0)
        {
            machineGunText.text = "";
            fireRate = 0.25f;
        }
    }

    public void DisableBeam()
    {
        beamLine.enabled = false;
        beamCollider.enabled = false;
        
    }

}
