using UnityEngine;
using System.Collections;

public class Pit : CellFeature {

    private BulletGridGenerator _grid;
    private TutorialTextManager text;

	// Use this for initialization
	void Start () {
        _grid = FindObjectOfType<BulletGridGenerator>();
        text = FindObjectOfType<TutorialTextManager>();
        NotifyResourceConsumed += ResourceConsumedHandler;
	}

    void ResourceConsumedHandler(int resourcesConsumed, bool exploded) {
        text.pitEncountered = true;
        _grid.DestroyNanobotAt(Cell.GridPosition);
    }
}
