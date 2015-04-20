using UnityEngine;
using System.Collections;

public class LevelWinDetector : MonoBehaviour {

    public static int jumpResourceForJump = 50;

    private Resources resources;

	// Use this for initialization
	void Start () {
        transform.GetChild(0).gameObject.SetActive(false);
        resources = GameObject.FindObjectOfType<Resources>();
	}
	
	// Update is called once per frame
	void Update () {
	    if (resources.getJumpResourceAmount() >= jumpResourceForJump) {
            transform.GetChild(0).gameObject.SetActive(true);
        }
	}
}
