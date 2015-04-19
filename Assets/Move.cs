using UnityEngine;
using System.Collections;

public class Move : MonoBehaviour, TimestepManager.TimestepListener {

    BulletGridGenerator currentLevel;
    public NanobotSchematic schematic;

	// Use this for initialization
	void Start() {
        currentLevel = FindObjectOfType<BulletGridGenerator>();
        GameObject.FindObjectOfType<TimestepManager>().addListener(this);
        schematic = GameObject.Instantiate(schematic);
	}

    public void notifyTimestep() {
        currentLevel.getCellAt(gameObject.GetComponent<GridPositionComponent>().position).Cell.GetComponent<Cell>().Eat(1, false);
        for (int x = 0; x < schematic.getTransformation().Length; x++) {
            if (schematic.getTransformation()[x] != null) {
                for (int y = 0; y < schematic.getTransformation()[x].Length; y++) {
                    if (schematic.getTransformation()[x][y] != null) {
                        currentLevel.moveBot(gameObject.GetComponent<GridPositionComponent>().position, schematic.getTransformation()[x][y], new GridPosition(x - 1, y - 1));
                    }
                }
            }
        }
        GameObject.FindObjectOfType<TimestepManager>().removeListener(this);
        GameObject.FindObjectOfType<TimestepManager>().destroyAtEnd(gameObject);
    }
}
