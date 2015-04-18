using UnityEngine;
using System.Collections;

public class PlacementMenuHandler : MonoBehaviour {

    private GameObject mouseFollowingSprite = null;
    private GameObject nanobotPrefab = null;

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

    public void clickNanobot(GameObject nanobotPrefab) {
        if (mouseFollowingSprite != null) {
            Destroy(mouseFollowingSprite);
        }

        this.nanobotPrefab = nanobotPrefab;
        mouseFollowingSprite = new GameObject();
        mouseFollowingSprite.AddComponent<SpriteRenderer>().sprite = nanobotPrefab.GetComponent<SpriteRenderer>().sprite;
        mouseFollowingSprite.transform.localScale = new Vector3(0.5f, 0.5f, 1);
        CellHighlighter.triggerHighlights();
    }

    public bool isNanobotDragging() {
        return mouseFollowingSprite != null;
    }

    public GameObject getDraggedNanobot() {
        if (mouseFollowingSprite == null) {
            return null;
        } else {
            return GameObject.Instantiate(nanobotPrefab);
        }
    }

    public void stopDragging() {
        Destroy(mouseFollowingSprite);
        mouseFollowingSprite = null;
        nanobotPrefab = null;
        CellHighlighter.clearHighlights();
    }
}
