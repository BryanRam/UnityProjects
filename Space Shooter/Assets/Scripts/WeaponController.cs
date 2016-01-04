using UnityEngine;
using System.Collections;

public class WeaponController : MonoBehaviour {
	public GameObject shot; //The laser object
	public Transform shotSpawn; //where the laser will spawn
	public float fireRate; //the rate that enemy fires laser
	public float delay; //the time enemy waits before firing the first laser
	
	void Start ()
	{
		InvokeRepeating ("Fire", delay, fireRate); //call Fire method in delay seconds, then everytime after fireRate seconds
	}
	
	void Fire ()
	{
		Instantiate(shot, shotSpawn.position, shotSpawn.rotation);
		audio.Play();
	}
}
