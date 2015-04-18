using UnityEngine;
using System.Collections;

public class Resources : MonoBehaviour {

    public int startPlacementResource = 5;
    public int startJumpResource = 0;
    public int startUpgradeResource = 0;

    private int placementResource;
    private int jumpResource;
    private int upgradeResource;

	// Use this for initialization
	void Start () {
        placementResource = startPlacementResource;
        jumpResource = startJumpResource;
        upgradeResource = startUpgradeResource;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public int getPlacementResourceAmount() {
        return placementResource;
    }

    public int getJumpResourceAmount() {
        return jumpResource;
    }

    public int getUpgradeResourceAmount() {
        return upgradeResource;
    }

    public void addPlacementResource(int amount) {
        placementResource += amount;
    }

    public void addJumpResource(int amount) {
        jumpResource += amount;
    }

    public void addUpgradeResource(int amount) {
        upgradeResource += amount;
    }

    public bool trySpendPlacementResource(int amount) {
        if (placementResource >= amount) {
            placementResource -= amount;
            return true;
        } else {
            return false;
        }
    }

    public bool trySpendJumpResource(int amount) {
        if (jumpResource >= amount) {
            jumpResource -= amount;
            return true;
        } else {
            return false;
        }
    }

    public bool trySpendUpgradeResource(int amount) {
        if (upgradeResource >= amount) {
            upgradeResource -= amount;
            return true;
        } else {
            return false;
        }
    }
}
