using UnityEngine;
using System.Collections;

public class Nanobot : MonoBehaviour, TimestepManager.TimestepListener {

    BulletGridGenerator currentLevel;
    public NanobotSchematic schematic;
    public int price;
    public string id;

	// Use this for initialization
	void Start() {
        currentLevel = FindObjectOfType<BulletGridGenerator>();
        GameObject.FindObjectOfType<TimestepManager>().addListener(this);
        schematic = GameObject.Instantiate(schematic);
	}

    public void notifyTimestep() {
        GridPosition position = gameObject.GetComponent<GridPositionComponent>().position;
        BulletGridGenerator.GameCell cell = currentLevel.getCellAt(position);
        cell.Cell.GetComponent<Cell>().Eat(1, false);
        if (cell.Nanobot == null) {
            // TODO: Trigger animation for nanobot death.
            return;
        }
        for (int x = 0; x < schematic.getTransformation().Length; x++) {
            if (schematic.getTransformation()[x] != null) {
                for (int y = 0; y < schematic.getTransformation()[x].Length; y++) {
                    if (schematic.getTransformation()[x][y] != null) {
                        currentLevel.moveBotAnimated(position, schematic.getTransformation()[x][y], new GridPosition(x - 1, y - 1), 5, false);
                    }
                }
            }
        }
    }
}
