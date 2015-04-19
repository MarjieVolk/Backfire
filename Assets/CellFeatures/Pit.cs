using UnityEngine;
using System.Collections;

public class Pit : CellFeature {

    private BulletGridGenerator _grid;

	// Use this for initialization
	void Start () {
        _grid = FindObjectOfType<BulletGridGenerator>();
        NotifyResourceConsumed += ResourceConsumedHandler;
	}

    void ResourceConsumedHandler(int resourcesConsumed, bool exploded) {
        _grid.DestroyNanobotAt(Cell.GridPosition);
    }
}
