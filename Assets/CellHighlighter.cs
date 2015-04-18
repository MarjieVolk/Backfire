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

    void OnMouseEnter() {
        if (isHighlighted) {
            gameObject.GetComponent<SpriteRenderer>().color = Color.yellow;
        }
    }

    void OnMouseExit() {
        if (isHighlighted) {
            gameObject.GetComponent<SpriteRenderer>().color = Color.green;
        }
    }

    public Cell getCell() {
        return gameObject.GetComponent<Cell>();
    }

    public void highlight() {
        if (getCell().nanobotCanBePlacedHere()) {
            gameObject.GetComponent<SpriteRenderer>().color = Color.green;
            isHighlighted = true;
        }
    }

    public void clearHighlight() {
        gameObject.GetComponent<SpriteRenderer>().color = Color.white;
        isHighlighted = false;
    }

}
