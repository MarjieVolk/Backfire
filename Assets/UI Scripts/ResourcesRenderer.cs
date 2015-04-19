using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ResourcesRenderer : MonoBehaviour {

    private Text placementAmount, jumpAmount, upgradeAmount;
    private GameObject placement, jump, upgrade;

	// Use this for initialization
	void Start () {
        placementAmount = GameObject.Find("PlacementAmount").GetComponent<Text>();
        jumpAmount = GameObject.Find("JumpAmount").GetComponent<Text>();
        upgradeAmount = GameObject.Find("UpgradeAmount").GetComponent<Text>();

        placement = GameObject.Find("PlacementResource");
        jump = GameObject.Find("JumpResource");
        upgrade = GameObject.Find("UpgradeResource");
	}
	
	// Update is called once per frame
	void Update () {
        Resources resources = GameObject.FindObjectOfType<Resources>();

        placement.SetActive(resources.getPlacementResourceAmount() > 0);
        if (resources.getPlacementResourceAmount() > 0) {
            placementAmount.text = "" + resources.getPlacementResourceAmount();
        }

        jump.SetActive(resources.getJumpResourceAmount() > 0);
        if (resources.getJumpResourceAmount() > 0) {
            jumpAmount.text = "" + resources.getJumpResourceAmount();
        }

        upgrade.SetActive(resources.getUpgradeResourceAmount() > 0);
        if (resources.getUpgradeResourceAmount() > 0) {
            upgradeAmount.text = "" + resources.getUpgradeResourceAmount();
        }
	}
}
