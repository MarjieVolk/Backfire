﻿using UnityEngine;
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
