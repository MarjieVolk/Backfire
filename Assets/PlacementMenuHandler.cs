using UnityEngine;
using System.Collections;

public class PlacementMenuHandler : MonoBehaviour {

    public GameObject nanobot;

    private GameObject mouseFollowingSprite = null;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (mouseFollowingSprite != null) {
            Vector3 position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mouseFollowingSprite.transform.position = new Vector3(position.x, position.y, mouseFollowingSprite.transform.position.z);
        }
	}

    public void clickNanobot(int index) {
        if (mouseFollowingSprite != null) {
            Destroy(mouseFollowingSprite);
        }

        mouseFollowingSprite = new GameObject();
        mouseFollowingSprite.AddComponent<SpriteRenderer>().sprite = nanobot.GetComponent<SpriteRenderer>().sprite;
        mouseFollowingSprite.transform.localScale = new Vector3(0.5f, 0.5f, 1);
        CellHighlighter.triggerHighlights();
    }

    public bool isNanobotDragging() {
        return mouseFollowingSprite != null;
    }

    public GameObject getDraggedNanobot() {
        return GameObject.Instantiate(nanobot);
    }

    public void stopDragging() {
        Destroy(mouseFollowingSprite);
        mouseFollowingSprite = null;
        CellHighlighter.clearHighlights();
    }
}
