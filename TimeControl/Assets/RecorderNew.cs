using UnityEngine;
using System.Collections.Generic;

/*public struct PlayerState //A struct representing the target object's position, animation state and direction
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
}*/

public class RecorderNew : MonoBehaviour {
	[SerializeField] TimeController timeController;
	[SerializeField] Player player; //array

	//public GameObject[] pState;
	 

	Dictionary<int, PlayerState> states = new Dictionary<int, PlayerState>(); //dictionary to hold target object's states for each frame that they're recorded
	
	Animator _animator;
	
	// Use this for initialization
	void Start () {
		_animator = GetComponent<Animator>();
		
	}
	
	public bool isRecording = true;
	
	void Update()
	{
		if (Input.GetKeyDown(KeyCode.P)) //Stop recording, give recorded states to corresponding player class, reset timer in timeController to 0
		{
			isRecording = !isRecording;
			
			if(!isRecording)
			{
				player.SetRecording(states);
				timeController.time = 0;
			}
		}
		
		if (!isRecording) {
			//Debug.Log (isRecording);
			player.SetRecording (states); //foreach
			
		} 
		else
			player.isPlaying = false; //foreach
		
		
	}
	
	
	
	void FixedUpdate () {
		//Debug.Log ("Parent:" + gameObject + "  isRecording:" + isRecording);
		if (isRecording)
		{//add the time the state was active as the key, and a new instance of PlayerState with the current position of target object, animator state, and direction
			//So basically think time points to PlayerState instance, or array[t] = PlayerState(pos, animState, direction);
			if(states.ContainsKey(timeController.time)) //if upon finishing rewinding you stop at a point already recorded in time
			{//overwrite the state present at that time, creating a new timeline
				/*foreach(PlayerState pstate in pState)
				 * {
				 * 		pstate = new PlayerState(pstate.transform.position, 
				                                 pstate._animator.GetCurrentAnimatorStateInfo(0).shortNameHash, 
				                                 pstate.transform.localScale.x > 0);

					}
				 */

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
