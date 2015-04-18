using UnityEngine;
using System.Collections;

public class Move : MonoBehaviour, TimestepManager.TimestepListener {

    BulletGridGenerator currentLevel;

	// Use this for initialization
	void Start () {
        currentLevel = FindObjectOfType<BulletGridGenerator>();
        GameObject.FindObjectOfType<TimestepManager>().addListener(this);
	}

    public void notifyTimestep() {
        currentLevel.moveMe(gameObject, 0, 1);
    }
}
