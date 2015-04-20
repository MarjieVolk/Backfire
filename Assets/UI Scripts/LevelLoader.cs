using UnityEngine;
using System.Collections;

public class LevelLoader : MonoBehaviour {

    private LevelManager levelManager;

    void Start() {
        levelManager = GameObject.FindObjectOfType<LevelManager>();
    }

    public void returnToMenu() {
        levelManager.returnToMenu();
    }

    public void advanceLevel() {
        levelManager.advanceLevel();
    }
}
