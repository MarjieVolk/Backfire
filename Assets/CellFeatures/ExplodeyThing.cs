using UnityEngine;
using System.Collections;

public class ExplodeyThing : CellFeature
{
    private GridPosition[] _adjacentOffsets = new GridPosition[4] { new GridPosition(1, 0), new GridPosition(0, 1), new GridPosition(-1, 0), new GridPosition(0, -1) };
    private BulletGridGenerator _grid;

    public Sprite DangerZoneSprite;
    public int DangerZoneThreshold;

    public int TerrainDamage;
    public int TerrainSplashDamage;

	// Use this for initialization
	void Start () {
        _grid = FindObjectOfType<BulletGridGenerator>();
        NotifyResourceConsumed += ResourceConsumedHandler;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void ResourceConsumedHandler(int resourcesConsumed, bool exploded)
    {
        if (resourcesConsumed == 0) return;
        Debug.Log(Amount);
        if (Amount == 0)
        {
            explode();
        }

        if (Amount < DangerZoneThreshold)
        {
            Sprite = DangerZoneSprite;
        }
    }

    void explode()
    {
        GridPosition cellPosition = Cell.GridPosition;
        explodeNanobots(cellPosition);
        explodeTerrain(cellPosition);
    }

    void explodeNanobots(GridPosition position) {
        _grid.DestroyNanobotAt(position);
        foreach (GridPosition offset in _adjacentOffsets){
            GridPosition adjacent = _grid.applyDelta(position, offset);
            if (adjacent != null) {
                _grid.DestroyNanobotAt(adjacent);
            }
        }
    }

    void explodeTerrain(GridPosition position)
    {
        _grid.getCellAt(position).Cell.GetComponent<Cell>().Eat(TerrainDamage, true);
        foreach (GridPosition offset in _adjacentOffsets)
        {
            GridPosition adjacent = _grid.applyDelta(position, offset);
            if (adjacent != null)
            {
                _grid.getCellAt(adjacent).Cell.GetComponent<Cell>().Eat(TerrainSplashDamage, true);
            }
        }
    }
}
