using UnityEngine;
using System.Collections;

public class NanobotSchematic : MonoBehaviour {

    public NanobotSchematic[] transformationRow1;
    public NanobotSchematic[] transformationRow2;
    public NanobotSchematic[] transformationRow3;

    public NanobotSchematic[][] transformation;

	void Awake() {
        transformation = new NanobotSchematic[3][];
        transformation[0] = transformationRow1;
        transformation[1] = transformationRow2;
        transformation[2] = transformationRow3;
    }

    public NanobotSchematic[][] getTransformation() {
        return transformation;
    }
}
