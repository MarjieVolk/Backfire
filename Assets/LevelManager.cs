using UnityEngine;
using System.Collections;

public class LevelManager : MonoBehaviour {

    public string[] levels;
    public string menu;
    public string winScreen;

    private int currentLevel = 0;
    private bool inMenu = true;

	void Start () {
        DontDestroyOnLoad(this);
	}
	
	void Update () {
	    if (!inMenu && Input.GetKey(KeyCode.Escape)) {
            returnToMenu();
        }
	}

    public void advanceLevel() {
        currentLevel++;
        loadCurrentLevel();
    }

    public void returnToMenu() {
        inMenu = true;
        Application.LoadLevel(menu);
    }

    public void loadCurrentLevel() {
        inMenu = false;
        if (currentLevel >= levels.Length) {
            currentLevel = 0;
            Application.LoadLevel(winScreen);
        } else {
            Application.LoadLevel(levels[currentLevel]);
        }
    }

    public void exitGame() {
        Application.Quit();
    }
}
