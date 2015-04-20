using UnityEngine;
using System.Collections;

public class GoToMenu : MonoBehaviour {

	void Update () {
        GameObject.FindObjectOfType<LevelManager>().returnToMenu();
	}
}
