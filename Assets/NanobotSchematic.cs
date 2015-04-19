using UnityEngine;
using System.Collections;

public class NanobotSchematic : MonoBehaviour {

    public Move[] transformationRow1;
    public Move[] transformationRow2;
    public Move[] transformationRow3;

    private Move[][] transformation;

	void Awake() {
        transformation = new Move[3][];
        transformation[0] = transformationRow1;
        transformation[1] = transformationRow2;
        transformation[2] = transformationRow3;
    }

    public Move[][] getTransformation() {
        return transformation;
    }
}
