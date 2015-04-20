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

    public AudioClip tileHoverSound;

    private bool isHighlighted = false;
    private bool isHover = false;

    void Start() {
        gameObject.AddComponent<AudioSource>();
    }

    void OnMouseEnter() {
        isHover = true;
        if (SoundManager.instance != null && isHighlighted && getCell().nanobotCanBePlacedHere()) {
            SoundManager.instance.RandomizeSfx(GetComponent<AudioSource>(), tileHoverSound);
        }
    }

    void OnMouseExit() {
        isHover = false;
    }

    void Update() {
        if (isHighlighted && getCell().nanobotCanBePlacedHere() && isHover) {
            gameObject.GetComponent<SpriteRenderer>().color = Color.green;
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
