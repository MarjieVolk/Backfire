using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class PlacementMenuHandler : MonoBehaviour {

    private GameObject mouseFollowingSprite = null;
    private Nanobot nanobotPrefab = null;
	
	void Update () {
        if (mouseFollowingSprite != null)
        {
            if (Input.GetMouseButtonDown(1)) stopDragging();
        }

        if (mouseFollowingSprite != null) {
            Vector3 position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mouseFollowingSprite.transform.position = new Vector3(position.x, position.y, mouseFollowingSprite.transform.position.z);
        }
	}

    public void clickNanobot(Nanobot nanobotPrefab) {
        if (mouseFollowingSprite != null) {
            Destroy(mouseFollowingSprite);
        }

        this.nanobotPrefab = nanobotPrefab;
        mouseFollowingSprite = new GameObject();
        mouseFollowingSprite.AddComponent<SpriteRenderer>().sprite = nanobotPrefab.GetComponent<SpriteRenderer>().sprite;
        mouseFollowingSprite.GetComponent<SpriteRenderer>().sortingOrder = 3;
        mouseFollowingSprite.transform.localScale = new Vector3(0.5f, 0.5f, 1);
        Vector3 position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseFollowingSprite.transform.position = new Vector3(position.x, position.y, mouseFollowingSprite.transform.position.z);
        CellHighlighter.triggerHighlights();
    }

    public bool isNanobotDragging() {
        return mouseFollowingSprite != null;
    }

    public Nanobot getDraggedNanobot() {
        if (mouseFollowingSprite == null) {
            return null;
        } else {
            return nanobotPrefab;
        }
    }

    public void stopDragging() {
        Destroy(mouseFollowingSprite);
        mouseFollowingSprite = null;
        nanobotPrefab = null;
        CellHighlighter.clearHighlights();
    }
}
