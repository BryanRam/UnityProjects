using UnityEngine;
using System.Collections;

public class SoundManager : MonoBehaviour {
	public AudioSource efxSource;
	public AudioSource musicSource;
	public static SoundManager instance = null;

	public float lowPitchRange = .95f;
	public float highPitchRange = 1.05f;

	void Awake () {
		if (instance == null) //Making Singleton pattern
			instance = this;
		else if (instance != this)
			Destroy (gameObject);

		DontDestroyOnLoad (gameObject);
	}

	public void PlaySingle(AudioClip clip) //call this from other scripts executing the game actions
	{
		efxSource.clip = clip;
		efxSource.Play ();
	}

	public void RandomizeSfx(params AudioClip [] clips) //the params keyword allows us to pass in a comma separated list of arguments of the same type as specified by the parameter 
	{
		int randomIndex = Random.Range (0, clips.Length); //this will be used to choose a random clip to play
		float randomPitch = Random.Range (lowPitchRange, highPitchRange); //"" random pitch

		efxSource.pitch = randomPitch;
		efxSource.clip = clips [randomIndex];
		efxSource.Play ();
	}


}
