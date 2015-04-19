using UnityEngine;
using System.Collections;

public class NanobotSchematic : MonoBehaviour {

    public Nanobot[] transformationRow1;
    public Nanobot[] transformationRow2;
    public Nanobot[] transformationRow3;

    private Nanobot[][] transformation;

	void Awake() {
        transformation = new Nanobot[3][];
        transformation[0] = transformationRow1;
        transformation[1] = transformationRow2;
        transformation[2] = transformationRow3;
    }

    public Nanobot[][] getTransformation() {
        return transformation;
    }
}
