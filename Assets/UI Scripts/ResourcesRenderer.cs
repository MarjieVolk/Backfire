using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ResourcesRenderer : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        Resources resources = GameObject.FindObjectOfType<Resources>();

        GameObject.Find("PlacementAmount").GetComponent<Text>().text = "" + resources.getPlacementResourceAmount();
        GameObject.Find("JumpAmount").GetComponent<Text>().text = "" + resources.getJumpResourceAmount();
        GameObject.Find("UpgradeAmount").GetComponent<Text>().text = "" + resources.getUpgradeResourceAmount();
	}
}
