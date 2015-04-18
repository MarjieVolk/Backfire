using UnityEngine;
using System.Collections;

public class Move : MonoBehaviour {

    BulletGridGenerator currentLevel;

	// Use this for initialization
	void Start () {
        currentLevel = FindObjectOfType<BulletGridGenerator>();
	}
	
	// Update is called once per frame
    void Update() {
        currentLevel.moveMe(gameObject, 0, 1);
    }
}
