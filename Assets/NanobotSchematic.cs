using UnityEngine;
using System.Collections;

public class NanobotSchematic : MonoBehaviour {

    public Nanobot[] transformationColumn1;
    public Nanobot[] transformationColumn2;
    public Nanobot[] transformationColumn3;

    private Nanobot[][] transformation;

	void Awake() {
        transformation = new Nanobot[3][];
        transformation[0] = transformationColumn1;
        transformation[1] = transformationColumn2;
        transformation[2] = transformationColumn3;
    }

    public Nanobot[][] getTransformation() {
        return transformation;
    }
}
