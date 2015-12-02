using UnityEngine;
using System.Collections.Generic;

/*This script replays the recorded actions of an object possessing the Recorder script
 * 
 */

public class Player : MonoBehaviour {
    [SerializeField] TimeController timeController;

    Dictionary<int, PlayerState> recording;

    Animator _animator;
	Recorder recorder; //CHANGE FOR ARRAY
    Rigidbody2D _rigidbody2d;
	UnityStandardAssets._2D.Platformer2DUserControl player;

    // Use this for initialization
    void Start()
    {
        _animator = GetComponent<Animator>();
        _rigidbody2d = GetComponent<Rigidbody2D>();
		player = GetComponent<UnityStandardAssets._2D.Platformer2DUserControl> ();
		this.recorder = GetComponent<Recorder> (); //CHANGE FOR ARRAY

    }

    public void SetRecording(Dictionary<int, PlayerState> recording)
    {
        this.recording = new Dictionary<int, PlayerState>(recording); //give this instance of recording all the states stored in the dictionary variable that was used to call
        isPlaying = true; //set isPlaying to true (play the recorded states)
        _rigidbody2d.isKinematic = true;
    }

    public bool isPlaying = false;

	void Update ()
	{
		if (gameObject.name == "CharacterRobotBoy") 
		{
			if (Input.GetKey (KeyCode.Q) && !recording.ContainsKey (timeController.time)) 
			{ //make isForward false (time rewinds) by pressing Q 
				timeController.modifier = 0;
			}
		}
	}


	void FixedUpdate () {
		/*Since TimeController is reset when P is pressed, and a recording is made from the very start of the game,
		 *any playback of the "past" will occur from the start of the game onward
		 *IN OTHER WORDS: Press 'P' = Game Time: 0, Get scenes from Recorder class, play scenes that correspond to current Game Time
		 */
        if (isPlaying) { //while recorded segment is playing

			player.enabled = false; //disable user input
			if (recording.ContainsKey (timeController.time)) { //if the time found in timeController is recorded as a key in recording
				PlayState (recording [timeController.time]); //play recording's state

			} else { //once time goes beyond the recorded segments, return full control of player object to player
				_rigidbody2d.isKinematic = false;
				isPlaying = false;
				player.enabled = true;
				recorder.isRecording = true; //CHANGE FOR ARRAY
			}
		} 

		else {
			_rigidbody2d.isKinematic = false;
			player.enabled = true;
			recorder.isRecording = true; //CHANGE FOR ARRAY
		}
	}

    void PlayState(PlayerState playerState) //makes target object play the position, state and direction given by playerState
    {
        transform.position = playerState.position;
        _animator.Play(playerState.animState);
        Vector3 localScale = transform.localScale;
        localScale.x = playerState.direction ? 
            Mathf.Abs(localScale.x) : -Mathf.Abs(localScale.x);
        transform.localScale = localScale;
    }
}
