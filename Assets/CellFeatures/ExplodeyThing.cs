using UnityEngine;
using System.Collections;

public class ExplodeyThing : CellFeature
{
    private GridPosition[] _adjacentOffsets = new GridPosition[4] { new GridPosition(1, 0), new GridPosition(0, 1), new GridPosition(-1, 0), new GridPosition(0, -1) };
    private BulletGridGenerator _grid;

    public Sprite DangerZoneSprite;
    public int DangerZoneThreshold;
    public override int Amount
    {
        set
        {
            _amount = value;

            if (_amount == 0)
            {
                explode();
            }

            if (_amount < DangerZoneThreshold)
            {
                Sprite = DangerZoneSprite;
            }
        }
    }

	// Use this for initialization
	void Start () {
        _grid = FindObjectOfType<BulletGridGenerator>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void explode()
    {
        Cell cell = GetComponent<Cell>();
        GridPosition cellPosition = cell.GridPosition;
        explodeNanobots(cellPosition);
        explodeTerrain(cellPosition);
    }

    void explodeNanobots(GridPosition position)
    {
        //TODO how do I kill the nanobots?
        GameObject nanobotToKill = _grid.getCellAt(position).Nanobot;
        foreach (GridPosition offset in _adjacentOffsets){
            GridPosition adjacent = _grid.applyDelta(position, offset);
            if (adjacent != null)
            {
                GameObject adjacentNanobotToKill = _grid.getCellAt(adjacent).Nanobot;
                // TODO kill this guy too
            }
        }
    }

    void explodeTerrain(GridPosition position)
    {
        //TODO how do I blow up terrain w/o giving the player resources? Answer: I can't b/c I implemented it dumbly
        GameObject terrainToExplode = _grid.getCellAt(position).Cell;
        foreach (GridPosition offset in _adjacentOffsets)
        {
            GridPosition adjacent = _grid.applyDelta(position, offset);
            if (adjacent != null)
            {
                GameObject adjacentTerrainToExplode = _grid.getCellAt(adjacent).Cell;
                // TODO kill this guy too
            }
        }
    }
}
