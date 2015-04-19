using UnityEngine;
using System.Collections;

public class Move : MonoBehaviour, TimestepManager.TimestepListener {

    BulletGridGenerator currentLevel;
    public NanobotSchematic schematic;

	// Use this for initialization
	void Start () {
        currentLevel = FindObjectOfType<BulletGridGenerator>();
        GameObject.FindObjectOfType<TimestepManager>().addListener(this);
	}

    void Awake() {
        schematic = new NanobotSchematic();
    }

    public void notifyTimestep() {
        for (int x = 0; x < schematic.transformation.Length; x++) {
            for (int y = 0; y < schematic.transformation[x].Length; y++) {
                currentLevel.moveBot(gameObject.GetComponent<GridPositionComponent>().position, schematic.transformation[x][y], new GridPosition(x, y));
            }
        }
        GameObject.FindObjectOfType<TimestepManager>().removeListener(this);
        GameObject.FindObjectOfType<TimestepManager>().destroyAtEnd(gameObject);
    }
}
