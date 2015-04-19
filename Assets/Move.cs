﻿using UnityEngine;
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
        for (int x = 0; x < schematic.getTransformation().Length; x++) {
            if (schematic.getTransformation()[x] != null) {
                for (int y = 0; y < schematic.getTransformation()[x].Length; y++) {
                    if (schematic.getTransformation()[x][y] != null) {
                        currentLevel.moveBot(gameObject.GetComponent<GridPositionComponent>().position, schematic.getTransformation()[x][y].schematic, new GridPosition(x, y));
                    }
                }
            }
        }
        GameObject.FindObjectOfType<TimestepManager>().removeListener(this);
        GameObject.FindObjectOfType<TimestepManager>().destroyAtEnd(gameObject);
    }
}
