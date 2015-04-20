using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ButtonHoverAudioPlayer : MonoBehaviour {

	// Use this for initialization
	void Start () {
        gameObject.AddComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void onHover() {
        if (SoundManager.instance != null && GetComponent<Button>().interactable) {
            SoundManager.instance.RandomizeSfx(GetComponent<AudioSource>(), SoundManager.instance.guiHoverSound);
        }
    }
}
