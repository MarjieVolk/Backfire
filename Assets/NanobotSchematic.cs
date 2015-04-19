using UnityEngine;
using System.Collections;

public class NanobotSchematic {

    public NanobotSchematic[][] transformation;

	// Use this for initialization
	public NanobotSchematic () {
        transformation = new NanobotSchematic[3][];
        for(int x = 0; x < 3; x++) {
            transformation[x] = new NanobotSchematic[3];
        }
    }

    public void notifyTimestep() {
    }
}
