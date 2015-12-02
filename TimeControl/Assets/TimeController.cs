using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class TimeController : MonoBehaviour 
{

    public int time; //represent time spent in game as an integer
	public Recorder[] recorder; //CHANGE FOR ARRAY
	//public RecorderNew recorder2;
	public Text Time;

	public int modifier = 1;

	void Start()
	{
		//Debug.Log ("Parent: " + recorder[0].isRecording);
	}

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E)) //make isForward true (time progresses normally) by pressing E
        {
            
			isForward = true;
        }
		if (Input.GetKey (KeyCode.Q)) //make isForward false (time rewinds) by pressing Q
		{ 

			isForward = false;

			if(!isForward)
			{//when isForward is false time rewinds until time = 0
				if(Input.GetKeyUp(KeyCode.LeftArrow)) //press Left Arrow to increase speed of time reversal until 4 times speed
				{	
					modifier = modifier >= 4 ? 4 : modifier + 1;
				}
				
				if(Input.GetKeyUp(KeyCode.RightArrow)) //press Right Arrow to decrease speed of time reversal until normal speed
				{	
					modifier = modifier <= -3 ? -3 : modifier - 1;
				}
				
				time = time - modifier;
				
				time = time < 0 ? 0 : time;
				//time = 0;
				
				foreach(Recorder record in recorder)
				{
					record.StopRecord(); //CHANGE FOR ARRAY
					
				}
				//recorder2.isRecording = false;
				
				/*if(recorder[0].name == "CloneBoy")
			{
				recorder[0].isRecording = false;
				recorder[0].test = true;
				Debug.Log ("In foreach: " + recorder[0].isRecording);
			}*/
				//recorder[0].isRecording=false;
				//recorder[1].isRecording=false;
				
				//Debug.Log ("0: " + recorder[0].isRecording);
				//Debug.Log ("1: " + recorder[1].isRecording);
					Time.text = "Time: " + time;
        	}

		}


		/*else {
			recorder.isRecording = true;
		}*/
    }

    public bool isForward = true;

	void FixedUpdate () 
	{
		//time normally progresses forward for each frame isForward is true
		if(Input.GetKeyUp(KeyCode.Q))
		{
			isForward=true;
			foreach(Recorder record in recorder)
			{
				record.StartRecord(); //CHANGE FOR ARRAY

		
				//Debug.Log ("0: " + recorder[0].isRecording);
				//Debug.Log ("1: " + recorder[1].isRecording);
			}
			//recorder2.isRecording = true;
		}

        if (isForward)
        {
            time++;
			modifier = 1;
			Time.text = "Time: " + time;

        }
        /*else
        {//when isForward is false time rewinds until time = 0
			if(Input.GetKeyUp(KeyCode.LeftArrow)) //press Left Arrow to increase speed of time reversal until 4 times speed
			{	
				modifier = modifier >= 4 ? 4 : modifier + 1;
			}

			if(Input.GetKeyUp(KeyCode.RightArrow)) //press Right Arrow to decrease speed of time reversal until normal speed
			{	
				modifier = modifier <= 0 ? 0 : modifier - 1;
			}

            time = time - modifier;

            time = time < 0 ? 0 : time;
			//time = 0;

			foreach(Recorder record in recorder)
			{
				record.StopRecord(); //CHANGE FOR ARRAY

			}
			recorder2.isRecording = false;

			/*if(recorder[0].name == "CloneBoy")
			{
				recorder[0].isRecording = false;
				recorder[0].test = true;
				Debug.Log ("In foreach: " + recorder[0].isRecording);
			}*/
			//recorder[0].isRecording=false;
			//recorder[1].isRecording=false;

			//Debug.Log ("0: " + recorder[0].isRecording);
			//Debug.Log ("1: " + recorder[1].isRecording);
		/*	Time.text = "Time: " + time;
        }*/
	}
}
