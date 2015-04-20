using UnityEngine;
using System.Collections;

public class SoundManager : MonoBehaviour {

    public AudioClip guiHoverSound;
	public AudioSource musicSource;
	public static SoundManager instance = null;
	
	public float lowPitchRange = .95f;
	public float highPitchRange = 1.05f;

	// Use this for initialization
	void Awake ()
	{
		if (instance == null)
			instance = this;
		else if (instance != this)
			Destroy (gameObject);
		
		DontDestroyOnLoad (gameObject);
	}
	
	public void PlaySingle(AudioSource source, AudioClip clip)
	{
		source.clip = clip;
		source.Play ();
	}

    public void RandomizeSfx(AudioSource source, AudioClip clip) {
        AudioClip[] clips = new AudioClip[] { clip };
        RandomizeSfx(source, clips);
    }

	public void RandomizeSfx(AudioSource source, AudioClip [] clips)
	{
		int randomIndex = Random.Range(0, clips.Length);
		float randomPitch = Random.Range (lowPitchRange, highPitchRange);
		
		source.pitch = randomPitch;
		source.clip = clips[randomIndex];
		source.Play();
	}
	
}
