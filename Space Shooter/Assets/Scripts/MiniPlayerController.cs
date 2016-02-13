using UnityEngine;
using System.Collections;

public class MiniPlayerController : MonoBehaviour {
	public float speed;
	public float tilt;
	public Boundary boundary;
	public PlayerController player;
	
	public GameObject shot;
	public Transform shotSpawn;
	public float fireRate;
	
	/*public GameObject miniplayer1; //For the miniship powerup
	public GameObject miniplayer2; //For the miniship powerup
	private Transform mini1;
	private Transform mini2; */
	
	private float nextFire = 0.0f;
	/*public GUIText machineGunText; //For the machinegun powerup
	private bool hasMachineGun; //For the machinegun powerup
	private int machineGunRounds = 0; //For the machinegun powerup */
	
	void Start()
	{
		player = GameObject.Find ("Player").GetComponent<PlayerController> ();
		//hasMachineGun = false;
		//machineGunText.text = "";
	}
	
	void Update()
	{
		//MachineGun ();
		
		if (Input.GetButton ("Fire1") && Time.time > nextFire) 
		{
			
			nextFire = Time.time + fireRate;
			Instantiate (shot, shotSpawn.position, shotSpawn.rotation);
			GetComponent<AudioSource>().Play();
		/*	if(hasMachineGun)
			{
				machineGunRounds--;
				machineGunText.text = "Volley Shots: " + machineGunRounds;
				if (machineGunRounds<=0)
					hasMachineGun = false;
			}*/
		}
		
		
		
	}
	
	void FixedUpdate()
	{
		float moveHorizontal = Input.GetAxis ("Horizontal");
		float moveVertical = Input.GetAxis ("Vertical");
		
		/*The Rigidbody Component is how we address the physics engine.
		 So if we want to change velocity, address the rigidbody component
         vector3 = x, y, z axes
		 */
		Vector3 movement = new Vector3 (moveHorizontal, 0.0f, moveVertical);
		GetComponent<Rigidbody>().velocity = movement * speed;  //uses a vector 3 value, this tells us the direction we're going and how fast as a vector and its magnitude
		GetComponent<Rigidbody>().position = new Vector3 
			(
				Mathf.Clamp (GetComponent<Rigidbody>().position.x, boundary.xMin, boundary.xMax), 
				0.0f, 
				Mathf.Clamp (GetComponent<Rigidbody>().position.z, boundary.zMin, boundary.zMax)
				);
		
		GetComponent<Rigidbody>().rotation = Quaternion.Euler (0.0f, 0.0f, GetComponent<Rigidbody>().velocity.x * -tilt);
		//	Debug.Log("M1: " + mini1.transform.parent.name + "  M2: " + mini2.transform.parent.name);
	}
	
	void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Powerup") 
		{
			Destroy(other.gameObject);
			if(other.name == "MachineGunPowerup(Clone)")
			{
				player.MachineGunObtained();
				
			}
			
			/*if(other.name == "MiniPlayerPowerup(Clone)")
			{
				
				
				mini1 = Instantiate (miniplayer1, new Vector3(0.0f, 0.0f, 0.0f), Quaternion.identity) as Transform;
				mini2 = Instantiate (miniplayer2, new Vector3(0.0f, 0.0f, 0.0f), Quaternion.identity) as Transform;
				
				mini1.transform.parent = GameObject.Find ("Player").transform;
				mini2.transform.parent = GameObject.Find ("Player").transform;
				Debug.Log("M1: " + mini1.transform.parent.name + "  M2: " + mini2.transform.parent.name); 
				
				
			} */

        if(other.name == "BeamPowerup(Clone)")
            {
                player.BeamObtained();
            }
		}
	}
	
	/*void MachineGun()
	{

		
		if (hasMachineGun) 
		{
			fireRate = 0.05f;
			machineGunText.text = "Volley Shots: " + machineGunRounds;
			//Debug.Log("Volley Shots: " + machineGunRounds);
		}
		
		if (!hasMachineGun) {
			fireRate = 0.25f;
			machineGunText.text = "";
		}
	} */
}
