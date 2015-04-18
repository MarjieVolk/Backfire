using UnityEngine;
using System.Collections;

public class CellHighlighter : MonoBehaviour {

    public static void triggerHighlights() {
        foreach (CellHighlighter highlighter in GameObject.FindObjectsOfType<CellHighlighter>()) {
            highlighter.highlight();
        }
    }

    public static void clearHighlights() {
        foreach (CellHighlighter highlighter in GameObject.FindObjectsOfType<CellHighlighter>()) {
            highlighter.clearHighlight();
        }
    }

    private bool isHighlighted = false;
    private bool isHover = false;

    void OnMouseEnter() {
        isHover = true;
    }

    void OnMouseExit() {
        isHover = false;
    }

    void Update() {
        if (isHighlighted && getCell().nanobotCanBePlacedHere()) {
            if (isHover) {
                gameObject.GetComponent<SpriteRenderer>().color = Color.green;
            } else {
                gameObject.GetComponent<SpriteRenderer>().color = Color.yellow;
            }
        } else {
            gameObject.GetComponent<SpriteRenderer>().color = Color.white;
        }
    }

    public Cell getCell() {
        return gameObject.GetComponent<Cell>();
    }

    public void highlight() {
        isHighlighted = true;
    }

    public void clearHighlight() {
        isHighlighted = false;
    }

}
