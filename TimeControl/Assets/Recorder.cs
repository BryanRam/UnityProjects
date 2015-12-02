using UnityEngine;
using System.Collections.Generic;

public struct PlayerState //A struct representing the target object's position, animation state and direction
{
    public Vector3 position;
    public int animState;
    public bool direction;

    public PlayerState(Vector3 position, int animState, bool direction) 
    {//constructor to set default values for this object's iteration of PlayerState
        this.position = position;
        this.animState = animState;
        this.direction = direction;
    }
}

public class Recorder : MonoBehaviour {
    [SerializeField] TimeController timeController;
    [SerializeField] Player player; //array

    Dictionary<int, PlayerState> states = new Dictionary<int, PlayerState>(); //dictionary to hold target object's states for each frame that they're recorded

    Animator _animator;
	public bool isRecording; 


	// Use this for initialization
	void Start () {
        _animator = GetComponent<Animator>();
		this.isRecording = true;

		//Debug.Log ("Parent:" + gameObject + "  isRecording:" + this.isRecording);
	}



    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P)) //Stop recording, give recorded states to corresponding player class, reset timer in timeController to 0
        {
            this.isRecording = !this.isRecording;

			if(!this.isRecording)
			{
				player.SetRecording(states);
				timeController.time = 0;
			}
        }

		if (!this.isRecording) {
			//Debug.Log (isRecording);
			player.SetRecording (states); //foreach

		} 
		else
			player.isPlaying = false; //foreach


    }

	public void StopRecord()
	{
		//Debug.Log ("Before Stop Record, Parent:" + gameObject + "  isRecording:" + this.isRecording);
		this.isRecording = false;
		//Debug.Log ("After Stop Record, Parent:" + gameObject + "  isRecording:" + this.isRecording);
	}

	public void StartRecord()
	{
		//Debug.Log ("Before Start Record, Parent:" + gameObject + "  isRecording:" + this.isRecording);
		this.isRecording = true;
		//Debug.Log ("After Start Record, Parent:" + gameObject + "  isRecording:" + this.isRecording);
	}

	void FixedUpdate () {

        if (this.isRecording)
        {//add the time the state was active as the key, and a new instance of PlayerState with the current position of target object, animator state, and direction
	    //So basically think time points to PlayerState instance, or array[t] = PlayerState(pos, animState, direction);
           if(states.ContainsKey(timeController.time)) //if upon finishing rewinding you stop at a point already recorded in time
			{//overwrite the state present at that time, creating a new timeline
				states[timeController.time] = new PlayerState(transform.position, 
				                                                  _animator.GetCurrentAnimatorStateInfo(0).shortNameHash, 
				                                                  transform.localScale.x > 0);
			}

			else
			{

			states.Add(timeController.time, new PlayerState(transform.position, 
                _animator.GetCurrentAnimatorStateInfo(0).shortNameHash, 
                transform.localScale.x > 0));
			}
        }
	}
}
